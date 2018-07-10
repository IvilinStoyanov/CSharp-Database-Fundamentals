using Microsoft.EntityFrameworkCore.Migrations;

namespace P01_HospitalDatabase.Data.Migrations
{
    public partial class AddDoctor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Specialty",
                table: "Doctors",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Specialty",
                table: "Doctors",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
