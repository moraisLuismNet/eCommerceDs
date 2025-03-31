﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eCommerceDs.Models;

#nullable disable

namespace eCommerceDs.Migrations
{
    [DbContext(typeof(eCommerceDsContext))]
    [Migration("20250313080352_CART")]
    partial class CART
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Group", b =>
                {
                    b.Property<int>("IdGroup")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdGroup"));

                    b.Property<string>("ImageGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MusicGenreId")
                        .HasColumnType("int");

                    b.Property<string>("NameGroup")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdGroup")
                        .HasName("PK__Groups__32DFFDB3F74504DB");

                    b.HasIndex("MusicGenreId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("eCommerceDs.Models.Cart", b =>
                {
                    b.Property<int>("IdCart")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCart"));

                    b.Property<bool>("ClosedCart")
                        .HasColumnType("bit");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdCart");

                    b.HasIndex("UserEmail")
                        .IsUnique();

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("eCommerceDs.Models.CartDetail", b =>
                {
                    b.Property<int>("IdCartDetail")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCartDetail"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RecordId")
                        .HasColumnType("int");

                    b.HasKey("IdCartDetail");

                    b.HasIndex("CartId");

                    b.HasIndex("RecordId");

                    b.ToTable("CartDetails");
                });

            modelBuilder.Entity("eCommerceDs.Models.MusicGenre", b =>
                {
                    b.Property<int>("IdMusicGenre")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMusicGenre"));

                    b.Property<string>("NameMusicGenre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdMusicGenre")
                        .HasName("PK__MusicalG__C2A4358176EF3AF4");

                    b.ToTable("MusicGenres");
                });

            modelBuilder.Entity("eCommerceDs.Models.Order", b =>
                {
                    b.Property<int>("IdOrder")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdOrder"));

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Total")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdOrder");

                    b.HasIndex("CartId");

                    b.HasIndex("UserEmail");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("eCommerceDs.Models.Record", b =>
                {
                    b.Property<int>("IdRecord")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRecord"));

                    b.Property<bool>("Discontinued")
                        .HasColumnType("bit");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("ImageRecord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<string>("TitleRecord")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("YearOfPublication")
                        .HasColumnType("int");

                    b.HasKey("IdRecord")
                        .HasName("PK__Records__356CCF9A247285E1");

                    b.HasIndex("GroupId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("eCommerceDs.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Email")
                        .HasName("PK__Users__A9D10535B2F51717");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Group", b =>
                {
                    b.HasOne("eCommerceDs.Models.MusicGenre", "MusicGenre")
                        .WithMany("Groups")
                        .HasForeignKey("MusicGenreId")
                        .IsRequired()
                        .HasConstraintName("FK_Groups_MusicalGenres");

                    b.Navigation("MusicGenre");
                });

            modelBuilder.Entity("eCommerceDs.Models.Cart", b =>
                {
                    b.HasOne("eCommerceDs.Models.User", "User")
                        .WithOne("Cart")
                        .HasForeignKey("eCommerceDs.Models.Cart", "UserEmail")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("eCommerceDs.Models.CartDetail", b =>
                {
                    b.HasOne("eCommerceDs.Models.Cart", "Cart")
                        .WithMany("CartDetails")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eCommerceDs.Models.Record", "Record")
                        .WithMany()
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("eCommerceDs.Models.Order", b =>
                {
                    b.HasOne("eCommerceDs.Models.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("eCommerceDs.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("User");
                });

            modelBuilder.Entity("eCommerceDs.Models.Record", b =>
                {
                    b.HasOne("Group", "Group")
                        .WithMany("Records")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK_Records_Groups");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Group", b =>
                {
                    b.Navigation("Records");
                });

            modelBuilder.Entity("eCommerceDs.Models.Cart", b =>
                {
                    b.Navigation("CartDetails");
                });

            modelBuilder.Entity("eCommerceDs.Models.MusicGenre", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("eCommerceDs.Models.User", b =>
                {
                    b.Navigation("Cart")
                        .IsRequired();

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
