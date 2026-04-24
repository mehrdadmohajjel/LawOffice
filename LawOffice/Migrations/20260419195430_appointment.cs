using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class appointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(253));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(257));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(143));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(146));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(147));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(148));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(170));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(172));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(173));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 19, 54, 27, 396, DateTimeKind.Utc).AddTicks(174));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 19, 54, 26, 979, DateTimeKind.Utc).AddTicks(2621), "$2a$11$t9nCvE.0PEYbA.iFjMyjue0YeaOvXXJrELbfUSMPKskDnrO8YibM." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 19, 54, 27, 190, DateTimeKind.Utc).AddTicks(7030), "$2a$11$6LkjyDZr3ZHXojDxl3g.zOaG3BxDvfz6gT7XM/VfYJ1p/E8Gkxn/W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 19, 19, 54, 27, 395, DateTimeKind.Utc).AddTicks(9428), "$2a$11$rmpjLiErwDnpfjjWgb5UyOXYnx7hWqA8BAwxrnQI9MLA.wpC52t7e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Appointments");

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
    }
}
