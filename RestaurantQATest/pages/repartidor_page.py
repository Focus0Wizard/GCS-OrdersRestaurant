"""
Page Object Model para la página de gestión de repartidores.
Contiene los localizadores y métodos para interactuar con el formulario de repartidores.
"""
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
from pages.base_page import BasePage


class RepartidorPage(BasePage):
    """
    Page Object para la página de repartidores (/Repartidores/Create).
    Encapsula la interacción con el formulario de registro/edición de repartidores.
    """

    # ==================== LOCALIZADORES ====================
    # Campos del formulario
    INPUT_NOMBRE = (By.NAME, "Repartidor.Nombre")
    INPUT_APELLIDO = (By.NAME, "Repartidor.Apellido")
    INPUT_TELEFONO = (By.NAME, "Repartidor.Telefono")
    SELECT_TIPO = (By.NAME, "Repartidor.Tipo")
    INPUT_ID = (By.NAME, "Repartidor.Id")
    
    # Mensajes de validación
    VALIDATION_NOMBRE = (By.CSS_SELECTOR, "span[data-valmsg-for='Repartidor.Nombre']")
    VALIDATION_APELLIDO = (By.CSS_SELECTOR, "span[data-valmsg-for='Repartidor.Apellido']")
    VALIDATION_TELEFONO = (By.CSS_SELECTOR, "span[data-valmsg-for='Repartidor.Telefono']")
    VALIDATION_TIPO = (By.CSS_SELECTOR, "span[data-valmsg-for='Repartidor.Tipo']")
    
    # Botones
    BTN_SUBMIT = (By.CSS_SELECTOR, "form button[type='submit']")
    BTN_VOLVER = (By.CSS_SELECTOR, "a.btn-secondary")
    
    # Mensajes de resultado
    ALERT_SUCCESS = (By.CSS_SELECTOR, ".alert-success")
    ALERT_DANGER = (By.CSS_SELECTOR, ".alert-danger")
    
    # Tabla de repartidores (en Index)
    TABLE_REPARTIDORES = (By.CSS_SELECTOR, "table.table")
    TABLE_ROWS = (By.CSS_SELECTOR, "table.table tbody tr")

    def __init__(self, driver):
        """
        Inicializa la página de repartidores.
        
        Args:
            driver: Instancia de WebDriver de Selenium
        """
        super().__init__(driver)

    def navigate(self, base_url):
        """
        Navega a la página de creación de repartidores.
        
        Args:
            base_url: URL base de la aplicación
        """
        self.navigate_to(f"{base_url}/Repartidores/Create")

    def fill_form(self, nombre=None, apellido=None, telefono=None, tipo=None):
        """
        Rellena el formulario de repartidor con los datos proporcionados.
        
        Args:
            nombre: Nombre del repartidor (opcional)
            apellido: Apellido del repartidor (opcional)
            telefono: Teléfono del repartidor (opcional)
            tipo: Tipo de repartidor - Interno/Externo/Temporal (opcional)
        """
        # Limpiar campos primero
        self.enter_text(self.INPUT_NOMBRE, "")
        self.enter_text(self.INPUT_APELLIDO, "")
        self.enter_text(self.INPUT_TELEFONO, "")
        
        # Llenar con nuevos valores
        if nombre is not None:
            self.enter_text(self.INPUT_NOMBRE, nombre)
        
        if apellido is not None:
            self.enter_text(self.INPUT_APELLIDO, apellido)
        
        if telefono is not None:
            self.enter_text(self.INPUT_TELEFONO, telefono)
        
        # Seleccionar tipo si se proporciona
        if tipo is not None and tipo:
            self.select_tipo(tipo)

    def select_tipo(self, tipo):
        """
        Selecciona el tipo de repartidor del dropdown.
        
        Args:
            tipo: Tipo de repartidor (Bicicleta, Moto, Auto, Interno, Externo, Temporal)
        """
        try:
            select_element = self.find_element(self.SELECT_TIPO)
            select = Select(select_element)
            
            # Mapear tipos del CSV a valores del select
            # Nota: Los valores pueden variar según la implementación
            tipo_mapping = {
                'Interno': 'Bicicleta',
                'Externo': 'Moto',
                'Temporal': 'Auto',
                'Bicicleta': 'Bicicleta',
                'Moto': 'Moto',
                'Auto': 'Auto'
            }
            
            valor_select = tipo_mapping.get(tipo, tipo)
            
            # Intentar seleccionar por valor visible
            try:
                select.select_by_visible_text(valor_select)
            except:
                # Si falla, intentar por valor
                try:
                    select.select_by_value(valor_select)
                except:
                    # Si todo falla, usar el valor original
                    pass
        except Exception as e:
            # Si no se puede seleccionar, continuar (puede ser que no exista la opción)
            pass

    def submit_form(self):
        """
        Envía el formulario haciendo clic en el botón de submit.
        """
        self.click(self.BTN_SUBMIT)

    def create_repartidor(self, nombre, apellido, telefono, tipo):
        """
        Crea un nuevo repartidor llenando el formulario y enviándolo.
        
        Args:
            nombre: Nombre del repartidor
            apellido: Apellido del repartidor
            telefono: Teléfono del repartidor
            tipo: Tipo de repartidor
        """
        self.fill_form(nombre, apellido, telefono, tipo)
        self.submit_form()

    def is_success_message_displayed(self):
        """
        Verifica si se muestra un mensaje de éxito.
        
        Returns:
            bool: True si el mensaje de éxito es visible, False en caso contrario
        """
        return self.is_element_visible(self.ALERT_SUCCESS, timeout=3)

    def is_on_index_page(self):
        """
        Verifica si la URL actual es la página de índice de repartidores.
        Esto indica que el formulario se envió correctamente y redirigió.
        
        Returns:
            bool: True si está en la página de índice, False en caso contrario
        """
        current_url = self.get_current_url()
        # Si la URL termina en /Repartidores/Index o /Repartidores, es la página de índice
        return "/Repartidores/Index" in current_url or current_url.endswith("/Repartidores")

    def has_validation_errors(self):
        """
        Verifica si hay errores de validación en el formulario.
        
        Returns:
            bool: True si hay errores de validación visibles, False en caso contrario
        """
        validation_locators = [
            self.VALIDATION_NOMBRE,
            self.VALIDATION_APELLIDO,
            self.VALIDATION_TELEFONO,
            self.VALIDATION_TIPO
        ]
        
        for locator in validation_locators:
            if self.is_element_visible(locator, timeout=2):
                # Verificar que el mensaje no esté vacío
                try:
                    message = self.get_text(locator)
                    if message.strip():
                        return True
                except:
                    continue
        
        return False

    def get_validation_errors(self):
        """
        Obtiene todos los mensajes de error de validación visibles.
        
        Returns:
            dict: Diccionario con los campos como claves y mensajes de error como valores
        """
        errors = {}
        
        validation_fields = {
            'nombre': self.VALIDATION_NOMBRE,
            'apellido': self.VALIDATION_APELLIDO,
            'telefono': self.VALIDATION_TELEFONO,
            'tipo': self.VALIDATION_TIPO
        }
        
        for field, locator in validation_fields.items():
            message = self.get_validation_message(locator)
            if message.strip():
                errors[field] = message
        
        return errors

    def is_repartidor_registered(self):
        """
        Verifica si el repartidor fue registrado exitosamente.
        Se considera exitoso si:
        1. Hay un mensaje de éxito visible, O
        2. La URL cambió a la página de índice (indicando redirección exitosa)
        
        Returns:
            bool: True si el repartidor fue registrado, False en caso contrario
        """
        # Primero verificar si hay mensaje de éxito
        if self.is_success_message_displayed():
            return True
        
        # Si no hay mensaje, verificar si redirigió a la página de índice
        # (esto indica que el formulario se procesó correctamente)
        return self.is_on_index_page()

    def get_table_row_count(self):
        """
        Obtiene la cantidad de repartidores en la tabla (solo en Index).
        
        Returns:
            int: Número de filas en la tabla de repartidores
        """
        try:
            rows = self.find_elements(self.TABLE_ROWS)
            return len(rows)
        except:
            return 0

    def clear_form(self):
        """
        Limpia todos los campos del formulario.
        """
        self.enter_text(self.INPUT_NOMBRE, "")
        self.enter_text(self.INPUT_APELLIDO, "")
        self.enter_text(self.INPUT_TELEFONO, "")
        
        # Resetear el select a la opción vacía
        try:
            select_element = self.find_element(self.SELECT_TIPO)
            select = Select(select_element)
            select.select_by_index(0)  # Seleccionar la primera opción (vacía)
        except:
            pass
