using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
   public class LodgeInfoCntrlViewModel :ItemCntrlViewModelBase
    {
        public String Region { get; set; }
        public String CampName { get; set; }
        public int LodgeIDX { get; set; } 
     
        public RangeObservableCollection<AirstripInfo> AirstripList { get; set; }
        public RangeObservableCollection<Company> CompanyList { get; set; }

        public AirstripInfo SelectedAirstrip { get; set; }

        public Company SelectedCompany { get; set; }

        public String Email { get; set; }

        public String NoBeds { get; set; }

        public String Manager { get; set; }

        public String Telephone { get; set; }

        public DateTime LatestCheckOut { get; set; }

        public DateTime EarliestCheckIn { get; set; }

        public String TPCode { get; set; }

        public LodgeInfoCntrlViewModel()
        {

          

        }

        public override void Init()
        {
            AirstripList = new RangeObservableCollection<AirstripInfo>();
            CompanyList = new RangeObservableCollection<Company>();

            var companies = Company.GetCompanyList();
            if (companies != null)
            {
                CompanyList.AddRange(companies);
                NotifyPropertyChanged("CompanyList");

            }


            var airstrips = AirstripInfo.GetAirstrips();


            if (airstrips != null)
            {
                AirstripList.AddRange(airstrips);
                NotifyPropertyChanged("AirstripList");
            }


            IsVisible = false;


        }

        public override bool Validate()
        {
            if (SelectedAirstrip == null)
            {
                FailWindow.Display("Please select an airstrip");
                return false;
            }
            if (SelectedCompany==null)
            {
                FailWindow.Display("Please select an operator");
                return false;
            }
 
            return true;
        }

        public override async Task<bool> Save()
        {
            try
            {
                var lodge = new Lodge();
                lodge.IsNew = IsNew;
         
                lodge.Name = CampName;
                lodge.IDX = LodgeIDX;
                lodge.IDX_Airstrip = SelectedAirstrip.IDX;
                lodge.IDX_Company = SelectedCompany.IDX;
                lodge.Email = Email;
                lodge.NoBeds = Convert.ToInt16(NoBeds);
                lodge.EarliestCheckIn = EarliestCheckIn;
                lodge.LatestCheckOut = LatestCheckOut;
                lodge.TPCode = TPCode;
                lodge.IDX_Country = ParentIDX;
                lodge.IsActive = true;

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await lodge.Save(Server, Database);
                }

                Lodge.UpdateCachedObject(lodge);                               
                return true;
            }
            catch (Exception ex )
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display ("Unable to save lodge data :" + Environment.NewLine + exMessage);
                return false;
 
            }

        }
        
        public void TPLookUp()
        {
            var tpLookUpView = new ChooseTPCodeView();
            var tpLookUpViewModel = tpLookUpView.DataContext as ChooseTPCodeViewModel;
            tpLookUpViewModel.Server = Server;
            tpLookUpViewModel.Database = Database;
            tpLookUpViewModel.PartialName = CampName;
            tpLookUpViewModel.Region = Region;
            if (tpLookUpView.ShowDialog()==true)
            {
                TPCode = tpLookUpViewModel.SelectedCRMCode.Code;
                NotifyPropertyChanged("TPCode");
            }
        }

        public void Refresh(Lodge lodge)
        {
            if (lodge!=null)
            {
                LodgeIDX = lodge.IDX;
                IsNew = false;
                CampName = lodge.Name;
                SelectedAirstrip = AirstripList.FirstOrDefault(x => x.IDX == lodge.IDX_Airstrip);
                SelectedCompany = CompanyList.FirstOrDefault(x => x.IDX == lodge.IDX_Company);
                
                Email = lodge.Email;
                EarliestCheckIn = lodge.EarliestCheckIn;
                LatestCheckOut = lodge.LatestCheckOut;
                Email = lodge.Email;
                NoBeds = lodge.NoBeds.Value.ToString() ?? "0";
                TPCode = lodge.TPCode;
                ParentIDX = lodge.IDX_Country;

                NotifyPropertyChanged("CampName");
                NotifyPropertyChanged("SelectedCompany");
                NotifyPropertyChanged("SelectedAirstrip");
                NotifyPropertyChanged("Email");
                NotifyPropertyChanged("Manager");
                NotifyPropertyChanged("EarliestCheckIn");
                NotifyPropertyChanged("LatestCheckOut");
                NotifyPropertyChanged("NoBeds");
                NotifyPropertyChanged("TPCode");
            };
          
        }

    }


}
