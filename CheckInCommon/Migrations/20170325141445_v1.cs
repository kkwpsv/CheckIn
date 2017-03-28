using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckIn.Common.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentInfo",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(maxLength: 11, nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DepartmentName = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentInfo", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "TelephoneInfo",
                columns: table => new
                {
                    TelephoneID = table.Column<int>(maxLength: 11, nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DepartmentID = table.Column<int>(maxLength: 11, nullable: false),
                    TelephoneNumber = table.Column<int>(maxLength: 20, nullable: false),
                    TelephoneSubordination = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelephoneInfo", x => x.TelephoneID);
                });

            migrationBuilder.CreateTable(
                name: "UserCheckInInfo",
                columns: table => new
                {
                    CheckInID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CheckInTime = table.Column<DateTime>(nullable: false),
                    CheckOutTime = table.Column<DateTime>(nullable: false),
                    EmployeeID = table.Column<string>(maxLength: 11, nullable: true),
                    OriCheckInTime = table.Column<DateTime>(nullable: false),
                    OriCheckOutTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCheckInInfo", x => x.CheckInID);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    UserID = table.Column<int>(maxLength: 11, nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DepartmentID = table.Column<int>(maxLength: 11, nullable: false),
                    Email = table.Column<int>(maxLength: 30, nullable: false),
                    EmployeeID = table.Column<string>(maxLength: 11, nullable: true),
                    HeadImage = table.Column<int>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    Password = table.Column<int>(maxLength: 32, nullable: false),
                    Token = table.Column<int>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentInfo");

            migrationBuilder.DropTable(
                name: "TelephoneInfo");

            migrationBuilder.DropTable(
                name: "UserCheckInInfo");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
