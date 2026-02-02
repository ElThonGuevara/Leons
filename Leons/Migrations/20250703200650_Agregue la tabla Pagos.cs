using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leons.Migrations
{
    /// <inheritdoc />
    public partial class AgreguelatablaPagos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    idPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    montoTotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    fechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    metodoPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.idPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Pedidos_idPedido",
                        column: x => x.idPedido,
                        principalTable: "Pedidos",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_idPedido",
                table: "Pagos",
                column: "idPedido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagos");
        }
    }
}
