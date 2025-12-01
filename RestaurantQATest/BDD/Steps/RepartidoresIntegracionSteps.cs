using TechTalk.SpecFlow;
using FluentAssertions;
using Restaurant.Entities;
using RestaurantQATest.BDD.Support;
using System.ComponentModel.DataAnnotations;

namespace RestaurantQATest.BDD.Steps
{
    [Binding]
    public class RepartidoresIntegracionSteps
    {
        private readonly IntegracionTestContext _context;

        public RepartidoresIntegracionSteps(IntegracionTestContext context)
        {
            _context = context;
        }

        [Given(@"que se tiene un nuevo repartidor con Nombre ""(.*)"", Apellido ""(.*)"", Teléfono ""(.*)"", EstadoEntrega ""(.*)"" y Tipo ""(.*)""")]
        public void GivenQueSeTieneUnNuevoRepartidorConDatos(string nombre, string apellido, string telefono, string estadoEntrega, string tipo)
        {
            _context.RepartidorActual = new Repartidore
            {
                Nombre = nombre,
                Apellido = apellido,
                Telefono = telefono,
                EstadoEntrega = estadoEntrega,
                Tipo = tipo,
                Estado = 1,
                FechaCreacion = DateTime.Now
            };
        }

        [When(@"guardo el repartidor en la base de datos")]
        public async Task WhenGuardoElRepartidorEnLaBaseDeDatos()
        {
            try
            {
                var validationContext = new ValidationContext(_context.RepartidorActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.RepartidorActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                await _context.RepartidorRepository.AddAsync(_context.RepartidorActual);
                await _context.RepartidorRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe devolver un Id válido para el repartidor")]
        public void ThenElSistemaDebeDevoverUnIdValidoParaElRepartidor()
        {
            _context.RepartidorActual.Should().NotBeNull();
            _context.RepartidorActual!.Id.Should().BeGreaterThan((short)0);
        }

        [Then(@"el repartidor con Teléfono ""(.*)"" debe existir en la base de datos")]
        public async Task ThenElRepartidorConTelefonoDebeExistirEnLaBaseDeDatos(string telefono)
        {
            var repartidores = await _context.RepartidorRepository.FindAsync(r => r.Telefono == telefono);
            repartidores.Should().NotBeEmpty();
            repartidores.First().Telefono.Should().Be(telefono);
        }

        [When(@"intento guardar el repartidor con teléfono inválido en la base de datos")]
        public async Task WhenIntentoGuardarElRepartidorConTelefonoInvalidoEnLaBaseDeDatos()
        {
            try
            {
                var validationContext = new ValidationContext(_context.RepartidorActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.RepartidorActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                await _context.RepartidorRepository.AddAsync(_context.RepartidorActual);
                await _context.RepartidorRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe rechazar el repartidor por validación de teléfono")]
        public void ThenElSistemaDebeRechazarElRepartidorPorValidacionDeTelefono()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [Given(@"que existe un repartidor con Id ""(.*)"", Nombre ""(.*)"", Apellido ""(.*)"", Teléfono ""(.*)"" y EstadoEntrega ""(.*)""")]
        public async Task GivenQueExisteUnRepartidorConDatos(string id, string nombre, string apellido, string telefono, string estadoEntrega)
        {
            var repartidor = new Repartidore
            {
                Id = short.Parse(id),
                Nombre = nombre,
                Apellido = apellido,
                Telefono = telefono,
                EstadoEntrega = estadoEntrega,
                Tipo = "Motocicleta",
                Estado = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Repartidores.Add(repartidor);
            await _context.DbContext.SaveChangesAsync();
            _context.RepartidorActual = repartidor;
        }

        [When(@"actualizo el EstadoEntrega del repartidor a ""(.*)""")]
        public async Task WhenActualizoElEstadoEntregaDelRepartidorA(string nuevoEstadoEntrega)
        {
            try
            {
                _context.RepartidorActual!.EstadoEntrega = nuevoEstadoEntrega;
                _context.RepartidorActual.UltimaActualizacion = DateTime.Now;

                _context.RepartidorRepository.Update(_context.RepartidorActual);
                await _context.RepartidorRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el repartidor debe tener el nuevo EstadoEntrega ""(.*)""")]
        public void ThenElRepartidorDebeTenerElNuevoEstadoEntrega(string estadoEntrega)
        {
            _context.RepartidorActual!.EstadoEntrega.Should().Be(estadoEntrega);
        }

        [Given(@"que existe un repartidor con Id ""(.*)"", Nombre ""(.*)"", Apellido ""(.*)"", Teléfono ""(.*)"" y Tipo ""(.*)""")]
        public async Task GivenQueExisteUnRepartidorConIdNombreApellidoTelefonoYTipo(string id, string nombre, string apellido, string telefono, string tipo)
        {
            var repartidor = new Repartidore
            {
                Id = short.Parse(id),
                Nombre = nombre,
                Apellido = apellido,
                Telefono = telefono,
                EstadoEntrega = "Disponible",
                Tipo = tipo,
                Estado = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Repartidores.Add(repartidor);
            await _context.DbContext.SaveChangesAsync();
            _context.RepartidorActual = repartidor;
        }

        [When(@"actualizo el Tipo del repartidor a ""(.*)""")]
        public async Task WhenActualizoElTipoDelRepartidorA(string nuevoTipo)
        {
            try
            {
                _context.RepartidorActual!.Tipo = nuevoTipo;
                _context.RepartidorActual.UltimaActualizacion = DateTime.Now;

                _context.RepartidorRepository.Update(_context.RepartidorActual);
                await _context.RepartidorRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el repartidor debe tener el nuevo Tipo ""(.*)""")]
        public void ThenElRepartidorDebeTenerElNuevoTipo(string tipo)
        {
            _context.RepartidorActual!.Tipo.Should().Be(tipo);
        }

        [When(@"intento actualizar el Nombre del repartidor a ""(.*)""")]
        public async Task WhenIntentoActualizarElNombreDelRepartidorA(string nuevoNombre)
        {
            try
            {
                _context.RepartidorActual!.Nombre = nuevoNombre;

                var validationContext = new ValidationContext(_context.RepartidorActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.RepartidorActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.RepartidorRepository.Update(_context.RepartidorActual);
                await _context.RepartidorRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe rechazar la actualización del repartidor por validación")]
        public void ThenElSistemaDebeRechazarLaActualizacionDelRepartidorPorValidacion()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [When(@"elimino el repartidor usando soft delete")]
        public async Task WhenEliminoElRepartidorUsandoSoftDelete()
        {
            await _context.RepartidorRepository.SoftDeleteAsync(_context.RepartidorActual);
        }

        [Then(@"el repartidor debe tener Estado igual a ""(.*)""")]
        public void ThenElRepartidorDebeTenerEstadoIgualA(string estado)
        {
            _context.RepartidorActual!.Estado.Should().Be(sbyte.Parse(estado));
        }

        [Then(@"el repartidor no debe aparecer en consultas de repartidores activos")]
        public async Task ThenElRepartidorNoDebeAparecerEnConsultasDeRepartidoresActivos()
        {
            var repartidoresActivos = await _context.RepartidorRepository.GetAllAsync();
            repartidoresActivos.Should().NotContain(r => r.Id == _context.RepartidorActual!.Id && r.Estado == 0);
        }

        [Given(@"que existen varios repartidores registrados con estado activo")]
        public async Task GivenQueExistenVariosRepartidoresRegistradosConEstadoActivo()
        {
            var repartidores = new[]
            {
                new Repartidore { Id = 100, Nombre = "Test1", Apellido = "Repartidor1", Telefono = "70111111", EstadoEntrega = "Disponible", Tipo = "Motocicleta", Estado = 1, FechaCreacion = DateTime.Now },
                new Repartidore { Id = 101, Nombre = "Test2", Apellido = "Repartidor2", Telefono = "70222222", EstadoEntrega = "Disponible", Tipo = "Bicicleta", Estado = 1, FechaCreacion = DateTime.Now },
                new Repartidore { Id = 102, Nombre = "Test3", Apellido = "Repartidor3", Telefono = "70333333", EstadoEntrega = "Disponible", Tipo = "Auto", Estado = 1, FechaCreacion = DateTime.Now }
            };

            _context.DbContext.Repartidores.AddRange(repartidores);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"solicito el listado de todos los repartidores")]
        public async Task WhenSolicitoElListadoDeTodosLosRepartidores()
        {
            var repartidores = await _context.RepartidorRepository.GetAllAsync();
            _context.DbContext.ChangeTracker.Clear();
        }

        [Then(@"el sistema debe devolver al menos un repartidor")]
        public async Task ThenElSistemaDebeDevoverAlMenosUnRepartidor()
        {
            var repartidores = await _context.RepartidorRepository.GetAllAsync();
            repartidores.Should().NotBeEmpty();
        }

        [Then(@"todos los repartidores deben tener Estado igual a ""(.*)""")]
        public async Task ThenTodosLosRepartidoresDebenTenerEstadoIgualA(string estado)
        {
            var repartidores = await _context.RepartidorRepository.GetAllAsync();
            repartidores.Should().OnlyContain(r => r.Estado == sbyte.Parse(estado));
        }
    }
}
