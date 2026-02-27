using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirRelacionamentoModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_MarcaId",
                table: "Veiculos");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_MarcaId2",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_MarcaId2",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "MarcaId2",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "Veiculos");

            migrationBuilder.AlterColumn<int>(
                name: "MarcaId",
                table: "Veiculos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ModeloId",
                table: "Veiculos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_ModeloId",
                table: "Veiculos",
                column: "ModeloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_Modelos_ModeloId",
                table: "Veiculos",
                column: "ModeloId",
                principalTable: "Modelos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_marcas_MarcaId",
                table: "Veiculos",
                column: "MarcaId",
                principalTable: "marcas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_marcas_ModeloId",
                table: "Veiculos",
                column: "ModeloId",
                principalTable: "marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_Modelos_ModeloId",
                table: "Veiculos");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_MarcaId",
                table: "Veiculos");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_marcas_ModeloId",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_ModeloId",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "ModeloId",
                table: "Veiculos");

            migrationBuilder.AlterColumn<int>(
                name: "MarcaId",
                table: "Veiculos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MarcaId2",
                table: "Veiculos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "Veiculos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_MarcaId2",
                table: "Veiculos",
                column: "MarcaId2");

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
    }
}
