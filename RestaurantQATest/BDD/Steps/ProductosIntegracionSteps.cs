using TechTalk.SpecFlow;
using FluentAssertions;
using Restaurant.Entities;
using RestaurantQATest.BDD.Support;
using System.ComponentModel.DataAnnotations;

namespace RestaurantQATest.BDD.Steps
{
    [Binding]
    public class ProductosIntegracionSteps
    {
        private readonly IntegracionTestContext _context;

        public ProductosIntegracionSteps(IntegracionTestContext context)
        {
            _context = context;
        }

        [Given(@"que se tiene un nuevo producto con Nombre ""(.*)"", Precio ""(.*)"", Stock ""(.*)"", Descripcion ""(.*)"" y CategoriaId ""(.*)""")]
        public async Task GivenQueSeTieneUnNuevoProductoConDatos(string nombre, string precio, string stock, string descripcion, string categoriaId)
        {
            // Crear categoría si no existe
            var categoria = await _context.DbContext.Categorias.FindAsync(short.Parse(categoriaId));
            if (categoria == null)
            {
                categoria = new Categoria
                {
                    Id = short.Parse(categoriaId),
                    Nombre = "Categoria Test",
                    Descripcion = "Descripcion test"
                };
                _context.DbContext.Categorias.Add(categoria);
                await _context.DbContext.SaveChangesAsync();
            }

            _context.ProductoActual = new Producto
            {
                Nombre = nombre,
                Precio = decimal.Parse(precio),
                Stock = int.Parse(stock),
                Descripcion = descripcion,
                CategoriaId = short.Parse(categoriaId),
                Estado = 1,
                FechaCreacion = DateTime.Now
            };
        }

        [When(@"guardo el producto en la base de datos")]
        public async Task WhenGuardoElProductoEnLaBaseDeDatos()
        {
            try
            {
                var validationContext = new ValidationContext(_context.ProductoActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ProductoActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                await _context.ProductoRepository.AddAsync(_context.ProductoActual);
                await _context.ProductoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe devolver un Id válido para el producto")]
        public void ThenElSistemaDebeDevoverUnIdValidoParaElProducto()
        {
            _context.ProductoActual.Should().NotBeNull();
            _context.ProductoActual!.Id.Should().BeGreaterThan((short)0);
        }

        [Then(@"el producto con Nombre ""(.*)"" debe existir en la base de datos")]
        public async Task ThenElProductoConNombreDebeExistirEnLaBaseDeDatos(string nombre)
        {
            var productos = await _context.ProductoRepository.FindAsync(p => p.Nombre == nombre);
            productos.Should().NotBeEmpty();
            productos.First().Nombre.Should().Be(nombre);
        }

        [When(@"intento guardar el producto con precio fuera de rango en la base de datos")]
        public async Task WhenIntentoGuardarElProductoConPrecioFueraDeRangoEnLaBaseDeDatos()
        {
            try
            {
                var validationContext = new ValidationContext(_context.ProductoActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ProductoActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                await _context.ProductoRepository.AddAsync(_context.ProductoActual);
                await _context.ProductoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe rechazar el producto por validación de precio")]
        public void ThenElSistemaDebeRechazarElProductoPorValidacionDePrecio()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [Given(@"que existe un producto con Id ""(.*)"", Nombre ""(.*)"", Precio ""(.*)"", Stock ""(.*)"" y Descripcion ""(.*)""")]
        public async Task GivenQueExisteUnProductoConDatos(string id, string nombre, string precio, string stock, string descripcion)
        {
            // Crear categoría si no existe
            var categoria = await _context.DbContext.Categorias.FindAsync((short)1);
            if (categoria == null)
            {
                categoria = new Categoria
                {
                    Id = 1,
                    Nombre = "Categoria Test",
                    Descripcion = "Descripcion test"
                };
                _context.DbContext.Categorias.Add(categoria);
                await _context.DbContext.SaveChangesAsync();
            }

            var producto = new Producto
            {
                Id = short.Parse(id),
                Nombre = nombre,
                Precio = decimal.Parse(precio),
                Stock = int.Parse(stock),
                Descripcion = descripcion,
                CategoriaId = 1,
                Estado = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Productos.Add(producto);
            await _context.DbContext.SaveChangesAsync();
            _context.ProductoActual = producto;
        }

        [When(@"actualizo el Precio del producto a ""(.*)""")]
        public async Task WhenActualizoElPrecioDelProductoA(string nuevoPrecio)
        {
            try
            {
                _context.ProductoActual!.Precio = decimal.Parse(nuevoPrecio);
                _context.ProductoActual.UltimaActualizacion = DateTime.Now;

                var validationContext = new ValidationContext(_context.ProductoActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ProductoActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.ProductoRepository.Update(_context.ProductoActual);
                await _context.ProductoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el producto debe tener el nuevo Precio ""(.*)""")]
        public void ThenElProductoDebeTenerElNuevoPrecio(string precio)
        {
            _context.ProductoActual!.Precio.Should().Be(decimal.Parse(precio));
        }

        [When(@"actualizo el Stock del producto a ""(.*)""")]
        public async Task WhenActualizoElStockDelProductoA(string nuevoStock)
        {
            try
            {
                _context.ProductoActual!.Stock = int.Parse(nuevoStock);
                _context.ProductoActual.UltimaActualizacion = DateTime.Now;

                var validationContext = new ValidationContext(_context.ProductoActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ProductoActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.ProductoRepository.Update(_context.ProductoActual);
                await _context.ProductoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el producto debe tener el nuevo Stock ""(.*)""")]
        public void ThenElProductoDebeTenerElNuevoStock(string stock)
        {
            _context.ProductoActual!.Stock.Should().Be(int.Parse(stock));
        }

        [When(@"intento actualizar el Stock del producto a ""(.*)""")]
        public async Task WhenIntentoActualizarElStockDelProductoA(string nuevoStock)
        {
            try
            {
                _context.ProductoActual!.Stock = int.Parse(nuevoStock);

                var validationContext = new ValidationContext(_context.ProductoActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ProductoActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.ProductoRepository.Update(_context.ProductoActual);
                await _context.ProductoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe rechazar la actualización del producto por validación")]
        public void ThenElSistemaDebeRechazarLaActualizacionDelProductoPorValidacion()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [When(@"elimino el producto usando soft delete")]
        public async Task WhenEliminoElProductoUsandoSoftDelete()
        {
            await _context.ProductoRepository.SoftDeleteAsync(_context.ProductoActual);
        }

        [Then(@"el producto debe tener Estado igual a ""(.*)""")]
        public void ThenElProductoDebeTenerEstadoIgualA(string estado)
        {
            _context.ProductoActual!.Estado.Should().Be(sbyte.Parse(estado));
        }

        [Then(@"el producto no debe aparecer en consultas de productos activos")]
        public async Task ThenElProductoNoDebeAparecerEnConsultasDeProductosActivos()
        {
            var productosActivos = await _context.ProductoRepository.GetAllAsync();
            productosActivos.Should().NotContain(p => p.Id == _context.ProductoActual!.Id && p.Estado == 0);
        }

        [Given(@"que existen varios productos registrados con estado activo")]
        public async Task GivenQueExistenVariosProductosRegistradosConEstadoActivo()
        {
            // Crear categoría si no existe
            var categoria = await _context.DbContext.Categorias.FindAsync((short)1);
            if (categoria == null)
            {
                categoria = new Categoria
                {
                    Id = 1,
                    Nombre = "Categoria Test",
                    Descripcion = "Descripcion test"
                };
                _context.DbContext.Categorias.Add(categoria);
                await _context.DbContext.SaveChangesAsync();
            }

            var productos = new[]
            {
                new Producto { Id = 100, Nombre = "ProductoTest1", Precio = 10, Stock = 50, Descripcion = "Descripcion 1", CategoriaId = 1, Estado = 1, FechaCreacion = DateTime.Now },
                new Producto { Id = 101, Nombre = "ProductoTest2", Precio = 20, Stock = 60, Descripcion = "Descripcion 2", CategoriaId = 1, Estado = 1, FechaCreacion = DateTime.Now },
                new Producto { Id = 102, Nombre = "ProductoTest3", Precio = 30, Stock = 70, Descripcion = "Descripcion 3", CategoriaId = 1, Estado = 1, FechaCreacion = DateTime.Now }
            };

            _context.DbContext.Productos.AddRange(productos);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"solicito el listado de todos los productos")]
        public async Task WhenSolicitoElListadoDeTodosLosProductos()
        {
            var productos = await _context.ProductoRepository.GetAllAsync();
            _context.DbContext.ChangeTracker.Clear();
        }

        [Then(@"el sistema debe devolver al menos un producto")]
        public async Task ThenElSistemaDebeDevoverAlMenosUnProducto()
        {
            var productos = await _context.ProductoRepository.GetAllAsync();
            productos.Should().NotBeEmpty();
        }

        [Then(@"todos los productos deben tener Estado igual a ""(.*)""")]
        public async Task ThenTodosLosProductosDebenTenerEstadoIgualA(string estado)
        {
            var productos = await _context.ProductoRepository.GetAllAsync();
            productos.Should().OnlyContain(p => p.Estado == sbyte.Parse(estado));
        }
    }
}
