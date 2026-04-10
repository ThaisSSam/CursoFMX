using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarUsuariosSeguranca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "usuarios");

            migrationBuilder.AddColumn<int>(
                name: "_versao_linha",
                table: "usuarios",
                type: "integer",
                rowVersion: true,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "precisa_trocar_senha",
                table: "usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_versao_linha",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "precisa_trocar_senha",
                table: "usuarios");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "usuarios",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }
    }
}
