using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListManagement.Entity.Migrations
{
    /// <inheritdoc />
    public partial class AssignedToPM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToPM",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssignedToPM",
                table: "Projects",
                column: "AssignedToPM");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_AssignedToPM",
                table: "Projects",
                column: "AssignedToPM",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_AssignedToPM",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AssignedToPM",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssignedToPM",
                table: "Projects");
        }
    }
}
