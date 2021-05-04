using Microsoft.EntityFrameworkCore.Migrations;

namespace EventsAPI.Migrations
{
    public partial class AddedEmployeeIdToParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EventParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EventParticipant");
        }
    }
}
