using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class updateAccountBank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "AccountBanks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "AccountBanks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "AccountBanks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "AccountBanks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AccountBanks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "AccountBanks");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "AccountBanks");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "AccountBanks");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "AccountBanks");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AccountBanks");
        }
    }
}
