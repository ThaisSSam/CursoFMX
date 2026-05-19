using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Treinamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sprint01_03_DescricaoCurta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                schema: "treinamento",
                table: "tb_usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "bloqueado_ate",
                schema: "treinamento",
                table: "tb_usuarios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "treinamento",
                table: "tb_usuarios",
                type: "varchar(100)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "senha_hash",
                schema: "treinamento",
                table: "tb_usuarios",
                type: "varchar(100)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "tentativas_login_invalidas",
                schema: "treinamento",
                table: "tb_usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativo",
                schema: "treinamento",
                table: "tb_usuarios");

            migrationBuilder.DropColumn(
                name: "bloqueado_ate",
                schema: "treinamento",
                table: "tb_usuarios");

            migrationBuilder.DropColumn(
                name: "email",
                schema: "treinamento",
                table: "tb_usuarios");

            migrationBuilder.DropColumn(
                name: "senha_hash",
                schema: "treinamento",
                table: "tb_usuarios");

            migrationBuilder.DropColumn(
                name: "tentativas_login_invalidas",
                schema: "treinamento",
                table: "tb_usuarios");
        }
    }
}
