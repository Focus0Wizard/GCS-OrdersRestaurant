using TechTalk.SpecFlow;
using RestaurantQATest.BDD.Support;

namespace RestaurantQATest.BDD.Hooks
{
    [Binding]
    public class IntegracionHooks
    {
        private readonly IntegracionTestContext _context;

        public IntegracionHooks(IntegracionTestContext context)
        {
            _context = context;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Limpiar estado antes de cada escenario
            _context.ValidationFailed = false;
            _context.LastException = null;
            _context.ClienteActual = null;
            _context.PedidoActual = null;
            _context.ProductoActual = null;
            _context.RepartidorActual = null;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Limpiar recursos después de cada escenario
            _context.Dispose();
        }
    }
}
