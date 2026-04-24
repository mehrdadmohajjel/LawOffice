using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class removedescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(1006));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(1008));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(1012));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(938));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(940));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(941));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(941));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(942));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(943));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(949));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(949));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 20, 6, 41, 55, 295, DateTimeKind.Utc).AddTicks(1583), "$2a$11$acwqf0ehopDSrsyWg6yBf.SyPR2kkWp5.caV0bwUH2K9FrIjEcGGK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 20, 6, 41, 55, 387, DateTimeKind.Utc).AddTicks(1276), "$2a$11$1z7oLceW0bZiTDnsE2NtjeGaxjCgXes2hNt6zq60MJnO4pRzfelxy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 20, 6, 41, 55, 478, DateTimeKind.Utc).AddTicks(814), "$2a$11$nvpCP9q5Q3UecZH0QRcl6ucUa7qSx/8sZK8VPYw0Thu6VVHsCSW4O" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
