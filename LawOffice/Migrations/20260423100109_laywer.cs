using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class laywer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7486));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7487));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7488));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7395));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7398));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7399));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7400));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7406));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7407));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7408));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7408));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 23, 10, 1, 9, 72, DateTimeKind.Utc).AddTicks(8267), "$2a$11$RxFXnNRQaKrQq72KDnWRae4DW358u46sH9lUkx.Mt2SlgvOctp3yS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 23, 10, 1, 9, 166, DateTimeKind.Utc).AddTicks(9057), "$2a$11$gMV0XSdaYsjwRHmIqDDXgOJzQks2BfCK9njSi21xPR07dIepdQHyi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 23, 10, 1, 9, 257, DateTimeKind.Utc).AddTicks(7152), "$2a$11$ZBJsC3VUo1AbbCRyV36LNeLismGGJMT2q2rAuQUGMmNLCaG6SZBsq" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "NationalCode",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "Clients");

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
    }
}
