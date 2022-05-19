using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelSystem.Migrations
{
    public partial class implementLikesAndDislikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisLikes",
                table: "TripPosts");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "TripPosts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisLikes",
                table: "TripPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "TripPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
