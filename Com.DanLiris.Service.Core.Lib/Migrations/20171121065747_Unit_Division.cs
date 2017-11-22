using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class Unit_Division : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Divisions_DivisionId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_DivisionId",
                table: "Units");

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "Units",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "Units");

            migrationBuilder.CreateIndex(
                name: "IX_Units_DivisionId",
                table: "Units",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Divisions_DivisionId",
                table: "Units",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
