using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPProject.Migrations
{
    public partial class CreateSliderTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titile",
                table: "SliderInfos",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SliderInfos",
                newName: "Titile");
        }
    }
}
