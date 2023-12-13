using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lts.Migrations
{
    /// <inheritdoc />
    public partial class AddGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeslaCarGuid",
                table: "TeslaCars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeslaCarGuid",
                table: "TeslaCars");
        }
    }
}
