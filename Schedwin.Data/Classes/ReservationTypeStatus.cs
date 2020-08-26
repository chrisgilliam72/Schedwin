using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class ReservationStatus
    {
        public int IDX { get; set; }
        public String Description { get; set; }

        public static explicit operator ReservationStatus(tbReservationStatus tbResType)
        {
            var resStatus = new ReservationStatus();
            resStatus.IDX = tbResType.pkReservationStatusID;
            resStatus.Description = tbResType.Description;
            return resStatus;
        }

        public static explicit operator ReservationStatus(tlst_ResStatus lstResStatus)
        {
            var resStatus = new ReservationStatus();
            resStatus.IDX = lstResStatus.IDX;
            resStatus.Description = lstResStatus.ReservationStatus;
            return resStatus;
        }
    }

    public class ReservationType
    {
        public int IDX { get; set; }
        public String Description { get; set; }

        public static explicit operator ReservationType(tbReservationType tbResType)
        {
            var resType = new ReservationType();
            resType.IDX = tbResType.pkReservationTypeID;
            resType.Description = tbResType.Description;
            return resType;
        }

        public static explicit operator ReservationType(tlst_ResType lstResType)
        {
            var resType = new ReservationType();
            resType.IDX = lstResType.IDX;
            resType.Description = lstResType.ResType;
            return resType;
        }
    }

    public class ReservationTypeStatus
    {
       
        private static List<ReservationStatus> _ReservationStatusCache = null;
        private static List<ReservationType> _ReservationTypeCacheList = null;

        public static async Task<List<ReservationStatus>> GetReservationStatusList()
        {

            if (_ReservationStatusCache != null)
                return _ReservationStatusCache;
            else
            {
             
                var ctx = new SchedwinGlobalEntities();

                using (ctx)
                {
                    var tmpLst = await ctx.tbReservationStatus1.ToListAsync();
                    _ReservationStatusCache = tmpLst.Select(x => (ReservationStatus)x).ToList();

                    return _ReservationStatusCache;
                }
            }

        }


        public static async  Task<List<ReservationStatus>>  GetReservationStatusList(String Server,String  regionalDBName)
        {

            if (_ReservationStatusCache != null)
                return _ReservationStatusCache;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var tmpLst= await ctx.tlst_ResStatus.ToListAsync();
                    _ReservationStatusCache = tmpLst.Select(x => (ReservationStatus)x).ToList();

                   return _ReservationStatusCache;
                }
            }

        }

        public static async Task<List<ReservationType>> GetReservationTypeList()
        {

            if (_ReservationTypeCacheList != null)
                return _ReservationTypeCacheList;
            else
            {

                var ctx = new SchedwinGlobalEntities();

                using (ctx)
                {
                    var tmpLst = await ctx.tbReservationTypes.ToListAsync();
                    _ReservationTypeCacheList = tmpLst.Select(x => (ReservationType)x).ToList();

                    return _ReservationTypeCacheList;
                }
            }

        }

        public static async Task<List<ReservationType>> GetReservationTypeList(String Server, String regionalDBName)
        {

            if (_ReservationTypeCacheList != null)
                return _ReservationTypeCacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var tmpLst = await ctx.tlst_ResType.ToListAsync();
                    _ReservationTypeCacheList = tmpLst.Select(x=>(ReservationType)x).ToList();

                    return _ReservationTypeCacheList;
                }
            }


        }

    }
}
