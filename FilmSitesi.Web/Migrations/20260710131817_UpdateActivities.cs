using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmSitesi.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Activities",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Activities",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Activities");
        }
    }
}
