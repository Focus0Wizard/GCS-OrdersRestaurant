"""
Configuración de fixtures de pytest para las pruebas de Selenium.
Proporciona configuración compartida para todos los tests.
"""
import pytest
from selenium import webdriver
from selenium.webdriver.chrome.service import Service as ChromeService
from selenium.webdriver.chrome.options import Options
from webdriver_manager.chrome import ChromeDriverManager


@pytest.fixture(scope="session")
def driver():
    """
    Fixture de sesión que proporciona una instancia de WebDriver.
    Se ejecuta una vez por sesión de pruebas y se comparte entre todos los tests.
    
    Yields:
        WebDriver: Instancia de Chrome WebDriver configurada
    """
    opts = Options()
    # Ejecutar en modo headless para CI/CD
    opts.add_argument("--headless=new")
    opts.add_argument("--no-sandbox")
    opts.add_argument("--disable-dev-shm-usage")
    opts.add_argument("--disable-gpu")
    opts.add_argument("--window-size=1920,1080")
    
    # Instalación automática del driver con webdriver-manager
    service = ChromeService(ChromeDriverManager().install())
    d = webdriver.Chrome(service=service, options=opts)
    d.implicitly_wait(5)
    
    yield d
    
    # Cleanup: cerrar el navegador después de todas las pruebas
    d.quit()


@pytest.fixture
def base_url():
    """
    Fixture que proporciona la URL base de la aplicación.
    
    Returns:
        str: URL base donde corre la aplicación
    """
    return "http://localhost:5020"


@pytest.fixture(autouse=True)
def setup_teardown(driver):
    """
    Fixture que se ejecuta antes y después de cada test.
    Útil para limpiar el estado entre pruebas.
    
    Args:
        driver: Instancia de WebDriver
    """
    yield
    # Limpiar cookies después de cada test
    driver.delete_all_cookies()


def pytest_configure(config):
    """
    Hook de pytest para configuración inicial.
    """
    config.addinivalue_line(
        "markers", "productos: marca tests relacionados con el módulo de productos"
    )
    config.addinivalue_line(
        "markers", "clientes: marca tests relacionados con el módulo de clientes"
    )
    config.addinivalue_line(
        "markers", "repartidores: marca tests relacionados con el módulo de repartidores"
    )
    config.addinivalue_line(
        "markers", "smoke: marca tests de smoke testing"
    )
    config.addinivalue_line(
        "markers", "regression: marca tests de regresión"
    )
