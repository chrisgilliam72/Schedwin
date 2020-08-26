using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Reservations.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchedRes = Schedwin.Reservations.Classes;

namespace Schedwin.Reservations
{
    
    public class ReservationBudgetViewModel :ViewModelBase
    {
        public bool IsStaffBooking { get; set; }
        public String DefaultCurrencyCode { get; set; }
        public int PaxCount { get; set; }
        public  SchedRes.ReservationLeg Leg { get; set; }


        public List<PriceList> CompanyPriceList { get; set; }
        public DateTime Date { get; set; }

        private List<SchedRes.GridReservationBudget> _listBudgets;
        public RangeObservableCollection<SchedRes.GridReservationBudget> ListBudgets
        {

            get
            {
                var tmpLst = new RangeObservableCollection<Classes.GridReservationBudget>();
                tmpLst.AddRange(_listBudgets.Where(x => x.LegBudget.DBState != DBState.IsDeleted).ToList());

                return tmpLst;
            }
        }

        public List<String> RateTypes { get; set; }

        public List<AirstripInfo> AirportList { get; set; }

        public List<AircraftType> ACTypeList { get; set; }

        public List<Currency> CurrencyList { get; set; }

        public List<AirportFee> FeeList { get; set; }

        public SchedRes.GridReservationBudget SelectedBudget { get; set; }

        public ReservationBudgetViewModel()
        {
            _listBudgets = new List<Classes.GridReservationBudget>();
            FeeList = new List<AirportFee>();
            RateTypes = new List<String>();

            RateTypes.Add("Seat");

            var airportFeeTypes = AirportFeeType.GetAllAirportFeeTypes();
            foreach (var feeType in airportFeeTypes)
                RateTypes.Add(feeType.Description);

            IsStaffBooking = false;

            RateTypes.Add("Cancellation");
            RateTypes.Add("Cur. Area");
            RateTypes.Add("KM");
            RateTypes.Add("Service");
            RateTypes.Add("Quote");
            RateTypes.Add("Zambia Pax Levy");
        }

        public void Refresh()
        {
 
            NotifyPropertyChanged("ListBudgets");
        }

        public async Task<SchedRes.GridReservationBudget> NewBudgetLine(String RateType)
        {
           
            var LastBudget = ListBudgets.LastOrDefault();
            var newBudget = new SchedRes.GridReservationBudget();
            newBudget.LegBudget = new SchedRes.ReservationLegBudget();
            newBudget.Date = Date;
            newBudget.LegBudget.IDX_PriceList = 999;
            if (UseGlobalDB)
                newBudget.LegBudget.IDX_AC_Type = await AircraftType.GetNoneAircraftIDX();
            else
            newBudget.LegBudget.IDX_AC_Type = await AircraftType.GetNoneAircraftIDX(Server, Database);
            if (LastBudget!=null)
            {
                newBudget.LegBudget.IDX_From = LastBudget.LegBudget.IDX_From;
                newBudget.LegBudget.FromAP = LastBudget.LegBudget.FromAP;
                newBudget.LegBudget.IDX_To = LastBudget.LegBudget.IDX_To;
                newBudget.LegBudget.ToAP = LastBudget.LegBudget.ToAP;
                newBudget.LegBudget.Currency = LastBudget.LegBudget.Currency;
            }
            else
            {
                newBudget.LegBudget.IDX_From = Leg.IDX_FromAP;
                newBudget.LegBudget.FromAP = Leg.FromAP;
                newBudget.LegBudget.IDX_To = Leg.IDX_ToAP;
                newBudget.LegBudget.ToAP = Leg.ToAP;
                newBudget.LegBudget.Currency = DefaultCurrencyCode;
            }
            newBudget.RateType = RateType;
       

            _listBudgets.Add(newBudget);
            Leg.Budgets.Add(newBudget.LegBudget);
            NotifyPropertyChanged("ListBudgets");

            return newBudget;
        }

        public void DeleteBudgetLine()
        {
            SelectedBudget.LegBudget.DBState = DBState.IsDeleted;
            NotifyPropertyChanged("ListBudgets");
        }


        public void Cancel()
        {
            Leg.Budgets.Clear();
        }

        public async Task<String> Save()
        {
            if (UseGlobalDB)
                return await SaveGlobal();
            else
                return await SaveRegional();
        }

        public async Task<String> SaveRegional()
        {
            var reservations = new SchedRes.Reservations();
            var legBudgets = _listBudgets.Select(x => x.LegBudget).ToList();
            bool success = false;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                success= await reservations.UpdateReservationLegBudgets(Leg.IDX, legBudgets, Server, Database);
            }

            return success ? "" : reservations.LastError;
        }

        public async Task<String> SaveGlobal()
        {
            var reservations = new SchedRes.Reservations();
            var legBudgets = _listBudgets.Select(x => x.LegBudget).ToList();
            bool success = false;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                success = await reservations.UpdateReservationLegBudgets(Leg.IDX, legBudgets);
            }

            return success ? "" : reservations.LastError;
        }

        public async void Init()
        {
            if (UseGlobalDB)
                await InitGlobal();
            else
                await InitRegional();
        }
        public async Task InitGlobal()
        {
            FeeList.Clear();
            ListBudgets.Clear();
            Leg.Budgets.Clear();

            var tmpList = await AirportFee.LoadAirportFee(Leg.IDX_FromAP);
            if (tmpList != null)
                FeeList.AddRange(tmpList);
            tmpList = await AirportFee.LoadAirportFee(Leg.IDX_ToAP);
            if (tmpList != null)
                FeeList.AddRange(tmpList);

            var reservations = new SchedRes.Reservations();
            var legBudgets = await reservations.GetReservationLegBudgets(Leg.IDX, Server, Database);
            if (legBudgets != null)
            {
                Leg.Budgets.AddRange(legBudgets);
                var existingBudgets = legBudgets.Select(x => (SchedRes.GridReservationBudget)x).ToList();
                _listBudgets.AddRange(existingBudgets);
                _listBudgets.ForEach(x => x.Date = Date);
            }

            if (IsStaffBooking)
            {
                await AddSeatRate();
                //await AddLandingFees();
                //await AddNavFees();
            }

            await AddDepartureTax();



            SchedRes.GridReservationBudget.RateTypes = RateTypes;
            SchedRes.GridReservationBudget.AirportList = AirportList;
            SchedRes.GridReservationBudget.ACTypeList = ACTypeList;
            SchedRes.GridReservationBudget.CurrencyList = CurrencyList;
            SchedRes.GridReservationBudget.PaxCount = PaxCount;
            NotifyPropertyChanged("ListBudgets");


        }

        public async Task InitRegional()
        {
            FeeList.Clear();
            ListBudgets.Clear();
            Leg.Budgets.Clear();
          
            var tmpList= await AirportFee.LoadAirportFee(Leg.IDX_FromAP, Server, Database);
            if (tmpList!=null)
                FeeList.AddRange(tmpList);
            tmpList = await AirportFee.LoadAirportFee(Leg.IDX_ToAP, Server, Database);
            if (tmpList!=null)
                FeeList.AddRange(tmpList);

            var reservations = new SchedRes.Reservations();
            var legBudgets = await reservations.GetReservationLegBudgets(Leg.IDX, Server, Database);
            if (legBudgets != null)
            {
                Leg.Budgets.AddRange(legBudgets);
                var existingBudgets = legBudgets.Select(x => (SchedRes.GridReservationBudget)x).ToList();
                _listBudgets.AddRange(existingBudgets);
                _listBudgets.ForEach(x => x.Date = Date);
            }

            if (IsStaffBooking)
            {
                await AddSeatRate();
                //await AddLandingFees();
                //await AddNavFees();
            }

            await AddDepartureTax();
        


            SchedRes.GridReservationBudget.RateTypes = RateTypes;
            SchedRes.GridReservationBudget.AirportList = AirportList;
            SchedRes.GridReservationBudget.ACTypeList = ACTypeList;
            SchedRes.GridReservationBudget.CurrencyList = CurrencyList;
            SchedRes.GridReservationBudget.PaxCount = PaxCount;
            NotifyPropertyChanged("ListBudgets");


        }

        private async Task AddNavFees()
        {
            var navFeeTotal = FeeList.Where(x => x.FeeName == "Nav Fees").Sum(x => x.Amount);
            if (navFeeTotal > 0)
            {
                var navFeesBudgetItem = ListBudgets.FirstOrDefault(x => x.RateType == "Nav Fees");
                if (navFeesBudgetItem == null)
                    navFeesBudgetItem = await NewBudgetLine("Nav Fees");
                navFeesBudgetItem.Rate = navFeeTotal;
                navFeesBudgetItem.Budget = navFeeTotal;
            }
 
     
        }
        
        private async Task AddLandingFees()
        {

            //var landingFeeBudgetItem = ListBudgets.FirstOrDefault(x => x.RateType == "Landing");
            //if (landingFeeItem != null && landingFeeBudgetItem == null)
            //{
            //    var newBudgeLine = await NewBudgetLine("Landing");
            //    newBudgeLine.Qty = 1;
            //    newBudgeLine.Rate = landingFeeItem.Amount;
            //}

            foreach (var legSchedItem in Leg.Schedules)
            {
                var fee = FeeList.FirstOrDefault(x => x.IDX_Airport == legSchedItem.IDX_ToAP && x.IDX_Aircraft_Type == legSchedItem.IDX_AircraftType && x.FeeName.Contains("Landing"));
                if (fee!=null)
                {
                    var landingFeeBudgetItem = ListBudgets.FirstOrDefault(x => x.RateType == fee.FeeName);
                    if (landingFeeBudgetItem == null)
                    {
                        var newGridItem = await NewBudgetLine(fee.FeeName);
                        newGridItem.IDX_AC_Type = legSchedItem.IDX_AircraftType;
                        newGridItem.Rate = fee.Amount;
                        newGridItem.Qty = 1;
                        newGridItem.Budget = fee.Amount;
                        newGridItem.LegBudget.Currency = fee.Currency;
                    }
                }
            }
        }

        private async Task  AddSeatRate()
        {
            var seatRateType= ListBudgets.FirstOrDefault(x => x.RateType == "Seat"); 
            if (seatRateType==null)
            {
                var newBudgeLine = await NewBudgetLine("Seat");

                var priceLst = CompanyPriceList.FirstOrDefault(x => x.StartIDX == Leg.IDX_FromAP && x.DestIDX == Leg.IDX_ToAP);
                if (priceLst == null)
                    priceLst = CompanyPriceList.FirstOrDefault(x => x.StartIDX == Leg.IDX_ToAP && x.DestIDX == Leg.IDX_FromAP);
                if (priceLst != null)
                {
                    newBudgeLine.Qty = PaxCount;
                    newBudgeLine.Rate = priceLst.SellRate;
                }
            }
    
        }

        private async Task AddDepartureTax()
        {
            var feeItem = FeeList.FirstOrDefault(x => x.IDX_Airport == Leg.IDX_FromAP && x.FeeName == "Departure Tax" && x.Currency== DefaultCurrencyCode);
            var landingFeeBudgetItem = ListBudgets.FirstOrDefault(x => x.RateType == "Departure Tax");
             if (feeItem != null && landingFeeBudgetItem==null )
            {
                var newBudgeLine = await  NewBudgetLine(feeItem.FeeName);
                newBudgeLine.Qty = PaxCount;
                newBudgeLine.Rate = feeItem.Amount;
            }
        }
    }
}
