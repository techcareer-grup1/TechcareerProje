using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechCareer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    About = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TotalHours = table.Column<double>(type: "float", nullable: false),
                    IsCertified = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgrammingLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoEducations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 188, 15, 217, 215, 66, 234, 131, 171, 184, 49, 210, 181, 20, 188, 40, 155, 2, 189, 126, 234, 210, 126, 48, 239, 33, 26, 116, 113, 39, 175, 88, 124, 60, 26, 62, 178, 130, 228, 111, 42, 143, 232, 13, 67, 219, 141, 208, 235, 73, 210, 170, 16, 235, 133, 128, 157, 171, 63, 231, 1, 138, 249, 91, 61 }, new byte[] { 135, 199, 126, 181, 81, 45, 69, 252, 132, 53, 206, 21, 228, 202, 162, 240, 135, 154, 16, 31, 186, 203, 174, 93, 162, 221, 68, 253, 133, 157, 156, 99, 224, 204, 159, 183, 8, 57, 40, 165, 224, 194, 250, 210, 203, 65, 155, 50, 36, 181, 177, 68, 167, 246, 60, 14, 107, 69, 154, 4, 40, 81, 58, 92, 100, 108, 167, 140, 35, 87, 167, 244, 70, 92, 240, 207, 136, 50, 130, 100, 13, 155, 185, 170, 48, 200, 144, 37, 41, 48, 46, 243, 211, 65, 89, 240, 184, 5, 73, 244, 147, 36, 59, 169, 159, 111, 108, 161, 154, 129, 2, 103, 208, 8, 115, 191, 89, 157, 20, 81, 19, 18, 202, 140, 65, 85, 18, 252 } });

            migrationBuilder.CreateIndex(
                name: "IX_Event_CategoryId",
                table: "Event",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoEducations_Title",
                table: "VideoEducations",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "VideoEducations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 202, 143, 232, 240, 34, 138, 97, 155, 88, 54, 47, 52, 219, 38, 41, 123, 89, 197, 212, 176, 60, 246, 97, 42, 135, 215, 56, 149, 130, 68, 223, 113, 124, 57, 147, 24, 151, 46, 117, 61, 65, 36, 235, 67, 24, 180, 175, 120, 189, 239, 74, 232, 248, 185, 33, 48, 157, 101, 194, 71, 224, 2, 71, 231 }, new byte[] { 3, 91, 155, 9, 74, 4, 198, 138, 226, 129, 246, 146, 11, 4, 46, 35, 152, 124, 72, 220, 3, 144, 17, 160, 137, 100, 196, 114, 103, 25, 247, 111, 150, 51, 191, 133, 254, 131, 7, 238, 34, 240, 202, 49, 151, 9, 92, 36, 237, 56, 120, 147, 195, 109, 250, 100, 133, 106, 55, 193, 20, 254, 82, 224, 251, 83, 180, 188, 164, 224, 128, 221, 138, 244, 185, 13, 201, 182, 20, 140, 27, 122, 67, 244, 10, 20, 48, 148, 208, 17, 159, 36, 126, 206, 180, 92, 216, 234, 159, 6, 135, 156, 117, 192, 111, 48, 90, 142, 29, 83, 219, 138, 137, 10, 44, 218, 225, 186, 158, 43, 70, 168, 229, 134, 239, 45, 66, 231 } });
        }
    }
}
