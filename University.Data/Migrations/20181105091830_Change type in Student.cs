using Microsoft.EntityFrameworkCore.Migrations;

namespace University.Data.Migrations
{
    public partial class ChangetypeinStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Department_DepartmentId1",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_DepartmentId1",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "Student");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "Student",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Student_DepartmentId",
                table: "Student",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Department_DepartmentId",
                table: "Student",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Department_DepartmentId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_DepartmentId",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Student",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId1",
                table: "Student",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_DepartmentId1",
                table: "Student",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Department_DepartmentId1",
                table: "Student",
                column: "DepartmentId1",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
