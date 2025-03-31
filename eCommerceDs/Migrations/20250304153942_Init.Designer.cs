﻿// <auto-generated />
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
    [Migration("20250304153942_Init")]
    partial class Init
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

            modelBuilder.Entity("Group", b =>
                {
                    b.HasOne("eCommerceDs.Models.MusicGenre", "MusicGenre")
                        .WithMany("Groups")
                        .HasForeignKey("MusicGenreId")
                        .IsRequired()
                        .HasConstraintName("FK_Groups_MusicalGenres");

                    b.Navigation("MusicGenre");
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

            modelBuilder.Entity("eCommerceDs.Models.MusicGenre", b =>
                {
                    b.Navigation("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
