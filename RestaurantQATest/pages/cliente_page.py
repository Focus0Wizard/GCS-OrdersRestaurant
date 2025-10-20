"""
Page Object para el módulo de Clientes
Maneja las interacciones con el formulario de registro de clientes
"""

from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from pages.base_page import BasePage
import time


class ClientePage(BasePage):
    """Page Object para la página de registro de clientes"""

    # ========== LOCALIZADORES ==========
    
    # Campos del formulario
    NOMBRE_INPUT = (By.ID, "Cliente_Nombre")
    APELLIDO_INPUT = (By.ID, "Cliente_Apellido")
    TELEFONO_INPUT = (By.ID, "Cliente_Telefono")
    CORREO_INPUT = (By.ID, "Cliente_Correo")
    
    # Botones
    SUBMIT_BUTTON = (By.CSS_SELECTOR, "button[type='submit']")
    VOLVER_BUTTON = (By.LINK_TEXT, "Volver")
    
    # Mensajes de validación
    NOMBRE_VALIDATION = (By.CSS_SELECTOR, "span[data-valmsg-for='Cliente.Nombre']")
    APELLIDO_VALIDATION = (By.CSS_SELECTOR, "span[data-valmsg-for='Cliente.Apellido']")
    TELEFONO_VALIDATION = (By.CSS_SELECTOR, "span[data-valmsg-for='Cliente.Telefono']")
    CORREO_VALIDATION = (By.CSS_SELECTOR, "span[data-valmsg-for='Cliente.Correo']")
    
    # Tabla de clientes (lista)
    CLIENTES_TABLE = (By.CSS_SELECTOR, "table.table")
    CLIENTE_ROW = (By.CSS_SELECTOR, "table.table tbody tr")
    
    # Mensajes de éxito/error
    SUCCESS_MESSAGE = (By.CSS_SELECTOR, ".alert-success")
    ERROR_MESSAGE = (By.CSS_SELECTOR, ".alert-danger")

    def __init__(self, driver, base_url):
        """
        Inicializa el Page Object de Cliente
        
        Args:
            driver: WebDriver de Selenium
            base_url: URL base de la aplicación
        """
        super().__init__(driver)
        self.base_url = base_url

    # ========== MÉTODOS DE NAVEGACIÓN ==========

    def navigate(self):
        """Navega a la página de creación de clientes"""
        url = f"{self.base_url}/Clientes/Create"
        self.driver.get(url)
        time.sleep(0.5)  # Pequeña espera para carga de la página

    def navigate_to_index(self):
        """Navega a la página de lista de clientes"""
        url = f"{self.base_url}/Clientes/Index"
        self.driver.get(url)
        time.sleep(0.5)

    # ========== MÉTODOS DE INTERACCIÓN CON EL FORMULARIO ==========

    def fill_nombre(self, nombre):
        """
        Ingresa el nombre del cliente
        
        Args:
            nombre: Nombre a ingresar
        """
        if nombre:
            self.enter_text(self.NOMBRE_INPUT, nombre)

    def fill_apellido(self, apellido):
        """
        Ingresa el apellido del cliente
        
        Args:
            apellido: Apellido a ingresar
        """
        if apellido:
            self.enter_text(self.APELLIDO_INPUT, apellido)

    def fill_telefono(self, telefono):
        """
        Ingresa el teléfono del cliente
        
        Args:
            telefono: Teléfono a ingresar
        """
        if telefono:
            self.enter_text(self.TELEFONO_INPUT, telefono)

    def fill_correo(self, correo):
        """
        Ingresa el correo del cliente
        
        Args:
            correo: Correo electrónico a ingresar
        """
        if correo:
            self.enter_text(self.CORREO_INPUT, correo)

    def fill_form(self, nombre, apellido, telefono, correo):
        """
        Completa el formulario de registro de cliente
        
        Args:
            nombre: Nombre del cliente
            apellido: Apellido del cliente
            telefono: Teléfono del cliente (opcional)
            correo: Correo electrónico del cliente
        """
        self.fill_nombre(nombre)
        self.fill_apellido(apellido)
        self.fill_telefono(telefono)
        self.fill_correo(correo)

    def submit_form(self):
        """Envía el formulario haciendo clic en el botón Guardar"""
        self.click(self.SUBMIT_BUTTON)
        time.sleep(1)  # Esperar a que se procese el formulario

    def click_volver(self):
        """Hace clic en el botón Volver"""
        self.click(self.VOLVER_BUTTON)

    # ========== MÉTODOS DE VALIDACIÓN ==========

    def has_validation_errors(self):
        """
        Verifica si hay errores de validación en el formulario
        
        Returns:
            bool: True si hay errores de validación, False en caso contrario
        """
        try:
            # Buscar cualquier span con clase text-danger que tenga contenido
            error_spans = self.driver.find_elements(By.CSS_SELECTOR, "span.text-danger")
            
            for span in error_spans:
                if span.text.strip():  # Si el span tiene texto
                    return True
            
            # También verificar mensajes HTML5 de validación
            campos = [self.NOMBRE_INPUT, self.APELLIDO_INPUT, self.TELEFONO_INPUT, self.CORREO_INPUT]
            for campo_locator in campos:
                try:
                    campo = self.find_element(campo_locator)
                    validation_message = campo.get_attribute("validationMessage")
                    if validation_message:
                        return True
                except:
                    continue
            
            return False
        except Exception as e:
            print(f"Error al verificar validaciones: {str(e)}")
            return False

    def get_validation_errors(self):
        """
        Obtiene la lista de mensajes de error de validación
        
        Returns:
            list: Lista de mensajes de error
        """
        errors = []
        
        # Errores de validación de spans
        error_spans = self.driver.find_elements(By.CSS_SELECTOR, "span.text-danger")
        for span in error_spans:
            text = span.text.strip()
            if text:
                errors.append(text)
        
        # Errores HTML5 de validación
        campos = [
            (self.NOMBRE_INPUT, "Nombre"),
            (self.APELLIDO_INPUT, "Apellido"),
            (self.TELEFONO_INPUT, "Teléfono"),
            (self.CORREO_INPUT, "Correo")
        ]
        
        for campo_locator, nombre_campo in campos:
            try:
                campo = self.find_element(campo_locator)
                validation_message = campo.get_attribute("validationMessage")
                if validation_message:
                    errors.append(f"{nombre_campo}: {validation_message}")
            except:
                continue
        
        return errors

    def get_nombre_validation_message(self):
        """Obtiene el mensaje de validación del campo Nombre"""
        return self.get_text(self.NOMBRE_VALIDATION)

    def get_apellido_validation_message(self):
        """Obtiene el mensaje de validación del campo Apellido"""
        return self.get_text(self.APELLIDO_VALIDATION)

    def get_telefono_validation_message(self):
        """Obtiene el mensaje de validación del campo Teléfono"""
        return self.get_text(self.TELEFONO_VALIDATION)

    def get_correo_validation_message(self):
        """Obtiene el mensaje de validación del campo Correo"""
        return self.get_text(self.CORREO_VALIDATION)

    # ========== MÉTODOS DE VERIFICACIÓN ==========

    def is_cliente_registered(self, nombre, apellido):
        """
        Verifica si el cliente fue registrado exitosamente
        
        Args:
            nombre: Nombre del cliente a buscar
            apellido: Apellido del cliente a buscar
            
        Returns:
            bool: True si el cliente está en la lista, False en caso contrario
        """
        try:
            # Navegar a la página de índice
            self.navigate_to_index()
            time.sleep(1)
            
            # Buscar en la tabla de clientes
            rows = self.driver.find_elements(*self.CLIENTE_ROW)
            
            for row in rows:
                cells = row.find_elements(By.TAG_NAME, "td")
                if len(cells) >= 2:
                    row_nombre = cells[0].text.strip()
                    row_apellido = cells[1].text.strip()
                    
                    # Comparación case-insensitive
                    if (row_nombre.lower() == nombre.lower() and 
                        row_apellido.lower() == apellido.lower()):
                        return True
            
            return False
            
        except Exception as e:
            print(f"Error al verificar registro de cliente: {str(e)}")
            return False

    def is_on_create_page(self):
        """
        Verifica si se está en la página de creación de clientes
        
        Returns:
            bool: True si está en la página de creación
        """
        try:
            return "Create" in self.driver.current_url or "Crear" in self.driver.title
        except:
            return False

    def is_on_index_page(self):
        """
        Verifica si se está en la página de lista de clientes
        
        Returns:
            bool: True si está en la página de índice
        """
        try:
            return "Index" in self.driver.current_url or "Lista" in self.driver.title
        except:
            return False

    def has_success_message(self):
        """
        Verifica si hay un mensaje de éxito
        
        Returns:
            bool: True si hay mensaje de éxito
        """
        return self.is_element_visible(self.SUCCESS_MESSAGE)

    def has_error_message(self):
        """
        Verifica si hay un mensaje de error
        
        Returns:
            bool: True si hay mensaje de error
        """
        return self.is_element_visible(self.ERROR_MESSAGE)

    # ========== MÉTODOS AUXILIARES ==========

    def clear_form(self):
        """Limpia todos los campos del formulario"""
        campos = [self.NOMBRE_INPUT, self.APELLIDO_INPUT, self.TELEFONO_INPUT, self.CORREO_INPUT]
        for campo in campos:
            try:
                element = self.find_element(campo)
                element.clear()
            except:
                continue

    def get_form_data(self):
        """
        Obtiene los valores actuales del formulario
        
        Returns:
            dict: Diccionario con los valores de cada campo
        """
        return {
            'nombre': self.find_element(self.NOMBRE_INPUT).get_attribute('value'),
            'apellido': self.find_element(self.APELLIDO_INPUT).get_attribute('value'),
            'telefono': self.find_element(self.TELEFONO_INPUT).get_attribute('value'),
            'correo': self.find_element(self.CORREO_INPUT).get_attribute('value')
        }

    def is_form_empty(self):
        """
        Verifica si el formulario está vacío
        
        Returns:
            bool: True si todos los campos están vacíos
        """
        data = self.get_form_data()
        return all(not value for value in data.values())

    def wait_for_page_load(self, timeout=10):
        """
        Espera a que la página esté completamente cargada
        
        Args:
            timeout: Tiempo máximo de espera en segundos
        """
        try:
            WebDriverWait(self.driver, timeout).until(
                lambda d: d.execute_script("return document.readyState") == "complete"
            )
        except Exception as e:
            print(f"Timeout esperando carga de página: {str(e)}")
