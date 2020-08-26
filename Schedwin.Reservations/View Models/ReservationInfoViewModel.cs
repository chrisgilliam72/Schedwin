using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchedRes = Schedwin.Reservations.Classes;
using Schedwin.Data.Classes;
using System.Collections.ObjectModel;
using Schedwin.Common;
namespace Schedwin.Reservations
{
    public class ReservationInfoViewModel : Schedwin.Common.ViewModelBase
    {
        private bool BusySaving { get; set; }
        public ReservationInfoView View { get; set; }
        public String DefaultCurrency { get; set; }

        public int CurrentUserID { get; set; }
        public SchedRes.Reservation Reservation { get; set; }

        public String ReservationName
        {
            get
            {
                return Reservation != null ? Reservation.Header.ReservationName : "";
            }
            set
            {
                Reservation.Header.ReservationName = value;
            }
        }

        public DateTime CaptureDate
        {
            get
            {

                return Reservation != null ? Reservation.Header.DateCaptured : DateTime.Today;
            }
            set
            {
                Reservation.Header.DateCaptured = value;
            }
        }

        public String SefofaneAgent
        {
            get
            {
                return Reservation != null ? Reservation.Header.SefofaneAgentName : "";
            }
            set

            {
                Reservation.Header.SefofaneAgentName = value;
            }
        }

        public int NoPax
        {
            get
            {
                return Reservation != null ? Reservation.Passengers.Count() : 0;
            }
        }

        public String Notes
        {
            get
            {
                return Reservation != null ? Reservation.Header.ReservationNote : "";
            }
            set
            {
                Reservation.Header.ReservationNote = value;
            }
        }

        public String Consultant
        {
            get
            {
                if (Reservation != null)
                {
                    if (String.IsNullOrEmpty(Reservation.Header.WishConsultant))
                    {
                        if (Reservation.Header.IDX_OperatorAgent.HasValue)
                        {
                            var agent = Agents.FirstOrDefault(x => x.Index == Reservation.Header.IDX_OperatorAgent.Value);
                            if (agent != null)
                                return agent.Description;
                            else
                                return "";
                        }
                        else
                            return "";

                    }
                    else
                        return Reservation.Header.WishConsultant;

                }
                else return "";
            }
            set
            {
                Reservation.Header.WishConsultant = value;
            }
        }

        public bool TicketRequired
        {
            get
            {
                return Reservation != null ? Reservation.Header.TicketRequired : false;
            }
            set
            {
                Reservation.Header.TicketRequired = value;
            }
        }
        public bool TicketPrinted
        {
            get
            {
                return Reservation != null ? Reservation.Header.TicketPrinted : false;
            }
            set
            {
                Reservation.Header.TicketPrinted = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return Reservation != null ? Reservation.Header.Active : false;
            }
            set
            {
                Reservation.Header.Active = value;
            }
        }
        
        public bool IsSplit
        {
            get
            {
                return Reservation != null ? Reservation.Header.IsSplit : false;

            }
            set
            {
                Reservation.Header.IsSplit = value;
            }
        }

        public bool IsMaster
        {
            get
            {
                return Reservation != null ? Reservation.Header.IsMaster : false;
            }
            set
            {
                Reservation.Header.IsMaster = value;
            }
        }

        public bool LegSelected
        {

            get
            {
                return _selectedLeg != null;
            }
        }

        public SchedRes.ReservationLookupObject SelectedOperator
        {
            get
            {
                return Operators.FirstOrDefault(x => x.Index == Reservation.Header.IDX_Operator);

            }
            set
            {
                Reservation.Header.IDX_Operator = value.Index;
                RefreshAgents(value.Index);
            }
        }
        public SchedRes.ReservationLookupObject SelectedCurrency
        {
            get
            {
                if (CurrenciesLst.Count > 0 && Reservation.Header.CurrencyID != null)
                    return Reservation != null ? CurrenciesLst.FirstOrDefault(x => x.Description == Reservation.Header.CurrencyID.TrimEnd(' ')) : CurrenciesLst[0];
                else
                    return null;

            }
            set
            {
                Reservation.Header.CurrencyID = value.Description.TrimEnd(' ');
            }
        }

        public SchedRes.ReservationLookupObject SelectedResType
        {
            get
            {
                if (ReservationTypes.Count > 0)
                    return Reservation != null ? ReservationTypes.FirstOrDefault(x => x.Index == Reservation.Header.IDX_ResType) : ReservationTypes[1];
                else
                    return null;
            }
            set
            {
                Reservation.Header.IDX_ResType = value.Index;
                CanAddPax = true;
                NotifyPropertyChanged("CanAddPax");
            }
        }


        public SchedRes.ReservationLookupObject SelectedReStatus
        {
            get
            {
                if (ReservationStatus.Count > 0)
                    return Reservation != null ? ReservationStatus.FirstOrDefault(x => x.Index == Reservation.Header.IDX_ResStatus) : ReservationStatus[0];
                else
                    return null;
            }
            set
            {
               foreach (var leg in Legs)
                {
                    if (value.Description == "Cancelled")
                        leg.Cancel();
                    else
                        leg.Restore();
                }
                

                
                Reservation.Header.IDX_ResStatus = value.Index;
                NotifyPropertyChanged("Legs");
            }
        }


        public SchedRes.ReservationLookupObject SelectedAgent
        {
            get
            {
                if (Agents.Count > 0)
                    return Agents.FirstOrDefault(x => x.Index == Reservation.Header.IDX_OperatorAgent);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    Reservation.Header.IDX_OperatorAgent = value.Index;
            }
        }
       
    
        public bool CanChangeDate { get; set; }

        public bool CanAddPax { get; set; }
        public List<AircraftType> ACTypes { get; set; }

        public List<Currency> CurrencyList { get; set; }

        public List<PriceList> CompanyPriceList { get; set; }

        public RangeObservableCollection<SchedRes.ReservationLookupObject> Operators { get; set; }
        public RangeObservableCollection<SchedRes.ReservationLookupObject> Agents { get; set; }

        public RangeObservableCollection<SchedRes.ReservationLookupObject> CurrenciesLst { get; set; }

        public RangeObservableCollection<SchedRes.ReservationLookupObject> ReservationTypes { get; set; }

        public RangeObservableCollection<SchedRes.ReservationLookupObject> ReservationStatus { get; set; }

        private RangeObservableCollection<AirstripInfo> _airportLst;
        public RangeObservableCollection<AirstripInfo> AirportList
        {
            get
            {
                return _airportLst;
            }
            set
            {
                _airportLst = value;
            }
        }



        public RangeObservableCollection<SchedRes.GridReservationLeg> Legs
        {
            get
            {
                var tmpCollection = new RangeObservableCollection<SchedRes.GridReservationLeg>();
                if (Reservation != null)
                {
                    var gridLegLst = Reservation.Legs.Select(x => (SchedRes.GridReservationLeg)x).ToList();
                    tmpCollection.AddRange(gridLegLst.OrderBy(x => x.Leg.SEQNo).ToList());
                }

                return tmpCollection;
            }
            set
            {
                Reservation.Legs = value.Select(x => x.Leg).ToList();
            }
        }

        public RangeObservableCollection<SchedRes.ReservationPax> Passengers

        {
            get
            {
                var tmpCollection = new RangeObservableCollection<SchedRes.ReservationPax>();
                if (Reservation != null)
                    tmpCollection.AddRange(Reservation.Passengers);
                return tmpCollection;
            }
            set
            {
                Reservation.Passengers = value.ToList();
            }
        }

        private SchedRes.GridReservationLeg _selectedLeg;
        public SchedRes.GridReservationLeg SelectedLeg
        {
            get
            {
                return _selectedLeg;
            }
            set
            {
                _selectedLeg = value;

                if (_selectedLeg != null)
                {
                    if (_selectedLeg.Leg.IsNew())
                        _selectedLeg.IsReadOnly = false;
                    else
                    {
                        if (!UseGlobalDB)
                            _selectedLeg.UpdateReadOnlyStatus(_selectedLeg.Leg.BookingDate, Server, Database);
                    }
                                   

                }

            }
        }

        public ObservableCollection<SchedRes.ReservationPax> SelectedPax { get; set; }


        public ReservationInfoViewModel()
        {
            BusySaving = false;
            Operators = new RangeObservableCollection<Classes.ReservationLookupObject>();
            Agents = new RangeObservableCollection<Classes.ReservationLookupObject>();
            CurrenciesLst = new RangeObservableCollection<Classes.ReservationLookupObject>();
            ReservationTypes = new RangeObservableCollection<Classes.ReservationLookupObject>();
            ReservationStatus = new RangeObservableCollection<SchedRes.ReservationLookupObject>();
            AirportList = new RangeObservableCollection<AirstripInfo>();
            CompanyPriceList = new List<PriceList>();


        }

        public bool ValidateData()
        {
            if (String.IsNullOrEmpty(ReservationName))
            {
                FailWindow.Display("Please enter reservation name");
                return false;
            }

            if (SelectedOperator == null)
            {
                FailWindow.Display("Please select a valid agent");
                return false;
            }


            if (SelectedCurrency == null)
            {
                FailWindow.Display("Please select a valid currency");
                return false;
            }

            if (SelectedResType == null)
            {
                FailWindow.Display("Please select a valid reservation type");
                return false;
            }

            if (SelectedReStatus == null)
            {
                FailWindow.Display("Please select a valid reservation status");
                return false;
            }



            //if (Legs == null || Legs.Count < 1)
            //{
            //    FailWindow.Display("Please add at least one leg to this reservation.");
            //    return false;
            //}

            return true;


        }


        public void SelectWishConsultant()
        {
            var wishUsers = WishUsers.GetWishUsers();
            var selectLst = new SelectItemWindow();
            selectLst.Init("Select consultant", wishUsers.Select(x => new ListboxItem { ID = x.IDX, Description = x.FullName }).ToList());
            selectLst.ShowDialog();
            if (selectLst.DialogResult.HasValue && selectLst.DialogResult.Value)
            {
                if (selectLst.SelectedItem != null)
                {
                    Reservation.Header.IDX_OperatorAgent = selectLst.SelectedItem.ID;
                    Consultant = selectLst.SelectedItem.Description;
                    NotifyPropertyChanged("Consultant");
                }

            }
        }

        public void SelectConsultant()
        {
            var selectLst = new SelectItemWindow();
            selectLst.Init("Select consultant", Agents.Select(x => new ListboxItem { ID = x.Index, Description = x.Description }).ToList());
            selectLst.ShowDialog();
            if (selectLst.DialogResult.HasValue &&  selectLst.DialogResult.Value)
            {
                if (selectLst.SelectedItem != null)
                {
                    Reservation.Header.IDX_OperatorAgent = selectLst.SelectedItem.ID;
                    Consultant = selectLst.SelectedItem.Description;
                    NotifyPropertyChanged("Consultant");
                }

            }
        }

        public void AddNewPax()
        {
            var addPaxView = new DefaultPassengersView();
            addPaxView.Owner = View;
            var addPaxViewModel = addPaxView.DataContext as DefaultPassengersViewModel;
            var currentPaxCount = Passengers.Count;

            if (SelectedResType != null)

                addPaxViewModel.CountryID = GetCountryIDFromDB(Database);
            {
                addPaxView.ShowDialog();

                if (addPaxView.DialogResult.HasValue && addPaxView.DialogResult.Value)
                {
                    int nPaxToAdd = addPaxViewModel.NoPax;
                    for (int i = 0; i < nPaxToAdd; i++)
                    {
                        var newPax = new SchedRes.ReservationPax();
                        newPax.FirstName = "Passenger " + Convert.ToInt32(currentPaxCount + i);
                        newPax.Surname = ReservationName;
                        newPax.Age = addPaxViewModel.DefaultAge;
                        newPax.Sex = addPaxViewModel.SelectedGender.Substring(0, 1);
                        newPax.Weight = addPaxViewModel.DefaultWeight;
                        newPax.IDX_PaxType = SelectedResType.Index;
                        newPax.TicketPrinted = false;
                        newPax.WishGuestID = 0;
                        newPax.LuggageWeight = addPaxViewModel.DefaultLuggageWeight;
                        Reservation.Passengers.Add(newPax);
                    }
                }

                NotifyPropertyChanged("NoPAx");
                NotifyPropertyChanged("Passengers");
            }
        }

        public void RemoveSelectedPax(List<SchedRes.ReservationPax> ListPax)
        {
            foreach (var pax in ListPax)
            {
                Reservation.RemovePassenger(pax);
            }

           
            NotifyPropertyChanged("NoPAx");
            NotifyPropertyChanged("Passengers");
        }


        public async Task<bool> RefreshPax()
        {
            var reservations = new SchedRes.Reservations();
            SchedRes.Reservation tmpReservation;

            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                tmpReservation = await reservations.WIRefreshPassengersFromWish(this.Reservation, Server, Database);
            }
            if (tmpReservation != null)
            {
                Reservation = tmpReservation;

                NotifyPropertyChanged("NoPAx");
                NotifyPropertyChanged("Passengers");
                return true;
            }
           else
            {
                FailWindow.Display("Unable to refresh guests from Wish:" + Environment.NewLine + reservations.LastError);
                return false;
            }
          
        }


        public async Task<SchedRes.Reservation> SplitPax(List<SchedRes.ReservationPax> ListPax)
        {
            int resHDRID = this.Reservation.Header.Res_IDX;
            List<int> paxIDS = ListPax.Select(x => x.IDX).ToList();
            var newReservation= await SchedRes.Reservations.SplitReservation(resHDRID, paxIDS, Server, Database);
            return newReservation;

        }

        public void AddNewLeg()
        {
            var newLeg = new SchedRes.ReservationLeg();
            
            if (Legs.Count > 0)
            {
                if (SelectedLeg!=null)
                {
                    int selIndex = Reservation.Legs.IndexOf(SelectedLeg.Leg);
                    newLeg.BookingDate = SelectedLeg.Leg.BookingDate;
                    Reservation.Legs.Insert(selIndex+1, newLeg);
                }
                else
                {                 
                    newLeg.BookingDate = Legs.Max(x => x.Leg.BookingDate).AddDays(1);
                    Reservation.Legs.Add(newLeg);
                }
              
            }
                
            else
            {
                newLeg.BookingDate = DateTime.Today;
                Reservation.Legs.Add(newLeg);
            }




            UpdateLegSeqNos();
            NotifyPropertyChanged("Legs");

        }

        public void RemoveLeg()
        {
            if (SelectedLeg!=null)
            {
                SelectedLeg.Remove();
                Reservation.Legs.Remove(SelectedLeg.Leg);
                NotifyPropertyChanged("Legs");
            }
          
        }

        public void CancelLeg()
        {
            if (SelectedLeg != null)
            {
                SelectedLeg.Cancel();
                NotifyPropertyChanged("Legs");
            }
              
 
        }

        public void LegRowChangeComitted()
        {   // await AddNavFees();
            if (SelectedLeg!=null)
            {
                int distance = APDistances.GetDistance(SelectedLeg.IDX_FromAP, SelectedLeg.IDX_ToAP);
                SelectedLeg.Distance = distance;
            }
        }

        public void ViewBudgetInfo()
        {
            if (SelectedLeg!=null)
            {
                var resBudgetInfoView = new ReservationBudgetView();
                resBudgetInfoView.Owner = View;
                var resBudgetInfoViewModel = resBudgetInfoView.DataContext as ReservationBudgetViewModel;
                resBudgetInfoViewModel.Leg = SelectedLeg.Leg;
                resBudgetInfoViewModel.Date = SelectedLeg.Leg.BookingDate;
                resBudgetInfoViewModel.Server = Server;
                resBudgetInfoViewModel.Database = Database;
                resBudgetInfoViewModel.AirportList = AirportList.ToList();
                resBudgetInfoViewModel.ACTypeList = ACTypes;
                resBudgetInfoViewModel.CurrencyList = CurrencyList;
                resBudgetInfoViewModel.CompanyPriceList = CompanyPriceList;
                if (SelectedCurrency!=null)
                    resBudgetInfoViewModel.DefaultCurrencyCode = SelectedCurrency.Description;
                resBudgetInfoViewModel.PaxCount = Passengers.Count();
                resBudgetInfoViewModel.IsStaffBooking = true ? (SelectedResType.Description == "Staff"|| SelectedResType.Description=="Freight") : false;
                resBudgetInfoViewModel.Init();

                resBudgetInfoView.ShowDialog();
            }

        }
        public async Task Init()
        {
            if (UseGlobalDB)
                await InitGlobal();
            else
               await  InitRegional();
        }

        public async Task InitGlobal()
        {
            await FillCombos();

            //var reservations = new SchedRes.Reservations();
            //Reservation = await reservations.LoadReservation(10156131, Server, Database);
            if (Reservation == null)
            {
                Reservation = new SchedRes.Reservation();
                Reservation.Header.DateCaptured = DateTime.Today;
                Reservation.Header.IDX_Personnel = CurrentUserID;
                Reservation.Header.Active = true;
                var defCurrency = await Currency.GetDefaultCurrency(RegionName);
                Reservation.Header.CurrencyID = defCurrency.Code;
                DefaultCurrency = defCurrency.Code;
                var agentDtls = OperatorAgent.GetOperatorAgentDetails(CurrentUserID);
                if (agentDtls != null)
                    Reservation.Header.SefofaneAgentName = agentDtls.Description;
                CanChangeDate = true;
            }
            else
            {
                CanAddPax = true;
                CanChangeDate = false;
                RefreshAgents(Reservation.Header.IDX_Operator);
                Reservation.Legs = Reservation.Legs.OrderBy(x => x.SEQNo).ThenBy(x => x.BookingDate).ThenBy(x => x.EarliestEx).ToList();
                UpdateLegSeqNos();
            }


            NotifyPropertyChanged("Consultant");
            NotifyPropertyChanged("Legs");
            NotifyPropertyChanged("Passengers");
            NotifyPropertyChanged("NoPAx");
            NotifyPropertyChanged("TicketRequired");
            NotifyPropertyChanged("TicketPrinted");
            NotifyPropertyChanged("IsActive");
            NotifyPropertyChanged("SefofaneAgent");
            NotifyPropertyChanged("Notes");
            NotifyPropertyChanged("SelectedOperator");
            NotifyPropertyChanged("SelectedAgent");
            NotifyPropertyChanged("SelectedReStatus");
            NotifyPropertyChanged("SelectedResType");
            NotifyPropertyChanged("SelectedCurrency");
            NotifyPropertyChanged("ReservationName");
            NotifyPropertyChanged("CaptureDate");
            NotifyPropertyChanged("CanAddPax");
            NotifyPropertyChanged("CanChangeDate");
            NotifyPropertyChanged("IsMaster");
            NotifyPropertyChanged("IsSplit");
        }

        public async Task InitRegional()
        {
            await FillCombos();

            //var reservations = new SchedRes.Reservations();
            //Reservation = await reservations.LoadReservation(10156131, Server, Database);
            if (Reservation == null)
            {
                Reservation = new SchedRes.Reservation();
                Reservation.Header.DateCaptured = DateTime.Today;
                Reservation.Header.IDX_Personnel = CurrentUserID;
                Reservation.Header.Active = true;
                var defCurrency  = await Currency.GetDefaultCurrency(Server, Database);
                Reservation.Header.CurrencyID = defCurrency.Code;
                DefaultCurrency = defCurrency.Code;
                var agentDtls = OperatorAgent.GetOperatorAgentDetails(CurrentUserID);
                if (agentDtls != null)
                    Reservation.Header.SefofaneAgentName = agentDtls.Description;
                CanChangeDate = true;
            }
            else
            {
                CanAddPax = true;
                CanChangeDate = false;
                RefreshAgents(Reservation.Header.IDX_Operator);
                Reservation.Legs= Reservation.Legs.OrderBy(x=>x.SEQNo).ThenBy(x=>x.BookingDate).ThenBy(x=>x.EarliestEx).ToList();
                UpdateLegSeqNos();
            }


            NotifyPropertyChanged("Consultant");
            NotifyPropertyChanged("Legs");
            NotifyPropertyChanged("Passengers");
            NotifyPropertyChanged("NoPAx");
            NotifyPropertyChanged("TicketRequired");
            NotifyPropertyChanged("TicketPrinted");
            NotifyPropertyChanged("IsActive");
            NotifyPropertyChanged("SefofaneAgent");
            NotifyPropertyChanged("Notes");
            NotifyPropertyChanged("SelectedOperator");
            NotifyPropertyChanged("SelectedAgent");
            NotifyPropertyChanged("SelectedReStatus");
            NotifyPropertyChanged("SelectedResType");
            NotifyPropertyChanged("SelectedCurrency");
            NotifyPropertyChanged("ReservationName");
            NotifyPropertyChanged("CaptureDate");
            NotifyPropertyChanged("CanAddPax");
            NotifyPropertyChanged("CanChangeDate");
            NotifyPropertyChanged("IsMaster");
            NotifyPropertyChanged("IsSplit");
        }


        public bool UpdateLegDistance()
        {
            if (SelectedLeg!=null)
            {
                int distance = APDistances.GetDistance(SelectedLeg.IDX_FromAP, SelectedLeg.IDX_ToAP);
                if (distance > -1)
                    SelectedLeg.Distance = distance;
                else
                    return false;
            }

            return true;
        }


        public async Task<bool> Save()
        {
            var retVal = false;
            if (UseGlobalDB)
                retVal = await SaveGlobal();
            else
                retVal = await SaveRegional();
            return retVal;
        }

        public async Task<bool> SaveGlobal()
        {
            try
            {
                if (!BusySaving)
                {
                    if (ValidateData())
                    {
                        BusySaving = true;
                        var reservations = new SchedRes.Reservations();
                        using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                        {
                            Reservation.Header.PaxCount = Reservation.Passengers.Count();
                            Reservation.Header.IDX_Country = 1;
                            var savedReservation = await reservations.SaveReservation(Reservation);
                            foreach (var savedleg in savedReservation.Legs)
                            {
                                var frmLeg = Reservation.Legs.FirstOrDefault(x => x.SEQNo == savedleg.SEQNo);
                                if (frmLeg != null)
                                    frmLeg.IDX = savedleg.IDX;
                            }
                            Reservation.Passengers.Clear();
                            Reservation.Passengers.AddRange(savedReservation.Passengers);
                        }
                        BusySaving = false;
                        return true;
                    }
                }

                return false;
            }

            catch (Exception ex)
            {
                BusySaving = false; ;
                FailWindow.Display("Failed to save reservation:\r\n" + ex.Message);
                return false;
            }
        }

        public async Task<bool> SaveRegional()
        {
            try
            {
                if (!BusySaving)
                {
                    if (ValidateData())
                    {
                        BusySaving = true;
                        var reservations = new SchedRes.Reservations();
                        using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                        {
                            Reservation.Header.PaxCount = Reservation.Passengers.Count();
                            if (Reservation.Header.ReservationStatus == "Cancelled")
                            {

                            }
                            var savedReservation = await reservations.SaveReservation(Reservation, Server, Database);
                            foreach (var savedleg in savedReservation.Legs)
                            {
                                var frmLeg = Reservation.Legs.FirstOrDefault(x => x.SEQNo == savedleg.SEQNo);
                                if (frmLeg != null)
                                    frmLeg.IDX = savedleg.IDX;
                            }
                            Reservation.Passengers.Clear();
                            Reservation.Passengers.AddRange(savedReservation.Passengers);
                        }
                        BusySaving = false; 
                        return true;
                    }
                } 
             
                return false;
            }

            catch (Exception ex)
            {
                BusySaving = false; ;
                FailWindow.Display("Failed to save reservation:\r\n" + ex.Message);
                return false;
            }
        }

        public static async Task ForceLookupLoad()
        {
            var operatorsTsk = Company.LoadCompanyList(false);
            var currenciesTsk = Currency.LoadCurrencies();
            var resStatusesTsk = ReservationTypeStatus.GetReservationStatusList();
            var resTypesTsk =  ReservationTypeStatus.GetReservationTypeList();
            var ACTypesTsk = AircraftType.LoadACTypes(false);
            var stndWeightsTask = StandardPassengerWeights.LoadStandardWeights();
            var airportFeeTypesTask = AirportFeeType.LoadAllAirportFeeTypes();
            await Task.WhenAll(stndWeightsTask, ACTypesTsk, operatorsTsk, currenciesTsk, resStatusesTsk, resTypesTsk, airportFeeTypesTask);
            AircraftType.IDX_AC_NONE = await AircraftType.GetNoneAircraftIDX();
        }

        public static async Task ForceLookupLoad(String Server, String Database)
        {
            var operatorsTsk = OperatorAgent.GetOperatorAgents(Server, Database);
            var currenciesTsk = Currency.GetGPCurrencyList(Server, Database);
            var resStatusesTsk = ReservationTypeStatus.GetReservationStatusList(Server, Database);
            var resTypesTsk = ReservationTypeStatus.GetReservationTypeList(Server, Database);
            var ACTypesTsk = AircraftType.LoadACTypes(Server, Database,false);
            var stndWeightsTask = StandardPassengerWeights.LoadStandardWeights(Server, Database);
            var airportFeeTypesTask = AirportFeeType.LoadAllAirportFeeTypes(Server, Database);
            await Task.WhenAll(stndWeightsTask, ACTypesTsk, operatorsTsk, currenciesTsk, resStatusesTsk, resTypesTsk, airportFeeTypesTask);
            AircraftType.IDX_AC_NONE= await AircraftType.GetNoneAircraftIDX(Server, Database);
        }

        public void CreateLegBudget(SchedRes.ReservationLeg leg)
        {
            if (leg != null)
            {
                leg.Budgets.Clear();

                var fromIDX = leg.IDX_FromAP;
                var toIDX = leg.IDX_ToAP;
                var priceLst = CompanyPriceList.FirstOrDefault(x => x.StartIDX == fromIDX && x.DestIDX == toIDX);

                var legbudget = new SchedRes.ReservationLegBudget();
                if  (SelectedCurrency!=null)
                    legbudget.Currency = SelectedCurrency.Description;
                legbudget.Qty = Reservation.Passengers.Count();
                legbudget.RateType = "Seat";
                legbudget.IDX_From = fromIDX;
                legbudget.FromAP = SelectedLeg.FromAP;
                legbudget.IDX_To = toIDX;
                legbudget.ToAP = SelectedLeg.ToAP;
                legbudget.Rate = priceLst.SellRate;
                legbudget.IDX_PriceList = 999;
                legbudget.IDX_AC_Type = AircraftType.IDX_AC_NONE;
                legbudget.Budget = legbudget.Rate * legbudget.Qty;
                leg.Budgets.Add(legbudget);

            }
        }

        public void LegTOSelectionChanged()
        {
            UpdateLegDistance();
            //CreateLegBudget();
        }

        public async void OperatorSelectionChanged()
        {
            if (SelectedOperator!=null)
            {
                CompanyPriceList.Clear();
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    try
                    {
                        if (!UseGlobalDB)
                        {
                            var priceList = await PriceList.GetSeatRatePriceList(this.SelectedOperator.Index, Server, Database);
                            if (priceList != null)
                                CompanyPriceList.AddRange(priceList);
                        }

                    }
                    catch(Exception ex)
                    {
                        var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                        var message = string.Join(Environment.NewLine, messages);
                        FailWindow.Display("Failed to load company price list :" + Environment.NewLine + message);
                    }
                }
            }

        }

        private void UpdateLegSeqNos()
        {
            int seqNo = 1;
            
            foreach (var leg in Reservation.Legs)
            {
                leg.SEQNo = seqNo++;
            }

           
        }

        private async Task FillCombos ()

        {
            Operators.Clear();
            Agents.Clear();
            CurrenciesLst.Clear();
            ReservationTypes.Clear();
            ReservationStatus.Clear();

            try
            {
                List <Company> operators = null;
                List<Currency> currencies = null;
                List<ReservationStatus> resStatuses = null;
                List<ReservationType> resTypes = null;
                List<AirstripInfo>  airStrips = AirstripInfo.GetAirstrips();
                List<AirStripExFor> exforLsts = null;

                if (UseGlobalDB)
                {
                    operators = await Company.LoadCompanyList(false);
                    currencies = await Currency.LoadCurrencies();
                    resStatuses = await ReservationTypeStatus.GetReservationStatusList();
                    resTypes = await ReservationTypeStatus.GetReservationTypeList();
                    exforLsts = await AirStripExFor.LoadExForList();
                    ACTypes = await AircraftType.LoadACTypes(false);
                }
                else
                {
                    operators = await Company.LoadCompanyList(Server, Database, false);
                    currencies = await Currency.GetGPCurrencyList(Server, Database);
                    resStatuses = await ReservationTypeStatus.GetReservationStatusList(Server, Database);
                    resTypes = await ReservationTypeStatus.GetReservationTypeList(Server, Database);
                    exforLsts = await CampExForAirstrips.GetExForListV2(Server, Database);
                    ACTypes = await AircraftType.LoadACTypes(Server, Database, false);
                }


                CurrencyList = currencies;

                _airportLst.AddRange(airStrips);

                SchedRes.GridReservationLeg.AirportList = airStrips.OrderBy(x => x.DisplayString).ToList();

                SchedRes.GridReservationLeg.AllExForList = exforLsts;

                var tmpOpsList = operators.Select(x => new SchedRes.ReservationLookupObject { Index = x.IDX, Description = x.Description }).ToList();
                Operators.AddRange(tmpOpsList);

                var tmpCurLst = currencies.Select(x => new SchedRes.ReservationLookupObject { Index = x.IDX, Description = x.Code.TrimEnd(' ') }).OrderBy(x => x.Description).ToList();
                CurrenciesLst.AddRange(tmpCurLst);

                var tmpstatLst = resStatuses.Select(x => new SchedRes.ReservationLookupObject { Index = x.IDX, Description = x.Description }).OrderBy(x => x.Description).ToList();
                ReservationStatus.AddRange(tmpstatLst);


                var tmpTypeLst = resTypes.Select(x => new SchedRes.ReservationLookupObject { Index = x.IDX, Description = x.Description }).OrderBy(x => x.Description).ToList();
                ReservationTypes.AddRange(tmpTypeLst);
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to load combo cache data :" + Environment.NewLine + message);
            }
  
 

            NotifyPropertyChanged("AirportList");
            NotifyPropertyChanged("Currencies");
            NotifyPropertyChanged("ReservationTypes");
            NotifyPropertyChanged("ReservationStatus");
            NotifyPropertyChanged("Operators");
        }

        private async void RefreshAgents(int idxOperator)
        {
            List<SchedRes.ReservationLookupObject> tmpAgntList = null;
            Agents.Clear();
            if (UseGlobalDB)
            {
                var agents = await OperatorAgent.GetAgentsForOperator(idxOperator);
                tmpAgntList = agents.Select(x => new SchedRes.ReservationLookupObject { Index = x.IDX, Description = x.Description }).ToList();
            }
            else
            {
                var agents = await OperatorAgent.GetAgentsForOperator(idxOperator, Server, Database);
                tmpAgntList=agents.Select(x => new SchedRes.ReservationLookupObject { Index = x.IDX, Description = x.Description }).ToList();
            }
            tmpAgntList = tmpAgntList.GroupBy(x => x.Index).Select(x => x.First()).OrderBy(x => x.Description).ToList();
            Agents.AddRange(tmpAgntList);
            
        }

        private int GetCountryIDFromDB(String regionalDBName)
        {
            int countryID = 0;

            switch (regionalDBName)
            {
                case "Sefofane_Bots": countryID = 1; break;
                case "Sefofane_Nam": countryID = 7; break;
                case "Sefofane_Zim": countryID = 3; break;
                default: countryID = 3; break;

            }

            return countryID;
        }

    
    }
}
