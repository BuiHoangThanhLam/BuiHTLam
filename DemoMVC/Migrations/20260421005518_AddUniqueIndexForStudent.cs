using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexForStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Student_Email",
                table: "Student",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_StudentCode",
                table: "Student",
                column: "StudentCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Student_Email",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_StudentCode",
                table: "Student");
        }
    }
}
