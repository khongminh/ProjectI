using Microsoft.EntityFrameworkCore.Migrations;

namespace University.Data.Migrations
{
    public partial class Changeteacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentId1",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Department_DepartmentId1",
                table: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_DepartmentId1",
                table: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Course_DepartmentId1",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "Course");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "Teacher",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "Course",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_DepartmentId",
                table: "Teacher",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_DepartmentId",
                table: "Course",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Department_DepartmentId",
                table: "Course",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Department_DepartmentId",
                table: "Teacher",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Department_DepartmentId",
                table: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_DepartmentId",
                table: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Course_DepartmentId",
                table: "Course");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Teacher",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId1",
                table: "Teacher",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Course",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId1",
                table: "Course",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_DepartmentId1",
                table: "Teacher",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Course_DepartmentId1",
                table: "Course",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Department_DepartmentId1",
                table: "Course",
                column: "DepartmentId1",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Department_DepartmentId1",
                table: "Teacher",
                column: "DepartmentId1",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
