using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class AircraftInfoCntrlViewModel : ItemCntrlViewModelBase
    {

        public DataDocumentsCnrlViewModel DataDocsViewModel { get; set; }
        public Company SelectedCompany { get; set; }
        public AircraftType SelectedAircraftType {get;set;}
        public String  SelectedAircraftTypeName { get; set; }
        public String  Registration { get; set; }

        public String SerialNumber { get; set; }

        public double TotalTimeAirframe { get; set; }

        public String SelectedOwner { get; set; }

        public float ShortCycleFee { get; set; }
        public DateTime? YearOfManufacture { get; set; }

        public DateTime? DateOfTotalTime { get; set; }

        public bool OwnAircraft { get; set; }

        public bool Active { get; set; }

        public float SellRate { get; set; }
        public Double EmptyMass { get; set; }

        public Double EmptyArm { get; set; }

        public String Colours { get; set; }

        public String Equipment { get; set; }

        public int Demurrage { get; set; }

        public int Speed { get; set; }

        public int ReserverFuel { get; set; }

        public int TotalFuel { get; set; }

        public int DOC { get; set; }

        public String InvoiceCode { get; set; }

        public String SATIB { get; set; }

        public DateTime InsuranceExpiryDate { get; set; }

        public String Liability { get; set; }
       
        public Company Underwriter { get; set; }

        public String ProfitCenter { get; set; }

        public RangeObservableCollection<AircraftType> ACTypesList { get; set; }
        public RangeObservableCollection<Company> CompanyList { get; set; }

        public AircraftInfoCntrlViewModel()
        {
            ACTypesList = new RangeObservableCollection<AircraftType>();
            CompanyList = new RangeObservableCollection<Company>();
        
        }


        public override void Init()
        {
            ACTypesList.AddRange(AircraftType.GetACTypes());
            NotifyPropertyChanged("ACTypesList");

            CompanyList.AddRange(Company.GetCompanyList());
            NotifyPropertyChanged("CompanyList");

            InsuranceExpiryDate = DateTime.Today;
            YearOfManufacture = DateTime.Today;
            DateOfTotalTime = DateTime.Today;


            Speed = 0;
            Demurrage = 0;
            ShortCycleFee = 0;
            ReserverFuel = 0;
            TotalFuel = 0;

            SellRate = 0;
            InvoiceCode = "";
            Liability = "";

            DataDocsViewModel.Init();

            NotifyPropertyChanged("YearOfManufacture");
            NotifyPropertyChanged("DateOfTotalTime");
            NotifyPropertyChanged("InsuranceExpiryDate");
        }

        public override bool Validate()
        {
            var invalidFieldLst = new List<String>();

            if (String.IsNullOrEmpty(Registration))
                invalidFieldLst.Add("Registration");

            if (SelectedAircraftType==null)
                invalidFieldLst.Add("Aircraft type");
            if (SelectedCompany==null)
                invalidFieldLst.Add("Aircraft owner");

            if (String.IsNullOrEmpty(SerialNumber))
                invalidFieldLst.Add("Serial Number");

            if (String.IsNullOrEmpty(Colours))
                invalidFieldLst.Add("Colours");

            if (String.IsNullOrEmpty(Equipment))
                invalidFieldLst.Add("Equipment");



            if (invalidFieldLst.Count > 0)
            {
                String validFailMess = "The following fields need to be completed:" + Environment.NewLine;
                String strfieldList = String.Join(Environment.NewLine, invalidFieldLst);
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
                var acInfo = new AircraftInfo();
                acInfo.IsNew = IsNew;
                acInfo.IDX = IDX;
                acInfo.Registration = Registration;
                if (SelectedAircraftType!=null)
                    acInfo.IDX_AC_Type = SelectedAircraftType.IDX;
                if (SelectedCompany!=null)
                     acInfo.IDX_Owner = SelectedCompany.IDX;
                acInfo.SerialNumber = SerialNumber;
                acInfo.YearOfManufacture=YearOfManufacture ;
                acInfo.DateOfTotalTime=DateOfTotalTime;
                acInfo.TotalTimeAirframe = TotalTimeAirframe;
                acInfo.Active = Active;
                acInfo.OwnAircraft = OwnAircraft;
                acInfo.SellRate = SellRate;
                acInfo.EmptyMass = EmptyMass;
                acInfo.EmptyArm = EmptyArm;
                acInfo.Colours = Colours;
                acInfo.AverageSpeed = Speed;
                acInfo.Demurrage = Demurrage;
                acInfo.ShortCycleFee = ShortCycleFee;
                acInfo.ReserveFuel = ReserverFuel;
                acInfo.TotalFuel = TotalFuel;
                acInfo.Equipment = Equipment;
                SellRate = acInfo.SellRate;
                acInfo.InvoiceCode = InvoiceCode;
                acInfo.InsuranceExpiryDate = InsuranceExpiryDate;
                acInfo.ProfitCenter = ProfitCenter;
                acInfo.Liability= Liability;
                if (Underwriter != null)
                    acInfo.IDX_UnderWriter = Underwriter.IDX;
           
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await acInfo.Save(Server, Database);
                    if (!IsNew)
                        await DataDocsViewModel.Save(IDX,"Aircraft",Server, Database);
                }


               
                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save aircraft data :" + Environment.NewLine + exMessage);
               return false;
            }
        }

        public async void Refresh(AircraftInfo aircraft)
        {
            try
            {
                IsNew = false;
                ACTypesList.AddRange(AircraftType.GetACTypes());
                NotifyPropertyChanged("ACTypesList");

                CompanyList.AddRange(Company.GetCompanyList());
                NotifyPropertyChanged("CompanyList");

                if (aircraft != null)
                {
                    IDX = aircraft.IDX;
                    Registration = aircraft.Registration;
                    SelectedAircraftType = ACTypesList.FirstOrDefault(x => x.IDX == aircraft.IDX_AC_Type);
                    SelectedCompany = CompanyList.FirstOrDefault(x => x.IDX == aircraft.IDX_Owner);
                    SerialNumber = aircraft.SerialNumber;
                    YearOfManufacture = aircraft.YearOfManufacture;
                    DateOfTotalTime = aircraft.DateOfTotalTime;
                    TotalTimeAirframe = aircraft.TotalTimeAirframe;
                    Active = aircraft.Active;
                    OwnAircraft = aircraft.OwnAircraft;
                    EmptyMass = aircraft.EmptyMass;
                    EmptyArm = aircraft.EmptyArm;
                    Colours = aircraft.Colours;
                    Speed = aircraft.AverageSpeed;
                    Demurrage = aircraft.Demurrage;
                    ShortCycleFee = aircraft.ShortCycleFee;
                    ReserverFuel = aircraft.ReserveFuel;
                    TotalFuel = aircraft.TotalFuel;
                    Equipment = aircraft.Equipment;
                    SellRate = aircraft.SellRate;
                    InvoiceCode = aircraft.InvoiceCode;
                    InsuranceExpiryDate = aircraft.InsuranceExpiryDate;
                    Liability = aircraft.Liability;
                    ProfitCenter = aircraft.ProfitCenter;
                    Underwriter = CompanyList.FirstOrDefault(x => x.IDX == aircraft.IDX_UnderWriter);
                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                    {
                        await DataDocsViewModel.LoadDocuments(aircraft.IDX, "Aircraft", Server, Database);
                    }

                    NotifyPropertyChanged("ProfitCenter");
                    NotifyPropertyChanged("TotalTimeAirframe");
                    NotifyPropertyChanged("SellRate");
                    NotifyPropertyChanged("InvoiceCode");
                    NotifyPropertyChanged("InsuranceExpiryDate");
                    NotifyPropertyChanged("Liability");
                    NotifyPropertyChanged("Underwriter");
                    NotifyPropertyChanged("Equipment");
                    NotifyPropertyChanged("EmptyMass");
                    NotifyPropertyChanged("EmptyArm");
                    NotifyPropertyChanged("Colours");
                    NotifyPropertyChanged("Speed");
                    NotifyPropertyChanged("Demurrage");
                    NotifyPropertyChanged("ShortCycleFee");
                    NotifyPropertyChanged("ReserverFuel");
                    NotifyPropertyChanged("TotalFuel");
                    NotifyPropertyChanged("Active");
                    NotifyPropertyChanged("OwnAircraft");
                    NotifyPropertyChanged("SelectedCompany");
                    NotifyPropertyChanged("SelectedAircraftType");
                    NotifyPropertyChanged("Registration");
                    NotifyPropertyChanged("SerialNumber");
                    NotifyPropertyChanged("YearOfManufacture");
                    NotifyPropertyChanged("DateOfTotalTime");
                }
            }
            catch(Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to refresh aircraft info :" + Environment.NewLine + exMessage);
            }
           
        }

    }

}
