using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopEase.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAppliedCouponIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server can't ALTER a column's IDENTITY property in place — drop and recreate it.
            migrationBuilder.DropPrimaryKey(name: "PK_AppliedCoupons", table: "AppliedCoupons");
            migrationBuilder.DropColumn(name: "UserId", table: "AppliedCoupons");
            migrationBuilder.AddColumn<int>(name: "UserId", table: "AppliedCoupons", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddPrimaryKey(name: "PK_AppliedCoupons", table: "AppliedCoupons", column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "PK_AppliedCoupons", table: "AppliedCoupons");
            migrationBuilder.DropColumn(name: "UserId", table: "AppliedCoupons");
            migrationBuilder.AddColumn<int>(name: "UserId", table: "AppliedCoupons", type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
            migrationBuilder.AddPrimaryKey(name: "PK_AppliedCoupons", table: "AppliedCoupons", column: "UserId");
        }
    }
}
