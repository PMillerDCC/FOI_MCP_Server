using MCP_Project.Models;

namespace MCP_Project.Services
{
    /// <summary>
    /// Exposes metadata about data sources that can be queried by MCP tools.
    /// </summary>
    public interface IDataSourceRegistry
    {
        /// <summary>
        /// Returns configured data source descriptions for discovery experiences.
        /// </summary>
        IEnumerable<DataSourceInfo> GetDataSources();
    }

    /// <summary>
    /// Default in-memory data source registry used by discovery tools.
    /// </summary>
    public class DataSourceRegistry : IDataSourceRegistry
    { 
        /// <summary>
        /// Returns static source metadata used to guide agent tool selection.
        /// </summary>
        public IEnumerable<DataSourceInfo> GetDataSources()
        {
            // This list is intentionally static for the PoC so callers can discover
            // available domains without querying each backend first.
            return new List<DataSourceInfo>
            { 
                new()
                { 
                    Name = "FOI Database",
                    Type = "Azure SQL Database",
                    Description = "Synthetic FOI dataset",
                    Capabilities =
                    { 
                        "Search",
                        "Filtering",
                        "Aggregation"
                    }
                },

                new()
                { 
                    Name = "ICT Contracts Register",
                    Type = "Spreadsheet",
                    Description = "Contracts management information",
                    Capabilities =
                    {
                        "Search",
                        "Contract lookup",
                        "Aggregation"
                    }
                }
            };

        }
    }
}
