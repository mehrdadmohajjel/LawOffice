using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class Detail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CaseNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9954));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9960));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9858));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9860));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9862));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9864));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 11, 32, 18, 823, DateTimeKind.Utc).AddTicks(7156), "$2a$11$/VA6MdRjJMNUIHSzk7sQxe.WTzZJkKgVFpwksz9J8x67wnhayLwS6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 11, 32, 18, 915, DateTimeKind.Utc).AddTicks(7584), "$2a$11$p/R/tPsncgEIR3iqQj/qyeFNIbibEm1cLe2HbukzMrDjPX0AN0YbC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 11, 32, 19, 12, DateTimeKind.Utc).AddTicks(9532), "$2a$11$ua8Dfd6a.PAobaE9wsY1qORHA2rloLb9/yoXLLSzJbuQsjJt9.T/2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "CaseNotes");

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2420));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 9, 3, 29, 114, DateTimeKind.Utc).AddTicks(2422));

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
        }
    }
}
