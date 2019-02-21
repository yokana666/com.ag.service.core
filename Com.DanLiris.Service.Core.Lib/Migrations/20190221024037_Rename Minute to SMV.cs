using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class RenameMinutetoSMV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinuteCutting",
                table: "StandardMinuteValues",
                newName: "SMVCutting");

            migrationBuilder.RenameColumn(
                name: "MinuteFinishing",
                table: "StandardMinuteValues",
                newName: "SMVFinishing");

            migrationBuilder.RenameColumn(
                name: "MinuteSewing",
                table: "StandardMinuteValues",
                newName: "SMVSewing");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SMVCutting",
                table: "StandardMinuteValues",
                newName: "MinuteCutting");

            migrationBuilder.RenameColumn(
                name: "SMVFinishing",
                table: "StandardMinuteValues",
                newName: "MinuteFinishing");

            migrationBuilder.RenameColumn(
                name: "SMVSewing",
                table: "StandardMinuteValues",
                newName: "MinuteSewing");
        }
    }
}
