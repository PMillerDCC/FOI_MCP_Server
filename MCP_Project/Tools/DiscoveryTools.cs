using MCP_Project.Services;
using ModelContextProtocol.Server;

[McpServerToolType]
/// <summary>
/// MCP discovery tools that expose available backend data sources.
/// </summary>
public class DiscoveryTools
{
    [McpServerTool]
    /// <summary>
    /// Returns metadata about queryable data sources.
    /// </summary>
    /// <param name="registry">Registry that supplies source metadata.</param>
    /// <returns>Collection of data source descriptors or an error payload.</returns>
    public static object GetAvailableDataSources(
        IDataSourceRegistry registry)
    {
        try
        {
            return registry.GetDataSources();
        }
        catch (Exception ex)
        {
            // Return a structured payload so MCP callers receive useful diagnostics
            // instead of a transport-level failure for discovery requests.
            return new
            {
                Error = "Failed to retrieve data sources",
                Details = ex.Message
            };
        }
    }
}