using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class ConvertCaseTypeToTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cases");

            migrationBuilder.AddColumn<long>(
                name: "CaseStatusId",
                table: "Cases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CaseStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersianTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CaseStatuses",
                columns: new[] { "Id", "CreatedAt", "DisplayOrder", "IsDeleted", "Name", "PersianTitle", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2418), 1, false, "Open", "جاری", null },
                    { 2L, new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2420), 2, false, "Closed", "مختومه", null },
                    { 3L, new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2422), 3, false, "Pending", "معلق", null }
                });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2353));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2354));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2355));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2356));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2356));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2357));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 9, 3, 28, 932, DateTimeKind.Utc).AddTicks(3457), "$2a$11$M.FKrhlK09XTJAFThEh1zerhlG0M7YtaP83osWd4to3.puV7zOcAe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 9, 3, 29, 23, DateTimeKind.Utc).AddTicks(4210), "$2a$11$cGI9BtmvWyXiZvT3jTBJHu7JDMV.j0jIqqJ0XiRSqZMklayfY6FkW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2149), "$2a$11$5KprkIXu0OHIE54yhrU8pO/cqsDhLblI4bAvB4CEPdKBpSM2LG.6i" });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CaseStatusId",
                table: "Cases",
                column: "CaseStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CaseStatuses_CaseStatusId",
                table: "Cases",
                column: "CaseStatusId",
                principalTable: "CaseStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CaseStatuses_CaseStatusId",
                table: "Cases");

            migrationBuilder.DropTable(
                name: "CaseStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Cases_CaseStatusId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CaseStatusId",
                table: "Cases");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(212));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(215));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(217));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 7, 4, 55, 105, DateTimeKind.Utc).AddTicks(727), "$2a$11$kxqfxgHMjGhxjO.R1IPFyuA2eaqXSmVDKHakMLg7TLWeHWm6W7e1O" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 7, 4, 55, 201, DateTimeKind.Utc).AddTicks(938), "$2a$11$oNtSf9ecgRuPcemqxnfSqu3UDggXm2kkvQZMR4QHV569TFH96BkyW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 7, 4, 55, 292, DateTimeKind.Utc).AddTicks(36), "$2a$11$XKBX4oIz6/Nl/AOUGiZYveyCO9O875SRHYra5LaCLSm71XFt7RYpe" });
        }
    }
}
