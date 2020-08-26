using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Data.Entity;
using System.Collections.ObjectModel;
using Schedwin.Data;
using Schedwin.Common;
using System.Diagnostics;

namespace Schedwin.WishIntegration.Classes
{
    public class WIOperatorAgent : ViewModelBase
    {
        public int IDX { get; set; }
        public string Agent { get; set; }
        public int IDX_Operator { get; set; }
        public String CompanyName { get; set; }
        public String CompanyCurrencyCode { get; set; }

        public static implicit operator WIOperatorAgent(tbOperator tbOperator)
        {
            var wiOpAgent = new WIOperatorAgent();
            wiOpAgent.Agent = "";
            wiOpAgent.IDX = -1;
            wiOpAgent.IDX_Operator = tbOperator.pkOperatorID;
            wiOpAgent.CompanyName = tbOperator.Name;
            wiOpAgent.CompanyCurrencyCode = "-";

            return wiOpAgent;
        }

        public static implicit operator WIOperatorAgent(tset_Companies tsetCompany)
        {
            var wiOpAgent = new WIOperatorAgent();
            wiOpAgent.Agent = "";
            wiOpAgent.IDX = -1;
            wiOpAgent.IDX_Operator = tsetCompany.IDX;
            wiOpAgent.CompanyName = tsetCompany.CompanyName;
            if (tsetCompany.tlst_Currency != null)
                wiOpAgent.CompanyCurrencyCode = tsetCompany.tlst_Currency.CODE;
            else
                wiOpAgent.CompanyCurrencyCode = "-";

            return wiOpAgent;
        }


        public static implicit operator WIOperatorAgent(vl_CompanyOperatorsAgents vlAgent)
        {
            var wiOpAgent = new WIOperatorAgent();
            wiOpAgent.Agent = vlAgent.Agent;
            wiOpAgent.IDX = vlAgent.IDX;
            wiOpAgent.IDX_Operator = vlAgent.IDX_Operator;
            wiOpAgent.CompanyName = vlAgent.CompanyName;
            return wiOpAgent;
        }
    }

    class ExFromToForPair
    {
        public String ExFrom { get; set; } 
        public String ToFor { get; set; }
    }

    public class WICharterBooking : ViewModelBase
    {

        public int BookingID { get; set; }
        public int PartyGroupID { get; set; }

        public String TPRef { get; set; }

        public String PartyGroupName { get; set; }
        public String ReservationName { get; set; }

        public DateTime BookingDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String ResStatus { get; set; }
        public String Notes { get; set; }
        public int PaxCount { get; set; }

        public String Consultant { get; set; }

        public List<WICharterBookingLeg> Legs { get; set; }

        public List<WICharterBookingPax> Pax { get; set; }


        private int _operatorID;
        public int OperatorID
        {
            get
            {
                return _operatorID;
            }
            set
            {
                _operatorID = value;
                NotifyPropertyChanged("_operatorID");
            }
        }



        WICharterBooking()
        {
            Legs = new List<WICharterBookingLeg>();
            Pax = new List<WICharterBookingPax>();
        }

        public void FilterLegs(DateTime startDate,DateTime endDate)
        {
            var allowedLegs = Legs.Where(x => x.BookingDate >= startDate && x.BookingDate <= endDate).ToList(); ;
            Legs = allowedLegs;
        }

        public WIReservationHeader ToWIReservation()
        {

            var wiResHeader = new WIReservationHeader();

            wiResHeader.WishResHeader.BookingID = BookingID;
            wiResHeader.WishResHeader.PartyGroupID = PartyGroupID;
            wiResHeader.WishResHeader.ReservationName = ReservationName;
            wiResHeader.WishResHeader.PartyGroupName = PartyGroupName;
            wiResHeader.WishResHeader.DateCaptured = BookingDate;
            wiResHeader.WishResHeader.PaxCount = PaxCount;
            wiResHeader.TPRef = TPRef;


            wiResHeader.WishResHeader.ReservationNote = Notes;
            wiResHeader.WishResHeader.WishResStatus = ResStatus;
            wiResHeader.WishResHeader.WishConsultant = Consultant;      
            var wiResLegs = Legs.Select(x => (WIReservationLeg)x).ToList();
            wiResHeader.Legs.AddRange(wiResLegs);

            var wiResPax = Pax.Select(x => (WIReservationPax)x).ToList();
            wiResHeader.PaxList.AddRange(wiResPax);

            if (wiResLegs.Count>0)
                wiResHeader.WishResHeader.DepartureDate = wiResLegs.Min(x=>x.WishResLeg.BookingDate);

            if (Legs.Count>0)
            {
                wiResHeader.WishResHeader.FirstBookingDate = Legs.Min(x => x.BookingDate);
                wiResHeader.WishResHeader.LastBookingDate = Legs.Max(x => x.BookingDate);
            }

            return wiResHeader;
        }

        static public async Task<List<WIOperatorAgent>> LoadOperatorAgents()
        {

            var ctx = new SchedwinGlobalEntities();

            using (ctx)
            {
                var items = await ctx.tbOperators.Where(x => x.Active).ToListAsync();
                if (items != null)
                {
                    var agentlst = items.Select(x => (WIOperatorAgent)x).ToList();
                    return agentlst;
                }
                return null;
            }
        }


        static public async Task<List<WIOperatorAgent>> GetOperatorAgents(String Server, String dbInstance)
        {
            try
            {
                var conString = RegionalConnectionGenerator.GetConnectionString(Server, dbInstance);
                var ctx = new SchedwinRegionalEntities(conString);
  
                using (ctx)
                {
                    var items = await ctx.tset_Companies.Include("tlst_Currency").Where(x => x.Active).ToListAsync();
                    if (items!=null)
                    {
                        var agentlst = items.Select(x => (WIOperatorAgent)x).ToList();
                        return agentlst;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var errString = string.Join(Environment.NewLine, messages);
                throw new Exception("Error retrieving Operator Agents :\r\n" + errString);
            }
        }

        static public async Task<List<WICharterBookingPax>> GetWishGuests(int partyGroupID)
        {
            try
            {
                var lstGuests = new List<WICharterBookingPax>();
                var ctx = new WISHEntities();
                using (ctx)
                {
                    var tbPartyGroup = await ctx.tbPartyGroups.Include("tbPartyGroup.tbGuests").
                                                    FirstOrDefaultAsync(x => x.pkPartyGroupId == partyGroupID);
                    if (tbPartyGroup != null)
                    {
                        foreach (var tbguest in tbPartyGroup.tbGuests)
                        {

                            var wiBookingGuest = new WICharterBookingPax();
                            wiBookingGuest.Age = Convert.ToInt32(tbguest.Age);
                            wiBookingGuest.FirstName = tbguest.Firstname;
                            wiBookingGuest.Surname = tbguest.Surname;
                            wiBookingGuest.WishGuestID = tbguest.pkGuestId;
                            wiBookingGuest.PassportNo = tbguest.PassportNumber;
                            wiBookingGuest.Nationality = tbguest.PassportNationality;
                            lstGuests.Add(wiBookingGuest);

                        }
                    }

                    return lstGuests;

                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var errString = string.Join(Environment.NewLine, messages);
                throw new Exception("Error retrieving Operator Agents :\r\n" + errString);
            }
        }

        static public async Task<WICharterBooking> RefreshBooking(int wishBookingID, int partyGroupID, int principalID, int soleUsePrincipalID)
        {
            var wiBooking = new WICharterBooking();
            var ctx = new WISHEntities();
            using (ctx)
            {
                var sectorBookings = await ctx.tbSectorBookings.Include("tbBookingFile")
                                                                 .Include("tbBookingFile.tbUser")
                                                                 .Include("tbPartyGroup")
                                                                 .Include("tbPartyGroup.tbGuests")
                                                                 .Include("tbSefBookings")
                                                                 .Include("tbSefBookings.tbPrincipal")
                                                                 .Include("tbSefBookings.tbPrincipal1")
                                                                 .Include("tbSefBookings.tbSefFlight")
                                                                .Include("tbCharterBookings")
                                                                .Include("tbCharterBookings.tbPrincipal")
                                                                .Include("tbCharterBookings.tbPrincipal1")
                                                                .Where(x => (x.fkBookingId==wishBookingID && x.fkPartyGroupId==partyGroupID)
                                                                             && (x.Status == "PROV" || x.Status == "CONF" || x.Status == "CANC") 
                                                                             && (x.fkSectorClass=="CH"  || x.fkSectorClass=="SF")
                                                                              && (x.fkPrincipalId == principalID || x.fkPrincipalId == soleUsePrincipalID))
                                                                 .ToListAsync();
                if (sectorBookings != null && sectorBookings.Count > 0)
                {


                    var bookingFile = sectorBookings.FirstOrDefault().tbBookingFile;
                    var firstSector = sectorBookings.FirstOrDefault();

                    if (firstSector.tbPartyGroup != null)
                    {
                        foreach (var guest in firstSector.tbPartyGroup.tbGuests)
                        {
                            var wiBookingGuest = new WICharterBookingPax();
                            wiBookingGuest.Age = Convert.ToInt32(guest.Age);
                            wiBookingGuest.FirstName = guest.Firstname;
                            wiBookingGuest.Surname = guest.Surname;
                            wiBookingGuest.Gender = guest.Gender;
                            wiBookingGuest.WishGuestID = guest.pkGuestId;
                            wiBookingGuest.PassportNo = guest.PassportNumber;
                            wiBookingGuest.Nationality = guest.PassportNationality;
                            wiBooking.Pax.Add(wiBookingGuest);
                        }

                    }

   
                    wiBooking.BookingID = bookingFile.pkBookingId;
                    wiBooking.ReservationName = bookingFile.PartyName;
                    wiBooking.TPRef = bookingFile.TPRef;
                    wiBooking.PartyGroupID = partyGroupID;

                    if (firstSector.tbPartyGroup != null)
                    {
                        wiBooking.PartyGroupName = firstSector.tbPartyGroup.Name;
                        wiBooking.PaxCount = firstSector.tbPartyGroup.tbGuests.Count();
                    }
                    if (bookingFile.Notes != null)
                        wiBooking.Notes = bookingFile.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", "");
                    wiBooking.ResStatus = bookingFile.BookingStatus;
                    wiBooking.Consultant = bookingFile.tbUser.PersonCode;

                    foreach (var sector in sectorBookings)
                    {
                        var wibookingLeg = new WICharterBookingLeg();
                        wibookingLeg.SectorBookingID = sector.pkSectorBookingId;
                        wibookingLeg.BookingDate = sector.StartDate.Value;
                        if (sector.Notes != null)
                            wibookingLeg.SectorNotes = sector.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", ""); ;

                        if (sector.tbAircraftType != null)
                            wibookingLeg.CharterType = sector.tbAircraftType.Type;

                        var tbSefSector = sector.tbSefBookings.FirstOrDefault();
                        var tbCHSector = sector.tbCharterBookings.FirstOrDefault();
                        if (tbSefSector != null)
                        {
                            wibookingLeg.From = tbSefSector.tbSefFlight.Description.Substring(0, 3);
                            wibookingLeg.ActualFrom = tbSefSector.tbSefFlight.Description.Substring(0, 3);
                            wibookingLeg.To = tbSefSector.tbSefFlight.Description.Substring(4, 3);
                            wibookingLeg.ActualTo = tbSefSector.tbSefFlight.Description.Substring(4, 3);

                            wibookingLeg.ETD = tbSefSector.tbSefFlight.StartDateTime;
                            wibookingLeg.ETA = tbSefSector.tbSefFlight.EndDateTime;
                            if (tbSefSector.tbPrincipal1 != null)
                            {
                                wibookingLeg.For = tbSefSector.tbPrincipal1.fkPrincipalName;
                                wibookingLeg.ActualFor = tbSefSector.tbPrincipal1.fkPrincipalName;
                            }

                            if (tbSefSector.tbPrincipal != null)
                            {
                                wibookingLeg.Ex = tbSefSector.tbPrincipal.fkPrincipalName;
                                wibookingLeg.ActualEx = tbSefSector.tbPrincipal.fkPrincipalName;
                            }
                        }
                        else if (tbCHSector != null)
                        {
                            wibookingLeg.ETD = sector.StartDate.Value;
                            wibookingLeg.ETA = sector.EndDate.Value;
                            if (tbCHSector.tbPrincipal1 != null)
                            {
                                wibookingLeg.ActualEx = tbCHSector.tbPrincipal1.fkPrincipalName;
                                wibookingLeg.Ex = tbCHSector.tbPrincipal1.fkPrincipalName;
                                wibookingLeg.ExCode = tbCHSector.tbPrincipal1.fkPrincipalCode;
                            }
                            if (tbCHSector.tbPrincipal != null)
                            {
                                wibookingLeg.ActualFor = tbCHSector.tbPrincipal.fkPrincipalName;
                                wibookingLeg.For = tbCHSector.tbPrincipal.fkPrincipalName;
                                wibookingLeg.ForCode= tbCHSector.tbPrincipal.fkPrincipalCode;
                            }


                        }

                        wibookingLeg.IsCancelled =sector.Status == "CANC" ?  true :  false;
                        wiBooking.Legs.Add(wibookingLeg);
                    }

                    wiBooking.StartDate = wiBooking.Legs.Min(x => x.BookingDate);
                    wiBooking.EndDate = wiBooking.Legs.Max(x=>x.BookingDate);

                    return wiBooking;
                }
            }

            return null;
        }

        static public async Task<List<WICharterBooking>> RetrieveSFBookingsEFF(int principalID, DateTime startDate, DateTime endDate)
        {
            var lstWIBookings = new List<WICharterBooking>();

            var ctx = new WISHEntities();
            using (ctx)
            {



                var tbSFSectorBookings = await ctx.tbSefBookings.Include("tbsectorBooking")
                                                                .Include("tbsectorBooking.tbBookingFile")
                                                                .Include("tbsectorBooking.tbBookingFile.tbUser")
                                                                .Include("tbsectorBooking.tbPartyGroup")
                                                                .Include("tbsectorBooking.tbPartyGroup.tbGuests")
                                                                .Include("tbPrincipal")
                                                                .Include("tbPrincipal1")
                                                                .Include("tbSefFlight")
                                                               .Where(x => (x.tbSectorBooking.StartDate >= startDate && x.tbSectorBooking.StartDate <= endDate)
                                                                            && (x.tbSectorBooking.Status == "PROV" || x.tbSectorBooking.Status == "CONF" || x.tbSectorBooking.Status=="CANC")
                                                                            && (x.tbSefFlight.fkPrincipalID == principalID))
                                                                .ToListAsync();
                Debug.WriteLine("A");
                var fligghtIDs = tbSFSectorBookings.Select(x => x.tbSefFlight.pkSefFlightID).Distinct().ToList();
                var tmpEnd = startDate.AddDays(7);
                var sefFlightLegs = await ctx.vwFlightLegs.Where(x => fligghtIDs.Contains(x.pkSefFlightID) && x.Leg_Date== startDate).ToListAsync();
                var sefFlightLegsGrping = sefFlightLegs.GroupBy(x => x.pkSefFlightID).ToList();

                var tbGrpedSectors = tbSFSectorBookings.GroupBy(x => x.tbSectorBooking.fkPartyGroupId).ToList();
                foreach (var tbSFBooking in tbGrpedSectors)
                {
                    Debug.WriteLine("B:"+tbSFBooking.Key.ToString());
                    var bookingFile = tbSFBooking.FirstOrDefault().tbSectorBooking.tbBookingFile;
                    var sectorBooking = tbSFBooking.FirstOrDefault().tbSectorBooking;

                    var wiBooking = new WICharterBooking();
                    wiBooking.BookingDate = bookingFile.DateBookingMade;
                    wiBooking.BookingID = bookingFile.pkBookingId;
                    wiBooking.TPRef = bookingFile.TPRef;
                    wiBooking.ReservationName = bookingFile.PartyName;
                    if (sectorBooking.fkPartyGroupId.HasValue)
                        wiBooking.PartyGroupID = sectorBooking.fkPartyGroupId.Value;
                    if (sectorBooking.tbPartyGroup != null)
                    {
                        wiBooking.PartyGroupName = sectorBooking.tbPartyGroup.Name;
                        wiBooking.PaxCount = sectorBooking.tbPartyGroup.tbGuests.Count();
                    }
                     
                    wiBooking.StartDate = tbSFBooking.Min(x => x.tbSectorBooking.StartDate).Value;
                    wiBooking.EndDate= tbSFBooking.Max(x => x.tbSectorBooking.StartDate).Value;
                    if (bookingFile.Notes != null)
                        wiBooking.Notes = bookingFile.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", "");
                    wiBooking.ResStatus = bookingFile.BookingStatus;
                    wiBooking.Consultant = bookingFile.tbUser.PersonCode;

                    foreach (var sector in tbSFBooking)
                    {
                        Debug.WriteLine("C");
                        var flightGrp = sefFlightLegsGrping.FirstOrDefault(x => x.Key == sector.fkSefFlightId);
                        if (flightGrp!=null)
                        {
                            var flightlegs = flightGrp.OrderBy(x => x.StartDateTime).ToList();
                            foreach (var flightLeg in flightlegs)
                            {
                                Debug.WriteLine("D:"+flightLeg.fkSefLegID+" "+ flightLeg.Leg_Description);
                                var wibookingLeg = new WICharterBookingLeg();
                                wibookingLeg.IsMultiLegFlight = true;
                                Debug.WriteLine("1");
                                wibookingLeg.SectorBookingID = sector.tbSectorBooking.pkSectorBookingId;
                                Debug.WriteLine("2: "+sector.tbSectorBooking.pkSectorBookingId);
                                wibookingLeg.BookingDate = sector.tbSectorBooking.StartDate.Value;
                                Debug.WriteLine("3");
                                wibookingLeg.ETD = flightLeg.StartDateTime;
                                Debug.WriteLine("4");
                                wibookingLeg.ETA = flightLeg.EndDateTime;
                                Debug.WriteLine("5");
                                wibookingLeg.From = flightLeg.Leg_Description.Substring(0, 3);
                                wibookingLeg.ActualFrom = flightLeg.Leg_Description.Substring(0, 3);
                                wibookingLeg.To = flightLeg.Leg_Description.Substring(4, 3);
                                wibookingLeg.ActualTo = flightLeg.Leg_Description.Substring(4, 3);
                                wibookingLeg.MultiLegFrom = flightLeg.flight_Description.Substring(0, 3);
                                wibookingLeg.MultiLegTo=flightLeg.flight_Description.Substring(4, 3);
                                if (sector.tbSectorBooking.Notes != null)
                                    wibookingLeg.SectorNotes = sector.tbSectorBooking.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", ""); ;
                                if (sector.tbPrincipal1 != null)
                                {
                                    wibookingLeg.For = sector.tbPrincipal1.fkPrincipalName;
                                    wibookingLeg.ActualFor = sector.tbPrincipal1.fkPrincipalName;
                                }
                               
                                if (sector.tbPrincipal != null)
                                {
                                    wibookingLeg.Ex = sector.tbPrincipal.fkPrincipalName;
                                    wibookingLeg.ActualEx = sector.tbPrincipal.fkPrincipalName;
                                }
                               
                                wibookingLeg.IsCancelled = sector.tbSectorBooking.Status == "CANC" ? true : false;
                                wiBooking.Legs.Add(wibookingLeg);
                                Debug.WriteLine("E");
                            }
                        }

                    }

                    if (sectorBooking.tbPartyGroup != null)
                    {
                        Debug.WriteLine("F");
                        foreach (var guest in sectorBooking.tbPartyGroup.tbGuests)
                        {
                            var wiBookingGuest = new WICharterBookingPax();
                            wiBookingGuest.Age = Convert.ToInt32(guest.Age);
                            wiBookingGuest.FirstName = guest.Firstname;
                            wiBookingGuest.Surname = guest.Surname;
                            wiBookingGuest.Gender = guest.Gender;
                            wiBookingGuest.PassportNo = guest.PassportNumber;
                            wiBookingGuest.Nationality = guest.PassportNationality;
                            wiBookingGuest.WishGuestID = guest.pkGuestId;
                            wiBooking.Pax.Add(wiBookingGuest);
                        }
                        Debug.WriteLine("G");
                    }

                    lstWIBookings.Add(wiBooking);
                }
            }


            return lstWIBookings;
        }


      static public async Task<List<WICharterBooking>> RetrieveCHBookingsEFF(int principalID, int soleUsePrincipalID ,DateTime startDate, DateTime endDate)
        {
            var lstWIBookings = new List<WICharterBooking>();

            var ctx = new WISHEntities();
            using (ctx)
            {
                var tbCHSectorBookings = await ctx.tbCharterBookings.Include("tbsectorBooking")
                                                                    .Include("tbSectorBooking.tbAircraftType")
                                                                   .Include("tbsectorBooking.tbBookingFile")
                                                                    .Include("tbsectorBooking.tbBookingFile.tbUser")
                                                                    .Include("tbsectorBooking.tbPartyGroup")
                                                                     .Include("tbsectorBooking.tbPartyGroup.tbGuests")
                                                                   .Include("tbPrincipal")
                                                                   .Include("tbPrincipal1")
                                                                   .Where(x => (x.tbSectorBooking.StartDate>= startDate && x.tbSectorBooking.StartDate<= endDate) 
                                                                                && (x.tbSectorBooking.Status == "PROV" || x.tbSectorBooking.Status == "CONF" || x.tbSectorBooking.Status == "CANC")
                                                                                && (x.tbSectorBooking.fkPrincipalId== principalID || x.tbSectorBooking.fkPrincipalId == soleUsePrincipalID))
                                                                    .ToListAsync();

                var tbGrpedSectors = tbCHSectorBookings.GroupBy(x => x.tbSectorBooking.fkPartyGroupId).ToList();
                foreach (var tbCHBooking in tbGrpedSectors)
                {
                   
                    var bookingFile = tbCHBooking.FirstOrDefault().tbSectorBooking.tbBookingFile;
                    var sectorBooking = tbCHBooking.FirstOrDefault().tbSectorBooking;
                    Debug.WriteLine("Parsing CH booking: " + bookingFile.pkBookingId);
                    var wiBooking = new WICharterBooking();
                    wiBooking.TPRef = bookingFile.TPRef;
                    wiBooking.BookingDate = bookingFile.DateBookingMade;
                    wiBooking.BookingID = bookingFile.pkBookingId;
                    wiBooking.ReservationName = bookingFile.PartyName;
                    if (sectorBooking.fkPartyGroupId.HasValue)
                     wiBooking.PartyGroupID = sectorBooking.fkPartyGroupId.Value;
                    if (sectorBooking.tbPartyGroup!=null)
                    {
                        wiBooking.PartyGroupName = sectorBooking.tbPartyGroup.Name;
                        wiBooking.PaxCount = sectorBooking.tbPartyGroup.tbGuests.Count();
                    }

                    if (bookingFile.Notes!=null)
                        wiBooking.Notes = bookingFile.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", ""); 
                    wiBooking.ResStatus = bookingFile.BookingStatus;
                    wiBooking.Consultant = bookingFile.tbUser.PersonCode;

                    wiBooking.StartDate = tbCHSectorBookings.Min(x => x.tbSectorBooking.StartDate).Value;
                    wiBooking.EndDate = tbCHSectorBookings.Max(x => x.tbSectorBooking.StartDate).Value;

                    foreach (var sector in tbCHBooking)
                    {
                        Debug.WriteLine("Start Sector : " + sector.FK_SectorBookingID);
                        var wibookingLeg = new WICharterBookingLeg();
                        wibookingLeg.SectorBookingID = sector.tbSectorBooking.pkSectorBookingId;
                        wibookingLeg.BookingDate = sector.tbSectorBooking.StartDate.Value;
                        wibookingLeg.ETD = sector.tbSectorBooking.StartDate.Value;
                        wibookingLeg.ETA = sector.tbSectorBooking.EndDate.Value;
                        if (sector.tbSectorBooking.Notes!=null)
                            wibookingLeg.SectorNotes = sector.tbSectorBooking.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", ""); ;
                        wibookingLeg.ActualFor = sector.tbPrincipal.fkPrincipalName;
                        wibookingLeg.For = sector.tbPrincipal.fkPrincipalName;
                        wibookingLeg.ForCode = sector.tbPrincipal.fkPrincipalCode;
                        wibookingLeg.ActualEx= sector.tbPrincipal1.fkPrincipalName;
                        wibookingLeg.Ex = sector.tbPrincipal1.fkPrincipalName;
                        wibookingLeg.ExCode = sector.tbPrincipal1.fkPrincipalCode;
                        if (sector.tbSectorBooking.tbAircraftType != null)
                            wibookingLeg.CharterType = sector.tbSectorBooking.tbAircraftType.Type;

                        wibookingLeg.SoleUse = sector.tbSectorBooking.fkPrincipalId == soleUsePrincipalID;
                        wibookingLeg.IsCancelled = sector.tbSectorBooking.Status == "CANC" ? true : false;
                        wiBooking.Legs.Add(wibookingLeg);

                        Debug.WriteLine("End Sector : " + sector.FK_SectorBookingID);
                    }

                    if (sectorBooking.tbPartyGroup!=null)
                    {
                        foreach (var guest in sectorBooking.tbPartyGroup.tbGuests)
                        {
                            var wiBookingGuest = new WICharterBookingPax();
                            wiBookingGuest.Age = Convert.ToInt32(guest.Age);
                            wiBookingGuest.FirstName = guest.Firstname;
                            wiBookingGuest.Surname = guest.Surname;
                            wiBookingGuest.Gender = guest.Gender;
                            wiBookingGuest.PassportNo = guest.PassportNumber;
                            wiBookingGuest.Nationality = guest.PassportNationality;
                            wiBookingGuest.WishGuestID = guest.pkGuestId;
                            wiBooking.Pax.Add(wiBookingGuest);
                        }
                    }

                    Debug.WriteLine("Finished parsing: " + bookingFile.pkBookingId);
                    lstWIBookings.Add(wiBooking);
                }
            }

    
            return lstWIBookings;
        }

        //static public async Task<List<WICharterBooking>> RetrieveSFBookingsEFF(int principalID, DateTime startDate, DateTime endDate)
        //{
        //    var lstWIBookings = new List<WICharterBooking>();

        //    var ctx = new WISHEntities();
        //    using (ctx)
        //    {
        //        var tbSFSectorBookings = await ctx.tbSefBookings.Include("tbsectorBooking")
        //                                                        .Include("tbsectorBooking.tbBookingFile")
        //                                                        .Include("tbsectorBooking.tbBookingFile.tbUser")
        //                                                        .Include("tbsectorBooking.tbPartyGroup")
        //                                                        .Include("tbsectorBooking.tbPartyGroup.tbGuests")
        //                                                        .Include("tbPrincipal")
        //                                                        .Include("tbPrincipal1")
        //                                                        .Include("tbSefFlight")
        //                                                       .Where(x => (x.tbSectorBooking.StartDate >= startDate && x.tbSectorBooking.StartDate <= endDate)
        //                                                                    && (x.tbSectorBooking.Status == "PROV" || x.tbSectorBooking.Status == "CONF")
        //                                                                    && (x.tbSefFlight.fkPrincipalID == principalID))
        //                                                        .ToListAsync();

        //        var tbGrpedSectors = tbSFSectorBookings.GroupBy(x => x.tbSectorBooking.fkPartyGroupId).ToList();
        //        foreach (var tbSFBooking in tbGrpedSectors)
        //        {
        //            var bookingFile = tbSFBooking.FirstOrDefault().tbSectorBooking.tbBookingFile;
        //            var sectorBooking = tbSFBooking.FirstOrDefault().tbSectorBooking;

        //            var wiBooking = new WICharterBooking();
        //            wiBooking.BookingDate = bookingFile.DateBookingMade;
        //            wiBooking.BookingID = bookingFile.pkBookingId;
        //            wiBooking.ReservationName = bookingFile.PartyName;
        //            if (sectorBooking.fkPartyGroupId.HasValue)
        //                wiBooking.PartyGroupID = sectorBooking.fkPartyGroupId.Value;
        //            if (sectorBooking.tbPartyGroup != null)
        //            {
        //                wiBooking.PartyGroupName = sectorBooking.tbPartyGroup.Name;
        //                wiBooking.PaxCount = sectorBooking.tbPartyGroup.tbGuests.Count();
        //            }

        //            wiBooking.DepartureDate = tbSFBooking.Min(x => x.tbSectorBooking.StartDate).Value;
        //            if (bookingFile.Notes != null)
        //                wiBooking.Notes = bookingFile.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", "");
        //            wiBooking.ResStatus = bookingFile.BookingStatus;
        //            wiBooking.Consultant = bookingFile.tbUser.PersonCode;

        //            foreach (var sector in tbSFBooking)
        //            {
        //                var wibookingLeg = new WICharterBookingLeg();
        //                wibookingLeg.SectorBookingID = sector.tbSectorBooking.pkSectorBookingId;
        //                wibookingLeg.BookingDate = sector.tbSectorBooking.StartDate.Value;
        //                wibookingLeg.ETD = sector.tbSefFlight.StartDateTime;
        //                wibookingLeg.ETA = sector.tbSefFlight.EndDateTime;

        //                wibookingLeg.From = sector.tbSefFlight.Description.Substring(0, 3);
        //                wibookingLeg.ActualFrom = sector.tbSefFlight.Description.Substring(0, 3);
        //                wibookingLeg.To = sector.tbSefFlight.Description.Substring(4, 3);
        //                wibookingLeg.ActualTo = sector.tbSefFlight.Description.Substring(4, 3);

        //                if (sector.tbSectorBooking.Notes != null)
        //                    wibookingLeg.SectorNotes = sector.tbSectorBooking.Notes.Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", ""); ;
        //                if (sector.tbPrincipal1 != null)
        //                {
        //                    wibookingLeg.For = sector.tbPrincipal1.fkPrincipalName;
        //                    wibookingLeg.ActualFor = sector.tbPrincipal1.fkPrincipalName;
        //                }

        //                if (sector.tbPrincipal != null)
        //                {
        //                    wibookingLeg.Ex = sector.tbPrincipal.fkPrincipalName;
        //                    wibookingLeg.ActualEx = sector.tbPrincipal.fkPrincipalName;
        //                }

        //                wiBooking.Legs.Add(wibookingLeg);
        //            }

        //            if (sectorBooking.tbPartyGroup != null)
        //            {
        //                foreach (var guest in sectorBooking.tbPartyGroup.tbGuests)
        //                {
        //                    var wiBookingGuest = new WICharterBookingPax();
        //                    wiBookingGuest.Age = Convert.ToInt32(guest.Age);
        //                    wiBookingGuest.FirstName = guest.Firstname;
        //                    wiBookingGuest.Surname = guest.Surname;
        //                    wiBookingGuest.Gender = guest.Gender;
        //                    wiBookingGuest.WishGuestID = guest.pkGuestId;
        //                    wiBooking.Pax.Add(wiBookingGuest);
        //                }

        //            }

        //            lstWIBookings.Add(wiBooking);
        //        }
        //    }


        //    return lstWIBookings;
        //}
    }
}
