using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProspectBusinessLayer
{

    #region ContractsPaymentsScheduleReportRow

    public class ContractsPaymentsScheduleReportRow
    {

        // Properties

        public string   ContractorName                      { get; set; }
        public string   AgreementDescription                { get; set; }
        public int      ContractReference                   { get; set; }
        public string   TenderReferenceSort                 { get; set; }
        public string   ServiceNo                           { get; set; }
        public string   RouteStart                          { get; set; }
        public string   RouteEnd                            { get; set; }
        public string   RouteText                           { get; set; }
        public string   DateStart                           { get; set; }
        public string   DateEnd                             { get; set; }
        public string   DateText                            { get; set; }
        public decimal  TenderPricePerDay                   { get; set; }
        public int      NoOfDaysOfOperationPerYear          { get; set; }
        public string   NoOfDaysOfOperationPerYearString    { get { return NoOfDaysOfOperationPerYear.ToString(); } }
        public decimal  ContractAmount                      { get; set; }
        public string   ContractAmountString                { get { return DCC_Formatting.ReportMoney(ContractAmount); } }
        public string   NetAmountDue                        { get; set; }
        public string   Variations                          { get; set; }
        public string   OrderString                         { get; set; }
        public decimal  ContractVariationAveragePricePerDay { get; set; }
        public decimal  ContractVariationFuelPriceElement   { get; set; }
        public string   ContractVariationClosingDate        { get; set; }
        public string   ContractorLicenceNo                 { get; set; }


        // Constructors

        public ContractsPaymentsScheduleReportRow()
        {
        }
       
    }

    #endregion


    #region ContractsPaymentsScheduleReportRowList

    public class ContractsPaymentsScheduleReportRowList : IEnumerable<ContractsPaymentsScheduleReportRow>
    {

        // Properties

        public List<ContractsPaymentsScheduleReportRow> All = new List<ContractsPaymentsScheduleReportRow>();

        // Constructors

        public ContractsPaymentsScheduleReportRowList(DCC_Database db, DateTime pStartDate, DateTime pEndDate)
        {
          
            decimal contractorSubTotal = 0;

            ContractVariationList contractVariationList = new ContractVariationList(db, pDateStart: pStartDate, pDateEnd: pEndDate);
            foreach (ContractVariation contractVariation in contractVariationList)

            {
                    Specification specification = new Specification();
                    specification.DateStart = pStartDate;
                    specification.DateEnd = pEndDate;

                    specification.OperatesOnMon                 = contractVariation.ContractVariationOperatesOnMon;
                    specification.OperatesOnTue                 = contractVariation.ContractVariationOperatesOnTue;
                    specification.OperatesOnWed                 = contractVariation.ContractVariationOperatesOnWed;
                    specification.OperatesOnThu                 = contractVariation.ContractVariationOperatesOnThu;
                    specification.OperatesOnFri                 = contractVariation.ContractVariationOperatesOnFri;
                    specification.OperatesOnSat                 = contractVariation.ContractVariationOperatesOnSat;
                    specification.OperatesOnSun                 = contractVariation.ContractVariationOperatesOnSun;
                    specification.OperatesOnSchoolDays          = contractVariation.ContractVariationOperatesOnSchoolDays;
                    specification.OperatesOnNonSchoolDays       = contractVariation.ContractVariationOperatesOnNonSchoolDays;
                    specification.OperatesOnBankHolidays        = contractVariation.ContractVariationOperatesOnBankHolidays;
                    specification.OperatesOnChristmasAndNewYear = contractVariation.ContractVariationOperatesOnChristmasAndNewYear;


                    if (!specification.OperatesOnMon &&
                        !specification.OperatesOnTue &&
                        !specification.OperatesOnWed &&
                        !specification.OperatesOnThu &&
                        !specification.OperatesOnFri &&
                        !specification.OperatesOnSat &&
                        !specification.OperatesOnSun &&
                        !specification.OperatesOnSchoolDays &&
                        !specification.OperatesOnNonSchoolDays &&
                        !specification.OperatesOnBankHolidays &&
                        !specification.OperatesOnChristmasAndNewYear)
                        {
                            specification.CalculatedNoOfDaysOfOperationFirstYear = specification.CalculatedNoOfDaysOfOperationFirstYear;
                        }

                specification.CalculateDaysOfOperation(db); 

                    

                    FuelCost originalPrice = new FuelCost(db, searchDate: contractVariation.ContractVariationDateEnd);
                    FuelCost newPrice = new FuelCost(db, searchDate: pStartDate);

                    int     daysInOperation = specification.CalculatedNoOfDaysOfOperationFirstYear;
                    decimal priceMinusFuel = 0;
                    decimal newFuelElement = 0;
                    if(originalPrice.Price > 0) { 
                    priceMinusFuel  = contractVariation.ContractVariationAveragePricePerDay - contractVariation.ContractVariationFuelPriceElement;
                    newFuelElement  = ((newPrice.Price / originalPrice.Price) * 100);
                        }
                    decimal total           = priceMinusFuel + newFuelElement;
                    decimal grandTotal      = total * daysInOperation;

                    ContractsPaymentsScheduleReportRow row = new ContractsPaymentsScheduleReportRow();

                    row.ContractorName                      = contractVariation.ContractorName + " - " + contractVariation.ContractorLicenceNo;
                    row.AgreementDescription                = contractVariation.AgreementDescription;
                    row.ContractReference                   = contractVariation.ContractReference;
                    row.TenderReferenceSort                 = contractVariation.TenderReferenceSort;
                    row.ServiceNo                           = contractVariation.ContractVariationServiceNo;
                    row.RouteStart                          = contractVariation.ContractVariationRouteStart;
                    row.RouteEnd                            = contractVariation.ContractVariationRouteEnd;
                    row.RouteText                           = contractVariation.ContractVariationRouteText;
                    row.DateStart                           = contractVariation.ContractVariationDateStartText;
                    row.DateEnd                             = contractVariation.ContractVariationDateEndText;
                    row.DateText                            = contractVariation.ContractVariationDatesText;
                    row.ContractVariationAveragePricePerDay = contractVariation.ContractVariationAveragePricePerDay;
                    row.ContractVariationFuelPriceElement   = contractVariation.ContractVariationFuelPriceElement;
                    row.ContractVariationClosingDate        = contractVariation.ContractVariationVariationDateStartText;
                    row.NoOfDaysOfOperationPerYear          = specification.CalculatedNoOfDaysOfOperationFirstYear;
                    row.ContractAmount                      = grandTotal;
                    contractorSubTotal                     += grandTotal;
                    //row.NetAmountDue                      =
                    //row.Variations                        =
                    row.OrderString                         = contractVariation.ContractorName + contractVariation.ContractReference;
                    Add(row);
                
            }

            //Create the total row
            //ContractsPaymentsScheduleReportRow totalRow = new ContractsPaymentsScheduleReportRow();
            //totalRow.ContractorName = "Total";
            //totalRow.ContractAmount = contractorSubTotal;
            //Add(totalRow);

            if (All != null)
            {
                All.Sort((x, y) =>
                {
                    // Null-coalescing operator is used to handle if OrderString is null.
                    string xOrderString = x?.OrderString ?? "";
                    string yOrderString = y?.OrderString ?? "";

                    return xOrderString.CompareTo(yOrderString);
                });
            }
            
            All.Sort((x, y) => x.OrderString.CompareTo(y.OrderString));
        }



        // Methods

        public void Add(ContractsPaymentsScheduleReportRow row)
        {
            All.Add(row);
        }



        // Enumerator

        public IEnumerator<ContractsPaymentsScheduleReportRow> GetEnumerator() { foreach (object o in All) yield return (ContractsPaymentsScheduleReportRow)o; }
        IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

    }

    #endregion

}
