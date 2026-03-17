using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPrecoVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "marca_id",
                table: "veiculos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "preco",
                table: "veiculos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte[]>(
                name: "versao_linha",
                table: "veiculos",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "ix_veiculos_marca_id",
                table: "veiculos",
                column: "marca_id");

            migrationBuilder.AddForeignKey(
                name: "fk_veiculos_marcas_marca_id",
                table: "veiculos",
                column: "marca_id",
                principalTable: "marcas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_veiculos_marcas_marca_id",
                table: "veiculos");

            migrationBuilder.DropIndex(
                name: "ix_veiculos_marca_id",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "marca_id",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "preco",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "versao_linha",
                table: "veiculos");
        }
    }
}
