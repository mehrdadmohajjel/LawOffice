using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class ConvertCaseTypeToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaseType",
                table: "Cases",
                newName: "CaseTypeId");

            migrationBuilder.AddColumn<long>(
                name: "CaseTypeId1",
                table: "Cases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CaseTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersianTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perfix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CaseTypes",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "Perfix", "PersianTitle", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3137), false, "Civil", "CVL", "حقوقی", null },
                    { 2L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3141), false, "Criminal", "CRM", "کیفری", null },
                    { 3L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3142), false, "Family", "FAM", "خانواده", null },
                    { 4L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3143), false, "Labor", "LBR", "کار", null },
                    { 5L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3143), false, "Commercial", "COM", "تجاری", null },
                    { 6L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3144), false, "Administrative", "ADM", "اداری", null },
                    { 7L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3145), false, "Real_Estate", "RES", "ملکی", null },
                    { 8L, new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3146), false, "Other", "OTH", "سایر", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 5, 40, 57, 639, DateTimeKind.Utc).AddTicks(9217), "$2a$11$x6sAY/YGQc9i2GXbvN4.DueiyrNvQ7dSZ3qg2idEvToPAiLhwJocm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 5, 40, 57, 731, DateTimeKind.Utc).AddTicks(5350), "$2a$11$GMjcKBGBcP8IhUGse2RN6.w7bY7uVMJVdObX2HfIlRKLTsHoD0Bzq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(2985), "$2a$11$33jPWKsZ0YPzqZU7lO9F5.CYHuWoNw0wqXBp.gALW016T6wGOV/32" });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CaseTypeId1",
                table: "Cases",
                column: "CaseTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CaseTypes_CaseTypeId1",
                table: "Cases",
                column: "CaseTypeId1",
                principalTable: "CaseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CaseTypes_CaseTypeId1",
                table: "Cases");

            migrationBuilder.DropTable(
                name: "CaseTypes");

            migrationBuilder.DropIndex(
                name: "IX_Cases_CaseTypeId1",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CaseTypeId1",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "CaseTypeId",
                table: "Cases",
                newName: "CaseType");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 18, 12, 14, 51, 247, DateTimeKind.Utc).AddTicks(8567), "$2a$11$RuE9E6S9F6TMdxp/BPRnyO5Xxq2cJH4BoqLZRKx7eHr6gDCOC/RlC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 18, 12, 14, 51, 338, DateTimeKind.Utc).AddTicks(8177), "$2a$11$6w/8Xncbm8PFHCqdyRGRbOR9YP1L/NJ4jUSzvILFGXmjiicJnH.xe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 18, 12, 14, 51, 430, DateTimeKind.Utc).AddTicks(6074), "$2a$11$4.Gsfb3JaaIBKSsd39ovreftQFIntu.wCz5QqI9KwJ0wT4mqphSyy" });
        }
    }
}
