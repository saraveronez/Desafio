using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace desafio_corep.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    Identidade = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Divida",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClienteId = table.Column<Guid>(nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Divida_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParcelaDivida",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DividaId = table.Column<Guid>(nullable: false),
                    DataVencimento = table.Column<DateTime>(nullable: false),
                    QuantidadeParcelas = table.Column<int>(nullable: false),
                    ValorOriginal = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DiasAtraso = table.Column<int>(nullable: false),
                    ValorJuros = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ValorFinal = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelaDivida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParcelaDivida_Divida_DividaId",
                        column: x => x.DividaId,
                        principalTable: "Divida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Divida_ClienteId",
                table: "Divida",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelaDivida_DividaId",
                table: "ParcelaDivida",
                column: "DividaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParcelaDivida");

            migrationBuilder.DropTable(
                name: "Divida");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
