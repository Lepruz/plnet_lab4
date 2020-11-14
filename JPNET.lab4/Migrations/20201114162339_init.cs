using Microsoft.EntityFrameworkCore.Migrations;

namespace JPNET.lab4.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klienci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    klient_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klienci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Przedmioty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    KlientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przedmioty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Przedmioty_Klienci_KlientId",
                        column: x => x.KlientId,
                        principalTable: "Klienci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "zamowienia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlientId = table.Column<int>(type: "int", nullable: true),
                    Zrealizowane = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zamowienia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_zamowienia_Klienci_KlientId",
                        column: x => x.KlientId,
                        principalTable: "Klienci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "zamowienia_internetowe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zamowienia_internetowe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_zamowienia_internetowe_zamowienia_Id",
                        column: x => x.Id,
                        principalTable: "zamowienia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZamowieniaPrzedmiotow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrzedmiotId = table.Column<int>(type: "int", nullable: true),
                    ZamowienieId = table.Column<int>(type: "int", nullable: true),
                    Liczba = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZamowieniaPrzedmiotow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZamowieniaPrzedmiotow_Przedmioty_PrzedmiotId",
                        column: x => x.PrzedmiotId,
                        principalTable: "Przedmioty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZamowieniaPrzedmiotow_zamowienia_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "zamowienia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Przedmioty_KlientId",
                table: "Przedmioty",
                column: "KlientId");

            migrationBuilder.CreateIndex(
                name: "IX_zamowienia_KlientId",
                table: "zamowienia",
                column: "KlientId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamowieniaPrzedmiotow_PrzedmiotId",
                table: "ZamowieniaPrzedmiotow",
                column: "PrzedmiotId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamowieniaPrzedmiotow_ZamowienieId",
                table: "ZamowieniaPrzedmiotow",
                column: "ZamowienieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zamowienia_internetowe");

            migrationBuilder.DropTable(
                name: "ZamowieniaPrzedmiotow");

            migrationBuilder.DropTable(
                name: "Przedmioty");

            migrationBuilder.DropTable(
                name: "zamowienia");

            migrationBuilder.DropTable(
                name: "Klienci");
        }
    }
}
