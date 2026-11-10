using System.ComponentModel;
using MCP_Project.Models;
using MCP_Project.Models.Database;
using MCP_Project.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace FOI_MCP_Server.Tools;

[McpServerToolType]
public sealed class DatabaseTools
{
    private readonly IConfiguration _configuration;
    private readonly DatabaseService _databaseService;
    private readonly AuditService _auditService;
    private readonly ILogger<DatabaseTools> _logger;

    public DatabaseTools(
        IConfiguration configuration,
        DatabaseService databaseService,
        AuditService auditService,
        ILogger<DatabaseTools> logger)
    {
        _configuration = configuration;
        _databaseService = databaseService;
        _auditService = auditService;
        _logger = logger;
    }

    /*
     * SQL database connectivity, discovery and search tools
     */

    [McpServerTool]
    [Description(
        "Tests whether a connection can be established to the configured " +
        "SQL database. Use this to check database availability and connectivity.")]
    public async Task<string> TestDatabaseConnection()
    {
        const string toolName = nameof(TestDatabaseConnection);
        const string requestData = "No parameters";

        try
        {
            _logger.LogInformation(
                "Tool executed: {ToolName}",
                toolName);

            var connectionString =
                _configuration.GetConnectionString(
                    "DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "The DefaultConnection connection string is not configured.");
            }

            await using var connection =
                new SqlConnection(connectionString);

            await connection.OpenAsync();

            const string response =
                "Database connection successful";

            _logger.LogInformation(
                "{ToolName} completed successfully",
                toolName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                response,
                true);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}",
                toolName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Database connection failed",
                false,
                ex.Message);

            return $"Database connection failed: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(
        "Returns schema information for a specified SQL database table, " +
        "including its column names, data types and nullable status. " +
        "Use this before searching a table when its column names are unknown.")]
    public async Task<TableInfo?> GetTableSchema(
        [Description(
            "The exact SQL table name, for example FOI_Requests or Contracts.")]
        string tableName)
    {
        const string toolName = nameof(GetTableSchema);
        var requestData = $"TableName={tableName}";

        try
        {
            ValidateRequiredValue(
                tableName,
                nameof(tableName),
                "A table name must be provided.");

            _logger.LogInformation(
                "Tool executed: {ToolName}. TableName={TableName}",
                toolName,
                tableName);

            var schema =
                await _databaseService.GetTableSchema(tableName);

            var responseSummary = schema is null
                ? $"No schema was found for table {tableName}"
                : $"Schema returned for table {tableName}";

            _logger.LogInformation(
                "{ToolName} completed. SchemaFound={SchemaFound}",
                toolName,
                schema is not null);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                responseSummary,
                true);

            return schema;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. TableName={TableName}",
                toolName,
                tableName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Table schema retrieval failed",
                false,
                ex.Message);

            throw;
        }
    }

    [McpServerTool]
    [Description(
        "Searches a specified SQL database table and column for a value. " +
        "Use GetTableSchema first when the available column names are unknown.")]
    public async Task<IReadOnlyList<DatabaseSearchResult>> SearchDatabase(
        [Description(
            "The exact SQL table name, for example Planning_Applications.")]
        string tableName,

        [Description(
            "The exact SQL column name to search, for example WardName.")]
        string columnName,

        [Description(
            "The text or value to find in the specified column.")]
        string searchValue)
    {
        const string toolName = nameof(SearchDatabase);

        var requestData =
            $"TableName={tableName}; " +
            $"ColumnName={columnName}; " +
            $"SearchValue={searchValue}";

        try
        {
            ValidateRequiredValue(
                tableName,
                nameof(tableName),
                "A table name must be provided.");

            ValidateRequiredValue(
                columnName,
                nameof(columnName),
                "A column name must be provided.");

            ValidateRequiredValue(
                searchValue,
                nameof(searchValue),
                "A search value must be provided.");

            _logger.LogInformation(
                "Tool executed: {ToolName}. " +
                "TableName={TableName}, ColumnName={ColumnName}",
                toolName,
                tableName,
                columnName);

            var results =
                await _databaseService.SearchDatabase(
                    tableName,
                    columnName,
                    searchValue);

            _logger.LogInformation(
                "{ToolName} returned {Count} matching rows",
                toolName,
                results.Count);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{results.Count} matching rows returned",
                true);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. " +
                "TableName={TableName}, ColumnName={ColumnName}",
                toolName,
                tableName,
                columnName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Targeted database search failed",
                false,
                ex.Message);

            throw;
        }
    }

    [McpServerTool]
    [Description(
        "Searches all searchable tables and text columns in the SQL database " +
        "for a value. Use this when the relevant table or column is unknown.")]
    public async Task<IReadOnlyList<DatabaseSearchResult>> SearchAllTables(
        [Description(
            "The text or value to find across the SQL database.")]
        string searchValue)
    {
        const string toolName = nameof(SearchAllTables);
        var requestData = $"SearchValue={searchValue}";

        try
        {
            ValidateRequiredValue(
                searchValue,
                nameof(searchValue),
                "A search value must be provided.");

            _logger.LogInformation(
                "Tool executed: {ToolName}. SearchValue={SearchValue}",
                toolName,
                searchValue);

            var results =
                await _databaseService.SearchAllTables(searchValue);

            _logger.LogInformation(
                "{ToolName} returned {Count} matching rows",
                toolName,
                results.Count);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{results.Count} matching rows returned",
                true);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}",
                toolName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Whole-database search failed",
                false,
                ex.Message);

            throw;
        }
    }

    /*
     * Shared validation and audit helpers
     */

    private static void ValidateRequiredValue(
        string value,
        string parameterName,
        string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                errorMessage,
                parameterName);
        }
    }

    private async Task WriteAuditLogAsync(
        string toolName,
        string requestData,
        string responseSummary,
        bool success,
        string? errorMessage = null)
    {
        try
        {
            await _auditService.LogAsync(new AuditLog
            {
                TimestampUtc = DateTime.UtcNow,
                ToolName = toolName,
                RequestData = requestData,
                ResponseSummary = responseSummary,
                Success = success,
                ErrorMessage = errorMessage
            });
        }
        catch (Exception auditException)
        {
            _logger.LogError(
                auditException,
                "Unable to write audit record for tool {ToolName}",
                toolName);
        }
    }
}
