﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ticket2U.API.Data;

namespace Ticket2U.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Ticket2U.API.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Complement")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<int>("Num")
                        .HasColumnType("integer");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnType("integer");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<string>("TitleEvent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("EventId");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Image", b =>
                {
                    b.Property<int>("IdImage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Alt")
                        .HasColumnType("text");

                    b.Property<int?>("EventId")
                        .HasColumnType("integer");

                    b.Property<string>("Src")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdImage");

                    b.HasIndex("EventId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Login", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Pass")
                        .HasColumnType("text");

                    b.Property<string>("Perfil")
                        .HasColumnType("text");

                    b.HasKey("LoginId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Lot", b =>
                {
                    b.Property<int>("LotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateStart")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("EventId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("LotId");

                    b.HasIndex("EventId");

                    b.ToTable("Lots");
                });

            modelBuilder.Entity("Ticket2U.API.Models.LotCategory", b =>
                {
                    b.Property<int>("LotCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Desc")
                        .HasColumnType("text");

                    b.Property<int?>("LotId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PriceCategory")
                        .HasColumnType("numeric");

                    b.HasKey("LotCategoryId");

                    b.HasIndex("LotId");

                    b.ToTable("LotCategories");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Phone", b =>
                {
                    b.Property<int>("PhoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<int?>("Type")
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("PhoneId");

                    b.HasIndex("UserId");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("EventId")
                        .HasColumnType("integer");

                    b.Property<int?>("LotCategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("LotId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("TicketId");

                    b.HasIndex("EventId");

                    b.HasIndex("LotCategoryId");

                    b.HasIndex("LotId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Ticket2U.API.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Cpf")
                        .HasColumnType("text");

                    b.Property<decimal?>("Credit")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("DateBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ImageId")
                        .HasColumnType("integer");

                    b.Property<int?>("LoginId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Rg")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.HasIndex("LoginId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Address", b =>
                {
                    b.HasOne("Ticket2U.API.Models.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticket2U.API.Models.Event", b =>
                {
                    b.HasOne("Ticket2U.API.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("Ticket2U.API.Models.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Ticket2U.API.Models.Image", b =>
                {
                    b.HasOne("Ticket2U.API.Models.Event", "Event")
                        .WithMany("Images")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticket2U.API.Models.Lot", b =>
                {
                    b.HasOne("Ticket2U.API.Models.Event", "Event")
                        .WithMany("Lots")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticket2U.API.Models.LotCategory", b =>
                {
                    b.HasOne("Ticket2U.API.Models.Lot", "Lot")
                        .WithMany("LotCategories")
                        .HasForeignKey("LotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticket2U.API.Models.Phone", b =>
                {
                    b.HasOne("Ticket2U.API.Models.User", "User")
                        .WithMany("Phones")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticket2U.API.Models.Ticket", b =>
                {
                    b.HasOne("Ticket2U.API.Models.Event", "Event")
                        .WithMany("Tickets")
                        .HasForeignKey("EventId");

                    b.HasOne("Ticket2U.API.Models.LotCategory", "LotCategory")
                        .WithMany()
                        .HasForeignKey("LotCategoryId");

                    b.HasOne("Ticket2U.API.Models.Lot", "Lot")
                        .WithMany()
                        .HasForeignKey("LotId");

                    b.HasOne("Ticket2U.API.Models.User", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Ticket2U.API.Models.User", b =>
                {
                    b.HasOne("Ticket2U.API.Models.Image", "Image")
                        .WithOne("User")
                        .HasForeignKey("Ticket2U.API.Models.User", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ticket2U.API.Models.Login", "Login")
                        .WithOne("User")
                        .HasForeignKey("Ticket2U.API.Models.User", "LoginId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
