using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Try4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TickerDetails",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Market = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Locale = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PrimaryExchange = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Cik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CompositeFigi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShareClassFigi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    State = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SicCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SicDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TickerRoot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HomepageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TotalEmployees = table.Column<long>(type: "bigint", nullable: false),
                    ListDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShareClassSharesOutstanding = table.Column<long>(type: "bigint", nullable: false),
                    WeightedSharesOutstanding = table.Column<long>(type: "bigint", nullable: false),
                    RoundLot = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Ticker_pk", x => x.Ticker);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshTokenExp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pk", x => x.Login);
                });

            migrationBuilder.CreateTable(
                name: "TickerOHLC",
                columns: table => new
                {
                    Multuplier = table.Column<long>(type: "bigint", nullable: false),
                    Timespan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    C = table.Column<float>(type: "real", nullable: false),
                    H = table.Column<float>(type: "real", nullable: false),
                    L = table.Column<float>(type: "real", nullable: false),
                    N = table.Column<long>(type: "bigint", nullable: false),
                    O = table.Column<float>(type: "real", nullable: false),
                    T = table.Column<long>(type: "bigint", nullable: false),
                    V = table.Column<long>(type: "bigint", nullable: false),
                    Vw = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Ohlc_pk", x => new { x.Symbol, x.Timespan, x.Multuplier });
                    table.ForeignKey(
                        name: "FK_TickerOHLC_TickerDetails_Symbol",
                        column: x => x.Symbol,
                        principalTable: "TickerDetails",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TickerOpenClose",
                columns: table => new
                {
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AfterHours = table.Column<float>(type: "real", nullable: false),
                    Close = table.Column<float>(type: "real", nullable: false),
                    High = table.Column<float>(type: "real", nullable: false),
                    Low = table.Column<float>(type: "real", nullable: false),
                    Open = table.Column<float>(type: "real", nullable: false),
                    PreMarket = table.Column<float>(type: "real", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TockerOpenClose_pk", x => new { x.Symbol, x.From });
                    table.ForeignKey(
                        name: "FK_TickerOpenClose_TickerDetails_Symbol",
                        column: x => x.Symbol,
                        principalTable: "TickerDetails",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TickerSimilar",
                columns: table => new
                {
                    TickerOneId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TickerTwoId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Ticker_Similar_pk", x => new { x.TickerOneId, x.TickerTwoId });
                    table.ForeignKey(
                        name: "FK_TickerSimilar_TickerDetails_TickerOneId",
                        column: x => x.TickerOneId,
                        principalTable: "TickerDetails",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TickerSimilar_TickerDetails_TickerTwoId",
                        column: x => x.TickerTwoId,
                        principalTable: "TickerDetails",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TickerUser",
                columns: table => new
                {
                    UserLogin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TickerSymbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Ticke_rUser_pk", x => new { x.TickerSymbol, x.UserLogin });
                    table.ForeignKey(
                        name: "FK_TickerUser_TickerDetails_TickerSymbol",
                        column: x => x.TickerSymbol,
                        principalTable: "TickerDetails",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TickerUser_Users_UserLogin",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TickerSimilar_TickerTwoId",
                table: "TickerSimilar",
                column: "TickerTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_TickerUser_UserLogin",
                table: "TickerUser",
                column: "UserLogin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerOHLC");

            migrationBuilder.DropTable(
                name: "TickerOpenClose");

            migrationBuilder.DropTable(
                name: "TickerSimilar");

            migrationBuilder.DropTable(
                name: "TickerUser");

            migrationBuilder.DropTable(
                name: "TickerDetails");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
