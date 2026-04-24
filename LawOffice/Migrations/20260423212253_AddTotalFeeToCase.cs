using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalFeeToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Financials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChequeStatus",
                table: "Financials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Financials",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmsReminderSent",
                table: "Financials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFee",
                table: "Cases",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4456));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4457));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4458));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4329));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4331));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4332));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4333));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4341));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4342));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4343));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4344));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 23, 21, 22, 50, 979, DateTimeKind.Utc).AddTicks(899), "$2a$11$MmxYgLOoKI7U8ijFzB3kc.hEElc3JsU4E8fPbtvI4zIbDy9FkKnpi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 23, 21, 22, 51, 96, DateTimeKind.Utc).AddTicks(2641), "$2a$11$65hyci92kYF2pVSc7cHcTuEW3VZl7LH3z/M7v4dbARgOmMzDds4iS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 23, 21, 22, 51, 212, DateTimeKind.Utc).AddTicks(4173), "$2a$11$sk0JBa0i.jlurKAUPPGDN.2YJ9Az8W6ai9zO5MdmwYK.wM5XMSCnS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "ChequeStatus",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "SmsReminderSent",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "TotalFee",
                table: "Cases");

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
    }
}
