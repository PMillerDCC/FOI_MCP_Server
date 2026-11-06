using MCP_Project.Services;
using ModelContextProtocol.Server;

[McpServerToolType]
public class DiscoveryTools
{
    [McpServerTool]
    public static object GetAvailableDataSources(
        IDataSourceRegistry registry)
    {
        try
        {
            return registry.GetDataSources();
        }
        catch (Exception ex)
        {
            return new
            {
                Error = "Failed to retrieve data sources",
                Details = ex.Message
            };
        }
    }
}