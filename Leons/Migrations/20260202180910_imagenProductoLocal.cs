using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leons.Migrations
{
    /// <inheritdoc />
    public partial class imagenProductoLocal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imagenLocal",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagenLocal",
                table: "Productos");
        }
    }
}
