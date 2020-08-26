using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class DataCache
    {

        public static async Task Init()
        {
            var tsk1 = Country.LoadCountries(true);
            var tsk6 = AirstripInfo.LoadAirstrips();
            var tsk7 = Lodge.LoadLodgeList(true);
            var tsk9 = AircraftInfo.LoadAircraftList(true);
            var tsk10 = AircraftType.LoadACTypes(true);
            await Task.WhenAll(tsk1,tsk6, tsk7, tsk9, tsk10);
        }

        public static async Task Init(String Server, string regionalDBName)
        {
            var tsk1 = APDistances.LoadDistanceMatrix(Server, regionalDBName);
            //var tsk2 = OperatorSeatRates.LoadSeatRateMatrix(Server, regionalDBName);
            var tsk3 = Country.LoadCountries(Server, regionalDBName);
            var tsk4 = AllowableDutyHours.LoadAllowableDuties(Server, regionalDBName);
            var tsk5 = AllowableDutyHours.LoadAllowableMatrix(Server, regionalDBName);
            var tsk6 = AirstripInfo.LoadAirstrips(Server, regionalDBName);
            var tsk7 = Lodge.LoadLodgeList(Server, regionalDBName,true);
            var tsk8 = PilotInfo.LoadPilotInfo(Server, regionalDBName);
            var tsk9 = AircraftInfo.LoadAircraftList(Server, regionalDBName,true);
            var tsk10 = AircraftType.LoadACTypes(Server, regionalDBName, true);
            await Task.WhenAll(tsk1, /*tsk2,*/ tsk3, tsk4, tsk5, tsk6, tsk7, tsk8, tsk9,tsk10);
        }


        //public static async task init(string Server, string regionaldbname)
        //{
        //    await apdistances.loaddistancematrix(Server, regionaldbname);
        //    await operatorseatrates.loadseatratematrix(Server, regionaldbname);
        //    await country.loadcountries(Server, regionaldbname);
        //    await allowabledutyhours.loadallowableduties(Server, regionaldbname);
        //    await allowabledutyhours.loadallowablematrix(Server, regionaldbname);
        //    await airstripinfo.loadairstrips(Server, regionaldbname);
        //    await lodge.getlodgeairstripinfo(Server, regionaldbname);
        //    await pilotinfo.loadpilotinfo(Server, regionaldbname);
        //    await aircraftinfo.loadaircraftlist(Server, regionaldbname);

        //}
    }
}
