using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Carbs",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Sodium",
                table: "FoodItems");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Meals",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MealItems",
                newName: "MealItemId");

            migrationBuilder.RenameColumn(
                name: "Sugar",
                table: "FoodItems",
                newName: "CaloriesPerServing");

            migrationBuilder.AddColumn<double>(
                name: "ServingMultiplier",
                table: "MealItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Protein",
                table: "FoodItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Fat",
                table: "FoodItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Carb",
                table: "FoodItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ServingSize",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DailyLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCalories = table.Column<int>(type: "int", nullable: false),
                    TotalCarb = table.Column<double>(type: "float", nullable: false),
                    TotalFat = table.Column<double>(type: "float", nullable: false),
                    TotalProtein = table.Column<double>(type: "float", nullable: false),
                    TotalWaterMl = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountMl = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyLogs");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "WaterLogs");

            migrationBuilder.DropColumn(
                name: "ServingMultiplier",
                table: "MealItems");

            migrationBuilder.DropColumn(
                name: "Carb",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "ServingSize",
                table: "FoodItems");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Meals",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "MealItemId",
                table: "MealItems",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CaloriesPerServing",
                table: "FoodItems",
                newName: "Sugar");

            migrationBuilder.AlterColumn<int>(
                name: "Protein",
                table: "FoodItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Fat",
                table: "FoodItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Calories",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Carbs",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sodium",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
