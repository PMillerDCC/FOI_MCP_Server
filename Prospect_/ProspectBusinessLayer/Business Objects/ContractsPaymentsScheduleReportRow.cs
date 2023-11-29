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

        public string  ContractorName             { get; set; }
        public string  AgreementDescription       { get; set; }
        public string  ContractReferenceSort      { get; set; }
        public string  TenderReferenceSort        { get; set; }
        public string  ServiceNo                  { get; set; }
        public string  RouteStart                 { get; set; }
        public string  RouteEnd                   { get; set; }
        public string  RouteText                  { get; set; }
        public string  DateStart                  { get; set; }
        public string  DateEnd                    { get; set; }
        public string  DateText                   { get; set; }
        public decimal TenderPricePerDay          { get; set; }
        public int     NoOfDaysOfOperationPerYear { get; set; }
        public string  ContractAmount             { get; set; }
        public string  NetAmountDue               { get; set; }
        public string  Variations                 { get; set; }
        public string  OrderString                { get; set; }
        public decimal ContractPricePerDay        { get; set; }



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
            SpecificationList specificationList = new SpecificationList(db, pStartDate: pStartDate, pEndDate: pEndDate);
            foreach (Specification specification in specificationList)
            {
                ContractsPaymentsScheduleReportRow row = new ContractsPaymentsScheduleReportRow();
                row.ContractorName = specification.ContractorName + " - " + specification.LicenceNo;
                row.AgreementDescription = specification.AgreementDescription;
                row.ContractReferenceSort = specification.ContractReferenceSort;
                row.TenderReferenceSort = specification.TenderReferenceSort;
                row.ServiceNo = specification.ServiceNo;
                row.RouteStart = specification.RouteStart;
                row.RouteEnd = specification.RouteEnd;
                row.RouteText = specification.RouteText;
                row.DateStart = specification.DateStartText;
                row.DateEnd = specification.DateEndText;
                row.DateText = specification.DatesText;
                row.TenderPricePerDay = specification.TenderPricePerDay;
                //row.NoOfDaysInOperation = specification.NoOfDaysInOperation;
                row.ContractPricePerDay = specification.ContractPricePerDay;
                //row.ContractAmount             = specification.ContractAmount;
                //row.NetAmountDue               =
                //row.Variations                 =
                row.OrderString = specification.ContractorName + specification.ContractReference;

                

                Add(row);
            }

            TenderSpecificationList tenderSpecificationList = new TenderSpecificationList(db, pStartDate: pStartDate, pEndDate: pEndDate);
            foreach (TenderSpecification tenderSpecification in tenderSpecificationList)
            {
                ContractsPaymentsScheduleReportRow row = new ContractsPaymentsScheduleReportRow();
                row.ContractorName = tenderSpecification.ContractorName + " - " + tenderSpecification.LicenceNo;
                row.AgreementDescription = tenderSpecification.AgreementDescription;
                row.ContractReferenceSort = tenderSpecification.ContractReferenceSort;
                row.TenderReferenceSort = tenderSpecification.TenderReferenceSort;
                row.ServiceNo = tenderSpecification.ServiceNo;
                row.RouteStart = tenderSpecification.RouteStart;
                row.RouteEnd = tenderSpecification.RouteEnd;
                row.RouteText = tenderSpecification.RouteText;
                row.DateStart = tenderSpecification.DateStartText;
                row.DateEnd = tenderSpecification.DateEndText;
                row.DateText = tenderSpecification.DatesText;
                //row.TenderPricePerDay          = 
                //row.NoOfDaysOfOperationPerYear =
                //row.ContractAmount             =
                //row.NetAmountDue               =
                //row.Variations                 =
                row.OrderString = tenderSpecification.ContractorName + tenderSpecification.ContractReference;
                Add(row);
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
