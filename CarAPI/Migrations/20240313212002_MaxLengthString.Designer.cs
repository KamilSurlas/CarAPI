﻿// <auto-generated />
using System;
using CarAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarAPI.Migrations
{
    [DbContext(typeof(CarDbContext))]
    [Migration("20240313212002_MaxLengthString")]
    partial class MaxLengthString
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CarAPI.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BodyType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("Drivetrain")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Mileage")
                        .HasColumnType("float");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("ProductionYear")
                        .HasColumnType("int");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("RegistrationNumber")
                        .IsUnique();

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarAPI.Entities.Engine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Displacement")
                        .IsRequired()
                        .HasColumnType("decimal(3,1)");

                    b.Property<string>("FuelType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Horsepower")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId")
                        .IsUnique();

                    b.ToTable("Engines");
                });

            modelBuilder.Entity("CarAPI.Entities.Insurance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("PolicyNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("StartDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AddedByUserId");

                    b.HasIndex("CarId")
                        .IsUnique();

                    b.HasIndex("PolicyNumber")
                        .IsUnique();

                    b.ToTable("Insurances");
                });

            modelBuilder.Entity("CarAPI.Entities.Repair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<decimal>("RepairCost")
                        .HasColumnType("decimal(7,3)");

                    b.Property<DateTime>("RepairDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AddedByUserId");

                    b.HasIndex("CarId");

                    b.ToTable("Repairs");
                });

            modelBuilder.Entity("CarAPI.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CarAPI.Entities.TechnicalReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("TechnicalReviewDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("TechnicalReviewResult")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddedByUserId");

                    b.HasIndex("CarId");

                    b.ToTable("TechnicalReviews");
                });

            modelBuilder.Entity("CarAPI.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CarAPI.Entities.Car", b =>
                {
                    b.HasOne("CarAPI.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("CarAPI.Entities.Engine", b =>
                {
                    b.HasOne("CarAPI.Entities.Car", "Car")
                        .WithOne("Engine")
                        .HasForeignKey("CarAPI.Entities.Engine", "CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarAPI.Entities.Insurance", b =>
                {
                    b.HasOne("CarAPI.Entities.User", "AddedByUser")
                        .WithMany()
                        .HasForeignKey("AddedByUserId");

                    b.HasOne("CarAPI.Entities.Car", "Car")
                        .WithOne("OcInsurance")
                        .HasForeignKey("CarAPI.Entities.Insurance", "CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddedByUser");

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarAPI.Entities.Repair", b =>
                {
                    b.HasOne("CarAPI.Entities.User", "AddedByUser")
                        .WithMany()
                        .HasForeignKey("AddedByUserId");

                    b.HasOne("CarAPI.Entities.Car", "Car")
                        .WithMany("CarRepairs")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddedByUser");

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarAPI.Entities.TechnicalReview", b =>
                {
                    b.HasOne("CarAPI.Entities.User", "AddedByUser")
                        .WithMany()
                        .HasForeignKey("AddedByUserId");

                    b.HasOne("CarAPI.Entities.Car", "Car")
                        .WithMany("TechnicalReviews")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddedByUser");

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarAPI.Entities.User", b =>
                {
                    b.HasOne("CarAPI.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CarAPI.Entities.Car", b =>
                {
                    b.Navigation("CarRepairs");

                    b.Navigation("Engine")
                        .IsRequired();

                    b.Navigation("OcInsurance")
                        .IsRequired();

                    b.Navigation("TechnicalReviews");
                });
#pragma warning restore 612, 618
        }
    }
}
