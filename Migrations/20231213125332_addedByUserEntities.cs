using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedByUserEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddedByUserId",
                table: "Repairs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedByUserId",
                table: "Insurances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_AddedByUserId",
                table: "Repairs",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_AddedByUserId",
                table: "Insurances",
                column: "AddedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Users_AddedByUserId",
                table: "Insurances",
                column: "AddedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_Users_AddedByUserId",
                table: "Repairs",
                column: "AddedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Users_AddedByUserId",
                table: "Insurances");

            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Users_AddedByUserId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_AddedByUserId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Insurances_AddedByUserId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "Insurances");
        }
    }
}
