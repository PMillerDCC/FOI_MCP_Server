using DCC_Standard;
using System;
using System.Web;

namespace ProspectBusinessLayer
{

    public class ContractsPaymentsScheduleParameters
    {

        // Properties

        public DateTime?   StartDate     { get; set; }
        public DateTime?   EndDate       { get; set; }
        public IHtmlString RawHTML       { get; set; }

        public string      StartDateText { get { return DCC_Formatting.NullDateTxt(StartDate, "dd MMMM yyyy"); } }
        public string      EndDateText   { get { return DCC_Formatting.NullDateTxt(EndDate, "dd MMMM yyyy"); } }

        // Constructors

        public ContractsPaymentsScheduleParameters()
        {
        }

        // Methods

        public void LoadHTML(DCC_Database db)
        {
            string htmlString = "", lastContractor = "";
            decimal contractorSubTotal = 0;
            decimal grandTotal = 0;
            ContractsPaymentsScheduleReportRowList rowList = new ContractsPaymentsScheduleReportRowList(db, pStartDate: StartDate.Value, pEndDate: EndDate.Value);
            
            foreach (ContractsPaymentsScheduleReportRow row in rowList)
            {
                 
                if (lastContractor != row.ContractorName)
                {
                    if (!string.IsNullOrEmpty(lastContractor))
                    {
                        // create total row for the last contractor before starting a new one

                        htmlString += "<tr>";
                        htmlString += "<td colspan=\"7\"></td>";
                        htmlString += "<td align=\"right\">Total amount due</td>";
                        htmlString += "<td align=\"right\"><strong>" + DCC_Formatting.ReportMoney(contractorSubTotal) + "</strong></td>"; // totalNetAmountDue on operator contract
                        htmlString += "</tr>";
                        htmlString += "</table>";

                        grandTotal += contractorSubTotal;

                        contractorSubTotal = 0; // reset the total for the next contractor
                        
                        htmlString += "<div class=\"printOnly\">";
                        htmlString += "<p class=\"note-border\">Notes:</p>";
                        htmlString += "<p>I CERTIFY THAT THE ABOVE AMOUNTS ARE DUE FOR PAYMENT & THAT THE CONTRACTS HAVE BEEN PERFORMED IN ACCORDANCE WITH THE SERVICE SPECIFICATION & CONDITIONS OF CONTRACT.</p>";
                        htmlString += "<p>Signed on behalf of Contractor ..............................</p>";
                        htmlString += "</div>";
                        htmlString += "</div>"; // Page Break Closing
                    }

                    htmlString += "<div class=\"contractor-table\">";
                    
                    // Display the contractor's name and license number

                    htmlString += "<h3>" + row.ContractorName + " " + row.ContractorLicenceNo + "</h3>";
                    
                    // Start a new table for the new contractor

                    htmlString += "<table style=\"margin: auto; width: 100%;\" class=\"marginBottom30\">";
                    
                    // create column headers
                    
                    htmlString += "<tr>";
                    htmlString += "<td>Contract Reference</td>";
                    htmlString += "<td>Agreement</td>";
                    htmlString += "<td>Service Number</td>";
                    htmlString += "<td>Route Information</td>";
                    htmlString += "<td align=\"right\">Price per Day</td>";
                    htmlString += "<td align=\"right\">Fuel element</td>";
                    htmlString += "<td align=\"right\">Total price per day</td>";
                    htmlString += "<td align=\"right\">Days operating</td>";
                    htmlString += "<td align=\"right\">Net Amount Due</td>";
                    htmlString += "</tr>";

                    lastContractor = row.ContractorName;
                }

                // Add data row for the current contract

                htmlString += "<tr>";
                htmlString += "<td><strong>" + row.ContractReference + "</strong></td>";
                htmlString += "<td><strong>" + row.AgreementDescription + "</strong></td>";
                htmlString += "<td><strong>" + row.ServiceNo + "</strong></td>";
                htmlString += "<td><strong>" + row.RouteText + "</strong></td>";
                htmlString += "<td align=\"right\"><strong>£" + row.ContractVariationAveragePricePerDay + "</strong></td>";
                htmlString += "<td align=\"right\"><strong>£" + row.ContractVariationFuelPriceElement + "</strong></td>";
                htmlString += "<td align=\"right\"><strong>" + (Convert.ToDecimal(row.ContractVariationAveragePricePerDay) + Convert.ToDecimal(row.ContractVariationFuelPriceElement)).ToString("F2") + "</strong></td>";
                htmlString += "<td align=\"right\"><strong>" + row.NoOfDaysOfOperationPerYear + "</strong></td>";
                htmlString += "<td align=\"right\"><strong>" + DCC_Formatting.ReportMoney(row.ContractAmount) + "</strong></td>";
                htmlString += "</tr>";

                contractorSubTotal += row.ContractAmount;
                
            }

            // create total row for the last contractor before starting a new one

            htmlString += "<tr>";
            htmlString += "<td colspan=\"7\"></td>";
            htmlString += "<td align=\"right\"><strong>Total amount due</strong></td>";
            htmlString += "<td align=\"right\"><strong>" + DCC_Formatting.ReportMoney(contractorSubTotal) + "</strong></td>"; // totalNetAmountDue on operator contract
            htmlString += "</tr>";
            htmlString += "</table>";
            
            grandTotal += contractorSubTotal;
            
            contractorSubTotal = 0; // reset the total for the next contractor

           
            htmlString += "<div class=\"printOnly\">";
            htmlString += "<p class=\"note-border\">Notes:</p>";
            htmlString += "<p>I CERTIFY THAT THE ABOVE AMOUNTS ARE DUE FOR PAYMENT & THAT THE CONTRACTS HAVE BEEN PERFORMED IN ACCORDANCE WITH THE SERVICE SPECIFICATION & CONDITIONS OF CONTRACT.</p>"; 
            htmlString += "<p>Signed on behalf of Contractor..............................</p>";
            htmlString += "</div>";
            htmlString += "</div>"; // Page Break Closing

            // create final total row for the last contractor

            htmlString += "<h3>Total amount due: " + DCC_Formatting.ReportMoney(grandTotal) + "</h3>";
            
            RawHTML = new HtmlString(htmlString);
        }

    }

}
