//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Schedwin.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbAirstrip
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbAirstrip()
        {
            this.tbLodges = new HashSet<tbLodge>();
            this.tbFlights = new HashSet<tbFlight>();
            this.tbReservationLegs = new HashSet<tbReservationLeg>();
            this.tbReservationLegs1 = new HashSet<tbReservationLeg>();
            this.tbAirportFees = new HashSet<tbAirportFee>();
            this.tbReservationLegBudgets = new HashSet<tbReservationLegBudget>();
            this.tbReservationLegBudgets1 = new HashSet<tbReservationLegBudget>();
        }
    
        public int pkAirstripID { get; set; }
        public int fkCountryID { get; set; }
        public string Name { get; set; }
        public string Designator { get; set; }
        public string AreaCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public short Altitude { get; set; }
        public float RunwayHeading { get; set; }
        public short RunwayLength { get; set; }
        public float SurfaceFactor { get; set; }
        public short TurnaroundTime { get; set; }
        public float OvernightFee { get; set; }
        public string IATA { get; set; }
        public float DepTaxInternal { get; set; }
        public float DepTaxInternational { get; set; }
        public float TASPermitFee { get; set; }
        public bool FuelPoint { get; set; }
        public bool CustomsPoint { get; set; }
        public double PilotNightStop { get; set; }
        public Nullable<int> fkAlternateAirport { get; set; }
        public int AlternateDist { get; set; }
        public Nullable<double> LatitudeDecimal { get; set; }
        public Nullable<double> LongitudeDecimal { get; set; }
        public System.DateTime Sunrise { get; set; }
        public System.DateTime Sunset { get; set; }
        public short NightStopRating { get; set; }
        public bool IsHeliport { get; set; }
        public int MagVariation { get; set; }
        public string TPCode { get; set; }
        public Nullable<bool> Active { get; set; }
        public string AlternateAirportTMPIATA { get; set; }
        public Nullable<int> fkCurrencyID { get; set; }
    
        public virtual tbCountry tbCountry { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbLodge> tbLodges { get; set; }
        public virtual tbCurrency tbCurrency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbFlight> tbFlights { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbReservationLeg> tbReservationLegs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbReservationLeg> tbReservationLegs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbAirportFee> tbAirportFees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbReservationLegBudget> tbReservationLegBudgets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbReservationLegBudget> tbReservationLegBudgets1 { get; set; }
    }
}
