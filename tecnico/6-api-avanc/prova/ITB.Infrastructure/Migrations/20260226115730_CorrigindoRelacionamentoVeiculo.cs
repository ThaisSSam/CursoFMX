using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoRelacionamentoVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_veiculos_marcas_MarcaId",
                table: "veiculos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_veiculos",
                table: "veiculos");

            migrationBuilder.RenameTable(
                name: "veiculos",
                newName: "Veiculos");

            migrationBuilder.RenameIndex(
                name: "IX_veiculos_MarcaId",
                table: "Veiculos",
                newName: "IX_Veiculos_MarcaId");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Veiculos",
                type: "char(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Modelo",
                table: "Veiculos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Veiculos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MarcaId2",
                table: "Veiculos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Veiculos",
                table: "Veiculos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_MarcaId2",
                table: "Veiculos",
                column: "MarcaId2");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Placa",
                table: "Veiculos",
                column: "Placa",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_marcas_MarcaId",
                table: "Veiculos",
                column: "MarcaId",
                principalTable: "marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_marcas_MarcaId2",
                table: "Veiculos",
                column: "MarcaId2",
                principalTable: "marcas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_MarcaId",
                table: "Veiculos");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_MarcaId2",
                table: "Veiculos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Veiculos",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_MarcaId2",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Placa",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "MarcaId2",
                table: "Veiculos");

            migrationBuilder.RenameTable(
                name: "Veiculos",
                newName: "veiculos");

            migrationBuilder.RenameIndex(
                name: "IX_Veiculos_MarcaId",
                table: "veiculos",
                newName: "IX_veiculos_MarcaId");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "veiculos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(7)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "Modelo",
                table: "veiculos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_veiculos",
                table: "veiculos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_veiculos_marcas_MarcaId",
                table: "veiculos",
                column: "MarcaId",
                principalTable: "marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
