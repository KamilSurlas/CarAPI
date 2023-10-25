using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAPI.Migrations
{
    /// <inheritdoc />
    public partial class unique_policy_number : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PolicyNumber",
                table: "Insurances",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_PolicyNumber",
                table: "Insurances",
                column: "PolicyNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Insurances_PolicyNumber",
                table: "Insurances");

            migrationBuilder.AlterColumn<string>(
                name: "PolicyNumber",
                table: "Insurances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
