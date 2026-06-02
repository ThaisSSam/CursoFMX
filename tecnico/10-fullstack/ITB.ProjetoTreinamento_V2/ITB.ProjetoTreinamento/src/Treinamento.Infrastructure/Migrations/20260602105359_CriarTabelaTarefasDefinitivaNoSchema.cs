using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Treinamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaTarefaDefinitivaNoSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_tarefas",
                schema: "treinamento",
                columns: table => new
                {
                    Cod = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Situacao = table.Column<int>(type: "INT", nullable: false),
                    Prioridade = table.Column<int>(type: "INT", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tarefas", x => x.Cod);
                    table.ForeignKey(
                        name: "FK_tb_tarefas_tb_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "treinamento",
                        principalTable: "tb_usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_tarefas_UsuarioId",
                schema: "treinamento",
                table: "tb_tarefas",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_tarefas",
                schema: "treinamento");
        }
    }
}
