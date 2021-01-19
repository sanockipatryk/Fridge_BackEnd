using Microsoft.EntityFrameworkCore.Migrations;

namespace Fridge_BackEnd.Migrations
{
    public partial class InvitationsRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InvitationAccepted",
                table: "FridgeUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InvitationPending",
                table: "FridgeUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationAccepted",
                table: "FridgeUsers");

            migrationBuilder.DropColumn(
                name: "InvitationPending",
                table: "FridgeUsers");
        }
    }
}
