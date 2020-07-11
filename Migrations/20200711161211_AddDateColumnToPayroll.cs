using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayrollApp.Migrations
{
    public partial class AddDateColumnToPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payrolls",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayrollDate",
                table: "Payrolls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "PayrollDate",
                table: "Payrolls");
        }
    }
}
