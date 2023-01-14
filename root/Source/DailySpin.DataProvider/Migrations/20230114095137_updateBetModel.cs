using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpin.Website.Migrations
{
    /// <inheritdoc />
    public partial class updateBetModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_BetsGlasses_BetsGlassId",
                table: "Bets");

            migrationBuilder.AlterColumn<Guid>(
                name: "BetsGlassId",
                table: "Bets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_BetsGlasses_BetsGlassId",
                table: "Bets",
                column: "BetsGlassId",
                principalTable: "BetsGlasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_BetsGlasses_BetsGlassId",
                table: "Bets");

            migrationBuilder.AlterColumn<Guid>(
                name: "BetsGlassId",
                table: "Bets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_BetsGlasses_BetsGlassId",
                table: "Bets",
                column: "BetsGlassId",
                principalTable: "BetsGlasses",
                principalColumn: "Id");
        }
    }
}
