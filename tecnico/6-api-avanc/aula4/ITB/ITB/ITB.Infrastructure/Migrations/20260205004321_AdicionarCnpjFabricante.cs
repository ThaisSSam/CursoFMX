using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCnpjFabricante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produtos_fabricantes_fabricanteid",
                table: "produtos");

            migrationBuilder.AddForeignKey(
                name: "FK_produtos_fabricantes_fabricanteid",
                table: "produtos",
                column: "fabricanteid",
                principalTable: "fabricantes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produtos_fabricantes_fabricanteid",
                table: "produtos");

            migrationBuilder.AddForeignKey(
                name: "FK_produtos_fabricantes_fabricanteid",
                table: "produtos",
                column: "fabricanteid",
                principalTable: "fabricantes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
