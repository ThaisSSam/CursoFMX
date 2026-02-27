using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoverColunaDuplicadaMarcaDeVerdade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modelos_marcas_MarcaId1",
                table: "Modelos");

            migrationBuilder.DropIndex(
                name: "IX_Modelos_MarcaId1",
                table: "Modelos");

            migrationBuilder.DropColumn(
                name: "MarcaId1",
                table: "Modelos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarcaId1",
                table: "Modelos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Modelos_MarcaId1",
                table: "Modelos",
                column: "MarcaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Modelos_marcas_MarcaId1",
                table: "Modelos",
                column: "MarcaId1",
                principalTable: "marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
