using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPProject.Migrations
{
    public partial class ChanageAppUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Baskets_BasketId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BasketId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "BasketId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BasketId",
                table: "AspNetUsers",
                column: "BasketId",
                unique: true,
                filter: "[BasketId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Baskets_BasketId",
                table: "AspNetUsers",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Baskets_BasketId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BasketId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "BasketId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BasketId",
                table: "AspNetUsers",
                column: "BasketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Baskets_BasketId",
                table: "AspNetUsers",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
