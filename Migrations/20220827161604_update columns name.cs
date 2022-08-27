using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth_jwt_api.Migrations
{
    public partial class updatecolumnsname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishYear",
                table: "Books",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "BookName",
                table: "Books",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Books",
                newName: "PublishYear");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Books",
                newName: "BookName");
        }
    }
}
