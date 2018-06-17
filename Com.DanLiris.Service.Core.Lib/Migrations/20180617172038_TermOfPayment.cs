using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class TermOfPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "termOfPayment",
                table: "TermOfPayments");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TermOfPayments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TermOfPayments");

            migrationBuilder.AddColumn<string>(
                name: "termOfPayment",
                table: "TermOfPayments",
                maxLength: 1000,
                nullable: true);
        }
    }
}
