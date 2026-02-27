using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoverColunaMarcaVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_ModeloId",
                table: "Veiculos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_marcas_ModeloId",
                table: "Veiculos",
                column: "ModeloId",
                principalTable: "marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
