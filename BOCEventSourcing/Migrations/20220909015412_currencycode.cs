using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOC.Core.Migrations
{
    public partial class currencycode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_CurrencyCode_CurrencyId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "CurrencyCode");

            migrationBuilder.DropIndex(
                name: "IX_Events_CurrencyId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "Events",
                newName: "Currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Events",
                newName: "CurrencyId");

            migrationBuilder.CreateTable(
                name: "CurrencyCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyCode", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CurrencyId",
                table: "Events",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_CurrencyCode_CurrencyId",
                table: "Events",
                column: "CurrencyId",
                principalTable: "CurrencyCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
