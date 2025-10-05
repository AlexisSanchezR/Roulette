using Microsoft.EntityFrameworkCore;
using Roulette.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
       
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RouletteModel> Roulette { get; set; }
        public DbSet<BetModel> Bet { get; set; }
        
    }
}
