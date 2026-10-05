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
            string htmlString = "<div style=\"position: relative; min-height: 100vh; padding-bottom: 150px;\">"; // Added relative position and padding

            string lastContractor = "";
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

                        // close main contractor table
                        htmlString += "</table>";

                        // totals mini-table (aligned to Net Amount Due column)
                        htmlString += "<table style=\"width:100%; font-size:11px; border-collapse:collapse; margin-top:4px; table-layout:fixed;\" class=\"marginBottom30\">";
                        htmlString += "<colgroup>";
                        htmlString += "<col style=\"width:62%;\"/>";
                        htmlString += "<col style=\"width:17%;\"/>";
                        htmlString += "<col style=\"width:11%;\"/>";
                        htmlString += "<col style=\"width:10%;\"/>";
                        htmlString += "</colgroup>";
                        htmlString += "<tr>";
                        htmlString += "<td></td>";
                        htmlString += "<td align=\"right\" style=\"white-space:nowrap;\"><strong>Total net amount due</strong></td>";
                        htmlString += "<td align=\"center\" style=\"white-space:nowrap;\"><strong>" + DCC_Formatting.ReportMoney(contractorSubTotal) + "</strong></td>";
                        htmlString += "<td></td>";
                        htmlString += "</tr>";
                        htmlString += "</table>";

                        grandTotal += contractorSubTotal;

                        contractorSubTotal = 0; // reset the total for the next contractor

                        //htmlString += "<div class=\"printOnly\">";
                        htmlString += "<table class=\"marginBottom30\" style=\"width: 100%; table-layout: fixed; font-size: 11px;\">";
                        htmlString += "<tr>";
                        htmlString += "<td>Notes</td>";
                        htmlString += "</tr>";
                        htmlString += "<tr>";
                        htmlString += "<td style=\"height:100px; border: 1px, solid;\"></td>";
                        htmlString += "</tr>";
                        htmlString += "<tr>";
                        htmlString += "<td>I CERTIFY THAT THE ABOVE AMOUNTS ARE DUE FOR PAYMENT & THAT THE CONTRACTS HAVE BEEN PERFORMED IN ACCORDANCE WITH THE SERVICE SPECIFICATION & CONDITIONS OF CONTRACT.</td>";
                        htmlString += "</tr>";
                        htmlString += "<tr>";
                        htmlString += "<td>Signed on behalf of Contractor .............................................</td>";
                        htmlString += "</tr>";
                        htmlString += "</table>";
                        //htmlString += "</div>";
                        htmlString += "</div>"; // Page Break Closing

                    }

                    htmlString += "<div class=\"contractor-table\">";

                    // Display the contractor's name and license number

                    htmlString += "<h3>" + row.ContractorName + " " + row.ContractorLicenceNo + "</h3>";

                    // Start a new table for the new contractor

                    htmlString += "<table style=\"margin: auto; width: 100%; table-layout:auto;\" class=\"marginBottom30\">";
                    htmlString += "<colgroup>";
                    htmlString += "<col style=\"width: 6%;\"/>";   // Contract Ref
                    htmlString += "<col style=\"width: 12%;\"/>";  // Agreement
                    htmlString += "<col style=\"width: 10%;\"/>";  // Service No
                    htmlString += "<col style=\"width: 10%;\"/>";  // Route Info
                    htmlString += "<col style=\"width: 12%;\"/>";  // Price per Day
                    htmlString += "<col style=\"width: 12%;\"/>";  // Fuel Element
                    htmlString += "<col style=\"width: 10%;\"/>";  // Total Price
                    htmlString += "<col style=\"width: 7%;\"/>";   // Days Operating
                    htmlString += "<col style=\"width: 11%;\"/>";  // Net Amount
                    htmlString += "<col style=\"width: 10%;\"/>";  // Variations
                    htmlString += "</colgroup>";
                    // create column headers

                    htmlString += "<tr>";
                    htmlString += "<td align=\"left\">Contract Ref</td>";
                    htmlString += "<td align=\"center\">Agreement</td>";
                    htmlString += "<td align=\"left\">Service No</td>";
                    htmlString += "<td align=\"left\">Route Info</td>";
                    htmlString += "<td align=\"center\">Price per Day<br/>(excluding fuel element)</td>";
                    htmlString += "<td align=\"center\">Fuel element<br/>(on day of pay period)</td>";
                    htmlString += "<td align=\"center\">Total price<br/>per day</td>";
                    htmlString += "<td align=\"center\">Days<br/>Operating</td>";
                    htmlString += "<td align=\"center\">Net Amount<br/>Due</td>";
                    htmlString += "<td align=\"center\">Variations</td>";
                    htmlString += "</tr>";

                    lastContractor = row.ContractorName;
                }

                // Add data row for the current contract

                htmlString += "<tr>";
                htmlString += "<td><strong>" + row.ContractReference + "</strong></td>";
                htmlString += "<td align=\"center\"><strong>" + row.AgreementDescription + "</strong></td>";
                htmlString += "<td align=\"left\"><strong>" + row.ServiceNo + "</strong></td>";
                htmlString += "<td><strong>" + row.RouteText + "</strong></td>";
                htmlString += "<td align=\"center\"><strong>£" + row.ContractVariationAveragePricePerDay + "</strong></td>";
                htmlString += "<td align=\"center\"><strong>£" + row.ContractVariationFuelPriceElement + "</strong></td>";
                htmlString += "<td align=\"center\"><strong>" + (Convert.ToDecimal(row.ContractVariationAveragePricePerDay) + Convert.ToDecimal(row.ContractVariationFuelPriceElement)).ToString("F2") + "</strong></td>";
                htmlString += "<td align=\"center\"><strong>" + row.NoOfDaysOfOperationPerYear + "</strong></td>";
                htmlString += "<td align=\"center\"><strong>" + DCC_Formatting.ReportMoney(row.ContractAmount) + "</strong></td>";
                htmlString += "<td align=\"center\">.................................</td>";
                htmlString += "</tr>";

                contractorSubTotal += row.ContractAmount;

            }

            // create total row for the last contractor before starting a new one

            // close main contractor table
            htmlString += "</table>";

            // totals mini-table (aligned to Net Amount Due column)
            htmlString += "<table style=\"margin:auto; width:100%; font-size:11px; border-collapse:collapse; margin-top:4px; table-layout:fixed;\" class=\"marginBottom30\">";
            htmlString += "<colgroup>";
            htmlString += "<col style=\"width:62%;\"/>";
            htmlString += "<col style=\"width:17%;\"/>";
            htmlString += "<col style=\"width:11%;\"/>";
            htmlString += "<col style=\"width:10%;\"/>";
            htmlString += "</colgroup>";
            htmlString += "<tr>";
            htmlString += "<td></td>";
            htmlString += "<td align=\"right\" style=\"white-space:nowrap;\"><strong>Total net amount due</strong></td>";
            htmlString += "<td align=\"center\" style=\"white-space:nowrap;\"><strong>" + DCC_Formatting.ReportMoney(contractorSubTotal) + "</strong></td>";
            htmlString += "<td></td>";
            htmlString += "</tr>";
            htmlString += "</table>";

            grandTotal += contractorSubTotal;

            contractorSubTotal = 0; // reset the total for the next contractor

            //htmlString += "<div class=\"printOnly\">";
            htmlString += "<table style=\"width: 100%; font-size: 11px; margin-top: 20px;\">";
            htmlString += "<tr>";
            htmlString += "<td>Notes</td>";
            htmlString += "</tr>";
            htmlString += "<tr>";
            htmlString += "<td style=\"height:100px; border:1px, solid;\"></td>";
            htmlString += "</tr>";
            htmlString += "<tr>";
            htmlString += "<td>I CERTIFY THAT THE ABOVE AMOUNTS ARE DUE FOR PAYMENT & THAT THE CONTRACTS HAVE BEEN PERFORMED IN ACCORDANCE WITH THE SERVICE SPECIFICATION & CONDITIONS OF CONTRACT.</td>";
            htmlString += "</tr>";
            htmlString += "<tr>";
            htmlString += "<td>Signed on behalf of Contractor .............................................</td>";
            htmlString += "</tr>";
            htmlString += "</table>";
            //htmlString += "</div>";

            // create final total row for the last contractor

            htmlString += "<h3>Total amount due: " + DCC_Formatting.ReportMoney(grandTotal) + "</h3>";

            htmlString += "</div>"; // Close the relative position container

            RawHTML = new HtmlString(htmlString);
        }

    }

}
