using MCP_Project.Models;

namespace MCP_Project.Services
{
    public interface IDataSourceRegistry
    {
        IEnumerable<DataSourceInfo> GetDataSources();
    }

    public class DataSourceRegistry : IDataSourceRegistry
    { 
        public IEnumerable<DataSourceInfo> GetDataSources()
        {
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
