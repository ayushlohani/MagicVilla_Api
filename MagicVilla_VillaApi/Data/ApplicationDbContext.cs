using MagicVilla_VillaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaApi.Data
{
    public class ApplicationDbContext : DbContext //part of entity framework
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; } //where villas is table name in db 

        //for connect database write "Connectionstring" in application.json

        //Add DBConetxt int program.cs


    }
}
