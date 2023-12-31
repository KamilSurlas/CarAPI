using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedByUserTechnicalreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddedByUserId",
                table: "TechnicalReviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalReviews_AddedByUserId",
                table: "TechnicalReviews",
                column: "AddedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalReviews_Users_AddedByUserId",
                table: "TechnicalReviews",
                column: "AddedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalReviews_Users_AddedByUserId",
                table: "TechnicalReviews");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalReviews_AddedByUserId",
                table: "TechnicalReviews");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "TechnicalReviews");
        }
    }
}
