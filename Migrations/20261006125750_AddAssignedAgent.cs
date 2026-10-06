using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResolveApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedAgent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedAgentId",
                table: "Tickets",
                column: "AssignedAgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_AssignedAgentId",
                table: "Tickets",
                column: "AssignedAgentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_AssignedAgentId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AssignedAgentId",
                table: "Tickets");
        }
    }
}
