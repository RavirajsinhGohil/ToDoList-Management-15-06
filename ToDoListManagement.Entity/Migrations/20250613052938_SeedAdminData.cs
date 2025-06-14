using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListManagement.Entity.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "IsDeleted", "Name", "PasswordHash", "Role" },
                values: new object[] { 1, "admin@outlook.com", false, "Admin", "$2a$11$YCiiJxUwumHUtegC05ahFej29UzVm/s1HRwPriPIuta4b.GWddWuW", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
