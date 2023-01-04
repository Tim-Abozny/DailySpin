﻿// <auto-generated />
using System;
using DailySpin.DataProvider.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DailySpin.Website.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DailySpin.DataProvider.Data.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DailySpin.Website.Models.Bet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BetsGlassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("UserBet")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("UserImage")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BetsGlassId");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("DailySpin.Website.Models.BetsGlass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BetMultiply")
                        .HasColumnType("int");

                    b.Property<long>("BetsCount")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("GlassImage")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<decimal>("TotalBetSum")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.ToTable("BetsGlasses");
                });

            modelBuilder.Entity("DailySpin.Website.Models.Chip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ColorType")
                        .HasColumnType("int");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("WinChip")
                        .HasColumnType("bit");

                    b.Property<int?>("WinChipHistoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WinChipHistoryId");

                    b.ToTable("Chips");
                });

            modelBuilder.Entity("DailySpin.Website.Models.WinChipHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("WinHistory");
                });

            modelBuilder.Entity("DailySpin.Website.Models.Bet", b =>
                {
                    b.HasOne("DailySpin.Website.Models.BetsGlass", null)
                        .WithMany("Bets")
                        .HasForeignKey("BetsGlassId");
                });

            modelBuilder.Entity("DailySpin.Website.Models.Chip", b =>
                {
                    b.HasOne("DailySpin.Website.Models.WinChipHistory", null)
                        .WithMany("WinChips")
                        .HasForeignKey("WinChipHistoryId");
                });

            modelBuilder.Entity("DailySpin.Website.Models.BetsGlass", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("DailySpin.Website.Models.WinChipHistory", b =>
                {
                    b.Navigation("WinChips");
                });
#pragma warning restore 612, 618
        }
    }
}
