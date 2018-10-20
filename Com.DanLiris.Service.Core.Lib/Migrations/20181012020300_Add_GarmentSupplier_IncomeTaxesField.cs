using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class Add_GarmentSupplier_IncomeTaxesField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncomeTaxesId",
                table: "GarmentSuppliers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxesName",
                table: "GarmentSuppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxesRate",
                table: "GarmentSuppliers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeTaxesId",
                table: "GarmentSuppliers");

            migrationBuilder.DropColumn(
                name: "IncomeTaxesName",
                table: "GarmentSuppliers");

            migrationBuilder.DropColumn(
                name: "IncomeTaxesRate",
                table: "GarmentSuppliers");
        }
    }
}
