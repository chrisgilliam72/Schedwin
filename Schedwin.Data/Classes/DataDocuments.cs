using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class DataDocument
    {
        public bool IsNew { get; set; }
        public String Title { get; set; }
        public String TypeName { get; set; }
        public Byte[] DocumentImage { get; set; } 

        public String InvType { get; set; }

        public int IDX { get; set; }

        public static explicit operator DataDocument(timg_Documents dbImgDocument)
        {
            var dataDoc = new DataDocument ();
            dataDoc.IDX = dbImgDocument.IDX;
            dataDoc.IsNew = false;
            dataDoc.TypeName = dbImgDocument.Name;
            dataDoc.Title = dbImgDocument.Title;
            dataDoc.DocumentImage = dbImgDocument.Document;
            dataDoc.InvType = dbImgDocument.Type;
            return dataDoc;
        }
        
        public static explicit operator timg_Documents(DataDocument dataDocument)
        {
            var timgDoc = new timg_Documents();
            timgDoc.IDX = dataDocument.IDX;
            timgDoc.Name = dataDocument.TypeName;
            timgDoc.PAge = 1;
            timgDoc.Type = "";
            timgDoc.Title = dataDocument.Title;
            timgDoc.Group = "Default";
            timgDoc.ExpDate = new DateTime(2050, 01, 01);
            timgDoc.Document = dataDocument.DocumentImage;
            timgDoc.Type = dataDocument.InvType;
            return timgDoc;

        }

        public static  async Task<List<DataDocument>> GetDataDocuments(int parentIDX, String invType,String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dbDocuments =await ctx.timg_Documents.Where(x => x.IDX_Parent == parentIDX && x.Type==invType).ToListAsync();
                var dataDocs = dbDocuments.Select(x => (DataDocument)x).OrderBy(x=>x.Title).ToList();
                return dataDocs;
            }

          
        }

        public static async Task SaveDataDocuments(List<DataDocument> newDocuments,int parentIDX, String invType, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dbImgDocList=newDocuments.Select(x => (timg_Documents)x).ToList();
                dbImgDocList.ForEach(x => x.IDX_Parent = parentIDX);
                dbImgDocList.ForEach(x => x.Type=invType);
                ctx.timg_Documents.AddRange(dbImgDocList);
                await ctx.SaveChangesAsync();
            }
        }

        public static async Task UpDateDataDocument(int parentIDX, String invType, DataDocument SelectedDocument, DateTime SelectedExpiryDate, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {

                var result = ctx.timg_Documents.FirstOrDefault(x => x.IDX_Parent == parentIDX && x.Type == invType && x.IDX == SelectedDocument.IDX);

                if (result != null)
                {
                    result.ExpDate = SelectedExpiryDate;
                }

                await ctx.SaveChangesAsync();
            }
        }

    }
}
