using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data.Classes;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Interop;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;

namespace Schedwin.Setup
{
    public class DataDocumentsCnrlViewModel :ViewModelBase
    {
        public RangeObservableCollection<String> DataDocumentTypes { get; set; }

        public RangeObservableCollection<DataDocument> AllDataDocuments { get; set; }
        public RangeObservableCollection<DataDocument> FilteredDataDocuments { get; set; }

        public DataDocument SelectedDataDocument { get; set; }

        public String SelectedDataDocumentType { get; set; }

        public ImageSource ImageSrc { get; set; }

        public bool CanAdd { get; set; }

        public DataDocumentsCnrlViewModel()
        {
            DataDocumentTypes = new RangeObservableCollection<string>();
            AllDataDocuments = new RangeObservableCollection<DataDocument>();
            FilteredDataDocuments = new RangeObservableCollection<DataDocument>();
            CanAdd = false;
        }

        public bool CanEditEditDate { get; set; }

        public DateTime SelectedExpiryDate { get; set; }

        public void RefreshDataDocuments()
        {
            FilteredDataDocuments.Clear();
            var tmpLst=AllDataDocuments.Where(x => x.TypeName == SelectedDataDocumentType).ToList();
            FilteredDataDocuments.AddRange(tmpLst);
            CanAdd = true;
            NotifyPropertyChanged("FilteredDataDocuments");
            NotifyPropertyChanged("CanAdd");
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

                    var newDocument = new DataDocument();
                    newDocument.IsNew = true;
                    newDocument.TypeName = SelectedDataDocumentType;
                    newDocument.Title = openFileDialog.SafeFileName;
                    newDocument.DocumentImage = ms.ToArray();
                    AllDataDocuments.Insert(0, newDocument);
                    RefreshDataDocuments();
                    SelectedDataDocument = newDocument;
                }

                NotifyPropertyChanged("SelectedDataDocument");
                NotifyPropertyChanged("ImageSrc");
            }
        }

        public void RefreshDocumentImage()
        {
            if (SelectedDataDocument != null)
            {
                var byteArrayImage = SelectedDataDocument.DocumentImage;

                if (byteArrayImage != null && byteArrayImage.Length > 0)
                {
                    var ms = new MemoryStream(byteArrayImage);

                    var bitmapImg = new BitmapImage();

                    bitmapImg.BeginInit();
                    bitmapImg.StreamSource = ms;
                    bitmapImg.EndInit();

                    ImageSrc = bitmapImg;
                    NotifyPropertyChanged("ImageSrc");
                }

            }
        }

        public async Task LoadDocuments(int parentIDX, String invType,String server, String database)

        {
            SelectedDataDocument = null;
            SelectedDataDocumentType = null;
            ImageSrc = null;
            CanAdd = false;

            FilteredDataDocuments.Clear();
            NotifyPropertyChanged("ImageSrc");
            NotifyPropertyChanged("SelectedDataDocument");
            NotifyPropertyChanged("SelectedDataDocumentType");
            NotifyPropertyChanged("FilteredDataDocuments");
            NotifyPropertyChanged("CanAdd");

            AllDataDocuments.Clear();
            var tmpLst = await DataDocument.GetDataDocuments(parentIDX,invType, server, database);
            AllDataDocuments.AddRange(tmpLst);
            NotifyPropertyChanged("AllDataDocuments");
        }

        public async Task Save(int parentIDX, String invType, String server, String database)
        {
            if (SelectedDataDocument == null)
            {
                var newItems = AllDataDocuments.Where(x => x.IsNew).ToList();
                await DataDocument.SaveDataDocuments(newItems, parentIDX, invType, server, database);
            }
            else
            {
                await DataDocument.UpDateDataDocument(parentIDX, invType, SelectedDataDocument, SelectedExpiryDate, server, database);
            }

        }
        public void Init()
        {
            SelectedDataDocument = null;
            SelectedDataDocumentType = null;
            ImageSrc = null;
            CanAdd = false;

            DataDocumentTypes.Clear();
            DataDocumentTypes.Add("CofA");
            DataDocumentTypes.Add("CofM");
            DataDocumentTypes.Add("CofR");
            DataDocumentTypes.Add("Insurance");
            DataDocumentTypes.Add("Log Airframe");
            DataDocumentTypes.Add("Log Engine");
            DataDocumentTypes.Add("Log Prop");
            DataDocumentTypes.Add("Mass and Balance");
            DataDocumentTypes.Add("Radio");

            NotifyPropertyChanged("DataDocumentTypes");
        }


    }
}
