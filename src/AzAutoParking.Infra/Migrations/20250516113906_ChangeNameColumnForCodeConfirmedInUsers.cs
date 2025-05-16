using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzAutoParking.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameColumnForCodeConfirmedInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeConfirmedAccount",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "CodeConfirmedAccount",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
