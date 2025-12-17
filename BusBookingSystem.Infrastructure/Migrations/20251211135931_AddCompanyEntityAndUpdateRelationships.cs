using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BusBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyEntityAndUpdateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Önce Companies tablosunu oluştur
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            // 2. Varsayılan bir Company oluştur (mevcut veriler için)
            migrationBuilder.Sql(@"
                INSERT INTO ""Companies"" (""Name"", ""CreatedDate"")
                VALUES ('Default Company', NOW())
                ON CONFLICT DO NOTHING;
            ");

            // 3. CompanyId kolonlarını nullable olarak ekle
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Trips",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Buses",
                type: "integer",
                nullable: true);

            // 4. Mevcut kayıtları varsayılan Company'ye ata
            migrationBuilder.Sql(@"
                UPDATE ""Buses"" SET ""CompanyId"" = (SELECT ""Id"" FROM ""Companies"" LIMIT 1) WHERE ""CompanyId"" IS NULL;
                UPDATE ""Trips"" SET ""CompanyId"" = (SELECT ""Id"" FROM ""Companies"" LIMIT 1) WHERE ""CompanyId"" IS NULL;
            ");

            // 5. CompanyId'yi NOT NULL yap
            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Trips",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Buses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CompanyId",
                table: "Trips",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_CompanyId",
                table: "Buses",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Companies_CompanyId",
                table: "Buses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Companies_CompanyId",
                table: "Trips",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Companies_CompanyId",
                table: "Buses");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Companies_CompanyId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CompanyId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Buses_CompanyId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Buses");
        }
    }
}
