using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedwin.Setup
{
    public class AirstripInfoCntrlViewModel : ItemCntrlViewModelBase
    {
        public String Region { get; set; }
        public bool IsActive { get; set; }

        public String Name { get; set; }
        public String ICAO { get; set; }
        public String IATA { get; set; }
        public String TPCode { get; set; }
        public String AreaCode { get; set; }

        public Currency SelectedCurrency { get; set; }

        public String Longitude { get; set; }

        public String Latitude { get; set; }

        public short Altitude { get; set; }

        public float RunwayHeading { get; set; }

        public short RunwayLength { get; set; }
        public float SurfaceFactor { get; set; }

        public String AlternateAirportCode { get; set; }

        public short TurnaroundTime { get; set; }

        public int AlternateDist { get; set; }

        public float OvernightFee { get; set; }

        public float TasPermitFee { get; set; }

        public float DomesticDepartureTax { get; set; }

        public double InternationalDepartureTax { get; set; }

        public bool CustomsPoint { get; set; }
        public bool FuelPoint { get; set; }

        public bool PilotNightStop { get; set; }

        public bool IsHeliport { get; set; }

        public int PilotNightStopRating { get; set; }

        public DateTime OpeningTime { get; set; }

        public DateTime ClosingTime { get; set; }

        public double DecimalLong { get; set; }

        public double DecimalLat { get; set; }

        public AirportFee SelectedFee { get; set; }
        public AirStripExForGridItem SelectedGridItem { get; set; }
        public AirstripInfo AlternateAirstrip { get; set; }

        public AirportFuel SelectedFuel { get; set; }

        public RangeObservableCollection<AirportFuel> FuelList { get; set; }
        public RangeObservableCollection<AircraftType> ACTypes { get; set; }
        public RangeObservableCollection<Currency> Currencies { get; set; }
        public RangeObservableCollection<AirstripInfo> Airstrips { get; set; }
        public RangeObservableCollection<String> ExForType { get; set; }

        public RangeObservableCollection<ACAirportLimits> AirstripLimits { get; set; }

        public RangeObservableCollection<AirStripExForGridItem> AirstripExForList { get; set; }

        public RangeObservableCollection<AirportFeeType> AirportFeeTypeList { get; set; }

        public RangeObservableCollection<AirportFee> FeesList { get; set; }

        public RangeObservableCollection<FuelType> FuelTypeList { get; set; }

        public ACAirportLimits SelectedACLimit { get; set; }

        public bool LimitSelected
        {
            get
            {
                return SelectedACLimit != null;
            }
        }
        public AirstripInfoCntrlViewModel()
        {
            ACTypes = new RangeObservableCollection<AircraftType>();
            Currencies = new RangeObservableCollection<Currency>();
            Airstrips = new RangeObservableCollection<AirstripInfo>();
            AirstripLimits = new RangeObservableCollection<ACAirportLimits>();

            AirstripExForList = new RangeObservableCollection<AirStripExForGridItem>();
            AirportFeeTypeList = new RangeObservableCollection<AirportFeeType>();
            FeesList = new RangeObservableCollection<AirportFee>();
            ExForType = new RangeObservableCollection<string>();
            FuelList = new RangeObservableCollection<AirportFuel>();
            FuelTypeList = new RangeObservableCollection<FuelType>();
            OpeningTime = new DateTime(2000, 01, 01,06,00,00);
            ClosingTime = new DateTime(2000, 01, 01, 11, 00, 00);

            IsActive = true;

        }

        public override void Init()
        {
            Currencies.AddRange(Currency.GetCurrencyList());
            Airstrips.AddRange(AirstripInfo.GetAirstrips());
            AirportFeeTypeList.AddRange(AirportFeeType.GetAllAirportFeeTypes());
            FuelTypeList.AddRange(FuelType.GetFuelTypes());
            NotifyPropertyChanged("Currencies");
            NotifyPropertyChanged("Airstrips");
            NotifyPropertyChanged("FeesList");
        }


        public override bool Validate()
        {
            var invalidFieldLst = new List<String>();

            if (String.IsNullOrEmpty(Name))
                invalidFieldLst.Add("Name");

            if (String.IsNullOrEmpty(ICAO))
                invalidFieldLst.Add("ICAO");

            if (String.IsNullOrEmpty(IATA))
                invalidFieldLst.Add("IATA");

            if (String.IsNullOrEmpty(Longitude))
                invalidFieldLst.Add("Longitude");

            if (String.IsNullOrEmpty(Latitude))
                invalidFieldLst.Add("Latitude");

            if (SelectedCurrency==null)
                invalidFieldLst.Add("Currency");

            if (AlternateAirstrip==null)
                invalidFieldLst.Add("Alternate Airstrip");

            if (invalidFieldLst.Count > 0)
            {
                String validFailMess = "The following fields need to be completed:"+Environment.NewLine;
                String strfieldList=String.Join(Environment.NewLine, invalidFieldLst);
                validFailMess += strfieldList;

                FailWindow.Display(validFailMess);
                return false;

            }

            return true;
        }
        public override async Task<bool> Save()
        {
            try
            {
                var airstripInfo = new AirstripInfo();
                airstripInfo.IsNew = IsNew;
                airstripInfo.IDX_Country = ParentIDX;
                airstripInfo.IDX = IDX;
                airstripInfo.Description = Name;
                airstripInfo.ICAO = ICAO;
                airstripInfo.Code = IATA;
                airstripInfo.TPCode = TPCode;
                airstripInfo.AreaCode = AreaCode;
                airstripInfo.Longitude = Longitude;
                airstripInfo.Latitude = Latitude;
                airstripInfo.Altitude = Altitude;
                airstripInfo.RunwayHeading = RunwayHeading;
                airstripInfo.RunwayLength = RunwayLength;
                airstripInfo.SurfaceFactor = SurfaceFactor;
                airstripInfo.AltDistance = AlternateDist;
                airstripInfo.AlternateCode = AlternateAirportCode;
                airstripInfo.TurnAroundTime = TurnaroundTime;
                airstripInfo.OvernightFee = OvernightFee;
                airstripInfo.TasPermitFee = TasPermitFee;
                airstripInfo.DepTaxInternal = DomesticDepartureTax;
                airstripInfo.OpeningTime = OpeningTime;
                airstripInfo.ClosingTime = ClosingTime;
                airstripInfo.FuelPoint = FuelPoint;
                airstripInfo.DecimalLong = CoordinateCoverter.ConvertDegreesToDecimal(Longitude);
                airstripInfo.DecimalLat = CoordinateCoverter.ConvertDegreesToDecimal(Latitude); ;
                airstripInfo.IsActive = IsActive;
                airstripInfo.IDX_CurrencyType = Currencies.FirstOrDefault(x => x.IDX == SelectedCurrency.IDX).IDX;
                airstripInfo.IDX_Alt = AlternateAirstrip.IDX;
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await airstripInfo.Save(Server, Database);
                    await ACAirportLimits.UpdateACLimits(IDX, AirstripLimits.ToList(), Server, Database);
                    await AirportFee.SaveAirportFees(FeesList.ToList(), Server, Database);
                    await AirportFuel.SaveAirportFuel(IDX, FuelList.ToList(), Server, Database);
                }



                AirstripInfo.UpdateCachedObject(airstripInfo);
                ACAirportLimits.UpdateCachedObjects(airstripInfo.IDX, AirstripLimits.ToList());


                return true;

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save airstrip info :" + Environment.NewLine + exMessage);
                return false;
            }
        }

        public void Refresh(AirstripInfo info, List<AirStripExFor> airstripExForList)
        {
            Currencies.Clear();
            Airstrips.Clear();
            ACTypes.Clear();
            FeesList.Clear();
            AirportFeeTypeList.Clear();
            FuelList.Clear();

            IsNew = false;
            IDX = info.IDX;
            Name = info.Description;
            ICAO = info.ICAO;
            IATA = info.Code;
            TPCode = info.TPCode;
            AreaCode = info.AreaCode;
            IsActive = info.IsActive;
            ParentIDX = info.IDX_Country;
            Altitude = info.Altitude;
            RunwayHeading = info.RunwayHeading;
            RunwayLength = info.RunwayLength;
            SurfaceFactor = info.SurfaceFactor;
            AlternateDist = info.AltDistance;
            AlternateAirportCode = info.AlternateCode;
            TurnaroundTime = info.TurnAroundTime;
            OvernightFee = info.OvernightFee;
            TasPermitFee = info.TasPermitFee;
            DomesticDepartureTax = info.DepTaxInternal;
            InternationalDepartureTax = info.DepTaxInternational;
            OpeningTime = info.OpeningTime;
            ClosingTime = info.ClosingTime;
            DecimalLong = info.DecimalLong;
            DecimalLat = info.DecimalLat;
            Latitude = info.Latitude;
            Longitude = info.Longitude;
            FuelPoint = info.FuelPoint;
            Currencies.AddRange(Currency.GetCurrencyList());
            Airstrips.AddRange(AirstripInfo.GetAirstrips());
            FuelList.AddRange(AirportFuel.GetAirportFuel(IDX));
            SelectedCurrency = Currencies.FirstOrDefault(x => x.IDX == info.IDX_CurrencyType);
            AlternateAirstrip = Airstrips.FirstOrDefault(x => x.IDX == info.IDX_Alt);

            //var Position = CoordinateCoverter.DecimalToLongLatString(DecimalLong, DecimalLat);
            //Latitude = Position.Latitude;
            //Longitude = Position.Longitude;

            AirstripExForList.Clear();
            AirstripExForList.AddRange(airstripExForList.Select(x=> (AirStripExForGridItem)x).ToList());

            AirstripLimits.Clear();
            AirstripLimits.AddRange(ACAirportLimits.GetAPLimits(info.IDX));

           
            var airportFeeList =  AirportFee.GetAirportFees(IDX);
            FeesList.AddRange(airportFeeList);
         

            FuelTypeList.Clear();
            FuelTypeList.AddRange(FuelType.GetFuelTypes());

            ACTypes.Clear();
            ACTypes.AddRange(AircraftType.GetACTypes());

            AirportFeeTypeList.AddRange(AirportFeeType.GetAllAirportFeeTypes());


            NotifyPropertyChanged("IsActive");
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("ICAO");
            NotifyPropertyChanged("IATA");
            NotifyPropertyChanged("TPCode");
            NotifyPropertyChanged("AreaCode");
            NotifyPropertyChanged("Longitude");
            NotifyPropertyChanged("Latitude");
            NotifyPropertyChanged("Altitude");
            NotifyPropertyChanged("RunwayHeading");
            NotifyPropertyChanged("RunwayLength");
            NotifyPropertyChanged("SurfaceFactor");
            NotifyPropertyChanged("AlternateDist");
            NotifyPropertyChanged("TurnaroundTime");
            NotifyPropertyChanged("AlternateAirportCode");
            NotifyPropertyChanged("SelectedCurrency");
            NotifyPropertyChanged("OvernightFee");
            NotifyPropertyChanged("TasPermitFee");
            NotifyPropertyChanged("DomesticDepartureTax");
            NotifyPropertyChanged("InternationalDepartureTax");

            NotifyPropertyChanged("CustomsPoint");
            NotifyPropertyChanged("FuelPoint");
            NotifyPropertyChanged("IsHeliport");
            NotifyPropertyChanged("PilotNightStop");
            NotifyPropertyChanged("PilotNightStopRating");
            NotifyPropertyChanged("OpeningTime");
            NotifyPropertyChanged("ClosingTime");
            NotifyPropertyChanged("Currencies");
            NotifyPropertyChanged("Airstrips");
            NotifyPropertyChanged("SelectedCurrency");
            NotifyPropertyChanged("AlternateAirstrip");
            NotifyPropertyChanged("AirstripExForList");
            NotifyPropertyChanged("AirstripLimits");
            NotifyPropertyChanged("AirportFeeTypeList");
            NotifyPropertyChanged("FeesList");
            NotifyPropertyChanged("FuelList");
        }

        public void TPLookUp()
        {
            var tpLookUpView = new ChooseTPCodeView();
            var tpLookUpViewModel = tpLookUpView.DataContext as ChooseTPCodeViewModel;
            tpLookUpViewModel.Server = Server;
            tpLookUpViewModel.Database = Database;
            tpLookUpViewModel.PartialName = Name;
            tpLookUpViewModel.Region = Region;
            if (tpLookUpView.ShowDialog() == true)
            {
                TPCode = tpLookUpViewModel.SelectedCRMCode.Code;
                NotifyPropertyChanged("TPCode");
            }
        }

        public void ShowOnMap()
        {
            var newMapCntrlView = new MapCntrlWindowView();
            var mapcntrlVM = newMapCntrlView.DataContext as MapCntrlWindowViewModel;
 
            mapcntrlVM.Init(Name,DecimalLong,DecimalLat);
            var dlgResult=newMapCntrlView.ShowDialog();

            if (dlgResult==true)
            {
                Longitude = mapcntrlVM.Longitude;
                Latitude = mapcntrlVM.Latitude;
                DecimalLat = mapcntrlVM.PosDecLat;
                DecimalLong = mapcntrlVM.PosDecLong;

                NotifyPropertyChanged("Longitude");
                NotifyPropertyChanged("Latitude");
            }

        }

        public void AddFuel()
        {
            var airportFuel = new AirportFuel();
            airportFuel.IDX_Airport = IDX;
            FuelList.Add(airportFuel);
            NotifyPropertyChanged("FuelList");
        }

        public void AddFee()
        {
            var airportFee = new AirportFee();
            airportFee.IDX_Airport = IDX;
            FeesList.Add(airportFee);
            NotifyPropertyChanged("FeesList");
        }

        public void RemoveFuel()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Delete Fuel entry";
            parameters.Content = "Are you sure want to delete this entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnFuelDeleteConfirmed;
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }

        public void RemoveFee()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Delete Fee entry";
            parameters.Content = "Are you sure want to delete this entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnFeeDeleteConfirmed;
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }
        public void AddLimits()
        {
            var acLimit = new ACAirportLimits();
            acLimit.IDX_Airport = IDX;
            acLimit.StartPeriod = DateTime.Today;
            acLimit.EndPeriod = acLimit.StartPeriod.AddYears(20);
            AirstripLimits.Add(acLimit);
            NotifyPropertyChanged("AirstripLimits");
        }



        public void RemoveLimit()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Delete Limit entry";
            parameters.Content = "Are you sure want to delete this entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnDeleteConfirmed;
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }

        private  void OnFuelDeleteConfirmed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    if (SelectedFuel != null)
                    {

                        FuelList.Remove(SelectedFuel);
                        NotifyPropertyChanged("FeesList");
                    }
                }
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }
        }

        private async void OnFeeDeleteConfirmed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    if (SelectedFee != null)
                    {
                        using (new StackedCursorOverride(Cursors.Wait))
                        {
                            await AirportFee.DeleteAirportFee(SelectedFee.IDX, Server, Database);
                            FeesList.Remove(SelectedFee);
        
                        }
                        NotifyPropertyChanged("FeesList");
                    }
                }
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }
        }

        private async void OnDeleteConfirmed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    if (SelectedACLimit != null)
                    {
                        using (new StackedCursorOverride(Cursors.Wait))
                        {

                            AirstripLimits.Remove(SelectedACLimit);
                            await ACAirportLimits.UpdateACLimits(IDX, AirstripLimits.ToList(), Server, Database);
                        }
                        ACAirportLimits.UpdateCachedObjects(IDX, AirstripLimits.ToList());
                        NotifyPropertyChanged("ACAirportLimits");
                    }
                }
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

        }

    }
}
