using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CheckIn.Common.Models;

namespace CheckIn.Common.Migrations
{
    [DbContext(typeof(CheckInContext))]
    [Migration("20170402174240_v9")]
    partial class v9
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

                    b.Property<bool>("HasCheckOut");

                    b.Property<DateTime>("OriCheckInTime");

                    b.Property<DateTime>("OriCheckOutTime");

                    b.Property<int>("UserID");

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

                    b.Property<string>("Email")
                        .HasMaxLength(30);

                    b.Property<string>("EmployeeID")
                        .HasMaxLength(11);

                    b.Property<byte[]>("HeadImage");

                    b.Property<string>("MobilephoneNumber")
                        .HasMaxLength(11);

                    b.Property<string>("Name")
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .HasMaxLength(32);

                    b.Property<int>("Right");

                    b.Property<DateTime>("SendSMSDate");

                    b.Property<int>("SendSMSTimes");

                    b.Property<string>("Token")
                        .HasMaxLength(100);

                    b.HasKey("UserID");

                    b.ToTable("UserInfo");
                });
        }
    }
}
