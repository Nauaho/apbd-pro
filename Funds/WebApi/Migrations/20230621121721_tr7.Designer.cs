﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Data;

#nullable disable

namespace WebApi.Migrations
{
    [DbContext(typeof(ProContext))]
    [Migration("20230621121721_tr7")]
    partial class tr7
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApi.Models.TickerDetails", b =>
                {
                    b.Property<string>("Ticker")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Cik")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("City")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CompositeFigi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CurrencyName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomepageUrl")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ListDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Locale")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Market")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PrimaryExchange")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("RoundLot")
                        .HasColumnType("int");

                    b.Property<string>("ShareClassFigi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("ShareClassSharesOutstanding")
                        .HasColumnType("bigint");

                    b.Property<string>("SicCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SicDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("TickerRoot")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("TotalEmployees")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("WeightedSharesOutstanding")
                        .HasColumnType("bigint");

                    b.HasKey("Ticker")
                        .HasName("Ticker_pk");

                    b.ToTable("TickerDetails");
                });

            modelBuilder.Entity("WebApi.Models.TickerOHLC", b =>
                {
                    b.Property<string>("Symbol")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Timespan")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Multuplier")
                        .HasColumnType("bigint");

                    b.Property<long>("T")
                        .HasColumnType("bigint");

                    b.Property<float>("C")
                        .HasColumnType("real");

                    b.Property<float>("H")
                        .HasColumnType("real");

                    b.Property<float>("L")
                        .HasColumnType("real");

                    b.Property<long>("N")
                        .HasColumnType("bigint");

                    b.Property<float>("O")
                        .HasColumnType("real");

                    b.Property<long>("V")
                        .HasColumnType("bigint");

                    b.Property<float>("Vw")
                        .HasColumnType("real");

                    b.HasKey("Symbol", "Timespan", "Multuplier", "T")
                        .HasName("Ohlc_pk");

                    b.ToTable("TickerOHLC");
                });

            modelBuilder.Entity("WebApi.Models.TickerOpenClose", b =>
                {
                    b.Property<string>("Symbol")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<float>("AfterHours")
                        .HasColumnType("real");

                    b.Property<float>("Close")
                        .HasColumnType("real");

                    b.Property<float>("High")
                        .HasColumnType("real");

                    b.Property<float>("Low")
                        .HasColumnType("real");

                    b.Property<float>("Open")
                        .HasColumnType("real");

                    b.Property<float>("PreMarket")
                        .HasColumnType("real");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint");

                    b.HasKey("Symbol", "From")
                        .HasName("TockerOpenClose_pk");

                    b.ToTable("TickerOpenClose");
                });

            modelBuilder.Entity("WebApi.Models.TickerSimilar", b =>
                {
                    b.Property<string>("TickerOneId")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TickerTwoId")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TickerOneId", "TickerTwoId")
                        .HasName("Ticker_Similar_pk");

                    b.HasIndex("TickerTwoId");

                    b.ToTable("TickerSimilar");
                });

            modelBuilder.Entity("WebApi.Models.TickerUser", b =>
                {
                    b.Property<string>("TickerSymbol")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserLogin")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TickerSymbol", "UserLogin")
                        .HasName("Ticke_rUser_pk");

                    b.HasIndex("UserLogin");

                    b.ToTable("TickerUser");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.Property<string>("Login")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("RefreshTokenExp")
                        .HasColumnType("datetime2");

                    b.HasKey("Login")
                        .HasName("User_pk");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApi.Models.TickerOHLC", b =>
                {
                    b.HasOne("WebApi.Models.TickerDetails", "Ticker")
                        .WithMany("TickerOHLCs")
                        .HasForeignKey("Symbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticker");
                });

            modelBuilder.Entity("WebApi.Models.TickerOpenClose", b =>
                {
                    b.HasOne("WebApi.Models.TickerDetails", "Ticker")
                        .WithMany("TickerOpenCloses")
                        .HasForeignKey("Symbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticker");
                });

            modelBuilder.Entity("WebApi.Models.TickerSimilar", b =>
                {
                    b.HasOne("WebApi.Models.TickerDetails", "TickerOne")
                        .WithMany("Similar")
                        .HasForeignKey("TickerOneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.TickerDetails", "TickerTwo")
                        .WithMany("SimilarTo")
                        .HasForeignKey("TickerTwoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TickerOne");

                    b.Navigation("TickerTwo");
                });

            modelBuilder.Entity("WebApi.Models.TickerUser", b =>
                {
                    b.HasOne("WebApi.Models.TickerDetails", "Ticker")
                        .WithMany("UsersWatching")
                        .HasForeignKey("TickerSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.User", "User")
                        .WithMany("TickersWatching")
                        .HasForeignKey("UserLogin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApi.Models.TickerDetails", b =>
                {
                    b.Navigation("Similar");

                    b.Navigation("SimilarTo");

                    b.Navigation("TickerOHLCs");

                    b.Navigation("TickerOpenCloses");

                    b.Navigation("UsersWatching");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.Navigation("TickersWatching");
                });
#pragma warning restore 612, 618
        }
    }
}
