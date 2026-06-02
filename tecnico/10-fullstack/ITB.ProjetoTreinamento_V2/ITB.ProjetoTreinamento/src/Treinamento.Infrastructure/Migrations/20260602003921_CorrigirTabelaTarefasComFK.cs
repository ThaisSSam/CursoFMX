using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirTabelaTarefasComFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tarefas",
                table: "tb_tarefas");

            migrationBuilder.DropColumn(
                name: "Responsavel",
                table: "tb_tarefas");

            migrationBuilder.RenameTable(
                name: "tb_tarefas",
                newName: "tb_tarefas");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "tb_tarefas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_tarefas",
                table: "tb_tarefas",
                column: "Cod");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tarefas_UsuarioId",
                table: "tb_tarefas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tarefas_tb_usuarios_UsuarioId",
                table: "tb_tarefas",
                column: "UsuarioId",
                principalSchema: "treinamento",
                principalTable: "tb_usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_tarefas_tb_usuarios_UsuarioId",
                table: "tb_tarefas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_tarefas",
                table: "tb_tarefas");

            migrationBuilder.DropIndex(
                name: "IX_tb_tarefas_UsuarioId",
                table: "tb_tarefas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "tb_tarefas");

            migrationBuilder.RenameTable(
                name: "tb_tarefas",
                newName: "tb_tarefas");

            migrationBuilder.AddColumn<string>(
                name: "Responsavel",
                table: "tb_tarefas",
                type: "VARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tarefas",
                table: "tb_tarefas",
                column: "Cod");
        }
    }
}
