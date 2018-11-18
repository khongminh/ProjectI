using Microsoft.EntityFrameworkCore.Migrations;

namespace University.Data.Migrations
{
    public partial class ChangeEnrollment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MidGrade",
                table: "Enrollment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "FinalGrade",
                table: "Enrollment",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MidGrade",
                table: "Enrollment",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FinalGrade",
                table: "Enrollment",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
