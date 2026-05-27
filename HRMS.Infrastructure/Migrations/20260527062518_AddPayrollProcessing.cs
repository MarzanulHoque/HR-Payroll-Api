using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPayrollProcessing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalarySlips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Month = table.Column<string>(type: "TEXT", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "TEXT", nullable: false),
                    Deductions = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetPay = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalarySlips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalarySlips_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalarySlips_EmployeeId",
                table: "SalarySlips",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalarySlips");
        }
    }
}
