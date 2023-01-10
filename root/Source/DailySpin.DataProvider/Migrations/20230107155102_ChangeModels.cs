using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpin.Website.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Bets",
                newName: "UserAccountId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "GlassImage",
                table: "BetsGlasses",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "Bets",
                newName: "UserID");

            migrationBuilder.AlterColumn<byte[]>(
                name: "GlassImage",
                table: "BetsGlasses",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }
    }
}
