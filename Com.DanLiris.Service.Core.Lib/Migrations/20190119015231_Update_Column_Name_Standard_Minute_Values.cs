using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class Update_Column_Name_Standard_Minute_Values : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cutting",
                table: "StandardMinuteValues");

            migrationBuilder.DropColumn(
                name: "Finishing",
                table: "StandardMinuteValues");

            migrationBuilder.DropColumn(
                name: "Sewing",
                table: "StandardMinuteValues");

            migrationBuilder.AddColumn<decimal>(
                name: "MinuteCutting",
                table: "StandardMinuteValues",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinuteFinishing",
                table: "StandardMinuteValues",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinuteSewing",
                table: "StandardMinuteValues",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinuteCutting",
                table: "StandardMinuteValues");

            migrationBuilder.DropColumn(
                name: "MinuteFinishing",
                table: "StandardMinuteValues");

            migrationBuilder.DropColumn(
                name: "MinuteSewing",
                table: "StandardMinuteValues");

            migrationBuilder.AddColumn<decimal>(
                name: "Cutting",
                table: "StandardMinuteValues",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Finishing",
                table: "StandardMinuteValues",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Sewing",
                table: "StandardMinuteValues",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
