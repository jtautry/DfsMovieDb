﻿// <auto-generated />
using System;
using Autry.DfsMovieDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Autry.DfsMovieDb.Migrations
{
    [DbContext(typeof(DfsMovieDbContext))]
    partial class DfsMovieDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Autry.DfsMovieDb.Models.Actor", b =>
                {
                    b.Property<int>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActorId");

                    b.ToTable("Actors");

                    b.HasData(
                        new
                        {
                            ActorId = 10,
                            Birthdate = new DateTime(1976, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "John",
                            LastName = "Doe"
                        },
                        new
                        {
                            ActorId = 11,
                            Birthdate = new DateTime(1964, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Keanu",
                            LastName = "Reeves"
                        },
                        new
                        {
                            ActorId = 12,
                            Birthdate = new DateTime(1961, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Laurence",
                            LastName = "Fishburne"
                        });
                });

            modelBuilder.Entity("Autry.DfsMovieDb.Models.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("MovieId");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            MovieId = 20,
                            Genre = "Sci-Fi",
                            Title = "The Matrix",
                            Year = 1999
                        },
                        new
                        {
                            MovieId = 21,
                            Genre = "Sci-Fi",
                            Title = "The Matrix Reloaded",
                            Year = 2003
                        },
                        new
                        {
                            MovieId = 22,
                            Genre = "Comedy",
                            Title = "Bill & Ted's Excellent Adventure",
                            Year = 1999
                        });
                });

            modelBuilder.Entity("Autry.DfsMovieDb.Models.MovieActor", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("ActorId")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "ActorId");

                    b.HasIndex("ActorId");

                    b.ToTable("MovieActor");

                    b.HasData(
                        new
                        {
                            MovieId = 20,
                            ActorId = 11
                        },
                        new
                        {
                            MovieId = 20,
                            ActorId = 12
                        },
                        new
                        {
                            MovieId = 21,
                            ActorId = 11
                        },
                        new
                        {
                            MovieId = 21,
                            ActorId = 12
                        },
                        new
                        {
                            MovieId = 22,
                            ActorId = 11
                        });
                });

            modelBuilder.Entity("Autry.DfsMovieDb.Models.MovieActor", b =>
                {
                    b.HasOne("Autry.DfsMovieDb.Models.Actor", "Actor")
                        .WithMany("MovieActors")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Autry.DfsMovieDb.Models.Movie", "Movie")
                        .WithMany("MovieActors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}