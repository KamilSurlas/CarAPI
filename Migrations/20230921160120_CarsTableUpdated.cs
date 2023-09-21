using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAPI.Migrations
{
    /// <inheritdoc />
    public partial class CarsTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Drivetrain",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Drivetrain",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Cars");
        }
    }
}
