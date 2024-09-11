using Microsoft.EntityFrameworkCore;
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
        optionsBuilder.UseSqlServer(Properties.Resources.RickMortyConnectString);

        base.OnConfiguring(optionsBuilder);

    }

}
