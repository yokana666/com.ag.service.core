using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class Fix_GarmentSupplier_PPH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IncomeTaxesRate",
                table: "GarmentSuppliers",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IncomeTaxesId",
                table: "GarmentSuppliers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IncomeTaxesRate",
                table: "GarmentSuppliers",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "IncomeTaxesId",
                table: "GarmentSuppliers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
