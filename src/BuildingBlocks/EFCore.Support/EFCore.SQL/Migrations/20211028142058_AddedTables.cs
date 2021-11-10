﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.SQL.Migrations
{
    public partial class AddedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoilProcessMaster",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Sr = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoilNo = table.Column<int>(nullable: false),
                    JangadNo = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    BranchId = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    FinancialId = table.Column<string>(nullable: true),
                    BoilType = table.Column<int>(nullable: false),
                    KapanId = table.Column<string>(nullable: true),
                    ShapeId = table.Column<string>(nullable: true),
                    SizeId = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    LossWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    RejectionWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    HandOverById = table.Column<string>(nullable: true),
                    HandOverToId = table.Column<string>(nullable: true),
                    SlipNo = table.Column<string>(nullable: true),
                    BoilCategoy = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoilProcessMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharniProcessMaster",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Sr = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharniNo = table.Column<int>(nullable: false),
                    JangadNo = table.Column<int>(nullable: false),
                    BoilJangadNo = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    BranchId = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    FinancialId = table.Column<string>(nullable: true),
                    CharniType = table.Column<int>(nullable: false),
                    KapanId = table.Column<string>(nullable: true),
                    ShapeId = table.Column<string>(nullable: true),
                    SizeId = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    CharniSizeId = table.Column<string>(nullable: true),
                    CharniWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    LossWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    RejectionWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    HandOverById = table.Column<string>(nullable: true),
                    HandOverToId = table.Column<string>(nullable: true),
                    SlipNo = table.Column<string>(nullable: true),
                    CharniCategoy = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharniProcessMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GalaProcessMaster",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Sr = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharniNo = table.Column<int>(nullable: false),
                    JangadNo = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    BranchId = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    FinancialId = table.Column<string>(nullable: true),
                    GalaProcessType = table.Column<int>(nullable: false),
                    KapanId = table.Column<string>(nullable: true),
                    ShapeId = table.Column<string>(nullable: true),
                    SizeId = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    GalaNumberId = table.Column<string>(nullable: true),
                    GalaWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    LossWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    RejectionWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    HandOverById = table.Column<string>(nullable: true),
                    HandOverToId = table.Column<string>(nullable: true),
                    SlipNo = table.Column<string>(nullable: true),
                    GalaCategoy = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalaProcessMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumberProcessMaster",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Sr = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberNo = table.Column<int>(nullable: false),
                    JangadNo = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    BranchId = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    FinancialId = table.Column<string>(nullable: true),
                    NumberProcessType = table.Column<int>(nullable: false),
                    KapanId = table.Column<string>(nullable: true),
                    ShapeId = table.Column<string>(nullable: true),
                    SizeId = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    GalaNumberId = table.Column<string>(nullable: true),
                    NumberId = table.Column<string>(nullable: true),
                    NumberWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    LossWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    RejectionWeight = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    HandOverById = table.Column<string>(nullable: true),
                    HandOverToId = table.Column<string>(nullable: true),
                    SlipNo = table.Column<string>(nullable: true),
                    NumberCategoy = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberProcessMaster", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoilProcessMaster");

            migrationBuilder.DropTable(
                name: "CharniProcessMaster");

            migrationBuilder.DropTable(
                name: "GalaProcessMaster");

            migrationBuilder.DropTable(
                name: "NumberProcessMaster");
        }
    }
}