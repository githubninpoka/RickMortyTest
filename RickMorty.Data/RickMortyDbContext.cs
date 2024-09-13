using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RickMorty.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RickMorty.Data;

public class RickMortyDbContext : DbContext
{
    public DbSet<Character> Characters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(Properties.Resources.RickMortyConnectString)
            .LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();


    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
