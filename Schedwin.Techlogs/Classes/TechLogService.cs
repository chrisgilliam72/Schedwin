using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Common;
using Schedwin.Data;

namespace Schedwin.Techlogs
{
    public class TechLogService : ViewModelBase
    {
        public bool IsModified { get; set; }
        public bool IsNew { get; set; }
        public int IDX { get; set; }
        public int IDX_TechLogID { get; set; }
        public DateTime Date { get; set; }
        public int IDX_Setup_Aircraft_Details { get; set; }
        public double? TTSinceCOfA { get; set; }
        public double? TTAirframe { get; set; }
        public double? TTEngineOH { get; set; }
        public double? TTEngine { get; set; }
        public double? TTPropOH { get; set; }
        public double? TTProp { get; set; }
        public double? TTSinceCOfM { get; set; }
        public double? TTEngine2OH { get; set; }
        public double? TTEngine2 { get; set; }
        public double? TTProp2OH { get; set; }
        public double? TTProp2 { get; set; }
        public bool DidTTSinceCofA { get; set; }
        public bool DIDTTSinceCofM { get; set; }
        public bool DidTTEngineOH { get; set; }
        public bool DidTTEngine { get; set; }
        public bool DidTTPropOH { get; set; }
        public bool DidTTProp { get; set; }
        public bool DidTTEngine2OH { get; set; }
        public bool DidTTEngine2 { get; set; }
        public bool DidTTProp2OH { get; set; }
        public bool DidTTProp2 { get; set; }


        public void Update(TechLogService lastTechlogService, double tachHours)
        {
            if (lastTechlogService.TTSinceCOfA != null)
                TTSinceCOfA = lastTechlogService.TTSinceCOfA+tachHours;

            if (lastTechlogService.TTAirframe != null)
                TTAirframe = lastTechlogService.TTAirframe+tachHours;

            if (lastTechlogService.TTEngineOH != null)
                TTEngineOH = lastTechlogService.TTEngineOH+tachHours;

            if (lastTechlogService.TTEngine != null)
                TTEngine = lastTechlogService.TTEngine+ tachHours;

            if (lastTechlogService.TTPropOH != null)
                TTPropOH = lastTechlogService.TTPropOH+tachHours;

            if (lastTechlogService.TTSinceCOfM != null)
                TTSinceCOfM = lastTechlogService.TTSinceCOfM+ tachHours;

            if (lastTechlogService.TTEngine2OH != null)
                TTEngine2OH = lastTechlogService.TTEngine2OH+tachHours;

            if (lastTechlogService.TTEngine2 != null)
                TTEngine2 = lastTechlogService.TTEngine2+tachHours;

            if (lastTechlogService.TTProp2OH != null)
                TTProp2OH = lastTechlogService.TTProp2OH+ tachHours;

            if (lastTechlogService.TTProp != null)
                TTProp = lastTechlogService.TTProp + tachHours;

            if (lastTechlogService.TTProp2 != null)
                TTProp2 = lastTechlogService.TTProp2+tachHours;

            NotifyPropertyChanged("IDX_TechLogID");
            NotifyPropertyChanged("TTSinceCOfA");
            NotifyPropertyChanged("TTAirframe");
            NotifyPropertyChanged("TTEngineOH");
            NotifyPropertyChanged("TTEngine");
            NotifyPropertyChanged("TTPropOH");
            NotifyPropertyChanged("TTSinceCOfM");
            NotifyPropertyChanged("TTEngine2OH");
            NotifyPropertyChanged("TTEngine2");
            NotifyPropertyChanged("TTProp2OH");
            NotifyPropertyChanged("TTProp2");
        }


        public static explicit operator TechLogService(T_Techlog_Services t_techlogService)
        {
            var techLogService = new TechLogService();
            techLogService.IsNew = false;
            techLogService.IDX = t_techlogService.IDX;
            techLogService.IDX_TechLogID = t_techlogService.TechlogID.Value;
            techLogService.Date = t_techlogService.Date;
            techLogService.IDX_Setup_Aircraft_Details = t_techlogService.IDX_Setup_Aircraft_Details;
            techLogService.TTSinceCOfA = t_techlogService.TTSinceCOfA;
            techLogService.TTSinceCOfM = t_techlogService.TTSinceCOfM;
            techLogService.TTAirframe = t_techlogService.TTAirframe;
            techLogService.TTEngineOH = t_techlogService.TTEngineOH;
            techLogService.TTEngine = t_techlogService.TTEngine;
            techLogService.TTPropOH = t_techlogService.TTPropOH;
            techLogService.TTProp = t_techlogService.TTProp;
            techLogService.DIDTTSinceCofM = t_techlogService.DIDTTSinceCofM;
            techLogService.TTEngine2OH = t_techlogService.TTEngine2OH;
            techLogService.TTEngine2 = t_techlogService.TTEngine2;
            techLogService.TTProp2OH = t_techlogService.TTProp2OH;
            techLogService.TTProp2 = t_techlogService.TTProp2;
            techLogService.DidTTSinceCofA = t_techlogService.DidTTSinceCofA;
            techLogService.DIDTTSinceCofM = t_techlogService.DIDTTSinceCofM;
            techLogService.DidTTEngineOH = t_techlogService.DidTTEngineOH;
            techLogService.DidTTEngine = t_techlogService.DidTTEngine;
            techLogService.DidTTPropOH = t_techlogService.DidTTProp2OH;
            techLogService.DidTTProp = t_techlogService.DidTTProp;
            techLogService.DidTTEngine2OH = t_techlogService.DidTTEngine2OH;
            techLogService.DidTTEngine2 = t_techlogService.DidTTEngine2;
            techLogService.DidTTProp2OH = t_techlogService.DidTTEngine2;
            techLogService.DidTTProp2 = t_techlogService.DidTTProp2;
            return techLogService;
        }

        public static explicit operator T_Techlog_Services(TechLogService techLogService)
        {
            var t_Techlog_Services = new T_Techlog_Services();
            t_Techlog_Services.IDX = techLogService.IDX;
            t_Techlog_Services.TechlogID = techLogService.IDX_TechLogID;
            t_Techlog_Services.Date = techLogService.Date;
            t_Techlog_Services.CoCode = "SEFO";
            t_Techlog_Services.IDX_Setup_Aircraft_Details = techLogService.IDX_Setup_Aircraft_Details;
            t_Techlog_Services.TTSinceCOfA = techLogService.TTSinceCOfA;
            t_Techlog_Services.TTSinceCOfM = techLogService.TTSinceCOfM;
            t_Techlog_Services.TTAirframe = techLogService.TTAirframe;
            t_Techlog_Services.TTEngineOH = techLogService.TTEngineOH;
            t_Techlog_Services.TTEngine = techLogService.TTEngine;
            t_Techlog_Services.TTPropOH = techLogService.TTPropOH;
            t_Techlog_Services.TTProp = techLogService.TTProp;
            t_Techlog_Services.DIDTTSinceCofM = techLogService.DIDTTSinceCofM;
            t_Techlog_Services.TTEngine2OH = techLogService.TTEngine2OH;
            t_Techlog_Services.TTEngine2 = techLogService.TTEngine2;
            t_Techlog_Services.TTProp2OH = techLogService.TTProp2OH;
            t_Techlog_Services.TTProp2 = techLogService.TTProp2;
            t_Techlog_Services.DidTTSinceCofA = techLogService.DidTTSinceCofA;
            t_Techlog_Services.DIDTTSinceCofM = techLogService.DIDTTSinceCofM;
            t_Techlog_Services.DidTTEngineOH = techLogService.DidTTEngineOH;
            t_Techlog_Services.DidTTEngine = techLogService.DidTTEngine;
            t_Techlog_Services.DidTTPropOH = techLogService.DidTTProp2OH;
            t_Techlog_Services.DidTTProp = techLogService.DidTTProp;
            t_Techlog_Services.DidTTEngine2OH = techLogService.DidTTEngine2OH;
            t_Techlog_Services.DidTTEngine2 = techLogService.DidTTEngine2;
            t_Techlog_Services.DidTTProp2OH = techLogService.DidTTEngine2;
            t_Techlog_Services.DidTTProp2 = techLogService.DidTTProp2;
            return t_Techlog_Services;
        }


        public static async Task Save(List<TechLogService> listServices, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var modifiedEntries = listServices.Where(x => x.IsModified && !x.IsNew).ToList();
       
            var newEntries = listServices.Where(x => x.IsNew).ToList();

            using (ctx)
            {
                if (modifiedEntries!=null && modifiedEntries.Count >0)
                {
                    var modifiedIDs = modifiedEntries.Select(x => x.IDX).ToList();
                    var dbEntriues = await ctx.T_Techlog_Services.Where(x => modifiedIDs.Contains(x.IDX)).ToListAsync();


                    foreach (var updatedEntry in modifiedEntries)
                    {
                        var dbTechLogService = dbEntriues.FirstOrDefault(x => x.IDX == updatedEntry.IDX);
                        if (dbTechLogService != null)
                        {


                            dbTechLogService.TTSinceCOfA = updatedEntry.TTSinceCOfA;
                            dbTechLogService.TTAirframe = updatedEntry.TTAirframe;
                            dbTechLogService.TTEngineOH = updatedEntry.TTEngineOH;
                            dbTechLogService.TTEngine = updatedEntry.TTEngine;
                            dbTechLogService.TTPropOH = updatedEntry.TTPropOH;
                            dbTechLogService.TTProp = updatedEntry.TTProp;
                            dbTechLogService.TTSinceCOfA = updatedEntry.TTSinceCOfA;
                            dbTechLogService.TTSinceCOfM = updatedEntry.TTSinceCOfM;
                            dbTechLogService.TTEngine2OH = updatedEntry.TTEngine2OH;
                            dbTechLogService.TTEngine2 = updatedEntry.TTEngine2;
                            dbTechLogService.TTProp2OH = updatedEntry.TTProp2OH;
                            dbTechLogService.TTProp2 = updatedEntry.TTProp2;
                            dbTechLogService.DidTTSinceCofA = updatedEntry.DidTTSinceCofA;
                            dbTechLogService.DIDTTSinceCofM = updatedEntry.DIDTTSinceCofM;
                            dbTechLogService.DidTTEngineOH = updatedEntry.DidTTEngineOH;
                            dbTechLogService.DidTTEngine = updatedEntry.DidTTEngine;
                            dbTechLogService.DidTTPropOH = updatedEntry.DidTTProp2OH;
                            dbTechLogService.DidTTProp = updatedEntry.DidTTProp;
                            dbTechLogService.DidTTEngine2OH = updatedEntry.DidTTEngine2OH;
                            dbTechLogService.DidTTEngine2 = updatedEntry.DidTTEngine2;
                            dbTechLogService.DidTTProp2OH = updatedEntry.DidTTEngine2;
                            dbTechLogService.DidTTProp2 = updatedEntry.DidTTProp2;
                        }
                    }
                }
      


                foreach (var newEntry in newEntries)
                {
                    var dbTechLogService = (T_Techlog_Services)newEntry;
                    ctx.T_Techlog_Services.Add(dbTechLogService);
                }

                //if (IsNew)
                //{



                //}
                //else
                //{
                //    dbTechLogService =
                //    if (dbTechLogService != null)
                //    {

                //    }


                //}

                await ctx.SaveChangesAsync();
                //if (dbtechlogservice!=null)
                //    idx = dbtechlogservice.idx;
            }
        }

        public static async Task DeleteHistoryEntry(int idx, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var dbtechlogServices = await ctx.T_Techlog_Services.FirstOrDefaultAsync(x => x.IDX == idx);
                if (dbtechlogServices!=null)
                {
                    ctx.T_Techlog_Services.Remove(dbtechlogServices);
                }

                await ctx.SaveChangesAsync();
            }
        }

        public static async Task<List<TechLogService>> GetTechlogServiceHistory(int idxACDetails, String serverName, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var dbtechlogServices= await ctx.T_Techlog_Services.Where(x => x.IDX_Setup_Aircraft_Details == idxACDetails).ToListAsync();
                var TechLogService =  dbtechlogServices.Select(x => (TechLogService)x).OrderByDescending(x=>x.IDX_TechLogID).ToList();

                return TechLogService;

            }

        }
    }
}
