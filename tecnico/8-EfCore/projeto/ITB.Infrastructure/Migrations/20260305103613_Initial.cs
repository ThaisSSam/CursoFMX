using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "marcas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_marcas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "modelos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    marca_id = table.Column<int>(type: "int", nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_modelos", x => x.id);
                    table.ForeignKey(
                        name: "fk_modelos_marcas_marca_id",
                        column: x => x.marca_id,
                        principalTable: "marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "veiculos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    placa = table.Column<string>(type: "varchar(100)", nullable: false),
                    ano = table.Column<int>(type: "int", nullable: false),
                    marca_id = table.Column<int>(type: "integer", nullable: false),
                    modelo_id = table.Column<int>(type: "int", nullable: false),
                    ativo = table.Column<bool>(type: "bool", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_veiculos", x => x.id);
                    table.ForeignKey(
                        name: "fk_veiculos_marcas_marca_id",
                        column: x => x.marca_id,
                        principalTable: "marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_veiculos_modelos_modelo_id",
                        column: x => x.modelo_id,
                        principalTable: "modelos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_modelos_marca_id",
                table: "modelos",
                column: "marca_id");

            migrationBuilder.CreateIndex(
                name: "ix_veiculos_marca_id",
                table: "veiculos",
                column: "marca_id");

            migrationBuilder.CreateIndex(
                name: "ix_veiculos_modelo_id",
                table: "veiculos",
                column: "modelo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "veiculos");

            migrationBuilder.DropTable(
                name: "modelos");

            migrationBuilder.DropTable(
                name: "marcas");
        }
    }
}
