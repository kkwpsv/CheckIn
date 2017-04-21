using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckIn.Common.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason1",
                table: "UserCheckInInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason2",
                table: "UserCheckInInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason1",
                table: "UserCheckInInfo");

            migrationBuilder.DropColumn(
                name: "Reason2",
                table: "UserCheckInInfo");
        }
    }
}
