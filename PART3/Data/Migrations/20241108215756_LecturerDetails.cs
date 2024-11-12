using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PART3.Data.Migrations
{
    /// <inheritdoc />
    public partial class LecturerDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lecture_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lecturer_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lecturer_Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lecturer_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lecturer_Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Module_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hours_Worked = table.Column<int>(type: "int", nullable: false),
                    Hourly_Rate = table.Column<int>(type: "int", nullable: false),
                    Date_Of_Session = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lecturers");
        }
    }
}
