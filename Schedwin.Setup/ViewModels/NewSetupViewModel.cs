using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Common;

using Schedwin.Data.Classes;
using Telerik.Windows.Controls;
using Schedwin.Data;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Schedwin.Setup
{
    public class NewSetupViewModel : Schedwin.Common.ViewModelBase
    {
        public String Region { get; set; }
        public NewSetupView View { get; set; }
        private bool _showInactive;
        public bool ShowInactive
        {
            get
            {
                return _showInactive;
            }
            set
            {
                _showInactive = value;
                NotifyPropertyChanged("ShowInactive");
            }
        }

        public String ShowHideInactiveText
        {
            get
            {
                return ShowInactive ? "Hide inactive items " : "Show inactive items";
            }
        }

        private String _statusText;
        public String StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                _statusText = value;
                NotifyPropertyChanged("StatusText");
            }
        }

        public SetupBaseTreeItem SelectedTreeItem { get; set; }
        public ObservableCollection<SetupBaseTreeItem> SetupItems { get; set; }
       
        
        public LodgeInfoCntrlViewModel ldgeCntrlVM { get; set; }
        public AirstripInfoCntrlViewModel airstrpCntrlVM { get; set; }
        public AircraftTypeCntrlViewModel aircraftTypeCntrlVM { get; set; }
        public AircraftInfoCntrlViewModel aircraftInfoCntrlVM { get; set; }

        public PilotInfoCntrlViewModel pilotInfoCntrlVM { get; set; }
        public UserInfoCntrlViewModel userInfoCntrlVM { get; set; }

        public FlightInfoCntrlViewModel flightInfoCntrlVM { get; set; }

        public ItemCntrlViewModelBase currentCntrolVM { get; set; }

        public DistanceCntrlViewModel distanceCntrlVM { get; set; }

        public PricelistCntrlViewModel pricelistCntrlVM { get; set; }

        public CompanyInfoCnrtrlViewModel companyInfoVM { get; set; }

        public List<AirStripExFor> AllExForList
        {
            get
            {
                if (UseGlobalDB)
                    return AirStripExFor.GetExForList();
                else
                    return CampExForAirstrips.GetExForList();
            }
        }

       public List<Company> CompanyList
        {
            get
            {
                return Company.GetCompanyList();
            }
        }

        public List<User> UserList
        {
            get
            {
                return User.GetUserList();
            }
        }

        public List<AirstripInfo> AirstripList
        {
            get
            {
                return AirstripInfo.GetAirstrips();
            }
        }
        public List<Lodge> LodgeList
        {
            get
            {
                return Lodge.GetLodgeList();
            }
        }

        public List<AircraftType> AircraftTypes { get; set; }

        public List<AircraftInfo> AircraftList
        {
            get
            {
                return AircraftInfo.GetAircraftList(false);
            }
        }

        public List<PilotInfo> PilotList
        {
            get
            {
                return PilotInfo.GetPilotList();
            }
        }

        public List <FlightInfo> FlightList

        {
            get
            {
                return FlightInfo.GetFlightList();
            }
        }


        public bool ShowSaveButton
        {
            get
            {
                if ((ldgeCntrlVM!=null && ldgeCntrlVM.IsVisible) || (airstrpCntrlVM!=null && airstrpCntrlVM.IsVisible) ||
                    (aircraftTypeCntrlVM!=null &&aircraftTypeCntrlVM.IsVisible) || (aircraftInfoCntrlVM!=null && aircraftInfoCntrlVM.IsVisible) ||
                    (userInfoCntrlVM !=null && userInfoCntrlVM.IsVisible)  || (pilotInfoCntrlVM!=null && pilotInfoCntrlVM.IsVisible) ||(flightInfoCntrlVM!=null && flightInfoCntrlVM.IsVisible) ||
                    companyInfoVM!=null && companyInfoVM.IsVisible)
                    return true;
                else
                    return false;
                 

            }
        }



        public NewSetupViewModel()
        {
            SetupItems = new ObservableCollection<SetupBaseTreeItem>();
            ShowInactive = false;

        }

        public async void Init()
        {
            if (UseGlobalDB)
                await InitNew();
            else
                await InitOld();
        }
        public async Task InitOld()
        {
            //        string exePath =
            //System.IO.Path.GetDirectoryName(
            //    System.Reflection.Assembly.GetEntryAssembly().Location);


            StatusText = "Loading...";
            var countries = Country.GetCountryList();
            try
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    AircraftTypes = await AircraftType.LoadACTypes(Server, Database,true);
                    var tsk1= AircraftInfo.LoadAircraftList(Server, Database,true);
                    var tsk2 = FuelType.LoadFuelTypes(Server, Database);
                    var tsk3 = User.LoadUserList(Server, Database,true);
                    var tsk4 = PilotInfo.LoadPilotInfo(Server, Database);
                    var tsk5 = FlightInfo.LoadFlightList(Server, Database,true);
                    var tsk6 = CampExForAirstrips.GetExForListV2(Server, Database);
                    var tsk7 = APDistances.LoadDistanceMatrix(Server, Database);
                    var tsk8 = UserType.LoadUserTypes(Server, Database);
                    var tsk9 = ACAirportLimits.LoadACAirportLimits(Server, Database);
                    var tsk10 = ACStationType.LoadStationTyoes(Server, Database);
                    var tsk11 = AirportFeeType.LoadAllAirportFeeTypes(Server, Database);

                    var tsk12 = Company.LoadCompanyList(Server, Database,true);
                    var tsk13 = Currency.GetGPCurrencyList(Server, Database);
                    var tsk14= AirportFuel.LoadFuelList(Server, Database);
                    var tsk15 = AirportFee.LoadAirportFees(Server, Database);
                    await Task.WhenAll(tsk1, tsk2, tsk3, tsk4, tsk5, tsk6, tsk7, tsk8, tsk9, tsk10,tsk11, tsk12, tsk13, tsk14,tsk15);

                }

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to load  setup cache:"+Environment.NewLine+ exMessage);
                StatusText = "Error loading setup cache ";
            }


            StatusText = "Initializing...";

            var CountriesRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, Description = "Countries" };

            var countryItemList = countries.Select(x => new SetupBaseTreeItem { IDX = x.IDX, Type = ItemType.country, Description = x.Name, ImagePath = @"/Schedwin.Setup;component/Images/Country16.png" }).OrderBy(x => x.Description).ToList();
          

            foreach (var countryItem in countryItemList)
            {
                var itemTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.lodge, Description = "Lodges", ParentIDX = countryItem.IDX };
                AddLodgeItems(itemTypeRootItem, countryItem.IDX);
                countryItem.SubItems.Add(itemTypeRootItem);

                itemTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.airstrip, Description = "Airstrips", ParentIDX = countryItem.IDX };
                AddAirstripItems(itemTypeRootItem, countryItem.IDX);
                countryItem.SubItems.Add(itemTypeRootItem);
                //var countryLodges= LodgeList.Where(x => x.IDX_Country == countryItem.IDX).ToList();
                //var lodgeItems=countryLodges.Select(x => new SetupBaseTreeItem { IDX=x.IDX,Type = ItemType.lodge,Description = x.Name }).OrderBy(x=>x.Description).ToList();
                //countryItem.SubItems.AddRange(lodgeItems);
                switch (countryItem.Description)
                {
                    case "Angola": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Angola16.png"; break;
                    case "Botswana": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Botswana16.png";break;
                    case "Kenya": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Kenya16.png"; break;
                    case "South Africa": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/SouthAfrica16.png"; break;
                    case "Namibia": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Namibia16.png"; break;
                    case "Rwanda": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Rwanda16.png"; break;
                    case "Zimbabwe": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Zimbabwe16.png"; break;
                    case "Zambia": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Zambia16.png"; break;



                }
                CountriesRootItem.SubItems.Add(countryItem);

                itemTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.flight, Description = "Flights", ParentIDX = countryItem.IDX };
                AddFlightItems(itemTypeRootItem, countryItem.IDX);
                countryItem.SubItems.Add(itemTypeRootItem);


            }

            SetupItems.Add(CountriesRootItem);

            var aircraftRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, Description = "Aircraft" };

            var aircraftTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType=ItemType.aircraftType ,Description = "Types" };
            AddAircraftTypes(aircraftTypeRootItem);

            var aircraftListRoomItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.aircraftList, Description = "List" };
            AddAircraftList(aircraftListRoomItem);

            aircraftRootItem.SubItems.Add(aircraftTypeRootItem);
            aircraftRootItem.SubItems.Add(aircraftListRoomItem);


            SetupItems.Add(aircraftRootItem);
            var personRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, Description = "Personnel" };
            var usersRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root,ChildrenType=ItemType.user, Description = "Users" };
            AddUserList(usersRootItem);
            var pilotsRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType=ItemType.pilot, Description = "Pilots" };
            AddPilotList(pilotsRootItem);
            personRootItem.SubItems.Add(usersRootItem);
            personRootItem.SubItems.Add(pilotsRootItem);
            SetupItems.Add(personRootItem);

            var setupCompanysRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.company, Description = "Companies" };
            AddCompanyList(setupCompanysRootItem);
            SetupItems.Add(setupCompanysRootItem);

            var setupDistanceItem= new SetupBaseTreeItem { IDX = -1, Type = ItemType.distance, ChildrenType = ItemType.distance, Description = "Distance table" };
            SetupItems.Add(setupDistanceItem);

            ldgeCntrlVM.Init();
            NotifyPropertyChanged("SetupItems");
            NotifyPropertyChanged("ShowSaveButton");
            StatusText = " ";

            airstrpCntrlVM.Region = Region;
            ldgeCntrlVM.Region = Region;
        }

        public async Task InitNew()
        {
            //        string exePath =
            //System.IO.Path.GetDirectoryName(
            //    System.Reflection.Assembly.GetEntryAssembly().Location);


            StatusText = "Loading...";
            var countries = await Country.LoadCountries(true);


            try
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    AircraftTypes = await AircraftType.LoadACTypes(true);

                    var tsk1= Lodge.LoadLodgeList(true);
                    var tsk2= AirstripInfo.LoadAirstrips();
                    var tsk3= FlightInfo.LoadFlightList(true);
                    var tsk4= AirStripExFor.LoadExForList();
                    var tsk5= AircraftInfo.LoadAircraftList( true);
                    var tsk6= User.LoadUserList(true);
                    var tsk7 = PilotInfo.LoadPilotInfo();

                    //var tsk2 = FuelType.LoadFuelTypes(Server, Database);


                    //var tsk6 = CampExForAirstrips.GetExForListV2(Server, Database);
                    //var tsk7 = APDistances.LoadDistanceMatrix(Server, Database);
                    //var tsk8 = UserType.LoadUserTypes(Server, Database);
                    //var tsk9 = ACAirportLimits.LoadACAirportLimits(Server, Database);
                    //var tsk10 = ACStationType.LoadStationTyoes(Server, Database);
                    //var tsk11 = AirportFeeType.LoadAllAirportFeeTypes(Server, Database);

                    //var tsk12 = Company.LoadCompanyList(Server, Database, true);
                    //var tsk13 = Currency.GetGPCurrencyList(Server, Database);
                    //var tsk14 = AirportFuel.LoadFuelList(Server, Database);

                    await Task.WhenAll(tsk1, tsk2, tsk3, tsk4, tsk5,tsk6,tsk7);
                    //await Task.WhenAll(tsk1, tsk2, tsk3, tsk4, tsk5, tsk6, tsk7, tsk8, tsk9, tsk10, tsk11, tsk12, tsk13, tsk14);

                }

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to load  setup cache:" + Environment.NewLine + exMessage);
                StatusText = "Error loading setup cache ";
            }


            StatusText = "Initializing...";

            var CountriesRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, Description = "Countries" };

            var countryItemList = countries.Select(x => new SetupBaseTreeItem { IDX = x.IDX, Type = ItemType.country, Description = x.Name, ImagePath = @"/Schedwin.Setup;component/Images/Country16.png" }).OrderBy(x => x.Description).ToList();


            foreach (var countryItem in countryItemList)
            {
                var itemTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.lodge, Description = "Lodges", ParentIDX = countryItem.IDX };
                AddLodgeItems(itemTypeRootItem, countryItem.IDX);
                countryItem.SubItems.Add(itemTypeRootItem);

                itemTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.airstrip, Description = "Airstrips", ParentIDX = countryItem.IDX };
                AddAirstripItems(itemTypeRootItem, countryItem.IDX);
                countryItem.SubItems.Add(itemTypeRootItem);
                //var countryLodges= LodgeList.Where(x => x.IDX_Country == countryItem.IDX).ToList();
                //var lodgeItems=countryLodges.Select(x => new SetupBaseTreeItem { IDX=x.IDX,Type = ItemType.lodge,Description = x.Name }).OrderBy(x=>x.Description).ToList();
                //countryItem.SubItems.AddRange(lodgeItems);
                switch (countryItem.Description)
                {
                    case "Angola": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Angola16.png"; break;
                    case "Botswana": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Botswana16.png"; break;
                    case "Kenya": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Kenya16.png"; break;
                    case "South Africa": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/SouthAfrica16.png"; break;
                    case "Namibia": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Namibia16.png"; break;
                    case "Rwanda": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Rwanda16.png"; break;
                    case "Zimbabwe": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Zimbabwe16.png"; break;
                    case "Zambia": countryItem.ImagePath = @"/Schedwin.Setup;component/Images/Zambia16.png"; break;



                }
                CountriesRootItem.SubItems.Add(countryItem);

                itemTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.flight, Description = "Flights", ParentIDX = countryItem.IDX };
                AddFlightItems(itemTypeRootItem, countryItem.IDX);
                countryItem.SubItems.Add(itemTypeRootItem);


            }

            SetupItems.Add(CountriesRootItem);

            var aircraftRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, Description = "Aircraft" };

            var aircraftTypeRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.aircraftType, Description = "Types" };
            AddAircraftTypes(aircraftTypeRootItem);

            var aircraftListRoomItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.aircraftList, Description = "List" };
            AddAircraftList(aircraftListRoomItem);

            aircraftRootItem.SubItems.Add(aircraftTypeRootItem);
            aircraftRootItem.SubItems.Add(aircraftListRoomItem);


            SetupItems.Add(aircraftRootItem);
            var personRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, Description = "Personnel" };
            var usersRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.user, Description = "Users" };
            AddUserList(usersRootItem);
            var pilotsRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.pilot, Description = "Pilots" };
            AddPilotList(pilotsRootItem);
            personRootItem.SubItems.Add(usersRootItem);
            personRootItem.SubItems.Add(pilotsRootItem);
            SetupItems.Add(personRootItem);

            var setupCompanysRootItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.Root, ChildrenType = ItemType.company, Description = "Companies" };
            AddCompanyList(setupCompanysRootItem);
            SetupItems.Add(setupCompanysRootItem);

            var setupDistanceItem = new SetupBaseTreeItem { IDX = -1, Type = ItemType.distance, ChildrenType = ItemType.distance, Description = "Distance table" };
            SetupItems.Add(setupDistanceItem);

            ldgeCntrlVM.Init();
            NotifyPropertyChanged("SetupItems");
            NotifyPropertyChanged("ShowSaveButton");
            StatusText = " ";

            airstrpCntrlVM.Region = Region;
            ldgeCntrlVM.Region = Region;
        }

        public void ClearControlPanel()
        {
            ldgeCntrlVM.IsVisible = false;
            airstrpCntrlVM.IsVisible = false;
            aircraftTypeCntrlVM.IsVisible = false;
            aircraftInfoCntrlVM.IsVisible = false;
            userInfoCntrlVM.IsVisible = false;
            pilotInfoCntrlVM.IsVisible = false;
            flightInfoCntrlVM.IsVisible = false;
            distanceCntrlVM.IsVisible = false;
            companyInfoVM.IsVisible = false;
        }


        public  void ShowControl(SetupBaseTreeItem selectedItem)
        {
            ClearControlPanel();

            switch (selectedItem.Type)
            {
                case ItemType.lodge:
                    currentCntrolVM = ldgeCntrlVM;
                    ldgeCntrlVM.Server = Server;
                    ldgeCntrlVM.Database = Database;
                    ldgeCntrlVM.Refresh(LodgeList.FirstOrDefault(x => x.IDX == selectedItem.IDX));
                    ldgeCntrlVM.IsVisible = true; break;

                case ItemType.airstrip:
                    currentCntrolVM = airstrpCntrlVM;
                    airstrpCntrlVM.Server = Server;
                    airstrpCntrlVM.Database = Database;
                    var tmpAirStripExForList=AllExForList.Where(x=>x.IDX_Airstrip== selectedItem.IDX).ToList();
                    airstrpCntrlVM.Refresh(AirstripList.FirstOrDefault(x => x.IDX == selectedItem.IDX), tmpAirStripExForList);
                    airstrpCntrlVM.IsVisible = true; break;

                case ItemType.aircraftType:
                    currentCntrolVM = aircraftTypeCntrlVM;
                    aircraftTypeCntrlVM.Server = Server;
                    aircraftTypeCntrlVM.Database = Database;
                    aircraftTypeCntrlVM.Refresh(AircraftTypes.FirstOrDefault(x => x.IDX == selectedItem.IDX));
                    aircraftTypeCntrlVM.IsVisible = true;
                    break;

                case ItemType.aircraftList:
                    currentCntrolVM = aircraftInfoCntrlVM;
                    aircraftInfoCntrlVM.Server = Server;
                    aircraftInfoCntrlVM.Database = Database;
                    aircraftInfoCntrlVM.Refresh(AircraftList.FirstOrDefault(x => x.IDX == selectedItem.IDX));
                    aircraftInfoCntrlVM.IsVisible = true;
                    break;

                case ItemType.user:
                    currentCntrolVM = userInfoCntrlVM;
                    userInfoCntrlVM.Server = Server;
                    userInfoCntrlVM.Database = Database;
                    //userInfoCntrlVM.UserName = UserList.FirstOrDefault(x => x.IDX == selectedItem.IDX).Username;
                    userInfoCntrlVM.Refresh(UserList.FirstOrDefault(x => x.IDX == selectedItem.IDX));
                    userInfoCntrlVM.IsVisible = true;
                    break;


                case ItemType.pilot:
                    currentCntrolVM = pilotInfoCntrlVM;
                    pilotInfoCntrlVM.Server = Server;
                    pilotInfoCntrlVM.Database = Database;
                    pilotInfoCntrlVM.Refresh(PilotList.FirstOrDefault(x=>x.IDX== selectedItem.IDX));
                    pilotInfoCntrlVM.IsVisible = true;
                    break;


                case ItemType.flight:
                    currentCntrolVM = flightInfoCntrlVM;
                    flightInfoCntrlVM.Server = Server;
                    flightInfoCntrlVM.Database = Database;
                    flightInfoCntrlVM.Refresh(FlightList.FirstOrDefault(x => x.IDX == selectedItem.IDX));
                    flightInfoCntrlVM.IsVisible = true;
                    break;


                case ItemType.distance:
                    currentCntrolVM = distanceCntrlVM;
                    distanceCntrlVM.Server = Server;
                    distanceCntrlVM.Database = Database;
                    distanceCntrlVM.Init();
                    distanceCntrlVM.IsVisible = true;
                    break;

                case ItemType.company:
                    currentCntrolVM = companyInfoVM;
                    companyInfoVM.Server = Server;
                    companyInfoVM.Database = Database;
                    companyInfoVM.Refresh(this.CompanyList.FirstOrDefault(x=>x.IDX==selectedItem.IDX));
                    companyInfoVM.IsVisible = true;
                    break;

            }

            NotifyPropertyChanged("ShowSaveButton");
        }


        public void ConfigureMenu(RadContextMenu menu)
        {

            if (SelectedTreeItem != null)
            {
                menu.Items.Clear();

                if (SelectedTreeItem.Type == ItemType.Root)
                {
                    var item = new RadMenuItem();
                    item.IconTemplate = (DataTemplate)View.Resources["AddIcon"];
                    item.Click += TreemMenuItem_Click;
                    switch (SelectedTreeItem.ChildrenType)
                    {
                        case ItemType.lodge: item.Header = "Add Lodge";
                                             menu.Items.Add(item);
                                            item = new RadMenuItem();
                                            item.Click += TreemMenuItem_Click;
                                             item.Header = "Find Lodge";
                                            item.IconTemplate = (DataTemplate)View.Resources["FindIcon"];
                                             menu.Items.Add(item);
                                            break;
                        case ItemType.aircraftList: item.Header = "Add Aircraft";
                                                                    menu.Items.Add(item);
                                                                    item = new RadMenuItem();
                                                                    item.Click += TreemMenuItem_Click;
                                                                    item.Header = "Find Aircraft";
                                                                    item.IconTemplate = (DataTemplate)View.Resources["FindIcon"];
                                                                    menu.Items.Add(item);
                                                                    break;
                        case ItemType.aircraftType: item.Header = "Add Aircraft Type"; menu.Items.Add(item); break;
                        case ItemType.airstrip: item.Header = "Add Airstrip";
                                                                menu.Items.Add(item);
                                                                item = new RadMenuItem();
                                                                item.Click += TreemMenuItem_Click;
                                                                item.Header = "Find Airstrip";
                                                                item.IconTemplate = (DataTemplate)View.Resources["FindIcon"];
                                                                menu.Items.Add(item);
                                                                break;
                        case ItemType.pilot: item.Header = "Add Pilot"; menu.Items.Add(item); break;
                        case ItemType.user: item.Header = "Add User"; menu.Items.Add(item); break;
                        case ItemType.flight: item.Header = "Add Flight";
                                                                menu.Items.Add(item);
                                                                item = new RadMenuItem();
                                                                item.Click += TreemMenuItem_Click;
                                                                item.Header = "Find Flight";
                                                                item.IconTemplate = (DataTemplate)View.Resources["FindIcon"];
                                                                menu.Items.Add(item);
                                                                break;
                        case ItemType.company: item.Header = "Add Company"; menu.Items.Add(item); break;
                    }

  
                }
                else
                {
                    var cancelItem = new RadMenuItem();
                    cancelItem.IconTemplate = (DataTemplate)View.Resources["CancelIcon"];
                    cancelItem.Header = "Deactivate";
                    cancelItem.Click += TreemMenuItem_Click;
                    menu.Items.Add(cancelItem);
                    if (SelectedTreeItem.Type == ItemType.lodge || SelectedTreeItem.Type == ItemType.user || SelectedTreeItem.Type == ItemType.flight 
                                                                || SelectedTreeItem.Type==ItemType.pilot  || SelectedTreeItem.Type == ItemType.aircraftList
                                                                || SelectedTreeItem.Type==ItemType.airstrip || SelectedTreeItem.Type==ItemType.company 
                                                                || SelectedTreeItem.Type==ItemType.aircraftType)
                        cancelItem.IsEnabled = true;
                    else
                        cancelItem.IsEnabled = false;
                }

                var refrshItem = new RadMenuItem();
                refrshItem.Click += TreemMenuItem_Click;
                refrshItem.Header = "Refresh";
                refrshItem.IconTemplate = (DataTemplate)View.Resources["RefreshIcon"];
                menu.Items.Add(refrshItem);
            }

            //var itemOther = new RadMenuItem
            //{
            //    Header = ShowHideInactiveText
            //};
            //menu.Items.Add(itemOther);
        }


        private SetupBaseTreeItem GetRootItem(ItemType type,int countryIDX)
        {
            var countriesRoomItem = SetupItems.First(x => x.Description== "Countries");
            var countryRootItem = countriesRoomItem.SubItems.First(x => x.IDX == countryIDX);
            switch (type)
            {
                case ItemType.flight:
                    var flightsItem = countryRootItem.SubItems.FirstOrDefault(x => x.Description == "Flights");
                    return flightsItem;
                case ItemType.lodge:
                    var lodgeItem = countryRootItem.SubItems.FirstOrDefault(x => x.Description == "Lodges");
                    return lodgeItem;
                case ItemType.airstrip:
                    var airstrip = countryRootItem.SubItems.FirstOrDefault(x => x.Description == "Airstrips");
                    return airstrip;

                default: return null;
            }
        }

        private SetupBaseTreeItem GetRootItem(ItemType type)
        {
            switch (type)
            {
                case ItemType.user:
                            var personnelItem = SetupItems.First(x => x.Description == "Personnel");
                            return personnelItem.SubItems.First(x => x.Description == "Users");
                case ItemType.aircraftList: var aircraftItem = SetupItems.FirstOrDefault(x => x.Description == "Aircraft");
                            return aircraftItem.SubItems.FirstOrDefault(x => x.Description == "List");
                case ItemType.pilot: personnelItem = SetupItems.First(x => x.Description == "Personnel");
                    return personnelItem.SubItems.FirstOrDefault(x => x.Description == "Pilots");
                case ItemType.aircraftType: var aircraftRootItem = SetupItems.FirstOrDefault(x => x.Description == "Aircraft");
                    return aircraftRootItem.SubItems.FirstOrDefault(x => x.Description == "Types");
                case ItemType.company:
                    var companyItem = SetupItems.FirstOrDefault(x => x.Description == "Companies");
                    return companyItem;
                default: return null;
            }
        }
        private void TreemMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var item = e.OriginalSource as RadMenuItem;
            var mnuText = item.Header as String;
            switch (mnuText)
            {
                case "Add Lodge": ShowAddNewLodge(SelectedTreeItem); break;
                case "Add Aircraft": ShowAddNewAircraft(SelectedTreeItem); break;
                case "Add Airstrip": ShowAddAirstrip(SelectedTreeItem); break;
                case "Add User": ShowAddUser(SelectedTreeItem); break;
                case "Add Pilot":ShowAddPilot(SelectedTreeItem);break;
                case "Add Flight": ShowAddFlight(SelectedTreeItem);break;
                case "Add Aircraft Type": ShowAddNewAircraftType(SelectedTreeItem);break;
                case "Add Company": ShowAddNewCompany(SelectedTreeItem);break;
                case "Find Aircraft": FindAircraft();break;
                case "Find Airstrip": FindAirstrip(); break;
                case "Find Lodge": FindLodge();break;
                case "Find Flight":FindFlight();break;
                case "Refresh": Refresh(SelectedTreeItem);break;
                case "Deactivate": Deactivate(SelectedTreeItem); break;
            }
        }
        private async void Deactivate(SetupBaseTreeItem selectedItem)
        {
            if (selectedItem != null)
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    switch (selectedItem.Type)
                    {
                        case ItemType.airstrip:
                            await AirstripInfo.DeactivateAirstrip(selectedItem.IDX, Server, Database);
                            var airstripInfo = AirstripList.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (airstripInfo!=null)
                            {
                                airstripInfo.IsActive = false;
                                AirstripInfo.UpdateCachedObject(airstripInfo);
                                var airstripRootItem = GetRootItem(ItemType.airstrip, selectedItem.ParentIDX);
                                RefreshAirstripList(airstripRootItem, airstripRootItem.ParentIDX);
                            }
                            break;
                        case ItemType.aircraftType:
                            await AircraftType.DeactivateAircraftType(selectedItem.IDX, Server, Database);
                            var acType = AircraftTypes.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (acType!=null)
                            {
                                acType.IsActive = false;
                                AircraftType.UpdateCachedObject(acType);
                                var acTypeRoot = GetRootItem(ItemType.aircraftType);
                                RefreshAircraftTypeList(acTypeRoot);
                            }
                            break;
                        case ItemType.aircraftList:
                            await AircraftInfo.DeactivateAircraft(selectedItem.IDX, Server, Database);
                            var acInfo = AircraftList.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (acInfo!=null)
                            {
                                acInfo.Active = false;
                                AircraftInfo.UpdateCachedObject(acInfo);
                                var acRootItem = GetRootItem(ItemType.aircraftList);
                                RefreshAircraftList(acRootItem);
                            };
                            break;
                        case ItemType.lodge:
                            await Lodge.DeactivateLodge(selectedItem.IDX, Server, Database);
                            var lodge = LodgeList.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (lodge!=null)
                            {
                                lodge.IsActive = false;
                                Lodge.UpdateCachedObject(lodge);
                                var lodgeRootItem = GetRootItem(ItemType.lodge, selectedItem.ParentIDX);
                                RefreshLodgeList(lodgeRootItem, lodgeRootItem.ParentIDX);

                            }
                            break;
                        case ItemType.flight:
                            await FlightInfo.DeactivateFlight(selectedItem.IDX, Server, Database);
                            var flight = FlightList.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (flight!=null)
                            {
                                flight.IsActive = false;
                                FlightInfo.UpdateCachedObject(flight);
                                var flightRootItem = GetRootItem(ItemType.flight, selectedItem.ParentIDX);
                                RefreshFlightList(flightRootItem, flightRootItem.ParentIDX);
                            }
                            break;
                        case ItemType.user:
                            await User.DeactivateUser(selectedItem.IDX, Server, Database);
                            var user = UserList.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (user != null)
                            {
                                user.Active = false;
                                User.UpdateCachedObject(user);
                                var userRootItem = GetRootItem(ItemType.user);
                                RefreshUserList(userRootItem);
                            }
                            break;
                        case ItemType.pilot:
                            await PilotInfo.DeactivatePilot(selectedItem.IDX, Server, Database);
                            var pilot = PilotList.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (pilot!=null)                           
                            {
                                pilot.Active = false;
                                PilotInfo.UpdateChachedObject(pilot);
                                var pilotRootItem = GetRootItem(ItemType.pilot);
                                RefreshPilotList(pilotRootItem);
                            }
                            break;
                        case ItemType.company:
                            await Company.DeactivateCompany(selectedItem.IDX, Server, Database);
                            var company = CompanyList.FirstOrDefault(x => x.IDX == selectedItem.IDX);
                            if (company != null)
                            {
                                company.IsActive = false;
                                Company.UpdateCachedObject(company);
                                var companyRootItem = GetRootItem(ItemType.company);
                                RefreshCompanyList(companyRootItem);
                            }
                            break;


                    }
                }
                ClearControlPanel();
            }

        }

        private async void Refresh(SetupBaseTreeItem parentItem)
        {
            if (parentItem != null)
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    switch (parentItem.ChildrenType)
                    {
                        case ItemType.company:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.airstrip, parentItem.ParentIDX);
                            await Company.LoadCompanyList(Server, Database, true);
                            RefreshCompanyList(parentItem);
                            break;
                        case ItemType.airstrip:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.airstrip, parentItem.ParentIDX);
                            await AirstripInfo.LoadAirstrips(Server, Database);
                            RefreshAirstripList(parentItem, parentItem.ParentIDX);
                            break;
                        case ItemType.aircraftType:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.aircraftType);
                            await AircraftType.LoadACTypes(Server, Database, true);                    
                            RefreshAircraftTypeList(parentItem);
                            break;
                        case ItemType.aircraftList:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.aircraftList);
                            await AircraftInfo.LoadAircraftList(Server, Database,true);
                            RefreshAircraftList(parentItem);
                            break;
                        case ItemType.lodge:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.flight, parentItem.ParentIDX);
                            await Lodge.LoadLodgeList(Server, Database,true);
                            RefreshLodgeList(parentItem, parentItem.ParentIDX);
                            break;
                        case ItemType.flight:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.flight, parentItem.ParentIDX);
                            await FlightInfo.LoadFlightList(Server, Database, true);
                            RefreshFlightList(parentItem, parentItem.ParentIDX);
                            break;
                        case ItemType.user:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.user);
                            await User.LoadUserList(Server, Database,true);
                            RefreshUserList(parentItem);
                            break;
                        case ItemType.pilot:
                            if (parentItem.Type != ItemType.Root)
                                parentItem = GetRootItem(ItemType.pilot);
                            await PilotInfo.LoadPilotInfo(Server, Database);
                            RefreshPilotList(parentItem);
                            break;
                              
                    }
                }
                ClearControlPanel();
            }
        }

        private async void FindFlight()
        {
            ClearControlPanel();
            try
            {
                var getTextWind = new GetTextWindow("Find Fight", "Flight description:");
                getTextWind.ShowDialog();
                if (getTextWind.DialogResult.HasValue && getTextWind.DialogResult.Value && !String.IsNullOrEmpty(getTextWind.InputText))
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        var flightInfo = await FlightInfo.FindFlight(getTextWind.InputText, Server, Database);
                        if (flightInfo != null)
                        {
                            currentCntrolVM = flightInfoCntrlVM;
                            flightInfoCntrlVM.Server = Server;
                            flightInfoCntrlVM.Database = Database;

                            flightInfoCntrlVM.Refresh(flightInfo);
                            flightInfoCntrlVM.IsVisible = true;
                        }
                        else
                            FailWindow.Display("flight not found");
                        NotifyPropertyChanged("ShowSaveButton");
                    }
                }
            }
            catch (Exception ex)
            {
                FailWindow.Display("An error has occurred :" + Environment.NewLine + ex.Message);
            }
        }

        private async void FindLodge()
        {
            ClearControlPanel();
            try
            {
                var getTextWind = new GetTextWindow("Find Lodge", "Lodge Name:");
                getTextWind.ShowDialog();
                if (getTextWind.DialogResult.HasValue && getTextWind.DialogResult.Value && !String.IsNullOrEmpty(getTextWind.InputText))
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        var lodgeInfo = await Lodge.FindLodge(getTextWind.InputText, Server, Database);
                        if (lodgeInfo != null)
                        {
                            currentCntrolVM = ldgeCntrlVM;
                            ldgeCntrlVM.Server = Server;
                            ldgeCntrlVM.Database = Database;

                            ldgeCntrlVM.Refresh(lodgeInfo);
                            ldgeCntrlVM.IsVisible = true;
                        }
                        else
                            FailWindow.Display("Lodge not found");
                        NotifyPropertyChanged("ShowSaveButton");
                    }
                }
            }
            catch (Exception ex)
            {

                FailWindow.Display("An error has occurred :" + Environment.NewLine + ex.Message);
            }
        }

        private async void FindAirstrip()
        {
            ClearControlPanel();

            try
            {
                var getTextWind = new GetTextWindow("Find Airstrip", "Airstrip Name:");
                getTextWind.ShowDialog();
                if (getTextWind.DialogResult.HasValue && getTextWind.DialogResult.Value && !String.IsNullOrEmpty(getTextWind.InputText))
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        var airstripInfo = await AirstripInfo.FindAirstrip(getTextWind.InputText, Server, Database);
                        if (airstripInfo != null)
                        {
                            currentCntrolVM = airstrpCntrlVM;
                            airstrpCntrlVM.Server = Server;
                            airstrpCntrlVM.Database = Database;
                            var tmpAirStripExForList = AllExForList.Where(x => x.IDX_Airstrip == airstripInfo.IDX).ToList();
                            airstrpCntrlVM.Refresh(airstripInfo, tmpAirStripExForList);
                            airstrpCntrlVM.IsVisible = true;
                        }
                        else
                             FailWindow.Display("Airstrip not found");
                        NotifyPropertyChanged("ShowSaveButton");
                    }
                }
            }
            catch (Exception ex)
            {

                FailWindow.Display("An error has occurred :" + Environment.NewLine + ex.Message);
            }
        }

        private async void FindAircraft()
        {
            ClearControlPanel();

            try
            {
                var getTextWind = new GetTextWindow("Find Aircraft", "Registration:");
                getTextWind.ShowDialog();
                if (getTextWind.DialogResult.HasValue && getTextWind.DialogResult.Value && !String.IsNullOrEmpty(getTextWind.InputText))
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        var aircraftInfo = await AircraftInfo.FindAircraft(getTextWind.InputText, Server, Database);
                        if (aircraftInfo != null)
                        {
                            currentCntrolVM = aircraftInfoCntrlVM;
                            aircraftInfoCntrlVM.Server = Server;
                            aircraftInfoCntrlVM.Database = Database;
                            aircraftInfoCntrlVM.Refresh(aircraftInfo);
                            aircraftInfoCntrlVM.IsVisible = true;


                            NotifyPropertyChanged("ShowSaveButton");
                        }

                        else
                            FailWindow.Display("Aircraft not found");
                    }
                }
            }
            catch (Exception ex)
            {
                FailWindow.Display("An error has occurred :" + Environment.NewLine + ex.Message);
            }
        }


        private void ShowAddFlight(SetupBaseTreeItem parentItem)
        {
            var newItemSetupView = new AddNewItemView(NewItemType.Flight, Server, Database, parentItem.ParentIDX);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;
            viewModel.WindowTitle = "Add Flight";
            var result = newItemSetupView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RefreshFlightList(parentItem, parentItem.ParentIDX);
            }
        }

        private void ShowAddUser(SetupBaseTreeItem parentItem)
        {
            var newItemSetupView = new AddNewItemView(NewItemType.User, Server, Database, parentItem.ParentIDX);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;
            viewModel.WindowTitle = "Add User";
            var result = newItemSetupView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RefreshUserList(parentItem);
            }
        }

        private void ShowAddPilot(SetupBaseTreeItem parentItem)
        {
            var newItemSetupView = new AddNewItemView(NewItemType.Pilot, Server, Database, parentItem.ParentIDX);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;
            viewModel.WindowTitle = "Add Pilot";
            var result = newItemSetupView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RefreshPilotList(parentItem);
            }
        }

        private void ShowAddAirstrip(SetupBaseTreeItem parentItem)
        {
            var newItemSetupView = new AddNewItemView(NewItemType.Airstrip, Server, Database, parentItem.ParentIDX);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;
            viewModel.WindowTitle = "Add Airstrip";
            var result = newItemSetupView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RefreshAirstripList(parentItem, parentItem.ParentIDX);
            }
        }

        private void ShowAddNewLodge(SetupBaseTreeItem parentItem)
        {
            var newItemSetupView = new AddNewItemView(NewItemType.Lodge, Server, Database, parentItem.ParentIDX);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;

            viewModel.WindowTitle = "Add Lodge";
            var result = newItemSetupView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RefreshLodgeList(parentItem, parentItem.ParentIDX);
            }

        }

        private void ShowAddNewCompany(SetupBaseTreeItem parentItem)
        {
            var newItemSetupView = new AddNewItemView(NewItemType.Company, Server, Database, parentItem.ParentIDX);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;

            viewModel.WindowTitle = "Add Company";
            var result = newItemSetupView.ShowDialog();
        }

        private void ShowAddNewAircraftType(SetupBaseTreeItem parentItem)
        {
           var newItemSetupView = new AddNewItemView(NewItemType.AircraftType, Server, Database, -1);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;

            viewModel.WindowTitle = "Add Aircraft Type";
            var result = newItemSetupView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RefreshAircraftTypeList(parentItem);
            }

        }
        private void ShowAddNewAircraft(SetupBaseTreeItem parentItem)
        {
            var newItemSetupView = new AddNewItemView(NewItemType.Aircraft, Server, Database, -1);
            var viewModel = newItemSetupView.DataContext as AddNewItemViewModel;

            viewModel.WindowTitle = "Add aircraft";
            var result = newItemSetupView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RefreshAircraftList(parentItem);
            }

        }
        
        private void RefreshCompanyList(SetupBaseTreeItem parentItem)
        {
            parentItem.SubItems.Clear();
            AddCompanyList(parentItem);
            parentItem.RefreshChildren();
        }
        private void RefreshFlightList(SetupBaseTreeItem parentItem, int countryIDX)
        {
            parentItem.SubItems.Clear();
            AddFlightItems(parentItem, countryIDX);
            parentItem.RefreshChildren();
        }

        private void RefreshPilotList(SetupBaseTreeItem parentItem)
        {
            parentItem.SubItems.Clear();
            AddPilotList(parentItem);
            parentItem.RefreshChildren();
        }

        private void RefreshAircraftTypeList(SetupBaseTreeItem parentItem)
        {
            
            parentItem.SubItems.Clear();
            AddAircraftTypes(parentItem);
            parentItem.RefreshChildren();
        }

        private void RefreshAircraftList(SetupBaseTreeItem parentItem)
        {
            parentItem.SubItems.Clear();
            AddAircraftList(parentItem);
            parentItem.RefreshChildren();
        }
        private void RefreshLodgeList(SetupBaseTreeItem parentItem, int countryIDX)
        {
            parentItem.SubItems.Clear();
            AddLodgeItems(parentItem, countryIDX);
            parentItem.RefreshChildren();
        }

        private void RefreshAirstripList(SetupBaseTreeItem parentItem, int countryIDX)
        {
            parentItem.SubItems.Clear();
            AddAirstripItems(parentItem, countryIDX);
            parentItem.RefreshChildren();
        }

        private void RefreshUserList(SetupBaseTreeItem parentItem)
        {
            parentItem.SubItems.Clear();
            AddUserList(parentItem);
            parentItem.RefreshChildren();
        }


        private void AddCompanyList(SetupBaseTreeItem companyRootItem)
        {
            if (CompanyList!=null)
            {
                var companies = CompanyList.Where(x=>x.IsActive).Select(x => new SetupBaseTreeItem { IDX = x.IDX, Type = ItemType.company, Description = x.Description, ImagePath = "/Schedwin.Setup;component/Images/company-16.png" }).ToList();
                companyRootItem.SubItems.AddRange(companies);
            }
        }

        private void AddPilotList(SetupBaseTreeItem pilotRootItem)
        {
            if (PilotList!=null)
            {
                var pilots = PilotList.Select(x => new SetupBaseTreeItem { IDX = x.IDX, Type = ItemType.pilot, Description = x.Name,
                                                                             ImagePath = "/Schedwin.Setup;component/Images/pilot16.png"}).ToList();
                pilotRootItem.SubItems.AddRange(pilots);
            }
        }

        private void AddUserList(SetupBaseTreeItem userRootItem)
        {
            if (UserList!=null)
            {
                var users = UserList.Where(x=>x.Active).Select(x=> new SetupBaseTreeItem { IDX = x.IDX, Type = ItemType.user,ChildrenType=ItemType.user, Description = x.FullName,
                                                                        ImagePath = "/Schedwin.Setup;component/Images/user216.png"});
                userRootItem.SubItems.AddRange(users);
            }
        }

        private void AddAircraftList(SetupBaseTreeItem aircraftInfoRoot)
        {
            if (AircraftList!=null)
            {
                var aircraftInfos = AircraftList.Where(x=>x.Active).Where(x=>x.Active).Select(x => new SetupBaseTreeItem { IDX = x.IDX, Type = ItemType.aircraftList,ChildrenType=ItemType.aircraftList,
                                                                                                                         Description = x.Registration,
                                                                                                                         ImagePath = "/Schedwin.Setup;component/Images/aircraft16.png"}).OrderBy(x => x.Description).ToList();
                aircraftInfoRoot.SubItems.AddRange(aircraftInfos);
            }
        }

        private void AddAircraftTypes(SetupBaseTreeItem aircraftTypeRoot)
        {
            if (AircraftTypes!=null)
            {
                var aircraftTypes= AircraftTypes.Where(x=>x.IsActive).Select(x => new SetupBaseTreeItem { IDX = x.IDX, Type = ItemType.aircraftType, Description = x.TypeName,
                                                                                     ImagePath = "/Schedwin.Setup;component/Images/Transport16.png" }).OrderBy(x => x.Description).ToList();
                aircraftTypeRoot.SubItems.AddRange(aircraftTypes);
            }
        }

        private void AddAirstripItems(SetupBaseTreeItem countryItem, int countryIDX)
        {
            if (AirstripList!=null)
            {
                var countryAirStrips = AirstripList.Where(x => x.IDX_Country == countryIDX).ToList();
                var airstripItems = countryAirStrips.Where(x=>x.IsActive).Select(x => new SetupBaseTreeItem { IDX = x.IDX, ParentIDX = countryItem.ParentIDX,
                                                                                        Type = ItemType.airstrip, ChildrenType=ItemType.airstrip,Description = x.Description + " (" + x.Code + ")",
                                                                                        ImagePath= "/Schedwin.Setup;component/Images/airport16.png"}).OrderBy(x => x.Description).ToList();
                countryItem.SubItems.AddRange(airstripItems);
            }
 
        }

        private void AddFlightItems(SetupBaseTreeItem countryItem, int countryIDX)
        {
            if (AirstripList != null)
            {
              
                var airstripItems = FlightList.Where(x=>x.IsActive).Select(x => new SetupBaseTreeItem
                {
                    IDX = x.IDX,
                    ChildrenType=ItemType.flight,
                    ParentIDX = countryItem.ParentIDX,
                    Type = ItemType.flight,
                    Description = x.Code,
                    ImagePath = "/Schedwin.Setup;component/Images/Flight16.png"
                }).OrderBy(x => x.Description).ToList();
                countryItem.SubItems.AddRange(airstripItems);
            }

        }

        private void AddLodgeItems(SetupBaseTreeItem countryItem, int countryIDX)
        {
            if (LodgeList!=null)
            {

                var countryLodges = LodgeList.Where(x => x.IDX_Country == countryIDX ).ToList();
                var lodgeItems = countryLodges.Where(x=>x.IsActive).Select(x => new SetupBaseTreeItem { IDX = x.IDX,ChildrenType=ItemType.lodge ,ParentIDX=countryItem.ParentIDX,
                                                    Type = ItemType.lodge, Description = x.Name,ImagePath= "/Schedwin.Setup;component/Images/hotel16.png"
                }).OrderBy(x => x.Description).ToList();
                countryItem.SubItems.AddRange(lodgeItems);
            }

        }



        public bool Validate()
        {
           var result= currentCntrolVM.Validate();
            if (!result)
                StatusText = "Validation failed";
            return result;
        }

        public async void Save()
        {
           StatusText = "Saving..";
           var result= await  currentCntrolVM.Save();

            StatusText =  result ? "Done." : "Error.";
        }
    }
}
