using Microsoft.Data.SqlClient;
using ModelContextProtocol.Server;

namespace FOI_MCP_Server.Tools;

[McpServerToolType]
public class DatabaseTool
{
    private readonly IConfiguration _configuration;

    public DatabaseTool(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [McpServerTool]
    public string TestDatabaseConnection()
    {
        try
        {
            var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

            using var connection =
            new SqlConnection(connectionString);

            connection.Open();

            return "Database connection successful";
        }
        catch (Exception ex)
        {
            return $"Database connection failed: {ex.Message}";
        }
    }
}
