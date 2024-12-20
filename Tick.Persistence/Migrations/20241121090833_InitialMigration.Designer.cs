﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Tick.Persistence;

#nullable disable

namespace Tick.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241121090833_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("API_TEMPLATE")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 30);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("ID");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("CONCURRENCY_STAMP");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("NAME");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("NORMALIZED_NAME");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("ROLE", "API_TEMPLATE");

                    b.HasData(
                        new
                        {
                            Id = "510057bf-a91a-4398-83e7-58a558ae5edd",
                            ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a23",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "76cdb59e-48da-4651-b300-a20e9c08a750",
                            ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a56",
                            Name = "Ticker",
                            NormalizedName = "TICKER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("CLAIM_TYPE");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("CLAIM_VALUE");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ROLE_ID");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("ROLE_CLAIMS", "API_TEMPLATE");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("CLAIM_TYPE");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("CLAIM_VALUE");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("USER_ID");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("USER_CLAIMS", "API_TEMPLATE");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("LOGIN_PROVIDER");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("PROVIDER_KEY");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("PROVIDER_DISPLAY_NAME");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("USER_ID");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("USER_LOGINS", "API_TEMPLATE");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("USER_ID");

                    b.Property<string>("RoleId")
                        .HasColumnType("text")
                        .HasColumnName("ROLE_ID");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("USER_ROLES", "API_TEMPLATE");

                    b.HasData(
                        new
                        {
                            UserId = "9a6a928b-0e11-4d5d-8a29-b8f04445e72",
                            RoleId = "76cdb59e-48da-4651-b300-a20e9c08a750"
                        },
                        new
                        {
                            UserId = "7cc5cd62-6240-44e5-b44f-bff0ae73342",
                            RoleId = "510057bf-a91a-4398-83e7-58a558ae5edd"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("USER_ID");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("LOGIN_PROVIDER");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("NAME");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("VALUE");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("USER_TOKENS", "API_TEMPLATE");
                });

            modelBuilder.Entity("Tick.Domain.Entities.BasicUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("ID");

                    b.Property<string>("ApiKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("API_KEY");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_AT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("CREATED_BY");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("NAME");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("STATUS");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATED_AT");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("UPDATED_BY");

                    b.HasKey("Id");

                    b.HasIndex("ApiKey")
                        .IsUnique();

                    b.ToTable("BASIC_USER", "API_TEMPLATE");

                    b.HasData(
                        new
                        {
                            Id = "BSR_482242804225Y91361313",
                            ApiKey = "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWMPBYMFOY",
                            CreatedAt = new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = "System",
                            Name = "Access Bank Key 1",
                            Status = 1,
                            UpdatedAt = new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc),
                            UpdatedBy = "System"
                        },
                        new
                        {
                            Id = "BSR_482242804225Y91908738",
                            ApiKey = "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWKDHWIWW",
                            CreatedAt = new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = "System",
                            Name = "AFF Key 1",
                            Status = 1,
                            UpdatedAt = new DateTime(2023, 10, 19, 23, 0, 0, 0, DateTimeKind.Utc),
                            UpdatedBy = "System"
                        });
                });

            modelBuilder.Entity("Tick.Domain.Entities.Task", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("ID");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_AT");

                    b.Property<string>("Details")
                        .HasColumnType("text")
                        .HasColumnName("DETAILS");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean")
                        .HasColumnName("IS_COMPLETED");

                    b.Property<bool>("IsImportant")
                        .HasColumnType("boolean")
                        .HasColumnName("IS_IMPORTANT");

                    b.Property<string>("TickerId")
                        .HasColumnType("text")
                        .HasColumnName("TICKER_ID");

                    b.HasKey("Id");

                    b.HasIndex("TickerId");

                    b.ToTable("TASK", "API_TEMPLATE");
                });

            modelBuilder.Entity("Tick.Domain.Entities.Ticker", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("ID");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("ACCESS_FAILED_COUNT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("CONCURRENCY_STAMP");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATED_AT");

                    b.Property<int?>("DefaultRole")
                        .HasColumnType("integer")
                        .HasColumnName("DEFAULT_ROLE");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("EMAIL");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("EMAIL_CONFIRMED");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("FIRST_NAME");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("IS_ACTIVE");

                    b.Property<bool>("IsLoggedIn")
                        .HasColumnType("boolean")
                        .HasColumnName("IS_LOGGED_IN");

                    b.Property<DateTime>("LastLoginTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("LAST_LOGIN_TIME");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("LAST_NAME");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("LOCKOUT_ENABLED");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("LOCKOUT_END");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("NORMALIZED_EMAIL");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("NORMALIZED_USER_NAME");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("PASSWORD_HASH");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("PHONE_NUMBER");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("PHONE_NUMBER_CONFIRMED");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("SECURITY_STAMP");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("TWO_FACTOR_ENABLED");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("USER_NAME");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("TICKER", "API_TEMPLATE");

                    b.HasData(
                        new
                        {
                            Id = "7cc5cd62-6240-44e5-b44f-bff0ae73342",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e45",
                            CreatedAt = new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc),
                            DefaultRole = 1,
                            Email = "chinwe.ibegbu@gmail.com",
                            EmailConfirmed = true,
                            FirstName = "Chinwe",
                            IsActive = true,
                            IsLoggedIn = false,
                            LastLoginTime = new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc),
                            LastName = "Ibegbu",
                            LockoutEnabled = false,
                            NormalizedEmail = "CHINWE.IBEGBU@GMAIL.COM",
                            NormalizedUserName = "IBEGBUC",
                            PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e93",
                            TwoFactorEnabled = false,
                            UpdatedAt = new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc),
                            UserName = "ibegbuc"
                        },
                        new
                        {
                            Id = "9a6a928b-0e11-4d5d-8a29-b8f04445e72",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e98",
                            CreatedAt = new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc),
                            DefaultRole = 2,
                            Email = "dabyaibegbu@gmail.com",
                            EmailConfirmed = true,
                            FirstName = "Daby",
                            IsActive = true,
                            IsLoggedIn = false,
                            LastLoginTime = new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc),
                            LastName = "Ibegbu",
                            LockoutEnabled = false,
                            NormalizedEmail = "DABYAIBEGBU@GMAIL.COM",
                            NormalizedUserName = "DABYIBEGBU",
                            PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e37",
                            TwoFactorEnabled = false,
                            UpdatedAt = new DateTime(2024, 8, 5, 23, 0, 0, 0, DateTimeKind.Utc),
                            UserName = "dabyibegbu"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Tick.Domain.Entities.Ticker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Tick.Domain.Entities.Ticker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tick.Domain.Entities.Ticker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Tick.Domain.Entities.Ticker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tick.Domain.Entities.Task", b =>
                {
                    b.HasOne("Tick.Domain.Entities.Ticker", "Ticker")
                        .WithMany("Tasks")
                        .HasForeignKey("TickerId");

                    b.Navigation("Ticker");
                });

            modelBuilder.Entity("Tick.Domain.Entities.Ticker", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
