using Microsoft.Win32;
using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Schedwin.Setup

 {
    public class PilotInfoCntrlViewModel : ItemCntrlViewModelBase
    {

        public bool IsActive { get; set; }
        public User SelectedPerson { get; set; }

        public String Nationality { get; set; }

        public String Address { get; set; }

        public String PassportNo { get; set; }

        public String ResidenceNo { get; set; }

        public String WorkPermitNo { get; set; }

        public String Telephone { get; set; }
        public short PilotWeight { get; set; }

        public DateTime? PassportExpiry { get; set; }

        public DateTime? ResidencePermitExpiry { get; set; }

        public String PilotLicenceNo { get; set; }

        public DateTime? WorkPermitExpiry { get; set; }

        public DateTime? PilotLicenceExpiry { get; set; }


        public double StartFlyingHours { get; set; }


         public DateTime? StartDate { get; set; }

        public String ContactName { get; set; }

        public String ContactCompany { get; set; }

        public String ContactAddress { get; set; }

        public String ContactPhone { get; set; }

        public DateTime? cOfT { get; set; }
        public DateTime? cotC208 { get; set; }
        public DateTime? cotC206 { get; set; }

        public DateTime? routeCheck { get; set; }
        public DateTime? cotPC12 { get; set; }

        public DateTime? cotC510 { get; set; }
        public DateTime? cotGA8 { get; set; }

        public DateTime? cotC172 { get; set; }
        public DateTime? cotC210 { get; set; }

        public DateTime? cotC402 { get; set; }
        public DateTime? cotC310 { get; set; }

        public ImageSource LicenceImgSrc { get; set; }

        public Byte[] LicenceImgByteArray { get; set; }

        public RangeObservableCollection<User>  UserList { get; set; }

        public RangeObservableCollection<ExpiryRatingGridItem> ExpiryGridItems { get; set; }

        public PilotInfoCntrlViewModel()
        {
            UserList = new RangeObservableCollection<User>();
            ExpiryGridItems = new RangeObservableCollection<ExpiryRatingGridItem>();

            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "Medical", RatingNotApplicable=true });
            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "Instrument Rating" });
            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "Instructor Rating" });
            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "Dangerous Goods" });
            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "CRM" });
            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "SEPT" });
            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "SMS" });
            ExpiryGridItems.Add(new ExpiryRatingGridItem { Name = "OPC" });

            NotifyPropertyChanged("ExpiryGridItems");
        }

        public override bool Validate()
        {
            if (ValidateGridItems())
            {
                var invalidFieldLst = new List<String>();

                if (SelectedPerson == null)
                    invalidFieldLst.Add("Person");

                if (String.IsNullOrEmpty(Nationality))
                    invalidFieldLst.Add("Nationality");


                if (String.IsNullOrEmpty(PassportNo))
                    invalidFieldLst.Add("Passport No");

                if (String.IsNullOrEmpty(Address))
                    invalidFieldLst.Add("Address");


                if (String.IsNullOrEmpty(Telephone))
                    invalidFieldLst.Add("Telephone");


                if (String.IsNullOrEmpty(PilotLicenceNo))
                    invalidFieldLst.Add("Pilot Licence No");


                if (PilotLicenceExpiry == null)
                    invalidFieldLst.Add("Pilot Licence Expiry");

                if (ResidencePermitExpiry == null)
                    invalidFieldLst.Add("Residence Permit Expiry");

                if (WorkPermitExpiry == null)
                    invalidFieldLst.Add("Work Permit Expiry");

                if (String.IsNullOrEmpty(WorkPermitNo))
                    invalidFieldLst.Add("Work Permit No");

                if (String.IsNullOrEmpty(ContactName))
                    invalidFieldLst.Add("Contact Name");

                if (String.IsNullOrEmpty(ContactAddress))
                    invalidFieldLst.Add("Contact Address");

                if (String.IsNullOrEmpty(ContactPhone))
                    invalidFieldLst.Add("Contact Phone");


                if (String.IsNullOrEmpty(ContactCompany))
                    invalidFieldLst.Add("Contact Company");


                if (StartDate == null)
                    invalidFieldLst.Add("Start Date");


                if (cOfT == null)
                    invalidFieldLst.Add("cOfT");


                if (routeCheck == null)
                    invalidFieldLst.Add("Route Check");


                if (invalidFieldLst.Count > 0)
                {
                    String validFailMess = "The following fields need to be completed:" + Environment.NewLine;
                    String strfieldList = String.Join(Environment.NewLine, invalidFieldLst);
                    validFailMess += strfieldList;

                    FailWindow.Display(validFailMess);
                    return false;

                }
            }  

            return true;

        }


        public async override Task<bool> Save()
        {
            try
            {
                var pilotInfo = new PilotInfo();
                pilotInfo.IsNew = IsNew;
                pilotInfo.IDX = IDX;
                pilotInfo.IDX_Personnel = SelectedPerson.IDX;
                pilotInfo.Nationality = Nationality;
                pilotInfo.PilotsAddress = Address;
                pilotInfo.PassportNumber = PassportNo;
                pilotInfo.Weight = PilotWeight;
                pilotInfo.PilotsTelephone = Telephone;

                pilotInfo.ResidencePermitNumber = ResidenceNo;
                pilotInfo.WorkPermitNumber = WorkPermitNo;
                pilotInfo.PassportExpiryDate = PassportExpiry.Value;
                pilotInfo.ResidencePermitExpiryDate = ResidencePermitExpiry.Value;
                pilotInfo.WorkPermitExpiryDate = WorkPermitExpiry.Value;

                pilotInfo.InstructorRatingExpiryDate = GetGridItem("Instructor Rating").Date;
                pilotInfo.InstrumentRatingExpiryDate = GetGridItem("Instrument Rating").Date;
                pilotInfo.DangerousGoodsExpiryDate = GetGridItem("Dangerous Goods").Date;
                pilotInfo.CRMExpiryDate = GetGridItem("CRM").Date;
                pilotInfo.MedicalExpiryDate = GetGridItem("Medical").Date;
                pilotInfo.SEPTExpiryDate = GetGridItem("SEPT").Date;
                pilotInfo.SMSExpiryDate = GetGridItem("SMS").Date;
                pilotInfo.OPCExpiryDate = GetGridItem("OPC").Date;




                pilotInfo.StartingDate = StartDate.Value;
                pilotInfo.StartingFlyingHours = StartFlyingHours;
                pilotInfo.PilotsLicenseNumber = PilotLicenceNo;
                pilotInfo.PilotsLicenseExpiryDate = PilotLicenceExpiry.Value;
                pilotInfo.RouteCheck = routeCheck.Value;

                pilotInfo.InstructorRating = GetGridItem("Instructor Rating").Rating;
                pilotInfo.InstrumentRating = GetGridItem("Instrument Rating").Rating;
                pilotInfo.DangerousGoods = GetGridItem("Dangerous Goods").Rating;
                pilotInfo.CRM = GetGridItem("CRM").Rating;
                pilotInfo.SEPT = GetGridItem("SEPT").Rating;
                pilotInfo.SMS = GetGridItem("SMS").Rating;
                pilotInfo.OPC = GetGridItem("OPC").Rating;

                pilotInfo.LicenceImage = this.LicenceImgByteArray;

                pilotInfo.ContactName = ContactName;
                pilotInfo.ContactCompany = ContactCompany;
                pilotInfo.ContactAddress = ContactAddress;
      
                pilotInfo.ContactTelephone = ContactPhone;

                pilotInfo.CofT = cOfT.Value;
                pilotInfo.COfT208 = cotC208;
                pilotInfo.COfT206 = cotC206;
                pilotInfo.COfTPC12 = cotPC12;
                pilotInfo.COfT510 = cotC510;
                pilotInfo.CofTGA8 = cotGA8;
                pilotInfo.CofTC172 = cotC172;
                pilotInfo.CofTC210 = cotC210;
                pilotInfo.COfT402 = cotC402;
                pilotInfo.COfT310 = cotC310;

                pilotInfo.Active = IsActive;


                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    if (PilotLicenceExpiry.HasValue)
                        await PilotRoster.UpdateLicencyExpiry(IDX, PilotLicenceExpiry.Value, Server, Database);
                    await pilotInfo.Save(Server, Database);
                }

                PilotInfo.UpdateChachedObject(pilotInfo);

                return true;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save pilot info:"+ Environment.NewLine+ exMessage);
                return false;
            }
        }

        public override void Init()
        {
            UserList.Clear();
            UserList.AddRange(User.GetUserList());
            IsActive = true;
            IsNew = true;
            PassportExpiry = null;
            ResidencePermitExpiry = null;
            PilotLicenceNo = null;
            WorkPermitExpiry = null;
            PilotLicenceExpiry = null;
            StartDate = null;
            cotC208 = null;
            cotC206 = null;
            routeCheck = null;
            cotPC12 = null;
            cotC510 = null;
            cotGA8 = null;
            cotC172 = null;
            cotC210 = null;
            cotC402 = null;
            cotC310 = null;

            UpdateGridRatingItem("Medical", true, DateTime.Today);

            NotifyPropertyChanged("cotC310");
            NotifyPropertyChanged("cotC402");
            NotifyPropertyChanged("cotC210");
            NotifyPropertyChanged("cotC172");
            NotifyPropertyChanged("cotGA8");
            NotifyPropertyChanged("cotC510");
            NotifyPropertyChanged("cotPC12");
            NotifyPropertyChanged("cotC206");
            NotifyPropertyChanged("cotC208");
            NotifyPropertyChanged("PassportExpiry");
            NotifyPropertyChanged("ResidencePermitExpiry");
            NotifyPropertyChanged("WorkPermitExpiry");
            NotifyPropertyChanged("PilotLicenceExpiry");
            NotifyPropertyChanged("InstrumentRatingExpiry");
            NotifyPropertyChanged("InstructorRatingExpiry");
            NotifyPropertyChanged("DangerousGoodsExpiry");
            NotifyPropertyChanged("CRMExpiry");
            NotifyPropertyChanged("MedicalExpiry");
            NotifyPropertyChanged("StartDate");
        } 
     

        public void Refresh(PilotInfo  pilotInfo)
        {
            IsNew = false;
            UserList.Clear();
            UserList.AddRange(User.GetUserList());

            SelectedPerson = UserList.FirstOrDefault(x => x.IDX == pilotInfo.IDX_Personnel);
            IDX = pilotInfo.IDX;
            IsActive = pilotInfo.Active;
            Nationality = pilotInfo.Nationality;
            Address = pilotInfo.PilotsAddress;
            PassportNo = pilotInfo.PassportNumber;
            PilotWeight = pilotInfo.Weight;
            Telephone = pilotInfo.PilotsTelephone;

            ResidenceNo = pilotInfo.ResidencePermitNumber;
            WorkPermitNo = pilotInfo.WorkPermitNumber;
            PassportExpiry = pilotInfo.PassportExpiryDate;
            ResidencePermitExpiry = pilotInfo.ResidencePermitExpiryDate;
            WorkPermitExpiry = pilotInfo.WorkPermitExpiryDate;


            UpdateGridRatingItem("Medical", true, pilotInfo.MedicalExpiryDate);
            UpdateGridRatingItem("Instrument Rating", pilotInfo.InstrumentRating, pilotInfo.InstrumentRatingExpiryDate);
            UpdateGridRatingItem("Instructor Rating", pilotInfo.InstructorRating, pilotInfo.InstructorRatingExpiryDate);
            UpdateGridRatingItem("Dangerous Goods" , pilotInfo.DangerousGoods, pilotInfo.DangerousGoodsExpiryDate);
            UpdateGridRatingItem("CRM", true, pilotInfo.CRMExpiryDate);
            UpdateGridRatingItem("SEPT", pilotInfo.SEPT, pilotInfo.SEPTExpiryDate);
            UpdateGridRatingItem("SMS", pilotInfo.SMS, pilotInfo.SMSExpiryDate);
            UpdateGridRatingItem("OPC", pilotInfo.OPC, pilotInfo.OPCExpiryDate);


            StartDate = pilotInfo.StartingDate;
            StartFlyingHours = pilotInfo.StartingFlyingHours;
            PilotLicenceNo = pilotInfo.PilotsLicenseNumber;


            PilotLicenceExpiry = pilotInfo.PilotsLicenseExpiryDate;
            routeCheck = pilotInfo.RouteCheck;
            ContactName = pilotInfo.ContactName;
            ContactCompany = pilotInfo.ContactCompany;
            ContactAddress = pilotInfo.ContactAddress;
            ContactPhone = pilotInfo.ContactTelephone;

            cOfT = pilotInfo.CofT;
            cotC208 = pilotInfo.COfT208;
            cotC206 = pilotInfo.COfT206;

            cotPC12 = pilotInfo.COfTPC12;
            cotC510 = pilotInfo.COfT510;
            cotGA8 = pilotInfo.CofTGA8;
            cotC172 = pilotInfo.CofTC172;
            cotC210 = pilotInfo.CofTC210;
            cotC402 = pilotInfo.COfT402;
            cotC310 = pilotInfo.COfT310;

           if (pilotInfo.LicenceImage!=null)
            {
                var byteArrayImage = pilotInfo.LicenceImage;
                if (byteArrayImage != null && byteArrayImage.Length > 0)
                {
                    var ms = new MemoryStream(byteArrayImage);

                    var bitmapImg = new BitmapImage();

                    bitmapImg.BeginInit();
                    bitmapImg.StreamSource = ms;
                    bitmapImg.EndInit();

                    this.LicenceImgSrc = bitmapImg;
                    this.LicenceImgByteArray = ms.ToArray();

                }
            }
            else
            {
                this.LicenceImgSrc =null;
                this.LicenceImgByteArray = null;
            }
            NotifyPropertyChanged("LicenceImgSrc");
            NotifyPropertyChanged("IsActive");
            NotifyPropertyChanged("Nationality");
            NotifyPropertyChanged("Address");
            NotifyPropertyChanged("PassportNo");
            NotifyPropertyChanged("PilotWeight");
            NotifyPropertyChanged("Telephone");
            NotifyPropertyChanged("WorkPermitNo");
            NotifyPropertyChanged("ResidenceNo");
            NotifyPropertyChanged("PassportNo");
            NotifyPropertyChanged("SelectedPerson");
            NotifyPropertyChanged("UserList");
            NotifyPropertyChanged("PilotLicenceNo");
            NotifyPropertyChanged("cOfT");
            NotifyPropertyChanged("cotC310");
            NotifyPropertyChanged("cotC402");
            NotifyPropertyChanged("cotC210");
            NotifyPropertyChanged("cotC172");
            NotifyPropertyChanged("cotGA8");
            NotifyPropertyChanged("cotC510");
            NotifyPropertyChanged("cotPC12");
            NotifyPropertyChanged("cotC206");
            NotifyPropertyChanged("cotC208");
            NotifyPropertyChanged("PassportExpiry");
            NotifyPropertyChanged("ResidencePermitExpiry");
            NotifyPropertyChanged("WorkPermitExpiry");
            NotifyPropertyChanged("PilotLicenceExpiry");
            NotifyPropertyChanged("MedicalExpiry");
            NotifyPropertyChanged("StartFlyingHours");
            NotifyPropertyChanged("StartDate");
            NotifyPropertyChanged("ContactPhone");
            NotifyPropertyChanged("ContactCompany");
            NotifyPropertyChanged("ContactName");
            NotifyPropertyChanged("ContactAddress");
            NotifyPropertyChanged("routeCheck");
        }

        public bool ValidateGridItems()
        {
            foreach (var item in ExpiryGridItems)
            {
                if (item.Rating && item.Date==null)
                {
                    FailWindow.Display(item.Name + " date must be selected");
                    return false;
                }
            }

            return true;
        }

        public ExpiryRatingGridItem GetGridItem(String ratingName)
        {
            var gridItem = this.ExpiryGridItems.FirstOrDefault(x => x.Name == ratingName);
            return gridItem;
        }

        private void UpdateGridRatingItem(String ratingName, bool rating, DateTime? expiryDate)
        {
            var gridItem = this.ExpiryGridItems.FirstOrDefault(x => x.Name == ratingName);
            if (gridItem!=null)
            {
                gridItem.Rating = rating;
                gridItem.Date = expiryDate;
            }
        }

        public void AddNewDocument()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap files (*.bmp)|*.txt| JPEG files (*.jpg)|*.jpg| PNG files (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                var image = Image.FromFile(openFileDialog.FileName);
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();

                    this.LicenceImgByteArray = ms.ToArray();
                    this.LicenceImgSrc = bitmapImage;
                }


                NotifyPropertyChanged("LicenceImgSrc");
            }
        }

    }
}
