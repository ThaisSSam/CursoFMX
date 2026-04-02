using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class precosDeVeiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "usuarios",
                newName: "nome");

            migrationBuilder.AddColumn<decimal>(
                name: "preco_custo",
                table: "veiculos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "preco_venda",
                table: "veiculos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preco_custo",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "preco_venda",
                table: "veiculos");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "usuarios",
                newName: "name");
        }
    }
}
