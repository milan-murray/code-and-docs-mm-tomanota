using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_web_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompletedPremadeTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    titlePremade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedPremadeTitles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromptBodies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    PromptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptBodies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Propmts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    LastProgressed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propmts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MostRecentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    CompletedPremadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeekProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Monday = table.Column<int>(type: "int", nullable: false),
                    Tuesday = table.Column<int>(type: "int", nullable: false),
                    Wednesday = table.Column<int>(type: "int", nullable: false),
                    Thursday = table.Column<int>(type: "int", nullable: false),
                    Friday = table.Column<int>(type: "int", nullable: false),
                    Saturday = table.Column<int>(type: "int", nullable: false),
                    Sunday = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YearProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Jan = table.Column<int>(type: "int", nullable: false),
                    Feb = table.Column<int>(type: "int", nullable: false),
                    Mar = table.Column<int>(type: "int", nullable: false),
                    Apr = table.Column<int>(type: "int", nullable: false),
                    May = table.Column<int>(type: "int", nullable: false),
                    Jun = table.Column<int>(type: "int", nullable: false),
                    Jul = table.Column<int>(type: "int", nullable: false),
                    Aug = table.Column<int>(type: "int", nullable: false),
                    Sep = table.Column<int>(type: "int", nullable: false),
                    Oct = table.Column<int>(type: "int", nullable: false),
                    Nov = table.Column<int>(type: "int", nullable: false),
                    Dec = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearProgresses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedPremadeTitles");

            migrationBuilder.DropTable(
                name: "PromptBodies");

            migrationBuilder.DropTable(
                name: "Propmts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WeekProgresses");

            migrationBuilder.DropTable(
                name: "YearProgresses");
        }
    }
}
