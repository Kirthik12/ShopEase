using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopEase.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCmsBackupEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server can't ALTER a column's IDENTITY property in place — drop and recreate it.
            migrationBuilder.DropPrimaryKey(name: "PK_CmsConfigs", table: "CmsConfigs");
            migrationBuilder.DropColumn(name: "Id", table: "CmsConfigs");
            migrationBuilder.AddColumn<int>(name: "Id", table: "CmsConfigs", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddPrimaryKey(name: "PK_CmsConfigs", table: "CmsConfigs", column: "Id");

            migrationBuilder.CreateTable(
                name: "BackupJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Retention = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastRunAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackupSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsStaging = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupSnapshots", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackupJobs");

            migrationBuilder.DropTable(
                name: "BackupSnapshots");

            migrationBuilder.DropPrimaryKey(name: "PK_CmsConfigs", table: "CmsConfigs");
            migrationBuilder.DropColumn(name: "Id", table: "CmsConfigs");
            migrationBuilder.AddColumn<int>(name: "Id", table: "CmsConfigs", type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
            migrationBuilder.AddPrimaryKey(name: "PK_CmsConfigs", table: "CmsConfigs", column: "Id");
        }
    }
}
