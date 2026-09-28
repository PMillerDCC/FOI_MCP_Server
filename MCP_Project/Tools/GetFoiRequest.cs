using Microsoft.Data.SqlClient;
using ModelContextProtocol.Server;

namespace FOI_MCP_Server.Tools;

[McpServerToolType]
public class GetFoiRequest
{
    private readonly IConfiguration _configuration;

    public GetFoiRequest(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [McpServerTool]
    public string GetFirstFoiRequest()
    {
        try
        {
            var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

            using var connection =
            new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(
            "SELECT TOP 1 Summary FROM FOI_Requests",
            connection);

            var result = command.ExecuteScalar();

            return result?.ToString() ?? "No records found";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
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
    [McpServerTool]
    public object Get_FoiRequest(int requestId)
    {
        try
        {
            var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

            using var connection =
            new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(
                @"SELECT *
                FROM FOI_Requests
                WHERE RequestID = @RequestID",
            connection);

            command.Parameters.AddWithValue("@RequestID", requestId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new
                {
                    RequestID = reader["RequestID"],
                    DateReceived = reader["DateReceived"],
                    RequesterType = reader["RequesterType"],
                    Summary = reader["Summary"],
                    Category = reader["Category"],
                    Status = reader["Status"],
                    DueDate = reader["DueDate"]
                };
            }

            return "Request not found";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
