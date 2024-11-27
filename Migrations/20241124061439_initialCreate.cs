using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nutrition.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nutritionists",
                columns: table => new
                {
                    sMaChuyenGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sHoTenChuyenGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sSDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sDiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iTuoi = table.Column<int>(type: "int", nullable: false),
                    bGioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    sChuyenMon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nutritionists", x => x.sMaChuyenGia);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<bool>(type: "bit", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    height = table.Column<float>(type: "real", nullable: false),
                    weight = table.Column<float>(type: "real", nullable: false),
                    health_info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eating_habits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    meals_per_day = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    account_type = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    nutritionist_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.account_id);
                    table.ForeignKey(
                        name: "FK_accounts_nutritionists_nutritionist_id",
                        column: x => x.nutritionist_id,
                        principalTable: "nutritionists",
                        principalColumn: "sMaChuyenGia");
                    table.ForeignKey(
                        name: "FK_accounts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "nutrition_plans",
                columns: table => new
                {
                    sMaKeHoach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sMaChuyenGia = table.Column<int>(type: "int", nullable: false),
                    sMaNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dNgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sChiTietKeHoach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sGioiYThucPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iSoCalo = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nutrition_plans", x => x.sMaKeHoach);
                    table.ForeignKey(
                        name: "FK_nutrition_plans_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    nutritionist_id = table.Column<int>(type: "int", nullable: true),
                    create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    schedule_type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.schedule_id);
                    table.ForeignKey(
                        name: "FK_schedules_nutritionists_nutritionist_id",
                        column: x => x.nutritionist_id,
                        principalTable: "nutritionists",
                        principalColumn: "sMaChuyenGia");
                    table.ForeignKey(
                        name: "FK_schedules_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_progress",
                columns: table => new
                {
                    progress_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    height = table.Column<float>(type: "real", nullable: false),
                    weight = table.Column<float>(type: "real", nullable: false),
                    health_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    commment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_progress", x => x.progress_id);
                    table.ForeignKey(
                        name: "FK_user_progress_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_nutritionist_id",
                table: "accounts",
                column: "nutritionist_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_user_id",
                table: "accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_nutrition_plans_user_id",
                table: "nutrition_plans",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_nutritionist_id",
                table: "schedules",
                column: "nutritionist_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_user_id",
                table: "schedules",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_progress_user_id",
                table: "user_progress",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "nutrition_plans");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "user_progress");

            migrationBuilder.DropTable(
                name: "nutritionists");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
