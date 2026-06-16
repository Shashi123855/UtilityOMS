using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UtilityOMS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LeadTechnician = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Residents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    SmsOptIn = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmailOptIn = table.Column<bool>(type: "INTEGER", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Cause = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ResolvedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    AffectedArea = table.Column<string>(type: "TEXT", nullable: false),
                    AffectedCustomers = table.Column<int>(type: "INTEGER", nullable: false),
                    Reportedby = table.Column<string>(type: "TEXT", nullable: false),
                    AssignedCrewId = table.Column<int>(type: "INTEGER", nullable: true),
                    AssigendCrewId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outages_Crews_AssigendCrewId",
                        column: x => x.AssigendCrewId,
                        principalTable: "Crews",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Crews",
                columns: new[] { "Id", "LeadTechnician", "Name", "PhoneNumber", "Status" },
                values: new object[,]
                {
                    { 1, "John Smith", "Alpha Team", "555-1001", 0 },
                    { 2, "Jane Doe", "Beta Team", "555-1002", 0 },
                    { 3, "Carlos Rivera", "Delta Team", "555-1003", 3 }
                });

            migrationBuilder.InsertData(
                table: "Residents",
                columns: new[] { "Id", "Address", "Email", "EmailOptIn", "Latitude", "Longitude", "Name", "PhoneNumber", "SmsOptIn" },
                values: new object[] { 1, "123 Main St", "alice@example.com", true, 40.712800000000001, -74.006, "Alice Johnson", "555-2001", true });

            migrationBuilder.CreateIndex(
                name: "IX_Outages_AssigendCrewId",
                table: "Outages",
                column: "AssigendCrewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outages");

            migrationBuilder.DropTable(
                name: "Residents");

            migrationBuilder.DropTable(
                name: "Crews");
        }
    }
}
