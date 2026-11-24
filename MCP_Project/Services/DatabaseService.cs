using MCP_Project.Models.Database;
using Microsoft.Data.SqlClient;

namespace MCP_Project.Services;

/// <summary>
/// Provides low-level SQL discovery and search operations used by MCP tools.
/// </summary>
public class DatabaseService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new <see cref="DatabaseService"/>.
    /// </summary>
    /// <param name="configuration">Application configuration containing database connection settings.</param>
    public DatabaseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a SQL connection using the configured DefaultConnection string.
    /// </summary>
    private SqlConnection GetConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));
    }

    /// <summary>
    /// Returns schema metadata for a SQL table.
    /// </summary>
    /// <param name="tableName">Table name to inspect.</param>
    /// <returns>
    /// A <see cref="TableInfo"/> with discovered columns, or <c>null</c> when no columns were found.
    /// </returns>
    public async Task<TableInfo?> GetTableSchema(string tableName)
    {
        using var connection = GetConnection();

        await connection.OpenAsync();

        var table = new TableInfo
        {
            TableName = tableName
        };

        // INFORMATION_SCHEMA is used to inspect table shape at runtime so MCP callers
        // can discover valid columns before issuing targeted search requests.
        const string sql = @"
            SELECT
                COLUMN_NAME,
                DATA_TYPE,
                IS_NULLABLE
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = @TableName
            ORDER BY ORDINAL_POSITION";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@TableName",
            tableName);

        using var reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            table.Columns.Add(new ColumnInfo
            {
                ColumnName =
                    reader["COLUMN_NAME"].ToString()!,

                DataType =
                    reader["DATA_TYPE"].ToString()!,

                IsNullable =
                    reader["IS_NULLABLE"].ToString() == "YES"
            });
        }

        return table.Columns.Count == 0
            ? null
            : table;
    }

    /// <summary>
    /// Searches one SQL table column for partial matches.
    /// </summary>
    /// <param name="tableName">Table to query.</param>
    /// <param name="columnName">Column to search.</param>
    /// <param name="searchValue">Value fragment to match.</param>
    /// <returns>List of matching rows.</returns>
    public async Task<List<DatabaseSearchResult>> SearchDatabase(
        string tableName,
        string columnName,
        string searchValue)
    {
        using var connection = GetConnection();

        await connection.OpenAsync();

        // Table and column identifiers are dynamic here to support generic discovery-driven
        // search across many datasets. Callers should provide trusted schema names.
        string sql = $@"
            SELECT TOP 100 *
            FROM [{tableName}]
            WHERE CAST([{columnName}] AS NVARCHAR(MAX))
                  LIKE @SearchValue";

        using var command =
            new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@SearchValue",
            $"%{searchValue}%");

        using var reader =
            await command.ExecuteReaderAsync();

        var results =
            new List<DatabaseSearchResult>();

        while (await reader.ReadAsync())
        {
            var row = new DatabaseSearchResult
            {
                TableName = tableName
            };

            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Values.Add(
                    reader.GetName(i),
                    reader.IsDBNull(i)
                        ? null
                        : reader.GetValue(i));
            }

            results.Add(row);
        }

        return results;
    }

    /// <summary>
    /// Searches all text columns in all base tables for a value.
    /// </summary>
    /// <param name="searchValue">Value to search for.</param>
    /// <returns>Flattened collection of row matches across all scanned tables.</returns>
    public async Task<List<DatabaseSearchResult>> SearchAllTables(
        string searchValue)
    {
        var results =
            new List<DatabaseSearchResult>();

        using var connection = GetConnection();

        await connection.OpenAsync();

        var tables = new List<string>();

        const string tableSql = @"
            SELECT TABLE_NAME
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_TYPE = 'BASE TABLE'
            ORDER BY TABLE_NAME";

        using (var command =
               new SqlCommand(tableSql, connection))
        {
            using var reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                tables.Add(
                    reader.GetString(0));
            }
        }

        foreach (var table in tables)
        {
            var columns =
                new List<string>();

            // Only text-compatible columns are scanned to avoid invalid casts and to
            // keep broad searches focused on user-readable content.
            const string columnSql = @"
                SELECT COLUMN_NAME
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName
                AND DATA_TYPE IN
                (
                    'varchar',
                    'nvarchar',
                    'char',
                    'nchar',
                    'text',
                    'ntext'
                )";

            using (var command =
                   new SqlCommand(columnSql, connection))
            {
                command.Parameters.AddWithValue(
                    "@TableName",
                    table);

                using var reader =
                    await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    columns.Add(
                        reader.GetString(0));
                }
            }

            foreach (var column in columns)
            {
                try
                {
                    var matches =
                        await SearchDatabase(
                            table,
                            column,
                            searchValue);

                    results.AddRange(matches);
                }
                catch
                {
                    // Keep whole-database search resilient: one problematic table/column
                    // should not prevent useful results from other tables.
                }
            }
        }

        return results;
    }
}
