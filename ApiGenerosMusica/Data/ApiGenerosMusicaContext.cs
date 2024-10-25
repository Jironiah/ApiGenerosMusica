using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiGenerosMusica.Models;

namespace ApiGenerosMusica.Data
{
    public class ApiGenerosMusicaContext : DbContext
    {
        public ApiGenerosMusicaContext (DbContextOptions<ApiGenerosMusicaContext> options)
            : base(options)
        {
        }

        public DbSet<ApiGenerosMusica.Models.Generos> Generos { get; set; } = default!;
    }
}
