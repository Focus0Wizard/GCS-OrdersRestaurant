using Microsoft.EntityFrameworkCore;
using Restaurant.Context;
using Restaurant.Entities;
using Restaurant.Repositories;

namespace RestaurantQATest.BDD.Support
{
    public class IntegracionTestContext
    {
        public MyDbContext DbContext { get; set; }
        public GenericRepository<Cliente> ClienteRepository { get; set; }
        public GenericRepository<Pedido> PedidoRepository { get; set; }
        public GenericRepository<Producto> ProductoRepository { get; set; }
        public GenericRepository<Repartidore> RepartidorRepository { get; set; }
        public GenericRepository<Categoria> CategoriaRepository { get; set; }
        public GenericRepository<Usuario> UsuarioRepository { get; set; }

        // Para almacenar entidades durante el escenario
        public Cliente? ClienteActual { get; set; }
        public Pedido? PedidoActual { get; set; }
        public Producto? ProductoActual { get; set; }
        public Repartidore? RepartidorActual { get; set; }

        // Para validaciones
        public bool ValidationFailed { get; set; }
        public Exception? LastException { get; set; }

        public IntegracionTestContext()
        {
            // Crear contexto InMemory con nombre único para cada escenario
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            DbContext = new MyDbContext(options);

            // Inicializar repositorios
            ClienteRepository = new GenericRepository<Cliente>(DbContext);
            PedidoRepository = new GenericRepository<Pedido>(DbContext);
            ProductoRepository = new GenericRepository<Producto>(DbContext);
            RepartidorRepository = new GenericRepository<Repartidore>(DbContext);
            CategoriaRepository = new GenericRepository<Categoria>(DbContext);
            UsuarioRepository = new GenericRepository<Usuario>(DbContext);

            ValidationFailed = false;
            LastException = null;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
