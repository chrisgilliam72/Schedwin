using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class AircraftType
    {
        public bool IsNew { get; set; }

        public bool IsActive { get; set; }
       
        public int IDX{ get; set; }

        public static int IDX_AC_NONE { get; set; }

        public String TypeName { get; set; }
        public String  Description {get;set;}

        public int Weight { get; set; }
        public float RangeKM { get; set; }

        public float RangeHours{ get; set; }

        public short Speed { get; set; }

        public short Pax { get; set; }

        public int IDX_Fueltype { get; set; }

        public short  FuelFlow { get; set; }
        
        public short TurnAroundTime { get; set; }
        public int MaxGrossWeight { get; set; }

        public bool TwinEngine { get; set; }

        public short Demurrage { get; set; }

        public short MaxStops { get; set; }


        public double FuelArm { get; set; }

        private static List<AircraftType> _listACTypes = null;


        public static explicit operator AircraftType(tbAircraftType tbAircraftType)
        {
            var aircraftType = new AircraftType();
            aircraftType.IsActive = tbAircraftType.Active;
            aircraftType.IsNew = false;
            aircraftType.IDX = tbAircraftType.pkAircraftTypeID;
            aircraftType.TypeName = tbAircraftType.ACType;
            aircraftType.Description = tbAircraftType.ShortDescription;
            aircraftType.MaxGrossWeight = tbAircraftType.MaxGrossWeight;
            aircraftType.RangeKM = tbAircraftType.RangeKm;
            aircraftType.Pax = tbAircraftType.NumberOfPassengers;
            aircraftType.Speed = tbAircraftType.BlockSpeed;
            aircraftType.RangeHours = tbAircraftType.RangeHours;
            aircraftType.IDX_Fueltype = tbAircraftType.FuelType;
            aircraftType.FuelFlow = tbAircraftType.FuelFlow;
            aircraftType.TurnAroundTime = tbAircraftType.AdditionalTurnaroundTime;
            aircraftType.RangeKM = tbAircraftType.RangeKm;
            aircraftType.RangeHours = tbAircraftType.RangeHours;
            aircraftType.TwinEngine = Convert.ToBoolean(tbAircraftType.TwinEngine);
            aircraftType.Description = tbAircraftType.ShortDescription;
            aircraftType.Demurrage = tbAircraftType.Demurrage;
            aircraftType.MaxStops = tbAircraftType.MaxStops;
            aircraftType.FuelArm = tbAircraftType.FuelArm ?? 0;
            return aircraftType;
        }

         public static explicit  operator AircraftType(tset_ACTypes tset)
        {
            var aircraftType = new AircraftType();
            aircraftType.IsActive = tset.Active;
            aircraftType.IsNew = false;
            aircraftType.IDX = tset.IDX;
            aircraftType.TypeName = tset.ACType;
            aircraftType.Description = tset.ShortDescription;
            aircraftType.MaxGrossWeight = tset.MaxGrossWeight;
            aircraftType.RangeKM = tset.RangeKm;
            aircraftType.Pax = tset.NumberOfPassengers;
            aircraftType.Speed = tset.BlockSpeed;
            aircraftType.RangeHours = tset.RangeHours;
            aircraftType.IDX_Fueltype = tset.FuelType;
            aircraftType.FuelFlow = tset.FuelFlow;
            aircraftType.TurnAroundTime = tset.AdditionalTurnaroundTime;
            aircraftType.RangeKM = tset.RangeKm;
            aircraftType.RangeHours = tset.RangeHours;
            aircraftType.TwinEngine = Convert.ToBoolean(tset.TwinEngine);
            aircraftType.Description = tset.ShortDescription;
            aircraftType.Demurrage = tset.Demurrage;
            aircraftType.MaxStops = tset.MaxStops;
            aircraftType.FuelArm = tset.FuelArm ?? 0;
            return aircraftType;
        }


   
        public static explicit operator tset_ACTypes(AircraftType aircraftType)
        {
            var tset = new tset_ACTypes();
            tset.IDX = aircraftType.IDX;
            tset.Active = aircraftType.IsActive;
            tset.ACType= aircraftType.TypeName;
            tset.ShortDescription = aircraftType.Description;
            tset.MaxGrossWeight= aircraftType.MaxGrossWeight;
            tset.RangeKm = aircraftType.RangeKM;
            tset.NumberOfPassengers = aircraftType.Pax;
            tset.BlockSpeed = aircraftType.Speed;
            tset.RangeHours = aircraftType.RangeHours;
            tset.FuelType = aircraftType.IDX_Fueltype;
            tset.FuelFlow = aircraftType.FuelFlow;
            tset.FuelArm = aircraftType.FuelArm;
            tset.AdditionalTurnaroundTime = aircraftType.TurnAroundTime;
            tset.RangeKm = aircraftType.RangeKM;
            tset.RangeHours=aircraftType.RangeHours;
            tset.TwinEngine = Convert.ToByte(aircraftType.TwinEngine);
            tset.Demurrage = aircraftType.Demurrage;
            tset.MaxStops = aircraftType.MaxStops;
            tset.CoCode = "SEFO";
            return tset;
        }



        public static List<AircraftType> GetACTypes()
        {
            if (_listACTypes != null)
                return _listACTypes.OrderBy(x => x.TypeName).ToList();
            else
                return null;
        }


        public static bool UpdateCachedObject(AircraftType newAircraftType)
        {
            if (_listACTypes != null)
            {
                var oldObject = _listACTypes.FirstOrDefault(x => x.IDX == newAircraftType.IDX);
                if (oldObject != null)
                    _listACTypes.Remove(oldObject);

                _listACTypes.Add(newAircraftType);
            }
            return false;

        }

        public static async Task DeactivateAircraftType(int IDX, String Server, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsetAircraft = await ctx.tset_ACTypes.FirstOrDefaultAsync(x => x.IDX == IDX);
                if (tsetAircraft != null)
                    tsetAircraft.Active = false;
                await ctx.SaveChangesAsync();


            }
        }

        public async Task<bool> Save(String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                tset_ACTypes tsetACType;
                if (IsNew)
                {
                    tsetACType = (tset_ACTypes)this;
                    ctx.tset_ACTypes.Add(tsetACType);

                }
                else
                {
                    tsetACType = ctx.tset_ACTypes.FirstOrDefault(x => x.IDX == IDX);
                    if (tsetACType!=null)
                    {
                        tsetACType.ACType = TypeName;
                        tsetACType.ShortDescription = Description;
                        tsetACType.MaxGrossWeight = MaxGrossWeight;
                        tsetACType.RangeKm = RangeKM;
                        tsetACType.NumberOfPassengers = Pax;
                        tsetACType.BlockSpeed = Speed;
                        tsetACType.RangeHours = RangeHours;
                        tsetACType.FuelType = IDX_Fueltype;
                        tsetACType.FuelFlow = FuelFlow;
                        tsetACType.FuelArm = FuelArm;
                        tsetACType.AdditionalTurnaroundTime = TurnAroundTime;
                        tsetACType.RangeKm = RangeKM;
                        tsetACType.RangeHours = RangeHours;
                        tsetACType.TwinEngine = Convert.ToByte(TwinEngine);
                        tsetACType.Demurrage = Demurrage;
                        tsetACType.MaxStops = MaxStops;
 
                    }
                }

                await ctx.SaveChangesAsync();
                IDX = tsetACType.IDX;



                return true;

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                throw new Exception(message);
            }
        }


        public static async Task<List<AircraftType>> LoadACTypes( bool forceReload)
        {
            if (_listACTypes == null || forceReload)
            {

                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    var tmplistACTypes = await ctx.tbAircraftTypes.Where(x => x.Active).ToListAsync();
                    _listACTypes = tmplistACTypes.Select(x => (AircraftType)x).OrderBy(x => x.TypeName).ToList();
                }


            }

            return _listACTypes;
        }
        public static  async Task<List<AircraftType>> LoadACTypes(String Server, string regionalDBName, bool forceReload)
        {

                if (_listACTypes == null  || forceReload)
                {
                    var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                    var ctx = new SchedwinRegionalEntities(constring);
                    using (ctx)
                    {
                        var tmplistACTypes = await ctx.tset_ACTypes.Where(x=>x.Active).ToListAsync();
                        _listACTypes = tmplistACTypes.Select(x => (AircraftType)x).OrderBy(x => x.TypeName).ToList();
                    }


                }

                return _listACTypes;   
        }

        public static String GetACTypeType(int idxACType)
        {
            var acType = GetACType(idxACType);
            return acType != null ? acType.TypeName : "";
        }
        
        public static String GetACTypeDescription(int idxACType)
        {
            var acType = GetACType(idxACType);
            return acType != null ? acType.Description : "";
        }

        public static AircraftType GetACType(int idxACTYpe)
        {
            if (_listACTypes != null)
            {
                var acType = _listACTypes.FirstOrDefault(x => x.IDX == idxACTYpe);
                return acType;
            }

            return null;
        }

        public static async Task<int> GetNoneAircraftIDX()
        {
            var acTypes = await LoadACTypes(false);

            var acType = acTypes.FirstOrDefault(x => x.TypeName == "None");

            return acType != null ? acType.IDX : -1;

        }


        public static async Task<int> GetNoneAircraftIDX(String Server, string regionalDBName)
        {
            var acTypes = await LoadACTypes(Server, regionalDBName,false);

            var acType = acTypes.FirstOrDefault(x => x.TypeName == "None");

            return acType != null ? acType.IDX : -1;
 
        }
    }
}
