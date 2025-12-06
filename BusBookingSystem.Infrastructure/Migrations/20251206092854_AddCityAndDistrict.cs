using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BusBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityAndDistrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Trips");

            migrationBuilder.AddColumn<int>(
                name: "DestinationCityId",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DestinationDistrictId",
                table: "Trips",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginCityId",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginDistrictId",
                table: "Trips",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DestinationCityId",
                table: "Trips",
                column: "DestinationCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DestinationDistrictId",
                table: "Trips",
                column: "DestinationDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_OriginCityId",
                table: "Trips",
                column: "OriginCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_OriginDistrictId",
                table: "Trips",
                column: "OriginDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Cities_DestinationCityId",
                table: "Trips",
                column: "DestinationCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Cities_OriginCityId",
                table: "Trips",
                column: "OriginCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Districts_DestinationDistrictId",
                table: "Trips",
                column: "DestinationDistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Districts_OriginDistrictId",
                table: "Trips",
                column: "OriginDistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Cities_DestinationCityId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Cities_OriginCityId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Districts_DestinationDistrictId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Districts_OriginDistrictId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Trips_DestinationCityId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_DestinationDistrictId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_OriginCityId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_OriginDistrictId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DestinationCityId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DestinationDistrictId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "OriginCityId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "OriginDistrictId",
                table: "Trips");

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "Trips",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Trips",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
