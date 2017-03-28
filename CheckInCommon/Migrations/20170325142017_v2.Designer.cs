using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CheckIn.Common.Models;

namespace CheckIn.Common.Migrations
{
    [DbContext(typeof(CheckInContext))]
    [Migration("20170325142017_v2")]
    partial class v2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("CheckIn.Common.Models.DepartmentInfo", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(11);

                    b.Property<string>("DepartmentName")
                        .HasMaxLength(40);

                    b.HasKey("DepartmentID");

                    b.ToTable("DepartmentInfo");
                });

            modelBuilder.Entity("CheckIn.Common.Models.TelephoneInfo", b =>
                {
                    b.Property<int>("TelephoneID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(11);

                    b.Property<int>("DepartmentID")
                        .HasMaxLength(11);

                    b.Property<string>("TelephoneNumber")
                        .HasMaxLength(20);

                    b.Property<string>("TelephoneSubordination")
                        .HasMaxLength(50);

                    b.HasKey("TelephoneID");

                    b.ToTable("TelephoneInfo");
                });

            modelBuilder.Entity("CheckIn.Common.Models.UserCheckInInfo", b =>
                {
                    b.Property<int>("CheckInID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CheckInTime");

                    b.Property<DateTime>("CheckOutTime");

                    b.Property<string>("EmployeeID")
                        .HasMaxLength(11);

                    b.Property<DateTime>("OriCheckInTime");

                    b.Property<DateTime>("OriCheckOutTime");

                    b.HasKey("CheckInID");

                    b.ToTable("UserCheckInInfo");
                });

            modelBuilder.Entity("CheckIn.Common.Models.UserInfo", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(11);

                    b.Property<int>("DepartmentID")
                        .HasMaxLength(11);

                    b.Property<int>("Email")
                        .HasMaxLength(30);

                    b.Property<string>("EmployeeID")
                        .HasMaxLength(11);

                    b.Property<int>("HeadImage")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasMaxLength(20);

                    b.Property<int>("Password")
                        .HasMaxLength(32);

                    b.Property<int>("Token")
                        .HasMaxLength(100);

                    b.HasKey("UserID");

                    b.ToTable("UserInfo");
                });
        }
    }
}
