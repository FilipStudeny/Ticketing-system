using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Migrations
{
    /// <inheritdoc />
    public partial class IDretype2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: "0000000FFFFF",
                column: "RegisterDate",
                value: new DateTime(2023, 10, 3, 15, 16, 53, 926, DateTimeKind.Local).AddTicks(9579));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: "0000000FFFFF",
                column: "RegisterDate",
                value: new DateTime(2023, 10, 3, 15, 1, 51, 697, DateTimeKind.Local).AddTicks(2298));
        }
    }
}
