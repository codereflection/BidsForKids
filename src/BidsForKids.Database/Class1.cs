using System;
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
        }

        public override void Down()
        {
            throw new NotImplementedException();
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
    }
}
