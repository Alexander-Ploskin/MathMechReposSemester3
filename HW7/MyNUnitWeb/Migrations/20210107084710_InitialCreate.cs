using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyNUnitWeb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RunHistory",
                columns: table => new
                {
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunHistory", x => x.DateTime);
                });

            migrationBuilder.CreateTable(
                name: "ReportAssemblies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestRunModelDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAssemblies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportAssemblies_RunHistory_TestRunModelDateTime",
                        column: x => x.TestRunModelDateTime,
                        principalTable: "RunHistory",
                        principalColumn: "DateTime",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportsTests",
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
                    table.PrimaryKey("PK_ReportsTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportsTests_ReportAssemblies_AssemblyReportModelId",
                        column: x => x.AssemblyReportModelId,
                        principalTable: "ReportAssemblies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportAssemblies_TestRunModelDateTime",
                table: "ReportAssemblies",
                column: "TestRunModelDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsTests_AssemblyReportModelId",
                table: "ReportsTests",
                column: "AssemblyReportModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportsTests");

            migrationBuilder.DropTable(
                name: "ReportAssemblies");

            migrationBuilder.DropTable(
                name: "RunHistory");
        }
    }
}
