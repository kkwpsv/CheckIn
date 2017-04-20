using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckIn.Common.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadImage",
                table: "UserInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "HeadImage",
                table: "UserInfo",
                maxLength: 8000,
                nullable: true);
        }
    }
}
