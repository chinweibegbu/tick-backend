using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tick.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "API_TEMPLATE");

            migrationBuilder.CreateTable(
                name: "BASIC_USER",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    API_KEY = table.Column<string>(type: "text", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATED_BY = table.Column<string>(type: "text", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "text", nullable: true),
                    UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BASIC_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLE",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NORMALIZED_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TICKER",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    FIRST_NAME = table.Column<string>(type: "text", nullable: true),
                    LAST_NAME = table.Column<string>(type: "text", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "boolean", nullable: false),
                    IS_LOGGED_IN = table.Column<bool>(type: "boolean", nullable: false),
                    DEFAULT_ROLE = table.Column<int>(type: "integer", nullable: true),
                    LAST_LOGIN_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "text", nullable: true),
                    USER_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NORMALIZED_USER_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EMAIL = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NORMALIZED_EMAIL = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EMAIL_CONFIRMED = table.Column<bool>(type: "boolean", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "text", nullable: true),
                    SECURITY_STAMP = table.Column<string>(type: "text", nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "text", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "text", nullable: true),
                    PHONE_NUMBER_CONFIRMED = table.Column<bool>(type: "boolean", nullable: false),
                    TWO_FACTOR_ENABLED = table.Column<bool>(type: "boolean", nullable: false),
                    LOCKOUT_END = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LOCKOUT_ENABLED = table.Column<bool>(type: "boolean", nullable: false),
                    ACCESS_FAILED_COUNT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TICKER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLE_CLAIMS",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ROLE_ID = table.Column<string>(type: "text", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "text", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_CLAIMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROLE_CLAIMS_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalSchema: "API_TEMPLATE",
                        principalTable: "ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TASK",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    DETAILS = table.Column<string>(type: "text", nullable: true),
                    IS_IMPORTANT = table.Column<bool>(type: "boolean", nullable: false),
                    IS_COMPLETED = table.Column<bool>(type: "boolean", nullable: false),
                    TICKER_ID = table.Column<string>(type: "text", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASK", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TASK_TICKER_TICKER_ID",
                        column: x => x.TICKER_ID,
                        principalSchema: "API_TEMPLATE",
                        principalTable: "TICKER",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "USER_CLAIMS",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<string>(type: "text", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "text", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_CLAIMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USER_CLAIMS_TICKER_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "API_TEMPLATE",
                        principalTable: "TICKER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_LOGINS",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    LOGIN_PROVIDER = table.Column<string>(type: "text", nullable: false),
                    PROVIDER_KEY = table.Column<string>(type: "text", nullable: false),
                    PROVIDER_DISPLAY_NAME = table.Column<string>(type: "text", nullable: true),
                    USER_ID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_LOGINS", x => new { x.LOGIN_PROVIDER, x.PROVIDER_KEY });
                    table.ForeignKey(
                        name: "FK_USER_LOGINS_TICKER_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "API_TEMPLATE",
                        principalTable: "TICKER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLES",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "text", nullable: false),
                    ROLE_ID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLES", x => new { x.USER_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_USER_ROLES_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalSchema: "API_TEMPLATE",
                        principalTable: "ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USER_ROLES_TICKER_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "API_TEMPLATE",
                        principalTable: "TICKER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_TOKENS",
                schema: "API_TEMPLATE",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "text", nullable: false),
                    LOGIN_PROVIDER = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    VALUE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_TOKENS", x => new { x.USER_ID, x.LOGIN_PROVIDER, x.NAME });
                    table.ForeignKey(
                        name: "FK_USER_TOKENS_TICKER_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "API_TEMPLATE",
                        principalTable: "TICKER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "API_TEMPLATE",
                table: "BASIC_USER",
                columns: new[] { "ID", "API_KEY", "CREATED_AT", "CREATED_BY", "NAME", "STATUS", "UPDATED_AT", "UPDATED_BY" },
                values: new object[,]
                {
                    { "BSR_482242804225Y91361313", "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWMPBYMFOY", new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc), "System", "Access Bank Key 1", 1, new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc), "System" },
                    { "BSR_482242804225Y91908738", "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWKDHWIWW", new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc), "System", "AFF Key 1", 1, new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc), "System" }
                });

            migrationBuilder.InsertData(
                schema: "API_TEMPLATE",
                table: "ROLE",
                columns: new[] { "ID", "CONCURRENCY_STAMP", "NAME", "NORMALIZED_NAME" },
                values: new object[,]
                {
                    { "510057bf-a91a-4398-83e7-58a558ae5edd", "71f781f7-e957-469b-96df-9f2035147a23", "Administrator", "ADMINISTRATOR" },
                    { "76cdb59e-48da-4651-b300-a20e9c08a750", "71f781f7-e957-469b-96df-9f2035147a56", "Ticker", "TICKER" }
                });

            migrationBuilder.InsertData(
                schema: "API_TEMPLATE",
                table: "TICKER",
                columns: new[] { "ID", "ACCESS_FAILED_COUNT", "CONCURRENCY_STAMP", "CREATED_AT", "DEFAULT_ROLE", "EMAIL", "EMAIL_CONFIRMED", "FIRST_NAME", "IS_ACTIVE", "IS_LOGGED_IN", "LAST_LOGIN_TIME", "LAST_NAME", "LOCKOUT_ENABLED", "LOCKOUT_END", "NORMALIZED_EMAIL", "NORMALIZED_USER_NAME", "PASSWORD_HASH", "PHONE_NUMBER", "PHONE_NUMBER_CONFIRMED", "ProfileImageUrl", "SECURITY_STAMP", "TWO_FACTOR_ENABLED", "UpdatedAt", "USER_NAME" },
                values: new object[,]
                {
                    { "7cc5cd62-6240-44e5-b44f-bff0ae73342", 0, "71f781f7-e957-469b-96df-9f2035147e45", new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc), 1, "chinwe.ibegbu@gmail.com", true, "Chinwe", true, false, new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc), "Ibegbu", false, null, "CHINWE.IBEGBU@GMAIL.COM", "IBEGBUC", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, true, null, "71f781f7-e957-469b-96df-9f2035147e93", false, new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc), "ibegbuc" },
                    { "9a6a928b-0e11-4d5d-8a29-b8f04445e72", 0, "71f781f7-e957-469b-96df-9f2035147e98", new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc), 2, "dabyaibegbu@gmail.com", true, "Daby", true, false, new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc), "Ibegbu", false, null, "DABYAIBEGBU@GMAIL.COM", "DABYIBEGBU", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, true, null, "71f781f7-e957-469b-96df-9f2035147e37", false, new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc), "dabyibegbu" }
                });

            migrationBuilder.InsertData(
                schema: "API_TEMPLATE",
                table: "USER_ROLES",
                columns: new[] { "ROLE_ID", "USER_ID" },
                values: new object[,]
                {
                    { "510057bf-a91a-4398-83e7-58a558ae5edd", "7cc5cd62-6240-44e5-b44f-bff0ae73342" },
                    { "76cdb59e-48da-4651-b300-a20e9c08a750", "9a6a928b-0e11-4d5d-8a29-b8f04445e72" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BASIC_USER_API_KEY",
                schema: "API_TEMPLATE",
                table: "BASIC_USER",
                column: "API_KEY",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "API_TEMPLATE",
                table: "ROLE",
                column: "NORMALIZED_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROLE_CLAIMS_ROLE_ID",
                schema: "API_TEMPLATE",
                table: "ROLE_CLAIMS",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TASK_TICKER_ID",
                schema: "API_TEMPLATE",
                table: "TASK",
                column: "TICKER_ID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "API_TEMPLATE",
                table: "TICKER",
                column: "NORMALIZED_EMAIL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "API_TEMPLATE",
                table: "TICKER",
                column: "NORMALIZED_USER_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_CLAIMS_USER_ID",
                schema: "API_TEMPLATE",
                table: "USER_CLAIMS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_LOGINS_USER_ID",
                schema: "API_TEMPLATE",
                table: "USER_LOGINS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLES_ROLE_ID",
                schema: "API_TEMPLATE",
                table: "USER_ROLES",
                column: "ROLE_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BASIC_USER",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "ROLE_CLAIMS",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "TASK",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "USER_CLAIMS",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "USER_LOGINS",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "USER_ROLES",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "USER_TOKENS",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "ROLE",
                schema: "API_TEMPLATE");

            migrationBuilder.DropTable(
                name: "TICKER",
                schema: "API_TEMPLATE");
        }
    }
}
