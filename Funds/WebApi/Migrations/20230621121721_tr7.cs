using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class tr7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickerUsers_TickerDetails_TickerSymbol",
                table: "tickerUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_tickerUsers_Users_UserLogin",
                table: "tickerUsers");

            migrationBuilder.RenameTable(
                name: "tickerUsers",
                newName: "TickerUser");

            migrationBuilder.RenameIndex(
                name: "IX_tickerUsers_UserLogin",
                table: "TickerUser",
                newName: "IX_TickerUser_UserLogin");

            migrationBuilder.AddForeignKey(
                name: "FK_TickerUser_TickerDetails_TickerSymbol",
                table: "TickerUser",
                column: "TickerSymbol",
                principalTable: "TickerDetails",
                principalColumn: "Ticker",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TickerUser_Users_UserLogin",
                table: "TickerUser",
                column: "UserLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TickerUser_TickerDetails_TickerSymbol",
                table: "TickerUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TickerUser_Users_UserLogin",
                table: "TickerUser");

            migrationBuilder.RenameTable(
                name: "TickerUser",
                newName: "tickerUsers");

            migrationBuilder.RenameIndex(
                name: "IX_TickerUser_UserLogin",
                table: "tickerUsers",
                newName: "IX_tickerUsers_UserLogin");

            migrationBuilder.AddForeignKey(
                name: "FK_tickerUsers_TickerDetails_TickerSymbol",
                table: "tickerUsers",
                column: "TickerSymbol",
                principalTable: "TickerDetails",
                principalColumn: "Ticker",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickerUsers_Users_UserLogin",
                table: "tickerUsers",
                column: "UserLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
