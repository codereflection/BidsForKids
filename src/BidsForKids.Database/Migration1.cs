using FluentMigrator;

namespace BidsForKids.Database
{
    [Migration(1)]
    public class Migration1 : Migration
    {
        public override void Up()
        {
            CreateAuctionTable();
            CreateCategoryTable();
            CreateContactProcurementTable();
            CreateDonatesTable();
            CreateDonorTable();
            CreateDonorTypeTable();
            CreateGeoLocationTable();
            CreateLocationCodeMapTable();
            CreateProcurementTable();
            CreateProcurementHistoryTable();
            CreateProcurementDonorTable();
            CreateProcurementTypeTable();
            CreateProcurerTable();
            CreateForeignKeys();
        }

        private void CreateForeignKeys()
        {
            Create.ForeignKey("FK_ContactProcurement_Auction")
                .FromTable("ContactProcurement").ForeignColumn("Auction_ID")
                .ToTable("Auction").PrimaryColumn("Auction_ID");
            Create.ForeignKey("FK_ContactProcurement_Contact")
                .FromTable("ContactProcurement").ForeignColumn("Donor_ID")
                .ToTable("Donor").PrimaryColumn("Donor_ID");
            Create.ForeignKey("FK_ContactProcurement_Procurement")
                .FromTable("ContactProcurement").ForeignColumn("Procurement_ID")
                .ToTable("Procurement").PrimaryColumn("Procurement_ID");
            Create.ForeignKey("FK_ContactProcurement_Procurer")
                .FromTable("ContactProcurement").ForeignColumn("Procurer_ID")
                .ToTable("Procurer").PrimaryColumn("Procurer_ID");


            Create.ForeignKey("FK_Donor_GeoLocation")
                .FromTable("Donor").ForeignColumn("GeoLocation_ID")
                .ToTable("GeoLocation").PrimaryColumn("GeoLocation_ID");
            Create.ForeignKey("FK_Donor_Procurer")
                .FromTable("Donor").ForeignColumn("Procurer_ID")
                .ToTable("Procurer").PrimaryColumn("Procurer_ID");


            Create.ForeignKey("FK_Procurement_Category")
                .FromTable("Procurement").ForeignColumn("Category_ID")
                .ToTable("Category").PrimaryColumn("Category_ID");
        }

        public override void Down()
        {
            DeleteForeignKeys();
            DeleteTables();
        }

        private void DeleteTables()
        {
            Delete.Table("Procurer");
            Delete.Table("ProcurementType");
            Delete.Table("ProcurementDonor");
            Delete.Table("ProcurementHistory");
            Delete.Table("Procurement");
            Delete.Table("LocationCodeMap");
            Delete.Table("GeoLocation");
            Delete.Table("DonorType");
            Delete.Table("Donor");
            Delete.Table("Donates");
            Delete.Table("ContactProcurement");
            Delete.Table("Category");
            Delete.Table("Auction");
        }

        private void DeleteForeignKeys()
        {
            Delete.ForeignKey("FK_Procurement_Category").OnTable("Procurement");
            Delete.ForeignKey("FK_Donor_Procurer").OnTable("Donor");
            Delete.ForeignKey("FK_Donor_GeoLocation").OnTable("Donor");
            Delete.ForeignKey("FK_ContactProcurement_Procurer").OnTable("ContactProcurement");
            Delete.ForeignKey("FK_ContactProcurement_Procurement").OnTable("ContactProcurement");
            Delete.ForeignKey("FK_ContactProcurement_Contact").OnTable("ContactProcurement");
            Delete.ForeignKey("FK_ContactProcurement_Auction").OnTable("ContactProcurement");
        }

        void CreateAuctionTable()
        {
            Create.Table("Auction")
                .WithColumn("Auction_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Year").AsInt32().NotNullable()
                .WithColumn("Name").AsAnsiString(255).NotNullable();
        }

        void CreateCategoryTable()
        {
            Create.Table("Category")
                .WithColumn("Category_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("CategoryName").AsAnsiString(255).NotNullable()
                .WithColumn("Description").AsAnsiString(1000).Nullable();
        }

        void CreateContactProcurementTable()
        {
            Create.Table("ContactProcurement")
                .WithColumn("ContactProcurement_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Donor_ID").AsInt32().NotNullable()
                .WithColumn("Procurement_ID").AsInt32().NotNullable()
                .WithColumn("Auction_ID").AsInt32().NotNullable()
                .WithColumn("Procurer_ID").AsInt32().Nullable();
        }

        void CreateDonatesTable()
        {
            Create.Table("Donates")
                .WithColumn("Donates_ID").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("Description").AsAnsiString(20).NotNullable();
        }

        void CreateDonorTable()
        {
            Create.Table("Donor")
                .WithColumn("Donor_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("BusinessName").AsAnsiString(255).Nullable()
                .WithColumn("FirstName").AsAnsiString(255).Nullable()
                .WithColumn("LastName").AsAnsiString(255).Nullable()
                .WithColumn("Address").AsAnsiString(255).Nullable()
                .WithColumn("City").AsAnsiString(255).Nullable()
                .WithColumn("State").AsAnsiString(2).NotNullable()
                .WithColumn("ZipCode").AsAnsiString(10).Nullable()
                .WithColumn("Phone1").AsAnsiString(20).Nullable()
                .WithColumn("Phone1Desc").AsAnsiString(10).Nullable()
                .WithColumn("Phone2").AsAnsiString(20).Nullable()
                .WithColumn("Phone2Desc").AsAnsiString(10).Nullable()
                .WithColumn("Phone3").AsAnsiString(20).Nullable()
                .WithColumn("Phone3Desc").AsAnsiString(10).Nullable()
                .WithColumn("Notes").AsAnsiString(int.MaxValue).Nullable()
                .WithColumn("Website").AsAnsiString(1024).Nullable()
                .WithColumn("Email").AsAnsiString(255).Nullable()
                .WithColumn("GeoLocation_ID").AsInt32().Nullable()
                .WithColumn("Donates").AsBoolean().Nullable()
                .WithColumn("CreatedOn").AsDateTime().Nullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedOn").AsDateTime().Nullable()
                .WithColumn("MailedPacket").AsBoolean().Nullable()
                .WithColumn("Procurer_ID").AsInt32().Nullable()
                .WithColumn("DonorType_ID").AsInt32().Nullable()
                .WithColumn("Closed").AsBoolean().Nullable();

            //Execute.Sql("CREATE Trigger_Donor_Set_CreatedOn HERE");
            //Execute.Sql("CREATE Trigger_Donor_Set_ModifiedOn HERE");
            const string triggers = @"
-- =============================================
-- Author:		Jeff Schumacher
-- Create date: Oct 24, 2009
-- Description:	Sets the ModifiedOn to GETDATE() after each record update
-- =============================================
CREATE TRIGGER [dbo].[Trigger_Donor_Set_ModifiedOn]
   ON  [dbo].[Donor]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE Donor
	SET Donor.ModifiedOn = GETDATE()
	FROM Donor
	JOIN Inserted ON Donor.Donor_ID = Inserted.Donor_ID
END
GO";
            Execute.Sql(triggers);
        }

        void CreateDonorTypeTable()
        {
            Create.Table("DonorType")
                .WithColumn("DonorType_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("DonorTypeDesc").AsAnsiString(50).NotNullable();
        }

        void CreateGeoLocationTable()
        {
            Create.Table("GeoLocation")
                .WithColumn("GeoLocation_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("GeoLocationName").AsAnsiString(255).NotNullable()
                .WithColumn("Description").AsAnsiString(1000).Nullable();
        }

        void CreateLocationCodeMapTable()
        {
            Create.Table("LocationCodeMap")
                .WithColumn("LocationCode").AsAnsiString(255).Nullable()
                .WithColumn("GeoLocation_ID").AsInt32().Nullable();
        }

        void CreateProcurementTable()
        {
            Create.Table("Procurement")
                .WithColumn("Procurement_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Description").AsAnsiString(2000).NotNullable()
                .WithColumn("Quantity").AsFloat().Nullable()
                .WithColumn("PerItemValue").AsCurrency().Nullable()
                .WithColumn("Notes").AsAnsiString(int.MaxValue).Nullable()
                .WithColumn("CatalogNumber").AsAnsiString(20).Nullable()
                .WithColumn("AuctionNumber").AsAnsiString(20).Nullable()
                .WithColumn("ItemNumber").AsAnsiString(20).Nullable()
                .WithColumn("EstimatedValue").AsCurrency().Nullable()
                .WithColumn("SoldFor").AsCurrency().Nullable()
                .WithColumn("Category_ID").AsInt32().Nullable()
                .WithColumn("Origin_ID").AsInt32().Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedOn").AsDateTime().Nullable()
                .WithColumn("Donation").AsAnsiString(2000).Nullable()
                .WithColumn("ThankYouLetterSent").AsBoolean().Nullable()
                .WithColumn("Limitations").AsAnsiString(500).Nullable()
                .WithColumn("Certificate").AsAnsiString(10).Nullable()
                .WithColumn("ProcurementType_ID").AsInt32().Nullable()
                .WithColumn("Title").AsAnsiString(2000).Nullable();

            const string triggers = @"
-- =============================================
-- Author:		Jeff Schumacher
-- Create date: 3/4/2010
-- Description:	Inserts old record information
-- from Procurement into Procurement_History
-- upon Delete
-- =============================================
CREATE TRIGGER [dbo].[Trigger_Procurement_DELETE]
   ON  [dbo].[Procurement]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   INSERT INTO [Procurement_History]
           ([Operation]
           ,[Procurement_ID]
           ,[Donation]
           ,[Description]
           ,[Quantity]
           ,[PerItemValue]
           --,[Notes]
           ,[CatalogNumber]
           ,[AuctionNumber]
           ,[ItemNumber]
           ,[EstimatedValue]
           ,[SoldFor]
           ,[Category_ID]
           ,[Origin_ID]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[ThankYouLetterSent]
           ,[Limitations]
           ,[Certificate]
           ,[ProcurementType_ID])
     SELECT
			'Delete'
           ,[Procurement_ID]
           ,[Donation]
           ,[Description]
           ,[Quantity]
           ,[PerItemValue]
           --,[Notes] -- cannot use 'text' datatype fields from the Deleted table
           ,[CatalogNumber]
           ,[AuctionNumber]
           ,[ItemNumber]
           ,[EstimatedValue]
           ,[SoldFor]
           ,[Category_ID]
           ,[Origin_ID]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[ThankYouLetterSent]
           ,[Limitations]
           ,[Certificate]
           ,[ProcurementType_ID]
	FROM Deleted

END
GO

-- =============================================
-- Author:		Jeff Schumacher
-- Create date: Oct 24, 2009
-- Description:	Sets the ModifiedOn to GETDATE() after each record update
-- =============================================
CREATE TRIGGER [dbo].[Trigger_Procurement_Set_ModifiedOn]
   ON  [dbo].[Procurement]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE Procurement
	SET Procurement.ModifiedOn = GETDATE()
	FROM Procurement
	JOIN Inserted ON Procurement.Procurement_ID = Inserted.Procurement_ID
END
GO

-- =============================================
-- Author:		Jeff Schumacher
-- Create date: 3/4/2010
-- Description:	Inserts old record information
-- from Procurement into Procurement_History
-- upon Update
-- =============================================
CREATE TRIGGER [dbo].[Trigger_Procurement_UPDATE]
   ON  [dbo].[Procurement]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   INSERT INTO [Procurement_History]
           ([Operation]
           ,[Procurement_ID]
           ,[Donation]
           ,[Description]
           ,[Quantity]
           ,[PerItemValue]
           --,[Notes]
           ,[CatalogNumber]
           ,[AuctionNumber]
           ,[ItemNumber]
           ,[EstimatedValue]
           ,[SoldFor]
           ,[Category_ID]
           ,[Origin_ID]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[ThankYouLetterSent]
           ,[Limitations]
           ,[Certificate]
           ,[ProcurementType_ID])
     SELECT
			'Update'
           ,[Procurement_ID]
           ,[Donation]
           ,[Description]
           ,[Quantity]
           ,[PerItemValue]
           --,[Notes] -- cannot use 'text' datatype fields from the Deleted table
           ,[CatalogNumber]
           ,[AuctionNumber]
           ,[ItemNumber]
           ,[EstimatedValue]
           ,[SoldFor]
           ,[Category_ID]
           ,[Origin_ID]
           ,[CreatedOn]
           ,[ModifiedOn]
           ,[ThankYouLetterSent]
           ,[Limitations]
           ,[Certificate]
           ,[ProcurementType_ID]
	FROM Deleted

END
GO";
            Execute.Sql(triggers);
        }

        void CreateProcurementHistoryTable()
        {
            Create.Table("ProcurementHistory")
                .WithColumn("ProcurementHistory_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Operation").AsAnsiString(6).NotNullable()
                .WithColumn("Procurement_ID").AsInt32().NotNullable()
                .WithColumn("Description").AsAnsiString(2000).NotNullable()
                .WithColumn("Quantity").AsFloat().Nullable()
                .WithColumn("PerItemValue").AsCurrency().Nullable()
                .WithColumn("Notes").AsAnsiString(int.MaxValue).Nullable()
                .WithColumn("CatalogNumber").AsAnsiString(20).Nullable()
                .WithColumn("AuctionNumber").AsAnsiString(20).Nullable()
                .WithColumn("ItemNumber").AsAnsiString(20).Nullable()
                .WithColumn("EstimatedValue").AsCurrency().Nullable()
                .WithColumn("SoldFor").AsCurrency().Nullable()
                .WithColumn("Category_ID").AsInt32().Nullable()
                .WithColumn("Origin_ID").AsInt32().Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("ModifiedOn").AsDateTime().Nullable()
                .WithColumn("Donation").AsAnsiString(2000).Nullable()
                .WithColumn("ThankYouLetterSent").AsBoolean().Nullable()
                .WithColumn("Limitations").AsAnsiString(500).Nullable()
                .WithColumn("Certificate").AsAnsiString(10).Nullable()
                .WithColumn("ProcurementType_ID").AsInt32().Nullable()
                .WithColumn("Title").AsAnsiString(2000).Nullable();
        }

        void CreateProcurementDonorTable()
        {
            Create.Table("ProcurementDonor")
                .WithColumn("ProcurementDonor_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Procurement_ID").AsInt32().NotNullable()
                .WithColumn("Donor_ID").AsInt32().NotNullable();
        }

        void CreateProcurementTypeTable()
        {
            Create.Table("ProcurementType")
                .WithColumn("ProcurementType_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("ProcurementTypeDesc").AsAnsiString(50).NotNullable()
                .WithColumn("DonorType_ID").AsInt32().Nullable();
        }

        void CreateProcurerTable()
        {
            Create.Table("Procurer")
                .WithColumn("Procurer_ID").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("FirstName").AsAnsiString(255).Nullable()
                .WithColumn("LastName").AsAnsiString(255).Nullable()
                .WithColumn("Phone").AsAnsiString(20).Nullable()
                .WithColumn("Email").AsAnsiString(255).Nullable()
                .WithColumn("Description").AsAnsiString(255).Nullable();
        }
    }
}
