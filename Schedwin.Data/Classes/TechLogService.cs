using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class TechLogService
    {
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


        public static explicit operator TechLogService(T_Techlog_Services t_techlogService)
        {
            var techLogService = new TechLogService();
            techLogService.IDX = t_techlogService.IDX;
            techLogService.IDX_TechLogID = t_techlogService.TechlogID.Value;
            techLogService.Date = t_techlogService.Date;
            techLogService.IDX_Setup_Aircraft_Details = t_techlogService.IDX_Setup_Aircraft_Details;
            techLogService.TTSinceCOfA = t_techlogService.TTSinceCOfA;
            techLogService.TTAirframe = t_techlogService.TTAirframe;
            techLogService.TTEngineOH = t_techlogService.TTEngineOH;
            techLogService.TTEngine = t_techlogService.TTEngine;
            techLogService.TTPropOH = t_techlogService.TTPropOH;
            techLogService.TTProp = t_techlogService.TTProp;
            techLogService.DIDTTSinceCofM = t_techlogService.DIDTTSinceCofM;
            techLogService.TTEngine2OH = t_techlogService.TTEngine2OH;
            techLogService.TTEngine2 = t_techlogService.TTEngine2OH;
            techLogService.TTProp2OH = t_techlogService.TTProp2OH;
            techLogService.TTProp2 = t_techlogService.TTProp2;
            techLogService.DidTTSinceCofA = t_techlogService.DidTTSinceCofA;
            techLogService.DIDTTSinceCofM = t_techlogService.DIDTTSinceCofM;
            techLogService.DidTTEngineOH = t_techlogService.DidTTEngineOH;
            techLogService. DidTTEngine = t_techlogService.DidTTEngine;
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
            t_Techlog_Services.IDX_Setup_Aircraft_Details = techLogService.IDX_Setup_Aircraft_Details;
            t_Techlog_Services.TTSinceCOfA = techLogService.TTSinceCOfA;
            t_Techlog_Services.TTAirframe = techLogService.TTAirframe;
            t_Techlog_Services.TTEngineOH = techLogService.TTEngineOH;
            t_Techlog_Services.TTEngine = techLogService.TTEngine;
            t_Techlog_Services.TTPropOH = techLogService.TTPropOH;
            t_Techlog_Services.TTProp = techLogService.TTProp;
            t_Techlog_Services.DIDTTSinceCofM = techLogService.DIDTTSinceCofM;
            t_Techlog_Services.TTEngine2OH = techLogService.TTEngine2OH;
            t_Techlog_Services.TTEngine2 = techLogService.TTEngine2OH;
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



        public static async Task<List<TechLogService>> GetTechlogServiceHistory(int idxACDetails, String serverName, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var dbtechlogServices= await ctx.T_Techlog_Services.Where(x => x.IDX_Setup_Aircraft_Details == idxACDetails).ToListAsync();
                var TechLogService =  dbtechlogServices.Select(x => (TechLogService)x).ToList();

                return TechLogService;

            }

        }
    }
}
