using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarColunasVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "preco",
                table: "veiculos",
                newName: "preco_venda");

            migrationBuilder.AddColumn<decimal>(
                name: "preco_custo",
                table: "veiculos",
                type: "numeric(20,3)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preco_custo",
                table: "veiculos");

            migrationBuilder.RenameColumn(
                name: "preco_venda",
                table: "veiculos",
                newName: "preco");
        }
    }
}
