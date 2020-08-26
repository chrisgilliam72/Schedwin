using Schedwin.Common;
using Schedwin.Data;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedwin.Setup
{
    public class CompanyInfoCnrtrlViewModel : ItemCntrlViewModelBase
    {
        public bool IsActive { get; set; }
        public String CompanyName { get; set; }

        public String CompanyRegistration { get; set; }

        public String PostalAddress { get; set; }

        public String PhysicalAddress { get; set; }

        public String TelephoneNumber { get; set; }

        public String Email { get; set; }

        public String GPCode { get; set; }

        public String VAT { get; set; }

        public CompanyType CompanyType { get; set; }

        public Currency Currency { get; set; }

        public Country Country { get; set; }

        public AirstripInfo Airstrip { get; set; }

        public RangeObservableCollection <CompanyType> CompanyTypeList { get; set; }

        public RangeObservableCollection<Country> Countries { get;  set; }

        public RangeObservableCollection<Currency> Currencies { get; set; }


        public RangeObservableCollection<AirstripInfo> Airstrips { get; set; }

        public RangeObservableCollection<User> AgentList { get; set; }
        public RangeObservableCollection<PriceList> PriceLists { get; set; }

        private User _selectedAgent;
        public User SelectedAgent
        {

            set
            {
                _selectedAgent = value;
                NotifyPropertyChanged("SelectedAgent");
                NotifyPropertyChanged("CanDeletAgent");
            }

            get
            {
                return _selectedAgent;
            }
        }

        private PriceList _selectedPriceListItem;
        public PriceList SelectedPriceListItem
        {
            set
            {
                _selectedPriceListItem = value;
                NotifyPropertyChanged("CanDelete");
            }
            get
            {
                return _selectedPriceListItem;
            }
        }

        public bool CanDelete
        {
            get
            {
                return SelectedPriceListItem != null;
            }
        }

        public bool CanDeletAgent
        {
            get
            {
                return SelectedAgent != null;
            }

        }

        public bool ShowPriceLists { get; set; }

        public bool ShowAgentLists { get; set; }

        public CompanyInfoCnrtrlViewModel()
        {
            CompanyTypeList = new RangeObservableCollection<CompanyType>();
            Countries = new RangeObservableCollection<Country>();
            Currencies = new RangeObservableCollection<Currency>();
            Airstrips = new RangeObservableCollection<AirstripInfo>();
            PriceLists = new RangeObservableCollection<PriceList>();
            AgentList = new RangeObservableCollection<User>();
        }

        public async Task FillLookups()
        {
            CompanyTypeList.Clear();
            Currencies.Clear();
            Countries.Clear();
            Airstrips.Clear();
            AgentList.Clear();

            var types = await CompanyType.GetCompanyTypes(Server, Database);
            CompanyTypeList.AddRange(types);

            Currencies.AddRange(Currency.GetCurrencyList());
            Countries.AddRange(Country.GetCountryList().OrderBy(x=>x.Name).ToList());
            Airstrips.AddRange(AirstripInfo.GetAirstrips());


            NotifyPropertyChanged("Countries");
            NotifyPropertyChanged("Airstrips");
            NotifyPropertyChanged("Currencies");
            NotifyPropertyChanged("CompanyTypeList");
        }

        public override async void Init()
        {
            try
            {
                await FillLookups();
                ShowPriceLists = false;
                ShowAgentLists = false;
                NotifyPropertyChanged("ShowPriceLists");
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to load company data :" + Environment.NewLine + exMessage);
            }
        }

        public void TPLookUp()
        {
            //var tpLookUpView = new ChooseTPCodeView();
            //var tpLookUpViewModel = tpLookUpView.DataContext as ChooseTPCodeViewModel;
            //tpLookUpViewModel.PartialName = CompanyName;
            //if (tpLookUpView.ShowDialog() == true)
            //{
            //    GPCode = tpLookUpViewModel.SelectedCRMCode.Code;
            //    NotifyPropertyChanged("GPCode");
            //}
        }



        public override bool Validate()
        {
            var invalidFieldLst = new List<String>();


            if (CompanyType == null)
                invalidFieldLst.Add("Company type not selected");

            if (Country == null)
                invalidFieldLst.Add("Country not selected");

            if (Currency == null)
                invalidFieldLst.Add("Currency not selected");

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
                var companyInfo = new Company();
                companyInfo.IsNew = IsNew;
                companyInfo.IDX = IDX;
                companyInfo.Registration = CompanyRegistration;
                companyInfo.Description = CompanyName;
                companyInfo.PhysicalAddress = PhysicalAddress;
                companyInfo.PostalAddress = PostalAddress;
                companyInfo.Email = Email;
                companyInfo.VatPercentage = Convert.ToByte(VAT);
                companyInfo.IsActive = IsActive;
                companyInfo.IDX_CompanyType = CompanyType.IDX;
                if (Currency!=null)
                    companyInfo.IDX_CurrencyType = Currency.IDX;
                companyInfo.GPID = GPCode;
                if (Airstrip!=null)
                    companyInfo.IDX_BaseAP = Airstrip.IDX;

                companyInfo.IDX_Country = Country.IDX;

                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await companyInfo.Save(Server, Database);
                }

                Company.UpdateCachedObject(companyInfo);
                return true;

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save company data :" + Environment.NewLine + exMessage);
                return false;
            }
        }

        public async void Refresh(Company companyInfo)
        {
            try
            {
                await FillLookups();
                PriceLists.Clear();
                AgentList.Clear();

                IDX = companyInfo.IDX;
                IsNew = false;
                CompanyName = companyInfo.Description;
                CompanyRegistration = companyInfo.Registration;
                PostalAddress = companyInfo.PostalAddress;
                PhysicalAddress = companyInfo.PhysicalAddress;
                GPCode = companyInfo.GPID;
                Email = companyInfo.Email;
                VAT = companyInfo.VatPercentage.ToString();
                IsActive = companyInfo.IsActive;
                CompanyType = CompanyTypeList.FirstOrDefault(x => x.IDX == companyInfo.IDX_CompanyType);
                Currency = Currencies.FirstOrDefault(x => x.IDX == companyInfo.IDX_CurrencyType);
                Country = Countries.FirstOrDefault(x => x.IDX == companyInfo.IDX_Country);
                Airstrip = Airstrips.FirstOrDefault(x => x.IDX == companyInfo.IDX_BaseAP);
               
                ShowPriceLists = true;
                ShowAgentLists = true;

                using (new StackedCursorOverride(Cursors.Wait))
                {
                    var tmpLst = await PriceList.GetSeatRatePriceList(companyInfo.IDX, Server, Database);
                    foreach (var item in tmpLst)
                    {
                        item.Start = AirstripInfo.GetAirstripCode(item.StartIDX);
                        item.End = AirstripInfo.GetAirstripCode(item.DestIDX);
                    }
                    PriceLists.AddRange(tmpLst);

                    var tmpUserLst=await User.GetAgentUsersForCompany(companyInfo.IDX, Server, Database);
                    AgentList.AddRange(tmpUserLst);
                }


                NotifyPropertyChanged("AgentList");
                NotifyPropertyChanged("IsActive");
                NotifyPropertyChanged("PriceLists");
                NotifyPropertyChanged("ShowAgentLists");
                NotifyPropertyChanged("ShowPriceLists");
                NotifyPropertyChanged("Airstrip");
                NotifyPropertyChanged("VAT");
                NotifyPropertyChanged("Currency");
                NotifyPropertyChanged("Country");
                NotifyPropertyChanged("CompanyType");
                NotifyPropertyChanged("CompanyName");
                NotifyPropertyChanged("CompanyRegistration");
                NotifyPropertyChanged("PostalAddress");
                NotifyPropertyChanged("PhysicalAddress");
                NotifyPropertyChanged("GPCode");
                NotifyPropertyChanged("Email");
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save load company data :" + Environment.NewLine + exMessage);
            }
        }

        public void RemoveAgent()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Remove Agent ";
            parameters.Content = "Are you sure want to remove this entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnAgentDeleteConfirmed; 
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }

        public void DeletePriceListEntry()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Delete Price List entry";
            parameters.Content = "Are you sure want to delete this entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnDeleteConfirmed;
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }
        private async void OnAgentDeleteConfirmed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    if (SelectedAgent != null)
                    {
                        using (new StackedCursorOverride(Cursors.Wait))
                        {
                            await User.DeleteAgentUser(SelectedAgent.IDX, Server, Database);
                            AgentList.Remove(SelectedAgent);
                            NotifyPropertyChanged("SelectedAgent");
                        }
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
                    if (SelectedPriceListItem != null)
                    {
                        using (new StackedCursorOverride(Cursors.Wait))
                        {
                            await PriceList.Delete(SelectedPriceListItem.IDX, SelectedPriceListItem.Type, Server, Database);
                            PriceLists.Remove(SelectedPriceListItem);
                        }
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

        public async Task<bool> SavePriceListEntry(PriceList priceList)
        {
            try
            {
                if (priceList != null)
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        await priceList.Save(Server, Database);
                    }
                    priceList.IsNew = false;
                }

                return true;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }


        public async Task<bool> SaveAgentEntry(User newAgent)
        {
            try
            {
                if (newAgent != null)
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        await newAgent.Save(Server, Database);
                    }
                    newAgent.IsNew = false;

                    User.UpdateCachedObject(newAgent);

                    SelectedAgent = newAgent;
                }

                return true;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }
        public void AddNewAgent()
        {
            var agent = new User();
            agent.IDX_Company = IDX;
            agent.IsNew = true;
            agent.Active = true;
            agent.IDX_UserType = 3;
            agent.Username = "dummy agent";
            agent.Email = "";
            agent.IsUser = false;
            AgentList.Insert(0, agent);
            NotifyPropertyChanged("AgentList");
        }

        public void AddNewPriceList()
        {
            var newPriceList = new PriceList();
            newPriceList.Name = CompanyName + " pricelist";
            newPriceList.IsNew = true;
            newPriceList.CompanyIDX = IDX;
            newPriceList.Type = "Seat";
            PriceLists.Insert(0, newPriceList);
            NotifyPropertyChanged("PriceLists");
        }
    }
}
