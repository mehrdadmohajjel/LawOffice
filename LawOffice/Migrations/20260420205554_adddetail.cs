using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class adddetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReminderJobId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4665));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4666));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4667));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4551));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4553));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4555));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4557));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4566));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4567));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 20, 20, 55, 50, 297, DateTimeKind.Utc).AddTicks(9640), "$2a$11$wZ8iiBbXFjGXwDYfxDxIte5cwa44o.0OlpfqrYGmbasHX0xiD9EBa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 20, 20, 55, 50, 432, DateTimeKind.Utc).AddTicks(762), "$2a$11$F3uliGFDJEMibRFf/66oZOvQlSzv6/ClvKA83u.Oj3V0xx9S8jyw6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 20, 20, 55, 50, 551, DateTimeKind.Utc).AddTicks(4084), "$2a$11$9lApBSr3tKV//sq/rSkAxOaGVhy.03IK6S4CkGBM84cMoE4QqbjEG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderJobId",
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
    }
}
