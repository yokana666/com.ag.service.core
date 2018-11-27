using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class Add_Columns_MachineSpinning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounterCondition",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UomId",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "MachineSpinnings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "MachineSpinnings");

            migrationBuilder.DropColumn(
                name: "CounterCondition",
                table: "MachineSpinnings");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "MachineSpinnings");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "MachineSpinnings");
        }
    }
}
