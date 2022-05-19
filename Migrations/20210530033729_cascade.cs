using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelSystem.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dislikedPosts_AspNetUsers_userID",
                table: "dislikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_dislikedPosts_TripPosts_postID",
                table: "dislikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedPosts_AspNetUsers_userID",
                table: "LikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedPosts_TripPosts_postID",
                table: "LikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedPosts_AspNetUsers_userID",
                table: "SavedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedPosts_TripPosts_postID",
                table: "SavedPosts");

            migrationBuilder.AddForeignKey(
                name: "FK_dislikedPosts_AspNetUsers_userID",
                table: "dislikedPosts",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dislikedPosts_TripPosts_postID",
                table: "dislikedPosts",
                column: "postID",
                principalTable: "TripPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedPosts_AspNetUsers_userID",
                table: "LikedPosts",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedPosts_TripPosts_postID",
                table: "LikedPosts",
                column: "postID",
                principalTable: "TripPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedPosts_AspNetUsers_userID",
                table: "SavedPosts",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedPosts_TripPosts_postID",
                table: "SavedPosts",
                column: "postID",
                principalTable: "TripPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dislikedPosts_AspNetUsers_userID",
                table: "dislikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_dislikedPosts_TripPosts_postID",
                table: "dislikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedPosts_AspNetUsers_userID",
                table: "LikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedPosts_TripPosts_postID",
                table: "LikedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedPosts_AspNetUsers_userID",
                table: "SavedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedPosts_TripPosts_postID",
                table: "SavedPosts");

            migrationBuilder.AddForeignKey(
                name: "FK_dislikedPosts_AspNetUsers_userID",
                table: "dislikedPosts",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dislikedPosts_TripPosts_postID",
                table: "dislikedPosts",
                column: "postID",
                principalTable: "TripPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedPosts_AspNetUsers_userID",
                table: "LikedPosts",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedPosts_TripPosts_postID",
                table: "LikedPosts",
                column: "postID",
                principalTable: "TripPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedPosts_AspNetUsers_userID",
                table: "SavedPosts",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedPosts_TripPosts_postID",
                table: "SavedPosts",
                column: "postID",
                principalTable: "TripPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
