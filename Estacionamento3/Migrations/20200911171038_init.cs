using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Estacionamento3.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    placa = table.Column<string>(nullable: false),
                    marca = table.Column<string>(nullable: true),
                    modelo = table.Column<string>(nullable: true),
                    cor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.placa);
                });

            migrationBuilder.CreateTable(
                name: "Patios",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    dataInicio = table.Column<DateTime>(nullable: false),
                    dataFim = table.Column<DateTime>(nullable: true),
                    tempo = table.Column<double>(nullable: true),
                    valor = table.Column<float>(nullable: true),
                    veiculoPlaca = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patios", x => x.id);
                    table.ForeignKey(
                        name: "FK_Patios_Veiculos_veiculoPlaca",
                        column: x => x.veiculoPlaca,
                        principalTable: "Veiculos",
                        principalColumn: "placa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patios_veiculoPlaca",
                table: "Patios",
                column: "veiculoPlaca");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patios");

            migrationBuilder.DropTable(
                name: "Veiculos");
        }
    }
}
