﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Healthcare.Data.Migrations
{
    /// <inheritdoc />
    public partial class uploadfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadedFilePath",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedFilePath",
                table: "Appointments");
        }
    }
}
