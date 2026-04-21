using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarColunaVersao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "veiculos",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "veiculos");
        }
    }
}
