using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Migrations
{
    public partial class AddProductSPPProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerAddress",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ColorName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Construction",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DesignCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DesignNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Lot",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderTypeCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderTypeName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductionOrderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductionOrderNo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductSPPProperties",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    BuyerAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    BuyerName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ColorName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Construction = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DesignCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DesignNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Lot = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OrderTypeCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    OrderTypeId = table.Column<int>(type: "int", nullable: false),
                    OrderTypeName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductionOrderNo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    _CreatedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _DeletedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _DeletedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _DeletedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    _LastModifiedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSPPProperties", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductSPPProperties_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSPPProperties");

            migrationBuilder.AddColumn<string>(
                name: "BuyerAddress",
                table: "Products",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "Products",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                table: "Products",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Construction",
                table: "Products",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignCode",
                table: "Products",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignNumber",
                table: "Products",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Products",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Lot",
                table: "Products",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderTypeCode",
                table: "Products",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderTypeId",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderTypeName",
                table: "Products",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionOrderId",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductionOrderNo",
                table: "Products",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }
    }
}
