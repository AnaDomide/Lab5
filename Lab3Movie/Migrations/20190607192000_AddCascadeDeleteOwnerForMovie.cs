using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab3Movie.Migrations
{
    public partial class AddCascadeDeleteOwnerForMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_OwnerId",
                table: "Movies");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_OwnerId",
                table: "Movies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_OwnerId",
                table: "Movies");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_OwnerId",
                table: "Movies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
