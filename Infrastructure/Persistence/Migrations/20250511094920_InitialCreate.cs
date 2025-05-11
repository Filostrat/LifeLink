using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BloodCompatibilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromBloodTypeId = table.Column<int>(type: "int", nullable: false),
                    ToBloodTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodCompatibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodCompatibilities_BloodTypes_FromBloodTypeId",
                        column: x => x.FromBloodTypeId,
                        principalTable: "BloodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BloodCompatibilities_BloodTypes_ToBloodTypeId",
                        column: x => x.ToBloodTypeId,
                        principalTable: "BloodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DonationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodTypeId = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    RadiusInMeters = table.Column<double>(type: "float", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationRequests_BloodTypes_BloodTypeId",
                        column: x => x.BloodTypeId,
                        principalTable: "BloodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodTypeId = table.Column<int>(type: "int", nullable: false),
                    LastDonation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donors_BloodTypes_BloodTypeId",
                        column: x => x.BloodTypeId,
                        principalTable: "BloodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonationRequestNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationRequestId = table.Column<int>(type: "int", nullable: false),
                    DonorId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequestNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationRequestNotification_DonationRequests_DonationRequestId",
                        column: x => x.DonationRequestId,
                        principalTable: "DonationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationPreferences",
                columns: table => new
                {
                    DonorId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPreferences", x => x.DonorId);
                    table.ForeignKey(
                        name: "FK_NotificationPreferences_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationPreferenceId = table.Column<int>(type: "int", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationChannels_NotificationPreferences_NotificationPreferenceId",
                        column: x => x.NotificationPreferenceId,
                        principalTable: "NotificationPreferences",
                        principalColumn: "DonorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BloodTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "I (0) Rh−" },
                    { 2, "I (0) Rh+" },
                    { 3, "II (A) Rh−" },
                    { 4, "II (A) Rh+" },
                    { 5, "III (B) Rh−" },
                    { 6, "III (B) Rh+" },
                    { 7, "IV (AB) Rh−" },
                    { 8, "IV (AB) Rh+" }
                });

            migrationBuilder.InsertData(
                table: "BloodCompatibilities",
                columns: new[] { "Id", "FromBloodTypeId", "ToBloodTypeId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 3 },
                    { 4, 1, 4 },
                    { 5, 1, 5 },
                    { 6, 1, 6 },
                    { 7, 1, 7 },
                    { 8, 1, 8 },
                    { 9, 2, 2 },
                    { 10, 2, 4 },
                    { 11, 2, 6 },
                    { 12, 2, 8 },
                    { 13, 3, 3 },
                    { 14, 3, 4 },
                    { 15, 3, 7 },
                    { 16, 3, 8 },
                    { 17, 4, 4 },
                    { 18, 4, 8 },
                    { 19, 5, 5 },
                    { 20, 5, 6 },
                    { 21, 5, 7 },
                    { 22, 5, 8 },
                    { 23, 6, 6 },
                    { 24, 6, 8 },
                    { 25, 7, 7 },
                    { 26, 7, 8 },
                    { 27, 8, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodCompatibilities_FromBloodTypeId",
                table: "BloodCompatibilities",
                column: "FromBloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodCompatibilities_ToBloodTypeId",
                table: "BloodCompatibilities",
                column: "ToBloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequestNotification_DonationRequestId",
                table: "DonationRequestNotification",
                column: "DonationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_BloodTypeId",
                table: "DonationRequests",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Donors_BloodTypeId",
                table: "Donors",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Donors_Email",
                table: "Donors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationChannels_NotificationPreferenceId",
                table: "NotificationChannels",
                column: "NotificationPreferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodCompatibilities");

            migrationBuilder.DropTable(
                name: "DonationRequestNotification");

            migrationBuilder.DropTable(
                name: "NotificationChannels");

            migrationBuilder.DropTable(
                name: "DonationRequests");

            migrationBuilder.DropTable(
                name: "NotificationPreferences");

            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropTable(
                name: "BloodTypes");
        }
    }
}
