using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class prova1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "modelo_id1",
                table: "veiculos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "marca_id1",
                table: "modelos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "marcas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_veiculos_modelo_id1",
                table: "veiculos",
                column: "modelo_id1");

            migrationBuilder.CreateIndex(
                name: "ix_modelos_marca_id1",
                table: "modelos",
                column: "marca_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_modelos_marcas_marca_id1",
                table: "modelos",
                column: "marca_id1",
                principalTable: "marcas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_veiculos_modelos_modelo_id1",
                table: "veiculos",
                column: "modelo_id1",
                principalTable: "modelos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_modelos_marcas_marca_id1",
                table: "modelos");

            migrationBuilder.DropForeignKey(
                name: "fk_veiculos_modelos_modelo_id1",
                table: "veiculos");

            migrationBuilder.DropIndex(
                name: "ix_veiculos_modelo_id1",
                table: "veiculos");

            migrationBuilder.DropIndex(
                name: "ix_modelos_marca_id1",
                table: "modelos");

            migrationBuilder.DropColumn(
                name: "modelo_id1",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "marca_id1",
                table: "modelos");

            migrationBuilder.DropColumn(
                name: "ativo",
                table: "marcas");
        }
    }
}
