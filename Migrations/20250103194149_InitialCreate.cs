using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbbouClima.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NIF = table.Column<string>(type: "TEXT", nullable: true),
                    Correo = table.Column<string>(type: "TEXT", nullable: true),
                    Telefono = table.Column<string>(type: "TEXT", nullable: true),
                    Direccion = table.Column<string>(type: "TEXT", nullable: true),
                    Borrado = table.Column<bool>(type: "INTEGER", nullable: false),
                    FechaRegistro = table.Column<string>(type: "TEXT", nullable: true),
                    FechaModificacion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Presupuestos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    NºPresupuesto = table.Column<string>(type: "TEXT", nullable: true),
                    FechaPresupuesto = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion1 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion2 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion3 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion4 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion5 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion6 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion7 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion8 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion9 = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion10 = table.Column<string>(type: "TEXT", nullable: true),
                    Cantidad1 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad2 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad3 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad4 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad5 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad6 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad7 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad8 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad9 = table.Column<int>(type: "INTEGER", nullable: true),
                    Cantidad10 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio1 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio2 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio3 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio4 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio5 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio6 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio7 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio8 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio9 = table.Column<int>(type: "INTEGER", nullable: true),
                    Precio10 = table.Column<int>(type: "INTEGER", nullable: true),
                    ImporteTotal = table.Column<decimal>(type: "TEXT", nullable: true),
                    IncludeIVA = table.Column<bool>(type: "INTEGER", nullable: false),
                    Enviado = table.Column<bool>(type: "INTEGER", nullable: false),
                    Validez = table.Column<int>(type: "INTEGER", nullable: true),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presupuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_ClienteId",
                table: "Presupuestos",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Presupuestos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
