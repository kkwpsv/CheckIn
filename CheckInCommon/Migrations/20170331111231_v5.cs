using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckIn.Common.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Right",
                table: "UserInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendSMSDate",
                table: "UserInfo",
                nullable: false,
                defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SendSMSTimes",
                table: "UserInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Right",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "SendSMSDate",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "SendSMSTimes",
                table: "UserInfo");
        }
    }
}
