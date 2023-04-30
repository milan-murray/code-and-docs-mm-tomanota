﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using user_web_API.Data;

#nullable disable

namespace user_web_API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230426132556_update1")]
    partial class update1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("user_web_API.CompletedPremadeTitle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("titlePremade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CompletedPremadeTitles");
                });

            modelBuilder.Entity("user_web_API.Prompt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastProgressed")
                        .HasColumnType("datetime2");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Propmts");
                });

            modelBuilder.Entity("user_web_API.PromptBody", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Heading")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PromptId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PromptBodies");
                });

            modelBuilder.Entity("user_web_API.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompletedPremadeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Language")
                        .HasColumnType("int");

                    b.Property<DateTime>("MostRecentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("user_web_API.WeekProgress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Friday")
                        .HasColumnType("int");

                    b.Property<int>("Monday")
                        .HasColumnType("int");

                    b.Property<int>("Saturday")
                        .HasColumnType("int");

                    b.Property<int>("Sunday")
                        .HasColumnType("int");

                    b.Property<int>("Thursday")
                        .HasColumnType("int");

                    b.Property<int>("Tuesday")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Wednesday")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WeekProgresses");
                });

            modelBuilder.Entity("user_web_API.YearProgress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Apr")
                        .HasColumnType("int");

                    b.Property<int>("Aug")
                        .HasColumnType("int");

                    b.Property<int>("Dec")
                        .HasColumnType("int");

                    b.Property<int>("Feb")
                        .HasColumnType("int");

                    b.Property<int>("Jan")
                        .HasColumnType("int");

                    b.Property<int>("Jul")
                        .HasColumnType("int");

                    b.Property<int>("Jun")
                        .HasColumnType("int");

                    b.Property<int>("Mar")
                        .HasColumnType("int");

                    b.Property<int>("May")
                        .HasColumnType("int");

                    b.Property<int>("Nov")
                        .HasColumnType("int");

                    b.Property<int>("Oct")
                        .HasColumnType("int");

                    b.Property<int>("Sep")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("YearProgresses");
                });
#pragma warning restore 612, 618
        }
    }
}