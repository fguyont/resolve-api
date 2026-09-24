using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResolveApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAiAnalysisToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AiAnalysis",
                table: "Tickets",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiAnalysis",
                table: "Tickets");
        }
    }
}
