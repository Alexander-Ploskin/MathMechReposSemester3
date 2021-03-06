﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyNUnitWeb.Models;

namespace MyNUnitWeb.Migrations
{
    [DbContext(typeof(TestArchive))]
    partial class TestArchiveModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("MyNUnitWeb.Models.AssemblyReportModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Failed")
                        .HasColumnType("int");

                    b.Property<int>("Ignored")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Passed")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TestRunModelDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TestRunModelDateTime");

                    b.ToTable("AssemblyReportModels");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.TestReportModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssemblyReportModelId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Passed")
                        .HasColumnType("bit");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<bool>("Valid")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AssemblyReportModelId");

                    b.ToTable("TestReportModels");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.TestRunModel", b =>
                {
                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("DateTime");

                    b.ToTable("RunModels");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.AssemblyReportModel", b =>
                {
                    b.HasOne("MyNUnitWeb.Models.TestRunModel", null)
                        .WithMany("AssemblyReports")
                        .HasForeignKey("TestRunModelDateTime");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.TestReportModel", b =>
                {
                    b.HasOne("MyNUnitWeb.Models.AssemblyReportModel", null)
                        .WithMany("TestReports")
                        .HasForeignKey("AssemblyReportModelId");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.AssemblyReportModel", b =>
                {
                    b.Navigation("TestReports");
                });

            modelBuilder.Entity("MyNUnitWeb.Models.TestRunModel", b =>
                {
                    b.Navigation("AssemblyReports");
                });
#pragma warning restore 612, 618
        }
    }
}
