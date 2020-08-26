using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class ReservationPax
    {
        public int IDX { get; set; }

        public bool IsNew
        {
            get
            {
                return IDX < 0;
            }
        }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }

        private String _firstname;
        public String FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                IsModified = true;
            }
        }

        private String _surname;
        public String Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                _surname = value;
                IsModified = true;
            }
        }

        private String _sex;
        public String Sex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;
                IsModified = true;
            }
        }

        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                IsModified = true;
            }
        }

        private int _paxType;
        public int IDX_PaxType
        {
            get
            {
                return _paxType;
            }
            set
            {
                _paxType = value;
                IsModified = true;
            }
        }

        private String _passengerType;
        public String PassengerType
        {
            get
            {
                return _passengerType;
            }
            set
            {
                _passengerType = value;
                IsModified = true;
            }
        }

        private int _weight;
        public int Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                IsModified = true;
            }
        }

        private String _passportNo;
        public String PassportNo
        {
           get
            {
                return _passportNo;
            }
            set
            {
                _passportNo = value;
                IsModified = true;
            }
        }

        private String _Nationality;
        public String Nationality
        {
            get
            {
                return _Nationality;
            }
            set
            {
                _Nationality = value;
                IsModified = true;
            }
        }

        private int _wishGuestID;
        public int WishGuestID
        {
            get
            {
                return _wishGuestID;
            }
            set
            {
                _wishGuestID = value;
                IsModified = true;
            }
        }

        private bool _ticketPrinted;
        public bool TicketPrinted
        {
            get
            {
                return _ticketPrinted;
            }
            set
            {
                _ticketPrinted = value;
                IsModified = true;
            }
        }


        private int _luggageWeight;
        public int LuggageWeight
        {
            get
            {
                return _luggageWeight;
            }
            set
            {
                _luggageWeight = value;
                IsModified = true;
            }
        }

        public ReservationPax()
        {
            IDX = -1;
            PassportNo = "-";
            Nationality = "-";
        }

        public ReservationPax(ReservationPax pax)
        {
            IDX = pax.IDX;
            FirstName = pax.FirstName;
            Surname = pax.Surname;
            Sex = pax.Sex;
            Age = pax.Age;
            IDX_PaxType = pax.IDX_PaxType;
            PassengerType = pax. PassengerType;
            Weight = pax.Weight;
            PassportNo = pax.PassportNo;
            Nationality = pax.Nationality;
            WishGuestID = pax.WishGuestID;
            TicketPrinted = pax.TicketPrinted;
            LuggageWeight = pax.LuggageWeight;
            IsModified = false;
        }

        public static explicit operator tbPassenger(ReservationPax resPax)
        {
            var tbPassenger = new tbPassenger();
            tbPassenger.pkPassengerID = resPax.IDX;
            tbPassenger.FirstName = resPax.FirstName;
            tbPassenger.Surname = resPax.Surname ?? "";
            tbPassenger.Age = Convert.ToByte(resPax.Age);
            tbPassenger.fkPassengerTypeID = resPax.IDX_PaxType;
            tbPassenger.Weight = resPax.Weight;
            tbPassenger.Passport = resPax.PassportNo;
            tbPassenger.Nationality = resPax.Nationality;
            tbPassenger.Sex = resPax.Sex;
            tbPassenger.WISHGuestID = resPax.WishGuestID;
            tbPassenger.TicketPrinted = resPax.TicketPrinted;
            tbPassenger.Luggageweight = resPax.LuggageWeight;

            return tbPassenger;
        }

        public static explicit operator tsch_Passengers(ReservationPax resPax)
        {
            var tschPax = new tsch_Passengers();
            tschPax.IDX = resPax.IDX;
            tschPax.FirstName = resPax.FirstName;
            tschPax.Surname = resPax.Surname;
            tschPax.Age = Convert.ToByte( resPax.Age );
            tschPax.IDX_PaxType = resPax.IDX_PaxType;
            tschPax.Weight = resPax.Weight;
            tschPax.Passport = resPax.PassportNo;
            tschPax.Nationality = resPax.Nationality;
            tschPax.Sex = resPax.Sex;
            tschPax.WISHGuestID = resPax.WishGuestID;
            tschPax.TicketPrinted = resPax.TicketPrinted;
            tschPax.Luggageweight = resPax.LuggageWeight;

            return tschPax;
        }


      
        public static explicit operator ReservationPax (tsch_Passengers dbPassenger)
        {
            var resPax = new ReservationPax();
            resPax.IDX = dbPassenger.IDX;
            resPax.FirstName = dbPassenger.FirstName!=null ? dbPassenger.FirstName : "";
            resPax.Surname = dbPassenger.Surname!=null ? dbPassenger.Surname :"";
            resPax.Age = dbPassenger.Age ?? 0;
            resPax.IDX_PaxType = dbPassenger.IDX_PaxType ?? 1;
            resPax.Weight = dbPassenger.Weight ?? 0;
            resPax.PassportNo = dbPassenger.Passport;
            resPax.Nationality = dbPassenger.Nationality;
            resPax.Sex = dbPassenger.Sex;
            resPax.WishGuestID = dbPassenger.WISHGuestID ?? 0;
            resPax.TicketPrinted = dbPassenger.TicketPrinted ?? false;
            resPax.LuggageWeight = dbPassenger.Luggageweight ?? 0;

            if (dbPassenger.tlst_PaxType != null)
                resPax.PassengerType= dbPassenger.tlst_PaxType.PaxType;
            resPax.IsModified = false;
            return resPax;

        }

        public static explicit operator ReservationPax(tbPassenger dbPassenger)
        {
            var resPax = new ReservationPax();
            resPax.IDX = dbPassenger.pkPassengerID;
            resPax.FirstName = dbPassenger.FirstName != null ? dbPassenger.FirstName : "";
            resPax.Surname = dbPassenger.Surname != null ? dbPassenger.Surname : "";
            resPax.Age = dbPassenger.Age ?? 0;
            resPax.IDX_PaxType = dbPassenger.fkPassengerTypeID ?? 1;
            resPax.Weight = dbPassenger.Weight ?? 0;
            resPax.PassportNo = dbPassenger.Passport;
            resPax.Nationality = dbPassenger.Nationality;
            resPax.Sex = dbPassenger.Sex;
            resPax.WishGuestID = dbPassenger.WISHGuestID ?? 0;
            resPax.TicketPrinted = dbPassenger.TicketPrinted ?? false;
            resPax.LuggageWeight = dbPassenger.Luggageweight ?? 0;

            if (dbPassenger.tbPassengerType != null)
                resPax.PassengerType = dbPassenger.tbPassengerType.Description;
            resPax.IsModified = false;
            return resPax;

        }

    }
}
