"""
Page Object Model para la página de gestión de productos.
Contiene los localizadores y métodos para interactuar con el formulario de productos.
"""
from selenium.webdriver.common.by import By
from pages.base_page import BasePage


class ProductoPage(BasePage):
    """
    Page Object para la página de productos (/Productos/Index).
    Encapsula la interacción con el formulario de registro/edición de productos.
    """

    # ==================== LOCALIZADORES ====================
    # Campos del formulario
    INPUT_NOMBRE = (By.NAME, "Producto.Nombre")
    INPUT_PRECIO = (By.NAME, "Producto.Precio")
    INPUT_STOCK = (By.NAME, "Producto.Stock")
    INPUT_DESCRIPCION = (By.NAME, "Producto.Descripcion")
    INPUT_CATEGORIA_ID = (By.NAME, "Producto.CategoriaId")
    INPUT_ID = (By.NAME, "Producto.Id")
    
    # Mensajes de validación
    VALIDATION_NOMBRE = (By.CSS_SELECTOR, "span[data-valmsg-for='Producto.Nombre']")
    VALIDATION_PRECIO = (By.CSS_SELECTOR, "span[data-valmsg-for='Producto.Precio']")
    VALIDATION_STOCK = (By.CSS_SELECTOR, "span[data-valmsg-for='Producto.Stock']")
    VALIDATION_DESCRIPCION = (By.CSS_SELECTOR, "span[data-valmsg-for='Producto.Descripcion']")
    VALIDATION_CATEGORIA = (By.CSS_SELECTOR, "span[data-valmsg-for='Producto.CategoriaId']")
    
    # Botones
    BTN_SUBMIT = (By.CSS_SELECTOR, "form button[type='submit']")
    
    # Mensajes de resultado
    ALERT_SUCCESS = (By.CSS_SELECTOR, ".alert-success")
    ALERT_DANGER = (By.CSS_SELECTOR, ".alert-danger")
    
    # Tabla de productos
    TABLE_PRODUCTOS = (By.CSS_SELECTOR, "table.table")
    TABLE_ROWS = (By.CSS_SELECTOR, "table.table tbody tr")

    def __init__(self, driver):
        """
        Inicializa la página de productos.
        
        Args:
            driver: Instancia de WebDriver de Selenium
        """
        super().__init__(driver)

    def navigate(self, base_url):
        """
        Navega a la página de productos.
        
        Args:
            base_url: URL base de la aplicación
        """
        self.navigate_to(f"{base_url}/Productos/Index")

    def fill_form(self, nombre=None, precio=None, stock=None, descripcion=None, categoria_id=1):
        """
        Rellena el formulario de producto con los datos proporcionados.
        
        Args:
            nombre: Nombre del producto (opcional)
            precio: Precio del producto (opcional)
            stock: Stock del producto (opcional)
            descripcion: Descripción del producto (opcional)
            categoria_id: ID de la categoría (por defecto 1)
        """
        # Limpiar campos primero
        self.enter_text(self.INPUT_NOMBRE, "")
        self.enter_text(self.INPUT_PRECIO, "")
        self.enter_text(self.INPUT_STOCK, "")
        self.enter_text(self.INPUT_DESCRIPCION, "")
        
        # Llenar con nuevos valores
        if nombre is not None:
            self.enter_text(self.INPUT_NOMBRE, nombre)
        
        if precio is not None:
            self.enter_text(self.INPUT_PRECIO, precio)
        
        if stock is not None:
            self.enter_text(self.INPUT_STOCK, stock)
        
        if descripcion is not None:
            self.enter_text(self.INPUT_DESCRIPCION, descripcion)
        
        self.enter_text(self.INPUT_CATEGORIA_ID, categoria_id)

    def submit_form(self):
        """
        Envía el formulario haciendo clic en el botón de submit.
        """
        self.click(self.BTN_SUBMIT)

    def create_producto(self, nombre, precio, stock, descripcion, categoria_id=1):
        """
        Crea un nuevo producto llenando el formulario y enviándolo.
        
        Args:
            nombre: Nombre del producto
            precio: Precio del producto
            stock: Stock del producto
            descripcion: Descripción del producto
            categoria_id: ID de la categoría (por defecto 1)
        """
        self.fill_form(nombre, precio, stock, descripcion, categoria_id)
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
        Verifica si la URL actual es la página de índice de productos.
        Esto indica que el formulario se envió correctamente y redirigió.
        
        Returns:
            bool: True si está en la página de índice, False en caso contrario
        """
        current_url = self.get_current_url()
        # Si la URL termina en /Productos/Index o /Productos, es la página de índice
        return "/Productos/Index" in current_url or current_url.endswith("/Productos")

    def has_validation_errors(self):
        """
        Verifica si hay errores de validación en el formulario.
        
        Returns:
            bool: True si hay errores de validación visibles, False en caso contrario
        """
        validation_locators = [
            self.VALIDATION_NOMBRE,
            self.VALIDATION_PRECIO,
            self.VALIDATION_STOCK,
            self.VALIDATION_DESCRIPCION,
            self.VALIDATION_CATEGORIA
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
            'precio': self.VALIDATION_PRECIO,
            'stock': self.VALIDATION_STOCK,
            'descripcion': self.VALIDATION_DESCRIPCION,
            'categoria': self.VALIDATION_CATEGORIA
        }
        
        for field, locator in validation_fields.items():
            message = self.get_validation_message(locator)
            if message.strip():
                errors[field] = message
        
        return errors

    def is_producto_registered(self):
        """
        Verifica si el producto fue registrado exitosamente.
        Se considera exitoso si:
        1. Hay un mensaje de éxito visible, O
        2. La URL cambió a la página de índice (indicando redirección exitosa)
        
        Returns:
            bool: True si el producto fue registrado, False en caso contrario
        """
        # Primero verificar si hay mensaje de éxito
        if self.is_success_message_displayed():
            return True
        
        # Si no hay mensaje, verificar si redirigió a la página de índice
        # (esto indica que el formulario se procesó correctamente)
        return self.is_on_index_page()

    def get_table_row_count(self):
        """
        Obtiene la cantidad de productos en la tabla.
        
        Returns:
            int: Número de filas en la tabla de productos
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
        self.enter_text(self.INPUT_PRECIO, "")
        self.enter_text(self.INPUT_STOCK, "")
        self.enter_text(self.INPUT_DESCRIPCION, "")
        self.enter_text(self.INPUT_CATEGORIA_ID, "")
