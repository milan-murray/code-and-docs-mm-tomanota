using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_web_API.Migrations
{
    /// <inheritdoc />
    public partial class promptChnage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Propmts",
                table: "Propmts");

            migrationBuilder.RenameTable(
                name: "Propmts",
                newName: "Prompts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prompts",
                table: "Prompts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Prompts",
                table: "Prompts");

            migrationBuilder.RenameTable(
                name: "Prompts",
                newName: "Propmts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Propmts",
                table: "Propmts",
                column: "Id");
        }
    }
}
