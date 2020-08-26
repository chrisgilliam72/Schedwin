using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedwinAPI
{
    public class SchedwinPassenger
    {
        public String FirstName { get; set; }
        public String Surname { get; set; }
        public int Age { get; set; }
        public String Sex { get; set; }
        public String PassportNo { get; set; }

        public String PassportNationality { get; set; }

        public static explicit operator SchedwinPassenger(tsch_Passengers tschPax)
        {
            var schedwinPax = new SchedwinPassenger();
            schedwinPax.FirstName = tschPax.FirstName;
            schedwinPax.Surname = tschPax.Surname;
            schedwinPax.Age = Convert.ToInt32(tschPax.Age);
            schedwinPax.Sex = tschPax.Sex;
            schedwinPax.PassportNo = schedwinPax.PassportNo;
            schedwinPax.PassportNationality = schedwinPax.PassportNationality;

            return schedwinPax;
        }

        public static explicit operator tsch_Passengers(SchedwinPassenger schedwinPax)
        {
            var tschPax = new tsch_Passengers();
            tschPax.FirstName = schedwinPax.FirstName;
            tschPax.Surname = schedwinPax.Surname;
            tschPax.Age = Convert.ToByte(schedwinPax.Age);

            tschPax.Sex = schedwinPax.Sex;
            tschPax.Passport = schedwinPax.PassportNo;
            tschPax.Nationality = schedwinPax.PassportNationality;

            return tschPax;
        }

    }

}

