using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingFPT.Migrations
{
    /// <inheritdoc />
    public partial class Topics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    NameTopic = table.Column<string>(type: "Varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "Varchar(200)", nullable: true),
                    Status = table.Column<string>(type: "Varchar(20)", nullable: false),
                    Documents = table.Column<string>(type: "Varchar(200)", nullable: false),
                    AttachFiles = table.Column<string>(type: "Varchar(200)", nullable: true),
                    TypeDocument = table.Column<string>(type: "Varchar(20)", nullable: false),
                    PosterTopic = table.Column<string>(type: "Varchar(200)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CoursesId",
                table: "Topics",
                column: "CoursesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
