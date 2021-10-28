﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.SQL.Migrations
{
    public partial class CharniMasterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharniMaster",
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
                    table.PrimaryKey("PK_CharniMaster", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharniMaster");
        }
    }
}