using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class Rename_Column_Compotition_into_Composition_on_GarmentProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Compotition",
                table: "GarmentProducts");

            migrationBuilder.AddColumn<string>(
                name: "Composition",
                table: "GarmentProducts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Composition",
                table: "GarmentProducts");

            migrationBuilder.AddColumn<string>(
                name: "Compotition",
                table: "GarmentProducts",
                nullable: true);
        }
    }
}
