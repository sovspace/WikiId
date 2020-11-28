using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class fixprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_ProfileId",
                table: "AccessRequests",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRequests_Profiles_ProfileId",
                table: "AccessRequests",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessRequests_Profiles_ProfileId",
                table: "AccessRequests");

            migrationBuilder.DropIndex(
                name: "IX_AccessRequests_ProfileId",
                table: "AccessRequests");
        }
    }
}
