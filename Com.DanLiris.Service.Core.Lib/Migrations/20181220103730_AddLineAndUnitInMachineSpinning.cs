using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class AddLineAndUnitInMachineSpinning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Line",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitId",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Line",
                table: "MachineSpinnings");

            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "MachineSpinnings");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "MachineSpinnings");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "MachineSpinnings");
        }
    }
}
