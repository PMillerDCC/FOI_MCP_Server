using DCC_Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProspectBusinessLayer
{

    #region ContractsPaymentsScheduleReportRow

    public class ContractsPaymentsScheduleReportRow
    {

        // Properties

        public string ContractorName { get; set; }
        public string AgreementDescription { get; set; }
        public int ContractReference { get; set; }
        public string TenderReferenceSort { get; set; }
        public string ServiceNo { get; set; }
        public string RouteStart { get; set; }
        public string RouteEnd { get; set; }
        public string RouteText { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string DateText { get; set; }
        public decimal TenderPricePerDay { get; set; }
        public int NoOfDaysOfOperationPerYear { get; set; }
        public string NoOfDaysOfOperationPerYearString { get { return NoOfDaysOfOperationPerYear.ToString(); } }
        public decimal ContractAmount { get; set; }
        public string ContractAmountString { get { return DCC_Formatting.ReportMoney(ContractAmount); } }
        public string NetAmountDue { get; set; }
        public string Variations { get; set; }
        public string OrderString { get; set; }
        public decimal ContractVariationAveragePricePerDay { get; set; }
        public decimal ContractVariationFuelPriceElement { get; set; }
        public string ContractVariationClosingDate { get; set; }
        public string ContractorLicenceNo { get; set; }


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

            // 1. Get all variations in date range
            DateTime sqlMin = new DateTime(1753, 1, 1);
            DateTime sqlMax = new DateTime(9999, 12, 31);

            System.Diagnostics.Debug.WriteLine($"SP CALL: pDateStart={sqlMin}, pDateEnd={sqlMax}");

            ContractVariationList contractVariationList = new ContractVariationList(db, pDateStart: sqlMin, pDateEnd: sqlMax);

            System.Diagnostics.Debug.WriteLine(
                $"Loaded variations: {contractVariationList.Count}, " + 
                $"Distinct contracts: {contractVariationList.Select(v => v.ContractReference).Distinct().Count()}");

            foreach (var v in contractVariationList) 
            { 
                System.Diagnostics.Debug.WriteLine(
                    $"VAR: Ref={v.ContractReference}, Tender={v.ContractVariation_Q_Tender}, " + 
                    $"Start={v.ContractVariationDateStart}, End={v.ContractVariationDateEnd}"); 
            }

            // Filter out orphaned variations
            var validVariations = contractVariationList
                .Where(v => v.ContractReference > 0 && 
                v.ContractVariation_Q_Tender > 0 && 
                !string.IsNullOrEmpty(v.ContractorName));

            foreach (var v in contractVariationList) 
            { 
                if (!(v.ContractReference > 0 && 
                    v.ContractVariation_Q_Tender > 0 && 
                    !string.IsNullOrEmpty(v.ContractorName))) 
                { 
                    System.Diagnostics.Debug.WriteLine(
                        $"Filtered OUT: Ref={v.ContractReference}, Tender={v.ContractVariation_Q_Tender}, Contractor={v.ContractorName}"); 
                } 
            }

            // 2. Group by contract (Tender)

            var grouped = validVariations.GroupBy(v => new
            {
                v.ContractReference, 
                v.ContractVariation_Q_Tender
            });

            foreach (var contractGroup in grouped)
            {
                var firstVar = contractGroup.First();

                DateTime contractStart = 
                    firstVar.ContractVariationDateStart
                    ?? firstVar.ContractVariationVariationDateStart
                    ?? sqlMin;

                DateTime contractEnd =
                    firstVar.ContractVariationDateEnd
                    ?? firstVar.ContractVariationVariationDateEnd
                    ?? sqlMax;

                // Skip contracts that do not overlap the report period
                if (contractEnd < pStartDate || contractStart > pEndDate)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Skipping Contract={contractGroup.Key.ContractReference}, " + 
                        $"contractStart={contractStart}, contractEnd={contractEnd}"); 
                    continue;
                }

                //For each contract, we need ALL its variations (not just those in the period)
                var allContractVariations = contractGroup 
                    .OrderBy(v => 
                    v.ContractVariationVariationDateStart
                    ?? v.ContractVariationDateStart
                    ?? sqlMin) 
                    .ToList(); 
                
                // 3. Build price segments based on variation start dates
                for (int i = 0; i < allContractVariations.Count; i++) 
                { 
                    var priceVar = allContractVariations[i]; 
                    
                    DateTime priceStart = 
                        priceVar.ContractVariationVariationDateStart
                        ?? priceVar.ContractVariationDateStart
                        ?? sqlMin;

                    DateTime priceEnd =
                        priceVar.ContractVariationVariationDateEnd
                        ?? (priceVar.ContractVariationNo > 0 ? sqlMax : priceVar.ContractVariationDateEnd)
                        ?? sqlMax;

                    // Segment start = later of report start or price variation start
                    DateTime segmentStart = (priceStart > pStartDate) 
                        ? priceStart 
                        : pStartDate;

                    // Determine when this price stops being effective
                    DateTime segmentEndCandidate; 
                    
                    if (i < allContractVariations.Count - 1) 
                    { 
                        // Next variation starts -> this price ends the day before
                        var nextVar = allContractVariations[i + 1]; 

                        DateTime nextStart = 
                            nextVar.ContractVariationVariationDateStart
                            ?? nextVar.ContractVariationDateStart
                            ?? sqlMax;
                        
                        segmentEndCandidate = nextStart.AddDays(-1); 
                    } 
                    else 
                    { 
                        // Last variation -> runs until report end
                        segmentEndCandidate = pEndDate; 
                    } 
                    
                    // Also respect the variation's own end date if present 
                    DateTime segmentEnd = segmentEndCandidate < priceEnd 
                        ? segmentEndCandidate 
                        : priceEnd;

                    System.Diagnostics.Debug.WriteLine(
                        $"SEGMENT RAW: Contract={contractGroup.Key.ContractReference}, " +
                        $"priceVarStart={priceStart:dd/MM/yyyy}, " +
                        $"segmentStart={segmentStart:dd/MM/yyyy}, " +
                        $"segmentEnd={segmentEnd:dd/MM/yyyy}");

                    // Clip to report period
                    if (segmentEnd > pEndDate) segmentEnd = pEndDate; 
                    
                    // If the segment doesn't overlap the report period, skip
                    if (segmentEnd < segmentStart) 
                        continue; 
                    
                    // 4. Find the operating-day variation as of segmentStart (carry-forward)
                    var opVar = allContractVariations 
                        .Where(v => 
                            (v.ContractVariationVariationDateStart ?? v.ContractVariationDateStart ?? sqlMin)
                            <= segmentStart)
                        .OrderByDescending(v => 
                        v.ContractVariationVariationDateStart ?? v.ContractVariationDateStart ?? sqlMin) 
                        .FirstOrDefault() ?? priceVar;

                    // Align segmentStart to the next operating day according to opVar
                    Specification tempSpec = new Specification 
                    { 
                        OperatesOnMon = opVar.ContractVariationOperatesOnMon, 
                        OperatesOnTue = opVar.ContractVariationOperatesOnTue, 
                        OperatesOnWed = opVar.ContractVariationOperatesOnWed, 
                        OperatesOnThu = opVar.ContractVariationOperatesOnThu, 
                        OperatesOnFri = opVar.ContractVariationOperatesOnFri, 
                        OperatesOnSat = opVar.ContractVariationOperatesOnSat, 
                        OperatesOnSun = opVar.ContractVariationOperatesOnSun 
                    }; 
                    
                    while (segmentStart <= segmentEnd) 
                    { 
                        bool operatesToday = 
                            (segmentStart.DayOfWeek == DayOfWeek.Monday    && tempSpec.OperatesOnMon) || 
                            (segmentStart.DayOfWeek == DayOfWeek.Tuesday   && tempSpec.OperatesOnTue) || 
                            (segmentStart.DayOfWeek == DayOfWeek.Wednesday && tempSpec.OperatesOnWed) || 
                            (segmentStart.DayOfWeek == DayOfWeek.Thursday  && tempSpec.OperatesOnThu) || 
                            (segmentStart.DayOfWeek == DayOfWeek.Friday    && tempSpec.OperatesOnFri) || 
                            (segmentStart.DayOfWeek == DayOfWeek.Saturday  && tempSpec.OperatesOnSat) || 
                            (segmentStart.DayOfWeek == DayOfWeek.Sunday    && tempSpec.OperatesOnSun); 
                        
                        if (operatesToday) 
                            break; 
                        
                        segmentStart = segmentStart.AddDays(1); 
                    }

                    System.Diagnostics.Debug.WriteLine(
                        $"SEGMENT ALIGNED: Contract={contractGroup.Key.ContractReference}, " + 
                        $"alignedStart={segmentStart:dd/MM/yyyy}, alignedEnd={segmentEnd:dd/MM/yyyy}, " + 
                        $"opVarStart={(opVar.ContractVariationVariationDateStart ?? opVar.ContractVariationDateStart):dd/MM/yyyy}, " + 
                        $"opVarRate={opVar.ContractVariationAveragePricePerDay}");


                    // If we’ve moved segmentStart past segmentEnd, this segment is now empty
                    if (segmentStart > segmentEnd) continue;

                    // 5. Build the row for this segment
                    var row = BuildRow(priceVar, opVar, db, segmentStart, segmentEnd); 
                    contractorSubTotal += row.ContractAmount; 
                    Add(row); 
                    
                } 
            }
                // Sort rows
                All.Sort((x, y) => x.OrderString.CompareTo(y.OrderString));
        }

        private string BuildOperatingDaysString(Specification spec)
        {
            var alldays = new[]
            {
                ("M", spec.OperatesOnMon),
                ("T", spec.OperatesOnTue),
                ("W", spec.OperatesOnWed),
                ("Th", spec.OperatesOnThu),
                ("F", spec.OperatesOnFri),
                ("Sa", spec.OperatesOnSat),
                ("Su", spec.OperatesOnSun)
            };

            var active = alldays.Where(d => d.Item2).Select(d => d.Item1).ToList();

            if (active.Count == 0)
                return "";

            if (active.Count == 1)
                return active[0];

            var sequences = new List<List<string>>();
            var current = new List<string> { active[0] };

            for (int i = 1; i < active.Count; i++)
            {
                int prevIndex = Array.FindIndex(alldays, d => d.Item1 == active[i - 1]);
                int currIndex = Array.FindIndex(alldays, d => d.Item1 == active[i]);

                if (currIndex == prevIndex + 1)
                {
                    current.Add(active[i]);
                }
                else
                {
                    sequences.Add(current);
                    current = new List<string> { active[i] };
                }
            }

            sequences.Add(current);

            var parts = sequences.Select(seq =>
            {
                if (seq.Count == 1)
                    return seq[0];
                else
                    return $"{seq.First()}-{seq.Last()}";
            });

            return string.Join(",", parts);

        }

        private Dictionary<DateTime, FuelCost> fuelCostCache = new Dictionary<DateTime, FuelCost>();
        private ContractsPaymentsScheduleReportRow BuildRow(

        ContractVariation priceVariation,
            ContractVariation opVariation,
            DCC_Database db,
            DateTime segmentStart,
            DateTime segmentEnd)

        {

            System.Diagnostics.Debug.WriteLine(
                $"[VAR] Ref={priceVariation.ContractReference}, " + 
                $"Tender={priceVariation.ContractVariation_Q_Tender}, " + 
                $"Avg={priceVariation.ContractVariationAveragePricePerDay}, " + 
                $"Fuel={priceVariation.ContractVariationFuelPriceElement}, " + 
                $"Seg={segmentStart:dd/MM/yyyy}-{segmentEnd:dd/MM/yyyy}, " + 
                $"Agreement={priceVariation.AgreementDescription}"
            );

            System.Diagnostics.Debug.WriteLine(
                $"[OP ] Ref={opVariation.ContractReference}, " + 
                $"Tender={opVariation.ContractVariation_Q_Tender}, " + 
                $"Avg={opVariation.ContractVariationAveragePricePerDay}, " + 
                $"Fuel={opVariation.ContractVariationFuelPriceElement}, " + 
                $"Start={opVariation.ContractVariationDateStart}, " + 
                $"End={opVariation.ContractVariationDateEnd}"
            );

            Specification spec = new Specification
            {
                DateStart                     = segmentStart,
                DateEnd                       = segmentEnd,
                OperatesOnMon                 = opVariation.ContractVariationOperatesOnMon,
                OperatesOnTue                 = opVariation.ContractVariationOperatesOnTue,
                OperatesOnWed                 = opVariation.ContractVariationOperatesOnWed,
                OperatesOnThu                 = opVariation.ContractVariationOperatesOnThu,
                OperatesOnFri                 = opVariation.ContractVariationOperatesOnFri,
                OperatesOnSat                 = opVariation.ContractVariationOperatesOnSat,
                OperatesOnSun                 = opVariation.ContractVariationOperatesOnSun,
                OperatesOnBankHolidays        = opVariation.ContractVariationOperatesOnBankHolidays,
                OperatesOnSchoolDays          = opVariation.ContractVariationOperatesOnSchoolDays,
                OperatesOnNonSchoolDays       = opVariation.ContractVariationOperatesOnNonSchoolDays,
                OperatesOnChristmasAndNewYear = opVariation.ContractVariationOperatesOnChristmasAndNewYear
            };

            int daysInOperation = spec.CalculateDaysOfOperationInPeriod(db, segmentStart, segmentEnd);

            FuelCost GetFuelCost(DateTime? date)
            {
                if (date == null)
                    return new FuelCost();

                DateTime key = date.Value.Date;

                if (!fuelCostCache.TryGetValue(key, out FuelCost fc))
                {
                    fc = new FuelCost(db, searchDate: key);
                    fuelCostCache[key] = fc;
                }

                return fc;
            }

            FuelCost originalPrice = GetFuelCost(
                priceVariation.ContractVariationVariationDateStart 
                ?? priceVariation.ContractVariationDateStart);
            
            FuelCost newPrice = GetFuelCost(segmentStart);

            decimal basePrice = priceVariation.ContractVariationAveragePricePerDay;
            decimal adjustedFuel = priceVariation.ContractVariationFuelPriceElement;

            decimal total;

            if (priceVariation.ContractVariationFuelPriceElement == 0
                || originalPrice.Price == 0
                || newPrice.Price == 0)
            {
                total = basePrice + adjustedFuel;
            }
            else
            {
                decimal fuelRatio      = newPrice.Price / originalPrice.Price;
                adjustedFuel   = priceVariation.ContractVariationFuelPriceElement * fuelRatio;
               
                total = basePrice + adjustedFuel;
            }

            decimal grandTotal = total * daysInOperation;

            ContractsPaymentsScheduleReportRow row = new ContractsPaymentsScheduleReportRow();

            row.ContractorName = priceVariation.ContractorName + " - " + priceVariation.ContractorLicenceNo;
            row.AgreementDescription = priceVariation.AgreementDescription;
            row.ContractReference = priceVariation.ContractReference;
            row.TenderReferenceSort = priceVariation.TenderReferenceSort;

            string daysString = BuildOperatingDaysString(spec);

            row.ServiceNo = string.IsNullOrWhiteSpace(daysString)
                ? priceVariation.ContractVariationServiceNo 
                : $"{priceVariation.ContractVariationServiceNo} {daysString}";

            row.RouteStart = priceVariation.ContractVariationRouteStart;
            row.RouteEnd = priceVariation.ContractVariationRouteEnd;
            row.RouteText = priceVariation.ContractVariationRouteText;

            // use the NEW variation date fields
            row.DateStart = segmentStart.ToShortDateString();
            row.DateEnd = segmentEnd.ToShortDateString();
            row.DateText = $"{segmentStart:dd/MM/yyyy} - {segmentEnd:dd/MM/yyyy}";

            row.ContractVariationAveragePricePerDay = basePrice;
            row.ContractVariationFuelPriceElement = adjustedFuel;
            row.ContractVariationClosingDate = priceVariation.ContractVariationVariationDateStartText;
            row.NoOfDaysOfOperationPerYear = daysInOperation;
            row.ContractAmount = grandTotal;

            row.OrderString = $"{priceVariation.ContractorName}-{priceVariation.ContractReference}";

            return row;


        }

        // Methods

        public void Add(ContractsPaymentsScheduleReportRow row)
        {
            All.Add(row);
        }



        // Enumerator

        public IEnumerator<ContractsPaymentsScheduleReportRow> GetEnumerator() 
        { 
            foreach (object o in All) yield return (ContractsPaymentsScheduleReportRow)o; 
        }
        IEnumerator IEnumerable.GetEnumerator() 
        { 
            foreach (object o in All) yield return "dummy"; 
        }

    }

    #endregion

}
