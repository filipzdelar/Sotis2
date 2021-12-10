﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sotis2.Data;

namespace Sotis2.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20211208085437_Init5")]
    partial class Init5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sotis2.Models.Answare", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnswareText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsItTrue")
                        .HasColumnType("bit");

                    b.Property<long>("QuestionID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Answare");
                });

            modelBuilder.Entity("Sotis2.Models.Attempt", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Accuracy")
                        .HasColumnType("real");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("TakenTime")
                        .HasColumnType("time");

                    b.HasKey("ID");

                    b.ToTable("Attempt");
                });

            modelBuilder.Entity("Sotis2.Models.Domain", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Domain");
                });

            modelBuilder.Entity("Sotis2.Models.Question", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DomainID")
                        .HasColumnType("bigint");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SubjectID")
                        .HasColumnType("bigint");

                    b.Property<long?>("TestID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("TestID");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("Sotis2.Models.Subject", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NameOfSubject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Sotis2.Models.Test", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("SubjectID")
                        .HasColumnType("bigint");

                    b.Property<TimeSpan>("TestDuration")
                        .HasColumnType("time");

                    b.HasKey("ID");

                    b.HasIndex("SubjectID");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("Sotis2.Models.Question", b =>
                {
                    b.HasOne("Sotis2.Models.Test", "Test")
                        .WithMany()
                        .HasForeignKey("TestID");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Sotis2.Models.Test", b =>
                {
                    b.HasOne("Sotis2.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectID");

                    b.Navigation("Subject");
                });
#pragma warning restore 612, 618
        }
    }
}
