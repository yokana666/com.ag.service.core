using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class Division : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_Division_DivisionId",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_Unit_Division_DivisionId",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Division",
                table: "Division");

            migrationBuilder.DropColumn(
                name: "MongoId",
                table: "Division");

            migrationBuilder.RenameTable(
                name: "Division",
                newName: "Divisions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Divisions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Divisions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Divisions_DivisionId",
                table: "Holiday",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_Divisions_DivisionId",
                table: "Unit",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_Divisions_DivisionId",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_Unit_Divisions_DivisionId",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions");

            migrationBuilder.RenameTable(
                name: "Divisions",
                newName: "Division");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Division",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Division",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MongoId",
                table: "Division",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Division",
                table: "Division",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Division_DivisionId",
                table: "Holiday",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_Division_DivisionId",
                table: "Unit",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
