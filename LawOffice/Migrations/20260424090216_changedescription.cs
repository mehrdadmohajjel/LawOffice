using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class changedescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RegisteredBy",
                table: "Financials",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Financials",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2416));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2417));

            migrationBuilder.UpdateData(
                table: "CaseStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2300));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2301));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2302));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2303));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2321));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2322));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(2324));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 24, 9, 2, 14, 43, DateTimeKind.Utc).AddTicks(7937), "$2a$11$31rb5KzwMUplJDObSUlCme/kfX7tT2RHhQW5xXK0s1SkP8T/qbqGC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 24, 9, 2, 14, 160, DateTimeKind.Utc).AddTicks(4256), "$2a$11$UQKwhVXngvBaLFXuluuJvuRmHlF8PAA7PNMX8ZPuC0x.Y.nXCbb/q" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 24, 9, 2, 14, 281, DateTimeKind.Utc).AddTicks(1719), "$2a$11$6pIyAA0ZQvQP.XlRIu16x.YIFq4QdoItVFP9ZCLlmnpQm8q6.d0zK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RegisteredBy",
                table: "Financials",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Financials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
