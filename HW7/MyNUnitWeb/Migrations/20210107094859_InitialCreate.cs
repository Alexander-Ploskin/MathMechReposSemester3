using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyNUnitWeb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RunModels",
                columns: table => new
                {
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunModels", x => x.DateTime);
                });

            migrationBuilder.CreateTable(
                name: "AssemblyReportModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passed = table.Column<int>(type: "int", nullable: false),
                    Failed = table.Column<int>(type: "int", nullable: false),
                    Ignored = table.Column<int>(type: "int", nullable: false),
                    TestRunModelDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyReportModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssemblyReportModels_RunModels_TestRunModelDateTime",
                        column: x => x.TestRunModelDateTime,
                        principalTable: "RunModels",
                        principalColumn: "DateTime",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestReportModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valid = table.Column<bool>(type: "bit", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssemblyReportModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestReportModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestReportModels_AssemblyReportModels_AssemblyReportModelId",
                        column: x => x.AssemblyReportModelId,
                        principalTable: "AssemblyReportModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyReportModels_TestRunModelDateTime",
                table: "AssemblyReportModels",
                column: "TestRunModelDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_TestReportModels_AssemblyReportModelId",
                table: "TestReportModels",
                column: "AssemblyReportModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestReportModels");

            migrationBuilder.DropTable(
                name: "AssemblyReportModels");

            migrationBuilder.DropTable(
                name: "RunModels");
        }
    }
}
