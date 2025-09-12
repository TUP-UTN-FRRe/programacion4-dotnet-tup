using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using minimal_api_01.Entidades;

namespace minimal_api_01
{
    public class PedidosDbContext: DbContext
    {
        public DbSet<Pedido> Pedidos => Set<Pedido>();

        protected override void OnConfiguring(
                                        DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("PedidosDB");
        }

    }
}
