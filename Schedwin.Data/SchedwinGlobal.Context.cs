﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SchedwinGlobalEntities : DbContext
    {
        public SchedwinGlobalEntities()
            : base("name=SchedwinGlobalEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbRole> tbRoles { get; set; }
        public virtual DbSet<tbUserRole> tbUserRoles { get; set; }
        public virtual DbSet<tbCanLogIn> tbCanLogIns { get; set; }
        public virtual DbSet<tbMininumVersionBuild> tbMininumVersionBuilds { get; set; }
        public virtual DbSet<tbUserLogonDetail> tbUserLogonDetails { get; set; }
        public virtual DbSet<tbUserSetting> tbUserSettings { get; set; }
        public virtual DbSet<tbDBRegionInfo> tbDBRegionInfoes { get; set; }
        public virtual DbSet<tbAirstrip> tbAirstrips { get; set; }
        public virtual DbSet<tbCountry> tbCountries { get; set; }
        public virtual DbSet<tbCurrency> tbCurrencies { get; set; }
        public virtual DbSet<tbLodge> tbLodges { get; set; }
        public virtual DbSet<tbFlight> tbFlights { get; set; }
        public virtual DbSet<tbAircraftType> tbAircraftTypes { get; set; }
        public virtual DbSet<tbModulePermission> tbModulePermissions { get; set; }
        public virtual DbSet<tbPassenger> tbPassengers { get; set; }
        public virtual DbSet<tbReservationStatus> tbReservationStatus1 { get; set; }
        public virtual DbSet<tbReservationType> tbReservationTypes { get; set; }
        public virtual DbSet<tbReservationLeg> tbReservationLegs { get; set; }
        public virtual DbSet<tbOperator> tbOperators { get; set; }
        public virtual DbSet<tbStandardPaxWeight> tbStandardPaxWeights { get; set; }
        public virtual DbSet<tbAirportFeeType> tbAirportFeeTypes { get; set; }
        public virtual DbSet<tbAircraft> tbAircrafts { get; set; }
        public virtual DbSet<tbOperatorAgent> tbOperatorAgents { get; set; }
        public virtual DbSet<tbReservationHeader> tbReservationHeaders { get; set; }
        public virtual DbSet<tbPassengerType> tbPassengerTypes { get; set; }
        public virtual DbSet<tbAirportFee> tbAirportFees { get; set; }
        public virtual DbSet<tbReservationLegBudget> tbReservationLegBudgets { get; set; }
        public virtual DbSet<tbWISHIntegrationHeader> tbWISHIntegrationHeaders { get; set; }
        public virtual DbSet<tbWishIntegrationLeg> tbWishIntegrationLegs { get; set; }
        public virtual DbSet<tbRosterDutyType> tbRosterDutyTypes { get; set; }
        public virtual DbSet<tbPilotRoster> tbPilotRosters { get; set; }
        public virtual DbSet<tbUser> tbUsers { get; set; }
        public virtual DbSet<tbPilot> tbPilots { get; set; }
    }
}
