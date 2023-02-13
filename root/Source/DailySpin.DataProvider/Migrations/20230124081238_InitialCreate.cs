using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpin.DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BetsGlasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GlassImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BetMultiply = table.Column<int>(type: "int", nullable: false),
                    BetsCount = table.Column<long>(type: "bigint", nullable: false),
                    TotalBetSum = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ColorType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetsGlasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roulettes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roulettes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WinHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WinHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BetsGlassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserBet = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_BetsGlasses_BetsGlassId",
                        column: x => x.BetsGlassId,
                        principalTable: "BetsGlasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ColorType = table.Column<int>(type: "int", nullable: false),
                    WinChip = table.Column<bool>(type: "bit", nullable: false),
                    WinChipHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chips_WinHistory_WinChipHistoryId",
                        column: x => x.WinChipHistoryId,
                        principalTable: "WinHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_BetsGlassId",
                table: "Bets",
                column: "BetsGlassId");

            migrationBuilder.CreateIndex(
                name: "IX_Chips_WinChipHistoryId",
                table: "Chips",
                column: "WinChipHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "Chips");

            migrationBuilder.DropTable(
                name: "Roulettes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BetsGlasses");

            migrationBuilder.DropTable(
                name: "WinHistory");
        }
    }
}
