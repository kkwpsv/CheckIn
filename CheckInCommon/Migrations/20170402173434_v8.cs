using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckIn.Common.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "UserCheckInInfo");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "UserCheckInInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "UserCheckInInfo");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeID",
                table: "UserCheckInInfo",
                maxLength: 11,
                nullable: true);
        }
    }
}
