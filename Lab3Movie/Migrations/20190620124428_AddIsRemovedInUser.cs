using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab3Movie.Migrations
{
    public partial class AddIsRemovedInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isRemoved",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isRemoved",
                table: "Users");
        }
    }
}
