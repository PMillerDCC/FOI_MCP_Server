using DCC_Standard;
using ProspectBusinessLayer;

namespace Prospect_Test
{
    // Define a data access layer interface
    public interface IDatabaseAccess
    {
        void UpdateFuelCost(int K_FuelCost, DateTime EffectiveFrom, decimal Price);
    }

    // The main class with the Update method
    public class FuelCostUpdater
    {
        private readonly IDatabaseAccess databaseAccess;

        public FuelCostUpdater(IDatabaseAccess databaseAccess)
        {
            this.databaseAccess = databaseAccess;
        }

        public int Update(int K_FuelCost, DateTime EffectiveFrom, decimal Price)
        {
            DCC_DatabaseFrame dbf = new DCC_DatabaseFrame("uFuelCost");
            dbf.AddTable("FuelCost");
            dbf.AddParameter("K_FuelCost", K_FuelCost);
            dbf.AddParameter("EffectiveFrom", EffectiveFrom);
            dbf.AddParameter("Price", Price);
            databaseAccess.UpdateFuelCost(K_FuelCost, EffectiveFrom, Price);
            return (dbf.OK ? K_FuelCost : -1);
        }

        public class ContractTests
        {
            // Mock data source
            private List<ContractVariation> mockContracts = new List<ContractVariation>
        {
            new ContractVariation { ContractVariationDateStart = new DateTime(2023, 4, 1), ContractVariationDateEnd = new DateTime(2023, 8, 1) },
            new ContractVariation { ContractVariationDateStart = new DateTime(2023, 6, 1), ContractVariationDateEnd = new DateTime(2023, 9, 1) },
            new ContractVariation { ContractVariationDateStart = new DateTime(2023, 10, 1), ContractVariationDateEnd = new DateTime(2024, 1, 1) }
        };

            [TestMethod]
            public void TestRetrieveContractsInDateRangeMock()
            {
                // Arrange
                DateTime startDate = new DateTime(2023, 5, 1);
                DateTime endDate = new DateTime(2023, 12, 31);

                List<ContractVariation> retrievedContracts = RetrieveContractsInDateRangeMock(startDate, endDate);

                // Act
                bool allContractsValid = true;
                foreach (var contract in retrievedContracts)
                {
                    if (!(contract.ContractVariationDateStart >= startDate && contract.ContractVariationDateEnd <= endDate))
                    {
                        allContractsValid = false;
                        break;
                    }
                }

                // Assert
                Assert.IsTrue(allContractsValid);
            }

            [TestMethod]
            public void IntegrationTestRetrieveContracts()
            {
                // Arrange
                DateTime startDate = new DateTime(2023, 5, 1);
                DateTime endDate = new DateTime(2023, 12, 31);

                List<ContractVariation> retrievedContracts = RetrieveContractsInDateRangeReal(startDate, endDate);

                // Act
                bool allContractsValid = true;
                foreach (var contract in retrievedContracts)
                {
                    if (!(contract.ContractVariationDateStart >= startDate && contract.ContractVariationDateEnd <= endDate))
                    {
                        allContractsValid = false;
                        break;
                    }
                }

                // Assert
                Assert.IsTrue(allContractsValid);
            }

            // Mock function to retrieve contracts
            public List<ContractVariation> RetrieveContractsInDateRangeMock(DateTime startDate, DateTime endDate)
            {
                return mockContracts.FindAll(c => c.ContractVariationDateStart >= startDate && c.ContractVariationDateEnd <= endDate);
            }

            public List<ContractVariation> RetrieveContractsInDateRangeReal(DateTime startDate, DateTime endDate)
            {
                // For this example, I'll just use the mock list, but in reality, 
                // this function should connect to the actual data source.
                return mockContracts.FindAll(c => c.ContractVariationDateStart >= startDate && c.ContractVariationDateEnd <= endDate);
            }
        }
    }
}