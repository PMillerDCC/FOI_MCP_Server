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
            bool firstTime = true;
            //decimal totalContractAmount = 0;//, totalVariations = 0;//, totalNetAmountDue = 0;
            ContractsPaymentsScheduleReportRowList rowList = new ContractsPaymentsScheduleReportRowList(db, pStartDate: StartDate.Value, pEndDate: EndDate.Value);
            foreach (ContractsPaymentsScheduleReportRow row in rowList)
            {
                if (lastContractor != row.ContractorName)
                {
                    if (firstTime)
                    {
                        firstTime = false;
                    }
                    else
                    {
                        // create total row
                        htmlString += "<tr>";
                        htmlString += "<td>TOTALS</td>";
                        htmlString += "<td></td>";
                        htmlString += "<td></td>";
                        htmlString += "<td></td>";
                        htmlString += "<td></td>";
                        htmlString += "<td></td>";
                        htmlString += "<td align=\"center\">£???</td>";
                        htmlString += "<td align=\"center\"></td>";
                        htmlString += "<td align=\"center\">£???</td>";
                        htmlString += "</tr>";
                        //totalContractAmount = 0;//totalVariations = 0;//totalNetAmountDue = 0;
                    }

                    htmlString += "<table width=\"100%\" class=\"marginBottom30\">";
                    htmlString += "<tr>";
                    
                    htmlString += "<tr>";
                    htmlString += "<h3>" + row.ContractorName + "</h3>";
                    htmlString += "</tr>";
                    htmlString += "<td><b>Agreement</b></td>";
                    htmlString += "<td><b>Contract Reference</b></td>";
                    htmlString += "<td><b>Service Number</b></td>";
                    htmlString += "<td><b>Route Start and End</b></td>";
                    htmlString += "<td align=\"center\"><b>Price per Day £</b></td>";
                    htmlString += "<td align=\"center\"><b>Number of Day</b></td>";
                    htmlString += "<td align=\"center\"><b>Contract Amount £</b></td>";
                    htmlString += "<td align=\"center\"><b>Variations</b></td>";
                    htmlString += "<td><b>Net Amount Due £</b></td>";
                    htmlString += "</tr>";
                    htmlString += "<tr/>";
                    lastContractor = row.ContractorName;
                }

                htmlString += "<tr>";
                htmlString += "<td>" + row.AgreementDescription + "</td>";
                htmlString += "<td>" + row.ContractReferenceSort + "</td>";
                htmlString += "<td>" + row.ServiceNo + "</td>";
                htmlString += "<td>" + row.RouteText + "</td>";
                htmlString += "<td align=\"center\">" + row.ContractPricePerDay + "</td>"; // Price per day - Contract price per day
                htmlString += "<td align=\"center\">" + "xxx" + "</td>"; // Calculate number of days operating between the selected dates
                htmlString += "<td align=\"center\">" + "xxx" + "</td>";
                htmlString += "<td align=\"center\">" + "" + "</td>";
                htmlString += "<td align=\"center\">" + "xxx" + "</td>";
                htmlString += "</tr>";

                //totalContractAmount += row.
                //totalVariations += row.
                //totalNetAmountDue += row.
            }

            // create final total row
            htmlString += "<tr>";
            htmlString += "<td></td>";
            htmlString += "<td></td>";
            htmlString += "<td></td>";
            htmlString += "<td></td>";
            htmlString += "<td></td>";
            htmlString += "<td>TOTALS</td>";
            htmlString += "<td>£999,999.99</td>"; // totalContractAmount
            htmlString += "<td></td>";          // Variations is a blank text area for the operator to add notes
            htmlString += "<td>£999,999.99</td>"; // totalNetAmountDue
            htmlString += "</tr>";

            htmlString += "</table>";

            RawHTML = new HtmlString(htmlString);
        }

    }

}
