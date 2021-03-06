﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AIMS_BD_IATI.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AIMS_DBEntities : DbContext
    {
        public AIMS_DBEntities()
            : base("name=AIMS_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblAgreementType> tblAgreementTypes { get; set; }
        public virtual DbSet<tblAidCategory> tblAidCategories { get; set; }
        public virtual DbSet<tblAIDEffectivenessIndicator> tblAIDEffectivenessIndicators { get; set; }
        public virtual DbSet<tblAIDEffectivenessPIUType> tblAIDEffectivenessPIUTypes { get; set; }
        public virtual DbSet<tblAIDEffectivenessResourceTiedType> tblAIDEffectivenessResourceTiedTypes { get; set; }
        public virtual DbSet<tblAIDPlanning> tblAIDPlannings { get; set; }
        public virtual DbSet<tblAIDPlanningDetail> tblAIDPlanningDetails { get; set; }
        public virtual DbSet<tblApprovalAuthority> tblApprovalAuthorities { get; set; }
        public virtual DbSet<tblApprovalStatu> tblApprovalStatus { get; set; }
        public virtual DbSet<tblAssistanceType> tblAssistanceTypes { get; set; }
        public virtual DbSet<tblCommonConfiguration> tblCommonConfigurations { get; set; }
        public virtual DbSet<tblCurrency> tblCurrencies { get; set; }
        public virtual DbSet<tblCurrencyBB> tblCurrencyBBs { get; set; }
        public virtual DbSet<tblCurrencyMapping> tblCurrencyMappings { get; set; }
        public virtual DbSet<tblDistrict> tblDistricts { get; set; }
        public virtual DbSet<tblDivision> tblDivisions { get; set; }
        public virtual DbSet<tblDocumentCategory> tblDocumentCategories { get; set; }
        public virtual DbSet<tblDocumentType> tblDocumentTypes { get; set; }
        public virtual DbSet<tblExchangeRate> tblExchangeRates { get; set; }
        public virtual DbSet<tblExchangeRateDetail> tblExchangeRateDetails { get; set; }
        public virtual DbSet<tblExecutingAgencyType> tblExecutingAgencyTypes { get; set; }
        public virtual DbSet<tblFinancialYear> tblFinancialYears { get; set; }
        public virtual DbSet<tblFundSource> tblFundSources { get; set; }
        public virtual DbSet<tblFundSourceCategory> tblFundSourceCategories { get; set; }
        public virtual DbSet<tblImplementationStatu> tblImplementationStatus { get; set; }
        public virtual DbSet<tblLoanAgreement> tblLoanAgreements { get; set; }
        public virtual DbSet<tblLoanRepaymentType> tblLoanRepaymentTypes { get; set; }
        public virtual DbSet<tblMinistry> tblMinistries { get; set; }
        public virtual DbSet<tblMinistryAgency> tblMinistryAgencies { get; set; }
        public virtual DbSet<tblNGOCSO> tblNGOCSOes { get; set; }
        public virtual DbSet<tblNGOOrganizationType> tblNGOOrganizationTypes { get; set; }
        public virtual DbSet<tblOrganizationType> tblOrganizationTypes { get; set; }
        public virtual DbSet<tblProjectADPInfo> tblProjectADPInfoes { get; set; }
        public virtual DbSet<tblProjectAnalyticalWorksMissionInfo> tblProjectAnalyticalWorksMissionInfoes { get; set; }
        public virtual DbSet<tblProjectAttachment> tblProjectAttachments { get; set; }
        public virtual DbSet<tblProjectExecutingAgency> tblProjectExecutingAgencies { get; set; }
        public virtual DbSet<tblProjectFundingActualDisbursement> tblProjectFundingActualDisbursements { get; set; }
        public virtual DbSet<tblProjectFundingCommitment> tblProjectFundingCommitments { get; set; }
        public virtual DbSet<tblProjectFundingExpenditure> tblProjectFundingExpenditures { get; set; }
        public virtual DbSet<tblProjectFundingPlannedDisbursement> tblProjectFundingPlannedDisbursements { get; set; }
        public virtual DbSet<tblProjectGeographicAllocation> tblProjectGeographicAllocations { get; set; }
        public virtual DbSet<tblProjectGoBExecutingAgency> tblProjectGoBExecutingAgencies { get; set; }
        public virtual DbSet<tblProjectInfo> tblProjectInfoes { get; set; }
        public virtual DbSet<tblProjectNote> tblProjectNotes { get; set; }
        public virtual DbSet<tblProjectSectoralAllocation> tblProjectSectoralAllocations { get; set; }
        public virtual DbSet<tblProjectThematicMarker> tblProjectThematicMarkers { get; set; }
        public virtual DbSet<tblProjectType> tblProjectTypes { get; set; }
        public virtual DbSet<tblRevisionStatu> tblRevisionStatus { get; set; }
        public virtual DbSet<tblSector> tblSectors { get; set; }
        public virtual DbSet<tblSectorSubSectorMap> tblSectorSubSectorMaps { get; set; }
        public virtual DbSet<tblSubSector> tblSubSectors { get; set; }
        public virtual DbSet<tblThematicMarker> tblThematicMarkers { get; set; }
        public virtual DbSet<tblTrustFund> tblTrustFunds { get; set; }
        public virtual DbSet<tblTrustFundDetail> tblTrustFundDetails { get; set; }
        public virtual DbSet<tblUpazila> tblUpazilas { get; set; }
        public virtual DbSet<TblUserActivity> TblUserActivities { get; set; }
        public virtual DbSet<tblUserFundSource> tblUserFundSources { get; set; }
        public virtual DbSet<tblUserProject> tblUserProjects { get; set; }
        public virtual DbSet<tblUserRegistrationInfo> tblUserRegistrationInfoes { get; set; }
        public virtual DbSet<tblUserRegistrationStatu> tblUserRegistrationStatus { get; set; }
        public virtual DbSet<tblVerificationStatu> tblVerificationStatus { get; set; }
        public virtual DbSet<CommonConfigType> CommonConfigTypes { get; set; }
        public virtual DbSet<tblExchangeRateBBApi> tblExchangeRateBBApis { get; set; }
        public virtual DbSet<vwExchangeRateBBApi> vwExchangeRateBBApis { get; set; }
        public virtual DbSet<tblPolicyMarker> tblPolicyMarkers { get; set; }
        public virtual DbSet<tblPolicyMarkerVocabulary> tblPolicyMarkerVocabularies { get; set; }
        public virtual DbSet<tblPolicySignificance> tblPolicySignificances { get; set; }
        public virtual DbSet<tblProjectFundingIncoming> tblProjectFundingIncomings { get; set; }
        public virtual DbSet<tblProjectPolicyMarker> tblProjectPolicyMarkers { get; set; }
        public virtual DbSet<tblProjectResult> tblProjectResults { get; set; }
        public virtual DbSet<tblProjectResultDetail> tblProjectResultDetails { get; set; }
        public virtual DbSet<tblResultIndicatorType> tblResultIndicatorTypes { get; set; }
        public virtual DbSet<tblTransactionType> tblTransactionTypes { get; set; }
    }
}
