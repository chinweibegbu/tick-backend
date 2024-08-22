using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tick.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                schema: "API_TEMPLATE",
                table: "TICKER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "API_TEMPLATE",
                table: "TICKER",
                keyColumn: "ID",
                keyValue: "7cc5cd62-6240-44e5-b44f-bff0ae73342",
                column: "ProfileImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                schema: "API_TEMPLATE",
                table: "TICKER",
                keyColumn: "ID",
                keyValue: "9a6a928b-0e11-4d5d-8a29-b8f04445e72",
                column: "ProfileImageUrl",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                schema: "API_TEMPLATE",
                table: "TICKER");
        }
    }
}
