﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CharitAble_current.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class charitable_dbEntities2 : DbContext
    {
        public charitable_dbEntities2()
            : base("name=charitable_dbEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Admin> tbl_Admin { get; set; }
        public virtual DbSet<tbl_Cases> tbl_Cases { get; set; }
        public virtual DbSet<tbl_DonationCategory> tbl_DonationCategory { get; set; }
        public virtual DbSet<tbl_DonationCondition> tbl_DonationCondition { get; set; }
        public virtual DbSet<tbl_Donations> tbl_Donations { get; set; }
        public virtual DbSet<tbl_DonorMaster> tbl_DonorMaster { get; set; }
        public virtual DbSet<tbl_DonorReplies> tbl_DonorReplies { get; set; }
        public virtual DbSet<tbl_Feedback> tbl_Feedback { get; set; }
        public virtual DbSet<tbl_Invoices> tbl_Invoices { get; set; }
        public virtual DbSet<tbl_MaterialGroup> tbl_MaterialGroup { get; set; }
        public virtual DbSet<tbl_MaterialMaster> tbl_MaterialMaster { get; set; }
        public virtual DbSet<tbl_MaterialType> tbl_MaterialType { get; set; }
        public virtual DbSet<tbl_NGOMaster> tbl_NGOMaster { get; set; }
        public virtual DbSet<tbl_Orders> tbl_Orders { get; set; }
        public virtual DbSet<tbl_PaymentInfo> tbl_PaymentInfo { get; set; }
        public virtual DbSet<tbl_Status> tbl_Status { get; set; }
        public virtual DbSet<tbl_SubscriptionPlan> tbl_SubscriptionPlan { get; set; }
        public virtual DbSet<tbl_SuccessStories> tbl_SuccessStories { get; set; }
        public virtual DbSet<tbl_Units> tbl_Units { get; set; }
        public virtual DbSet<tbl_Users> tbl_Users { get; set; }
        public virtual DbSet<tbl_UserType> tbl_UserType { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
        public virtual DbSet<ipv6_database_firewall_rules> ipv6_database_firewall_rules { get; set; }
        public virtual DbSet<NGOResponse> NGOResponses { get; set; }
    
        public virtual int SP_AddCase(string caseTitle, Nullable<System.DateTime> postedDate, string description, byte[] picture)
        {
            var caseTitleParameter = caseTitle != null ?
                new ObjectParameter("CaseTitle", caseTitle) :
                new ObjectParameter("CaseTitle", typeof(string));
    
            var postedDateParameter = postedDate.HasValue ?
                new ObjectParameter("PostedDate", postedDate) :
                new ObjectParameter("PostedDate", typeof(System.DateTime));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var pictureParameter = picture != null ?
                new ObjectParameter("Picture", picture) :
                new ObjectParameter("Picture", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AddCase", caseTitleParameter, postedDateParameter, descriptionParameter, pictureParameter);
        }
    
        public virtual int SP_AddDonation(Nullable<int> donorID, string donationTitle, Nullable<short> quantity, Nullable<decimal> weight, Nullable<byte> quantityPerUnit, Nullable<System.DateTime> expiryDate, string description, Nullable<long> locationCOD, byte[] picture)
        {
            var donorIDParameter = donorID.HasValue ?
                new ObjectParameter("DonorID", donorID) :
                new ObjectParameter("DonorID", typeof(int));
    
            var donationTitleParameter = donationTitle != null ?
                new ObjectParameter("DonationTitle", donationTitle) :
                new ObjectParameter("DonationTitle", typeof(string));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(short));
    
            var weightParameter = weight.HasValue ?
                new ObjectParameter("Weight", weight) :
                new ObjectParameter("Weight", typeof(decimal));
    
            var quantityPerUnitParameter = quantityPerUnit.HasValue ?
                new ObjectParameter("QuantityPerUnit", quantityPerUnit) :
                new ObjectParameter("QuantityPerUnit", typeof(byte));
    
            var expiryDateParameter = expiryDate.HasValue ?
                new ObjectParameter("ExpiryDate", expiryDate) :
                new ObjectParameter("ExpiryDate", typeof(System.DateTime));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var locationCODParameter = locationCOD.HasValue ?
                new ObjectParameter("LocationCOD", locationCOD) :
                new ObjectParameter("LocationCOD", typeof(long));
    
            var pictureParameter = picture != null ?
                new ObjectParameter("Picture", picture) :
                new ObjectParameter("Picture", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AddDonation", donorIDParameter, donationTitleParameter, quantityParameter, weightParameter, quantityPerUnitParameter, expiryDateParameter, descriptionParameter, locationCODParameter, pictureParameter);
        }
    
        public virtual ObjectResult<SP_SelectAllAdmins_Result> SP_SelectAllAdmins()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllAdmins_Result>("SP_SelectAllAdmins");
        }
    
        public virtual ObjectResult<SP_SelectAllCases_Result> SP_SelectAllCases()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllCases_Result>("SP_SelectAllCases");
        }
    
        public virtual ObjectResult<SP_SelectAllDonations_Result> SP_SelectAllDonations()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllDonations_Result>("SP_SelectAllDonations");
        }
    
        public virtual ObjectResult<SP_SelectAllDonors_Result> SP_SelectAllDonors()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllDonors_Result>("SP_SelectAllDonors");
        }
    
        public virtual ObjectResult<SP_SelectAllInvoices_Result> SP_SelectAllInvoices()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllInvoices_Result>("SP_SelectAllInvoices");
        }
    
        public virtual ObjectResult<SP_SelectAllMaterialGroup_Result> SP_SelectAllMaterialGroup()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllMaterialGroup_Result>("SP_SelectAllMaterialGroup");
        }
    
        public virtual ObjectResult<SP_SelectAllMaterials_Result> SP_SelectAllMaterials()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllMaterials_Result>("SP_SelectAllMaterials");
        }
    
        public virtual ObjectResult<SP_SelectAllMaterialType_Result> SP_SelectAllMaterialType()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllMaterialType_Result>("SP_SelectAllMaterialType");
        }
    
        public virtual ObjectResult<SP_SelectAllNgos_Result> SP_SelectAllNgos()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllNgos_Result>("SP_SelectAllNgos");
        }
    
        public virtual ObjectResult<SP_SelectAllOrders_Result> SP_SelectAllOrders()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllOrders_Result>("SP_SelectAllOrders");
        }
    
        public virtual ObjectResult<SP_SelectAllSubscriptionPlans_Result> SP_SelectAllSubscriptionPlans()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllSubscriptionPlans_Result>("SP_SelectAllSubscriptionPlans");
        }
    
        public virtual ObjectResult<SP_SelectAllSuccessStories_Result> SP_SelectAllSuccessStories()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllSuccessStories_Result>("SP_SelectAllSuccessStories");
        }
    
        public virtual ObjectResult<SP_SelectAllUsers_Result> SP_SelectAllUsers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllUsers_Result>("SP_SelectAllUsers");
        }
    
        public virtual ObjectResult<SP_SelectAllUserType_Result> SP_SelectAllUserType()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectAllUserType_Result>("SP_SelectAllUserType");
        }
    
        public virtual ObjectResult<SP_SelectPaymentInfo_Result> SP_SelectPaymentInfo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SelectPaymentInfo_Result>("SP_SelectPaymentInfo");
        }
    
        public virtual int SP_UpdateCaseAgainstCaseID(Nullable<int> caseID, string caseTitle, Nullable<System.DateTime> postedDate, string description, byte[] picture)
        {
            var caseIDParameter = caseID.HasValue ?
                new ObjectParameter("CaseID", caseID) :
                new ObjectParameter("CaseID", typeof(int));
    
            var caseTitleParameter = caseTitle != null ?
                new ObjectParameter("CaseTitle", caseTitle) :
                new ObjectParameter("CaseTitle", typeof(string));
    
            var postedDateParameter = postedDate.HasValue ?
                new ObjectParameter("PostedDate", postedDate) :
                new ObjectParameter("PostedDate", typeof(System.DateTime));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var pictureParameter = picture != null ?
                new ObjectParameter("Picture", picture) :
                new ObjectParameter("Picture", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateCaseAgainstCaseID", caseIDParameter, caseTitleParameter, postedDateParameter, descriptionParameter, pictureParameter);
        }
    
        public virtual int SP_UpdateCaseAgainstNgoID(Nullable<int> ngoID, string caseTitle, Nullable<System.DateTime> postedDate, string description, byte[] picture)
        {
            var ngoIDParameter = ngoID.HasValue ?
                new ObjectParameter("NgoID", ngoID) :
                new ObjectParameter("NgoID", typeof(int));
    
            var caseTitleParameter = caseTitle != null ?
                new ObjectParameter("CaseTitle", caseTitle) :
                new ObjectParameter("CaseTitle", typeof(string));
    
            var postedDateParameter = postedDate.HasValue ?
                new ObjectParameter("PostedDate", postedDate) :
                new ObjectParameter("PostedDate", typeof(System.DateTime));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var pictureParameter = picture != null ?
                new ObjectParameter("Picture", picture) :
                new ObjectParameter("Picture", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateCaseAgainstNgoID", ngoIDParameter, caseTitleParameter, postedDateParameter, descriptionParameter, pictureParameter);
        }
    
        public virtual int SP_UpdateDonationByDonationID(Nullable<int> donationID, Nullable<int> donorID, string donationTitle, Nullable<short> quantity, Nullable<decimal> weight, Nullable<byte> quantityPerUnit, Nullable<System.DateTime> expiryDate, string description, Nullable<long> locationCOD, byte[] picture)
        {
            var donationIDParameter = donationID.HasValue ?
                new ObjectParameter("DonationID", donationID) :
                new ObjectParameter("DonationID", typeof(int));
    
            var donorIDParameter = donorID.HasValue ?
                new ObjectParameter("DonorID", donorID) :
                new ObjectParameter("DonorID", typeof(int));
    
            var donationTitleParameter = donationTitle != null ?
                new ObjectParameter("DonationTitle", donationTitle) :
                new ObjectParameter("DonationTitle", typeof(string));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(short));
    
            var weightParameter = weight.HasValue ?
                new ObjectParameter("Weight", weight) :
                new ObjectParameter("Weight", typeof(decimal));
    
            var quantityPerUnitParameter = quantityPerUnit.HasValue ?
                new ObjectParameter("QuantityPerUnit", quantityPerUnit) :
                new ObjectParameter("QuantityPerUnit", typeof(byte));
    
            var expiryDateParameter = expiryDate.HasValue ?
                new ObjectParameter("ExpiryDate", expiryDate) :
                new ObjectParameter("ExpiryDate", typeof(System.DateTime));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var locationCODParameter = locationCOD.HasValue ?
                new ObjectParameter("LocationCOD", locationCOD) :
                new ObjectParameter("LocationCOD", typeof(long));
    
            var pictureParameter = picture != null ?
                new ObjectParameter("Picture", picture) :
                new ObjectParameter("Picture", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateDonationByDonationID", donationIDParameter, donorIDParameter, donationTitleParameter, quantityParameter, weightParameter, quantityPerUnitParameter, expiryDateParameter, descriptionParameter, locationCODParameter, pictureParameter);
        }
    
        public virtual int SP_UpdateDonationByDonorID(Nullable<int> donationID, Nullable<int> donorID, string donationTitle, Nullable<short> quantity, Nullable<decimal> weight, Nullable<byte> quantityPerUnit, Nullable<System.DateTime> expiryDate, string description, Nullable<long> locationCOD, byte[] picture)
        {
            var donationIDParameter = donationID.HasValue ?
                new ObjectParameter("DonationID", donationID) :
                new ObjectParameter("DonationID", typeof(int));
    
            var donorIDParameter = donorID.HasValue ?
                new ObjectParameter("DonorID", donorID) :
                new ObjectParameter("DonorID", typeof(int));
    
            var donationTitleParameter = donationTitle != null ?
                new ObjectParameter("DonationTitle", donationTitle) :
                new ObjectParameter("DonationTitle", typeof(string));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(short));
    
            var weightParameter = weight.HasValue ?
                new ObjectParameter("Weight", weight) :
                new ObjectParameter("Weight", typeof(decimal));
    
            var quantityPerUnitParameter = quantityPerUnit.HasValue ?
                new ObjectParameter("QuantityPerUnit", quantityPerUnit) :
                new ObjectParameter("QuantityPerUnit", typeof(byte));
    
            var expiryDateParameter = expiryDate.HasValue ?
                new ObjectParameter("ExpiryDate", expiryDate) :
                new ObjectParameter("ExpiryDate", typeof(System.DateTime));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var locationCODParameter = locationCOD.HasValue ?
                new ObjectParameter("LocationCOD", locationCOD) :
                new ObjectParameter("LocationCOD", typeof(long));
    
            var pictureParameter = picture != null ?
                new ObjectParameter("Picture", picture) :
                new ObjectParameter("Picture", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateDonationByDonorID", donationIDParameter, donorIDParameter, donationTitleParameter, quantityParameter, weightParameter, quantityPerUnitParameter, expiryDateParameter, descriptionParameter, locationCODParameter, pictureParameter);
        }
    }
}
