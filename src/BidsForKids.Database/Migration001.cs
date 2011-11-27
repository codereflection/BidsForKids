using FluentMigrator;

namespace BidsForKids.Database
{
    [Migration(001)]
    public class Migration001 : Migration
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
                .WithColumn("Auction_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("Year").AsInt16().NotNullable()
                .WithColumn("Name").AsString(255).NotNullable();
        }

        void CreateCategoryTable()
        {
            Create.Table("Category")
                .WithColumn("Category_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("CategoryName").AsString(255).NotNullable()
                .WithColumn("Description").AsString(1000).Nullable();
        }

        void CreateContactProcurementTable()
        {
            Create.Table("ContactProcurement")
                .WithColumn("ContactProcurement_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("Donor_ID").AsInt16().NotNullable()
                .WithColumn("Procurement_ID").AsInt16().NotNullable()
                .WithColumn("Auction_ID").AsInt16().NotNullable()
                .WithColumn("Procurer_ID").AsInt16().Nullable();
        }

        void CreateDonatesTable()
        {
            Create.Table("Donates")
                .WithColumn("Donates_ID").AsInt16().PrimaryKey().NotNullable()
                .WithColumn("Description").AsString(20).NotNullable();
        }

        void CreateDonorTable()
        {
            Create.Table("Donor")
                .WithColumn("Donor_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("BusinessName").AsString(255).Nullable()
                .WithColumn("FirstName").AsString(255).Nullable()
                .WithColumn("LastName").AsString(255).Nullable()
                .WithColumn("Address").AsString(255).Nullable()
                .WithColumn("City").AsString(255).Nullable()
                .WithColumn("State").AsString(2).NotNullable()
                .WithColumn("ZipCode").AsString(10).Nullable()
                .WithColumn("Phone1").AsString(20).Nullable()
                .WithColumn("Phone1Desc").AsString(10).Nullable()
                .WithColumn("Phone2").AsString(20).Nullable()
                .WithColumn("Phone2Desc").AsString(10).Nullable()
                .WithColumn("Phone3").AsString(20).Nullable()
                .WithColumn("Phone3Desc").AsString(10).Nullable()
                .WithColumn("Notes").AsString(int.MaxValue).Nullable()
                .WithColumn("Website").AsString(1024).Nullable()
                .WithColumn("Email").AsString(255).Nullable()
                .WithColumn("GeoLocation_ID").AsInt16().Nullable()
                .WithColumn("Donates").AsBoolean().Nullable()
                .WithColumn("CreatedOn").AsDateTime().Nullable()
                .WithColumn("ModifiedOn").AsDateTime().Nullable()
                .WithColumn("MailedPacket").AsBoolean().Nullable()
                .WithColumn("Procurer_ID").AsInt16().Nullable()
                .WithColumn("DonorType_ID").AsInt16().Nullable()
                .WithColumn("Closed").AsBoolean().Nullable();

            //Execute.Sql("CREATE Trigger_Donor_Set_CreatedOn HERE");
            //Execute.Sql("CREATE Trigger_Donor_Set_ModifiedOn HERE");
        }

        void CreateDonorTypeTable()
        {
            Create.Table("DonorType")
                .WithColumn("DonorType_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("DonorTypeDesc").AsString(50).NotNullable();
        }

        void CreateGeoLocationTable()
        {
            Create.Table("GeoLocation")
                .WithColumn("GeoLocation_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("GeoLocationName").AsString(255).NotNullable()
                .WithColumn("Description").AsString(1000).Nullable();
        }

        void CreateLocationCodeMapTable()
        {
            Create.Table("LocationCodeMap")
                .WithColumn("LocationCode").AsString(255).Nullable()
                .WithColumn("GeoLocation_ID").AsInt16().Nullable();
        }

        void CreateProcurementTable()
        {
            Create.Table("Procurement")
                .WithColumn("Procurement_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("Description").AsString(2000).NotNullable()
                .WithColumn("Quantity").AsFloat().Nullable()
                .WithColumn("PerItemValue").AsDecimal(4, 10).Nullable()
                .WithColumn("Notes").AsString(int.MaxValue).Nullable()
                .WithColumn("CatalogNumber").AsString(20).Nullable()
                .WithColumn("AuctionNumber").AsString(20).Nullable()
                .WithColumn("ItemNumber").AsString(20).Nullable()
                .WithColumn("EstimatedValue").AsDecimal(4, 10).Nullable()
                .WithColumn("SoldFor").AsDecimal(4, 10).Nullable()
                .WithColumn("Cateory_ID").AsInt16().Nullable()
                .WithColumn("Origin_ID").AsInt16().Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("ModifiedOn").AsDateTime().Nullable()
                .WithColumn("Donation").AsString(2000).Nullable()
                .WithColumn("ThankYouLetterSent").AsBoolean().Nullable()
                .WithColumn("Limitations").AsString(500).Nullable()
                .WithColumn("Certificate").AsString(10).Nullable()
                .WithColumn("ProcurementType_ID").AsInt16().Nullable()
                .WithColumn("Title").AsString(2000).Nullable();

            //Execute.Sql("CREATE Trigger_Procurement_DELETE");
            //Execute.Sql("CREATE Trigger_Procurement_UPDATE");
            //Execute.Sql("CREATE Trigger_Set_CreatedOn");
            //Execute.Sql("CREATE Trigger_Set_ModifiedOn");
        }

        void CreateProcurementHistoryTable()
        {
            Create.Table("ProcurementHistory")
                .WithColumn("ProcurementHistory_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("Operation").AsString(6).NotNullable()
                .WithColumn("Procurement_ID").AsInt16().NotNullable()
                .WithColumn("Description").AsString(2000).NotNullable()
                .WithColumn("Quantity").AsFloat().Nullable()
                .WithColumn("PerItemValue").AsDecimal(4, 10).Nullable()
                .WithColumn("Notes").AsString(int.MaxValue).Nullable()
                .WithColumn("CatalogNumber").AsString(20).Nullable()
                .WithColumn("AuctionNumber").AsString(20).Nullable()
                .WithColumn("ItemNumber").AsString(20).Nullable()
                .WithColumn("EstimatedValue").AsDecimal(4, 10).Nullable()
                .WithColumn("SoldFor").AsDecimal(4, 10).Nullable()
                .WithColumn("Cateory_ID").AsInt16().Nullable()
                .WithColumn("Origin_ID").AsInt16().Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("ModifiedOn").AsDateTime().Nullable()
                .WithColumn("Donation").AsString(2000).Nullable()
                .WithColumn("ThankYouLetterSent").AsBoolean().Nullable()
                .WithColumn("Limitations").AsString(500).Nullable()
                .WithColumn("Certificate").AsString(10).Nullable()
                .WithColumn("ProcurementType_ID").AsInt16().Nullable()
                .WithColumn("Title").AsString(2000).Nullable();
        }

        void CreateProcurementDonorTable()
        {
            Create.Table("ProcurementDonor")
                .WithColumn("ProcurementDonor_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("Procurement_ID").AsInt16().NotNullable()
                .WithColumn("Donor_ID").AsInt16().NotNullable();
        }

        void CreateProcurementTypeTable()
        {
            Create.Table("ProcurementType")
                .WithColumn("ProcurementType_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("ProcurementTypeDesc").AsString(50).NotNullable()
                .WithColumn("DonorType_ID").AsInt16().Nullable();
        }

        void CreateProcurerTable()
        {
            Create.Table("Procurer")
                .WithColumn("Procurer_ID").AsInt16().PrimaryKey().Identity().NotNullable()
                .WithColumn("FirstName").AsString(255).Nullable()
                .WithColumn("LastName").AsString(255).Nullable()
                .WithColumn("Phone").AsString(20).Nullable()
                .WithColumn("Email").AsString(255).Nullable()
                .WithColumn("Description").AsString(255).Nullable();
        }
    }
}
