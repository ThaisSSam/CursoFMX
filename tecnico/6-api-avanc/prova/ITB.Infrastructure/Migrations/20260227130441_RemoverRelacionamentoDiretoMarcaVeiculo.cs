using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoverRelacionamentoDiretoMarcaVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_MarcaId",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_MarcaId",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "MarcaId",
                table: "Veiculos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarcaId",
                table: "Veiculos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_MarcaId",
                table: "Veiculos",
                column: "MarcaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_marcas_MarcaId",
                table: "Veiculos",
                column: "MarcaId",
                principalTable: "marcas",
                principalColumn: "Id");
        }
    }
}
