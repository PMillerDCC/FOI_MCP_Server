using MCP_Project.Models.Database;
using Microsoft.Data.SqlClient;

namespace MCP_Project.Services;

public class DatabaseService
{
    private readonly IConfiguration _configuration;

    public DatabaseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task<TableInfo?> GetTableSchema(string tableName)
    {
        using var connection = GetConnection();

        await connection.OpenAsync();

        var table = new TableInfo
        {
            TableName = tableName
        };

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

    public async Task<List<DatabaseSearchResult>> SearchDatabase(
        string tableName,
        string columnName,
        string searchValue)
    {
        using var connection = GetConnection();

        await connection.OpenAsync();

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
                    // Ignore individual table/column failures
                    // and continue searching.
                }
            }
        }

        return results;
    }
}
