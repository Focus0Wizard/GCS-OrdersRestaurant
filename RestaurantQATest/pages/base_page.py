"""
Módulo base para todos los Page Objects.
Contiene métodos comunes reutilizables para interactuar con elementos web.
"""
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException, NoSuchElementException


class BasePage:
    """
    Clase base que proporciona métodos comunes para todas las páginas.
    Implementa el patrón Page Object Model (POM).
    """

    def __init__(self, driver):
        """
        Inicializa la página base con el driver de Selenium.
        
        Args:
            driver: Instancia de WebDriver de Selenium
        """
        self.driver = driver
        self.wait = WebDriverWait(driver, 10)

    def find_element(self, locator):
        """
        Encuentra un elemento en la página.
        
        Args:
            locator: Tupla (By, valor) para localizar el elemento
            
        Returns:
            WebElement encontrado
        """
        return self.driver.find_element(*locator)

    def find_elements(self, locator):
        """
        Encuentra múltiples elementos en la página.
        
        Args:
            locator: Tupla (By, valor) para localizar los elementos
            
        Returns:
            Lista de WebElements encontrados
        """
        return self.driver.find_elements(*locator)

    def click(self, locator):
        """
        Hace clic en un elemento.
        
        Args:
            locator: Tupla (By, valor) del elemento
        """
        element = self.wait.until(EC.element_to_be_clickable(locator))
        element.click()

    def enter_text(self, locator, text):
        """
        Ingresa texto en un campo de entrada.
        
        Args:
            locator: Tupla (By, valor) del campo
            text: Texto a ingresar
        """
        element = self.wait.until(EC.visibility_of_element_located(locator))
        element.clear()
        if text:
            element.send_keys(str(text))

    def get_text(self, locator):
        """
        Obtiene el texto de un elemento.
        
        Args:
            locator: Tupla (By, valor) del elemento
            
        Returns:
            str: Texto del elemento
        """
        element = self.wait.until(EC.visibility_of_element_located(locator))
        return element.text

    def is_element_visible(self, locator, timeout=5):
        """
        Verifica si un elemento es visible en la página.
        
        Args:
            locator: Tupla (By, valor) del elemento
            timeout: Tiempo máximo de espera en segundos
            
        Returns:
            bool: True si el elemento es visible, False en caso contrario
        """
        try:
            wait = WebDriverWait(self.driver, timeout)
            wait.until(EC.visibility_of_element_located(locator))
            return True
        except TimeoutException:
            return False

    def is_element_present(self, locator):
        """
        Verifica si un elemento está presente en el DOM.
        
        Args:
            locator: Tupla (By, valor) del elemento
            
        Returns:
            bool: True si el elemento está presente, False en caso contrario
        """
        try:
            self.find_element(locator)
            return True
        except NoSuchElementException:
            return False

    def wait_for_element(self, locator, timeout=10):
        """
        Espera hasta que un elemento esté presente.
        
        Args:
            locator: Tupla (By, valor) del elemento
            timeout: Tiempo máximo de espera en segundos
            
        Returns:
            WebElement cuando esté presente
        """
        wait = WebDriverWait(self.driver, timeout)
        return wait.until(EC.presence_of_element_located(locator))

    def get_current_url(self):
        """
        Obtiene la URL actual del navegador.
        
        Returns:
            str: URL actual
        """
        return self.driver.current_url

    def navigate_to(self, url):
        """
        Navega a una URL específica.
        
        Args:
            url: URL de destino
        """
        self.driver.get(url)

    def get_page_title(self):
        """
        Obtiene el título de la página actual.
        
        Returns:
            str: Título de la página
        """
        return self.driver.title

    def scroll_to_element(self, locator):
        """
        Hace scroll hasta un elemento específico.
        
        Args:
            locator: Tupla (By, valor) del elemento
        """
        element = self.find_element(locator)
        self.driver.execute_script("arguments[0].scrollIntoView(true);", element)

    def get_validation_message(self, locator):
        """
        Obtiene el mensaje de validación de un campo.
        
        Args:
            locator: Tupla (By, valor) del elemento de validación
            
        Returns:
            str: Mensaje de validación o cadena vacía
        """
        try:
            return self.get_text(locator)
        except:
            return ""
