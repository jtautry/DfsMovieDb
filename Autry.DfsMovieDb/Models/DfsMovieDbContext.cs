using Autry.DfsMovieDb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Autry.DfsMovieDb
{
    public class DfsMovieDbContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActor { get; set; }

        public DfsMovieDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.MovieId, ma.ActorId });

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            //actor
            var john = new Actor() { ActorId = 10, FirstName = "John", LastName = "Doe", Birthdate = new DateTime(1976, 09, 02) };
            var keanu = new Actor() { ActorId = 11, FirstName = "Keanu", LastName = "Reeves", Birthdate = new DateTime(1964, 09, 02) };
            var laurence = new Actor() { ActorId = 12, FirstName = "Laurence", LastName = "Fishburne", Birthdate = new DateTime(1961, 07, 30) };

            modelBuilder.Entity<Actor>().HasData(new List<Actor> { john, keanu, laurence });

            //movie
            var matrix = new Movie() { MovieId = 20, Title = "The Matrix", Year = 1999, Genre = "Sci-Fi" };
            var matrix2 = new Movie() { MovieId = 21, Title = "The Matrix Reloaded", Year = 2003, Genre = "Sci-Fi" };
            var bill = new Movie() { MovieId = 22, Title = "Bill & Ted's Excellent Adventure", Year = 1999, Genre = "Comedy" };

            modelBuilder.Entity<Movie>().HasData(new List<Movie> { matrix, matrix2, bill });

            //movieactor
            modelBuilder.Entity<MovieActor>().HasData(new List<MovieActor>() 
            { 
                new MovieActor(){ MovieId = matrix.MovieId, ActorId = keanu.ActorId },
                new MovieActor(){ MovieId = matrix.MovieId, ActorId = laurence.ActorId },
                new MovieActor(){ MovieId = matrix2.MovieId, ActorId = keanu.ActorId },
                new MovieActor(){ MovieId = matrix2.MovieId, ActorId = laurence.ActorId },
                new MovieActor(){ MovieId = bill.MovieId, ActorId = keanu.ActorId },
            });
        }
    }
}
