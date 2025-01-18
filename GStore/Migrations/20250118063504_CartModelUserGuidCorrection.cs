using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GStore.Migrations
{
    /// <inheritdoc />
    public partial class CartModelUserGuidCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId1",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Carts",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Carts",
                newName: "CodeClient");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_UserId1",
                table: "Carts",
                newName: "IX_Carts_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_ClientId",
                table: "Carts",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_ClientId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "CodeClient",
                table: "Carts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Carts",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ClientId",
                table: "Carts",
                newName: "IX_Carts_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId1",
                table: "Carts",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
