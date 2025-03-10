﻿// <auto-generated />
using System;
using DocumentVerification.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DocumentVerification.Migrations
{
    [DbContext(typeof(MyAppContext))]
    [Migration("20250307214433_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DocumentVerification.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("VerificationCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("DocumentVerification.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DocumentVerification.Models.VerificationLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<int?>("DocumentId1")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VerifiedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("DocumentId1");

                    b.HasIndex("UserId");

                    b.HasIndex("VerifiedBy");

                    b.ToTable("VerificationLogs");
                });

            modelBuilder.Entity("DocumentVerification.Models.Document", b =>
                {
                    b.HasOne("DocumentVerification.Models.User", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DocumentVerification.Models.VerificationLog", b =>
                {
                    b.HasOne("DocumentVerification.Models.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DocumentVerification.Models.Document", null)
                        .WithMany("VerificationLogs")
                        .HasForeignKey("DocumentId1");

                    b.HasOne("DocumentVerification.Models.User", null)
                        .WithMany("VerificationLogs")
                        .HasForeignKey("UserId");

                    b.HasOne("DocumentVerification.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("VerifiedBy")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DocumentVerification.Models.Document", b =>
                {
                    b.Navigation("VerificationLogs");
                });

            modelBuilder.Entity("DocumentVerification.Models.User", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("VerificationLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
