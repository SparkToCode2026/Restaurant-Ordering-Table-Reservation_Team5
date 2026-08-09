using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team5.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MenuCategories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MenuCategories");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "UserEmail");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tables",
                newName: "TableId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MenuItems",
                newName: "MenuItemName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MenuItems",
                newName: "MenuItemDescription");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Ingredients",
                newName: "IngredientName");

            migrationBuilder.AddColumn<string>(
                name: "MenuCategoryDescription",
                table: "MenuCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MenuCategoryName",
                table: "MenuCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuCategoryDescription",
                table: "MenuCategories");

            migrationBuilder.DropColumn(
                name: "MenuCategoryName",
                table: "MenuCategories");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "TableId",
                table: "Tables",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MenuItemName",
                table: "MenuItems",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "MenuItemDescription",
                table: "MenuItems",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "IngredientName",
                table: "Ingredients",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MenuCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MenuCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
