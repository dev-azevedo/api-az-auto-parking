using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzAutoParking.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameColumnToParkingNumberInParking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberParking",
                table: "Parkings",
                newName: "ParkingNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParkingNumber",
                table: "Parkings",
                newName: "NumberParking");
        }
    }
}
