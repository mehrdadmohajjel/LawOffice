using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawOffice.Migrations
{
    /// <inheritdoc />
    public partial class ConvertCaseTypeToTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CaseTypes_CaseTypeId1",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_CaseTypeId1",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CaseTypeId1",
                table: "Cases");

            migrationBuilder.AlterColumn<long>(
                name: "CaseTypeId",
                table: "Cases",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CaseTypeId",
                table: "Cases",
                column: "CaseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CaseTypes_CaseTypeId",
                table: "Cases",
                column: "CaseTypeId",
                principalTable: "CaseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CaseTypes_CaseTypeId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_CaseTypeId",
                table: "Cases");

            migrationBuilder.AlterColumn<int>(
                name: "CaseTypeId",
                table: "Cases",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CaseTypeId1",
                table: "Cases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3141));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3142));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3143));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3143));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3144));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3145));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 5, 40, 57, 822, DateTimeKind.Utc).AddTicks(3146));

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
    }
}
