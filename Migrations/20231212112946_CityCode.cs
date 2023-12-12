using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lts.Migrations
{
    /// <inheritdoc />
    public partial class CityCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CityCode",
                table: "CityCode");

            migrationBuilder.RenameTable(
                name: "CityCode",
                newName: "CityCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityCodes",
                table: "CityCodes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CityCodes",
                table: "CityCodes");

            migrationBuilder.RenameTable(
                name: "CityCodes",
                newName: "CityCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityCode",
                table: "CityCode",
                column: "Id");
        }
    }
}
