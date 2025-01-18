using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GStore.Migrations
{
    /// <inheritdoc />
    public partial class UserTbleCodeClientAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientCode",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientCode",
                table: "Users");
        }
    }
}
