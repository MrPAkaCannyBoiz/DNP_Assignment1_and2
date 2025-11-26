using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfcRepositories;

public class AppContext : DbContext // extend DbContext from Entity Framework Core
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // configure the database to use SQLite with a file named app.db
        optionsBuilder.UseSqlite("Data Source=C:\\VIA_uni_stuffs\\Semester3_hetero_system\\DNP1\\Assignment\\FirstAssignmentProject\\DNP_Assignment1_and2\\EfcRepositories\\app.db");
    }
}
