using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wt_lab6_6minimalapi_krivetskij.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "ExperienceYears", "FullName", "IsActive", "Office", "Specialty" },
                values: new object[,]
                {
                    { 1001, 12, "Иванов Сергей Петрович", true, 101, "Терапевт" },
                    { 1002, 8, "Петрова Анна Викторовна", true, 205, "Кардиолог" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Age", "AppointmentDate", "CreatedAt", "Diagnosis", "DoctorId", "FullName", "IsActive" },
                values: new object[,]
                {
                    { 1001, 35, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 14, 9, 0, 0, 0, DateTimeKind.Utc), "Плановый осмотр", 1001, "Сидоров Алексей Николаевич", true },
                    { 1002, 42, new DateTime(2026, 11, 21, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 14, 9, 0, 0, 0, DateTimeKind.Utc), "Контрольное обследование", 1002, "Козлова Мария Андреевна", true },
                    { 1003, 28, new DateTime(2026, 11, 22, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 14, 9, 0, 0, 0, DateTimeKind.Utc), "Консультация терапевта", 1001, "Смирнов Дмитрий Олегович", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1003);
        }
    }
}
