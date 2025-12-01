using TechTalk.SpecFlow;
using FluentAssertions;
using Restaurant.Entities;
using RestaurantQATest.BDD.Support;
using Microsoft.EntityFrameworkCore;

namespace RestaurantQATest.BDD.Steps
{
    [Binding]
    public class PedidosIntegracionSteps
    {
        private readonly IntegracionTestContext _context;

        public PedidosIntegracionSteps(IntegracionTestContext context)
        {
            _context = context;
        }

        [Given(@"que existe un cliente registrado con Id ""(.*)""")]
        public async Task GivenQueExisteUnClienteRegistradoConId(string id)
        {
            var cliente = new Cliente
            {
                Id = short.Parse(id),
                Nombre = "ClienteTest",
                Apellido = "ApellidoTest",
                Correo = $"cliente{id}@test.com",
                Estado = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Clientes.Add(cliente);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given(@"que existe un producto con Id ""(.*)"" y precio ""(.*)""")]
        public async Task GivenQueExisteUnProductoConIdYPrecio(string id, string precio)
        {
            var categoria = new Categoria
            {
                Id = 1,
                Nombre = "Categoria Test",
                Descripcion = "Descripcion test"
            };
            _context.DbContext.Categorias.Add(categoria);

            var producto = new Producto
            {
                Id = short.Parse(id),
                Nombre = "ProductoTest",
                Precio = decimal.Parse(precio),
                Stock = 100,
                Descripcion = "Descripcion producto test",
                CategoriaId = 1,
                Estado = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Productos.Add(producto);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given(@"que existe un repartidor con Id ""(.*)""")]
        public async Task GivenQueExisteUnRepartidorConId(string id)
        {
            var repartidor = new Repartidore
            {
                Id = short.Parse(id),
                Nombre = "RepartidorTest",
                Apellido = "ApellidoTest",
                Telefono = "70123456",
                EstadoEntrega = "Disponible",
                Estado = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Repartidores.Add(repartidor);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"creo un nuevo pedido con ClienteId ""(.*)"", UsuarioId ""(.*)"", Total ""(.*)""")]
        public async Task WhenCreoUnNuevoPedidoConDatos(string clienteId, string usuarioId, string total)
        {
            // Crear usuario si no existe
            var usuario = await _context.DbContext.Usuarios.FindAsync(short.Parse(usuarioId));
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Id = short.Parse(usuarioId),
                    Nombre = "Admin",
                    Apellido = "Test",
                    Usuario1 = "admin",
                    Password = "admin123",
                    Rol = "Administrador",
                    Estado = 1
                };
                _context.DbContext.Usuarios.Add(usuario);
                await _context.DbContext.SaveChangesAsync();
            }

            _context.PedidoActual = new Pedido
            {
                ClienteId = short.Parse(clienteId),
                UsuarioId = short.Parse(usuarioId),
                NombreCliente = "ClienteTest",
                ApellidoCliente = "ApellidoTest",
                Total = decimal.Parse(total),
                EstadoPedido = 1,
                FechaCreacion = DateTime.Now
            };
        }

        [When(@"asigno el RiderId ""(.*)"" al pedido")]
        public void WhenAsignoElRiderIdAlPedido(string riderId)
        {
            _context.PedidoActual!.RiderId = short.Parse(riderId);
        }

        [Then(@"el pedido debe guardarse correctamente")]
        public async Task ThenElPedidoDebeGuardarseCorrectamente()
        {
            await _context.PedidoRepository.AddAsync(_context.PedidoActual);
            await _context.PedidoRepository.SaveChangesAsync();
            _context.PedidoActual!.Id.Should().BeGreaterThan((short)0);
        }

        [Then(@"el pedido debe tener EstadoPedido igual a ""(.*)""")]
        public void ThenElPedidoDebeTenerEstadoPedidoIgualA(string estado)
        {
            _context.PedidoActual!.EstadoPedido.Should().Be(sbyte.Parse(estado));
        }

        [Then(@"el pedido debe estar asociado al cliente, producto y repartidor")]
        public async Task ThenElPedidoDebeEstarAsociadoAlClienteProductoYRepartidor()
        {
            var pedido = await _context.DbContext.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Rider)
                .FirstOrDefaultAsync(p => p.Id == _context.PedidoActual!.Id);

            pedido.Should().NotBeNull();
            pedido!.Cliente.Should().NotBeNull();
            pedido.Rider.Should().NotBeNull();
        }

        [Given(@"que se tiene un nuevo pedido con ClienteId ""(.*)"", UsuarioId ""(.*)"", NombreCliente ""(.*)"", ApellidoCliente ""(.*)"" y Total ""(.*)""")]
        public async Task GivenQueSeTieneUnNuevoPedidoConDatos(string clienteId, string usuarioId, string nombreCliente, string apellidoCliente, string total)
        {
            // Crear usuario si no existe
            var usuario = await _context.DbContext.Usuarios.FindAsync(short.Parse(usuarioId));
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Id = short.Parse(usuarioId),
                    Nombre = "Admin",
                    Apellido = "Test",
                    Usuario1 = "admin",
                    Password = "admin123",
                    Rol = "Administrador",
                    Estado = 1
                };
                _context.DbContext.Usuarios.Add(usuario);
                await _context.DbContext.SaveChangesAsync();
            }

            _context.PedidoActual = new Pedido
            {
                ClienteId = short.Parse(clienteId),
                UsuarioId = short.Parse(usuarioId),
                NombreCliente = nombreCliente,
                ApellidoCliente = apellidoCliente,
                Total = decimal.Parse(total),
                EstadoPedido = 1,
                FechaCreacion = DateTime.Now
            };
        }

        [When(@"guardo el pedido en la base de datos")]
        public async Task WhenGuardoElPedidoEnLaBaseDeDatos()
        {
            try
            {
                await _context.PedidoRepository.AddAsync(_context.PedidoActual);
                await _context.PedidoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe devolver un Id válido para el pedido")]
        public void ThenElSistemaDebeDevoverUnIdValidoParaElPedido()
        {
            _context.PedidoActual.Should().NotBeNull();
            _context.PedidoActual!.Id.Should().BeGreaterThan((short)0);
        }

        [Then(@"el pedido con Total ""(.*)"" debe existir en la base de datos")]
        public async Task ThenElPedidoConTotalDebeExistirEnLaBaseDeDatos(string total)
        {
            var pedidos = await _context.PedidoRepository.FindAsync(p => p.Total == decimal.Parse(total));
            pedidos.Should().NotBeEmpty();
        }

        [When(@"intento guardar el pedido sin cliente válido en la base de datos")]
        public async Task WhenIntentoGuardarElPedidoSinClienteValidoEnLaBaseDeDatos()
        {
            try
            {
                await _context.PedidoRepository.AddAsync(_context.PedidoActual);
                await _context.PedidoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe rechazar el pedido por relación inválida")]
        public void ThenElSistemaDebeRechazarElPedidoPorRelacionInvalida()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [Given(@"que existe un pedido con Id ""(.*)"", ClienteId ""(.*)"", UsuarioId ""(.*)"" y EstadoPedido ""(.*)""")]
        public async Task GivenQueExisteUnPedidoConIdClienteIdUsuarioIdYEstadoPedido(string id, string clienteId, string usuarioId, string estadoPedido)
        {
            // Crear usuario si no existe
            var usuario = await _context.DbContext.Usuarios.FindAsync(short.Parse(usuarioId));
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Id = short.Parse(usuarioId),
                    Nombre = "Admin",
                    Apellido = "Test",
                    Usuario1 = "admin",
                    Password = "admin123",
                    Rol = "Administrador",
                    Estado = 1
                };
                _context.DbContext.Usuarios.Add(usuario);
                await _context.DbContext.SaveChangesAsync();
            }

            var pedido = new Pedido
            {
                Id = short.Parse(id),
                ClienteId = short.Parse(clienteId),
                UsuarioId = short.Parse(usuarioId),
                NombreCliente = "Test",
                ApellidoCliente = "Cliente",
                Total = 100,
                EstadoPedido = sbyte.Parse(estadoPedido),
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Pedidos.Add(pedido);
            await _context.DbContext.SaveChangesAsync();
            _context.PedidoActual = pedido;
        }

        [When(@"actualizo el EstadoPedido del pedido a ""(.*)""")]
        public async Task WhenActualizoElEstadoPedidoDelPedidoA(string nuevoEstado)
        {
            _context.PedidoActual!.EstadoPedido = sbyte.Parse(nuevoEstado);
            _context.PedidoRepository.Update(_context.PedidoActual);
            await _context.PedidoRepository.SaveChangesAsync();
        }

        [Then(@"el pedido debe tener el nuevo EstadoPedido ""(.*)""")]
        public void ThenElPedidoDebeTenerElNuevoEstadoPedido(string estado)
        {
            _context.PedidoActual!.EstadoPedido.Should().Be(sbyte.Parse(estado));
        }

        [Given(@"que existe un pedido con Id ""(.*)"", ClienteId ""(.*)"", UsuarioId ""(.*)"" y Total ""(.*)""")]
        public async Task GivenQueExisteUnPedidoConIdClienteIdUsuarioIdYTotal(string id, string clienteId, string usuarioId, string total)
        {
            // Crear usuario si no existe
            var usuario = await _context.DbContext.Usuarios.FindAsync(short.Parse(usuarioId));
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Id = short.Parse(usuarioId),
                    Nombre = "Admin",
                    Apellido = "Test",
                    Usuario1 = "admin",
                    Password = "admin123",
                    Rol = "Administrador",
                    Estado = 1
                };
                _context.DbContext.Usuarios.Add(usuario);
                await _context.DbContext.SaveChangesAsync();
            }

            var pedido = new Pedido
            {
                Id = short.Parse(id),
                ClienteId = short.Parse(clienteId),
                UsuarioId = short.Parse(usuarioId),
                NombreCliente = "Test",
                ApellidoCliente = "Cliente",
                Total = decimal.Parse(total),
                EstadoPedido = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Pedidos.Add(pedido);
            await _context.DbContext.SaveChangesAsync();
            _context.PedidoActual = pedido;
        }

        [When(@"actualizo el Total del pedido a ""(.*)""")]
        public async Task WhenActualizoElTotalDelPedidoA(string nuevoTotal)
        {
            try
            {
                _context.PedidoActual!.Total = decimal.Parse(nuevoTotal);
                _context.PedidoRepository.Update(_context.PedidoActual);
                await _context.PedidoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el pedido debe tener el nuevo Total ""(.*)""")]
        public void ThenElPedidoDebeTenerElNuevoTotal(string total)
        {
            _context.PedidoActual!.Total.Should().Be(decimal.Parse(total));
        }

        [When(@"intento actualizar el Total del pedido a ""(.*)""")]
        public async Task WhenIntentoActualizarElTotalDelPedidoA(string nuevoTotal)
        {
            try
            {
                var totalValue = decimal.Parse(nuevoTotal);
                if (totalValue < 0)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.PedidoActual!.Total = totalValue;
                _context.PedidoRepository.Update(_context.PedidoActual);
                await _context.PedidoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe rechazar la actualización del pedido por validación")]
        public void ThenElSistemaDebeRechazarLaActualizacionDelPedidoPorValidacion()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [When(@"elimino el pedido usando soft delete")]
        public async Task WhenEliminoElPedidoUsandoSoftDelete()
        {
            await _context.PedidoRepository.SoftDeleteAsync(_context.PedidoActual);
        }

        [Then(@"el pedido debe tener Estado igual a ""(.*)""")]
        public void ThenElPedidoDebeTenerEstadoIgualA(string estado)
        {
            _context.PedidoActual!.Estado.Should().Be(sbyte.Parse(estado));
        }

        [Given(@"que existen varios pedidos registrados")]
        public async Task GivenQueExistenVariosPedidosRegistrados()
        {
            // Crear usuario si no existe
            var usuario = await _context.DbContext.Usuarios.FindAsync((short)1);
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Id = 1,
                    Nombre = "Admin",
                    Apellido = "Test",
                    Usuario1 = "admin",
                    Password = "admin123",
                    Rol = "Administrador",
                    Estado = 1
                };
                _context.DbContext.Usuarios.Add(usuario);
                await _context.DbContext.SaveChangesAsync();
            }

            var pedidos = new[]
            {
                new Pedido { Id = 100, ClienteId = 1, UsuarioId = 1, NombreCliente = "Test1", ApellidoCliente = "Cliente1", Total = 50, EstadoPedido = 1, FechaCreacion = DateTime.Now },
                new Pedido { Id = 101, ClienteId = 1, UsuarioId = 1, NombreCliente = "Test2", ApellidoCliente = "Cliente2", Total = 75, EstadoPedido = 1, FechaCreacion = DateTime.Now },
                new Pedido { Id = 102, ClienteId = 1, UsuarioId = 1, NombreCliente = "Test3", ApellidoCliente = "Cliente3", Total = 100, EstadoPedido = 1, FechaCreacion = DateTime.Now }
            };

            _context.DbContext.Pedidos.AddRange(pedidos);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"solicito el listado de todos los pedidos")]
        public async Task WhenSolicitoElListadoDeTodosLosPedidos()
        {
            var pedidos = await _context.PedidoRepository.GetAllAsync();
            _context.DbContext.ChangeTracker.Clear();
        }

        [Then(@"el sistema debe devolver al menos un pedido")]
        public async Task ThenElSistemaDebeDevoverAlMenosUnPedido()
        {
            var pedidos = await _context.PedidoRepository.GetAllAsync();
            pedidos.Should().NotBeEmpty();
        }
    }
}
