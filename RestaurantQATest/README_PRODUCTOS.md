# 🧪 Suite de Pruebas Automatizadas - RestaurantQA

## 📋 Descripción

Suite completa de pruebas automatizadas para la aplicación **RestaurantQA**, implementada con **Python**, **Selenium WebDriver** y **pytest**. Las pruebas están basadas en el diseño de **particiones equivalentes** y siguen el patrón **Page Object Model (POM)** para maximizar la mantenibilidad y reutilización del código.

---

## 🏗️ Estructura del Proyecto

```
RestaurantQATest/
│
├── 📁 Data/                          # Datos de prueba en CSV
│   ├── productos_tests.csv          # 33 casos de prueba para productos
│   └── clientes_tests.csv           # 36 casos de prueba para clientes
│
├── 📁 pages/                         # Page Object Model (POM)
│   ├── __init__.py
│   ├── base_page.py                 # Clase base con métodos comunes
│   └── producto_page.py             # Page Object para productos
│
├── 📁 tests/                         # Suite de pruebas
│   ├── __init__.py
│   └── test_productos.py            # Tests automatizados de productos
│
├── 📁 reports/                       # Reportes generados (auto-creado)
│   ├── report.html                  # Reporte HTML de pytest
│   └── pytest.log                   # Logs de ejecución
│
├── 📄 conftest.py                    # Configuración de fixtures de pytest
├── 📄 pytest.ini                     # Configuración de pytest
├── 📄 requirements.txt               # Dependencias del proyecto
└── 📄 README.md                      # Esta documentación
```

---

## 🎯 Módulos de Prueba

### ✅ Módulo: Productos

**Archivo de pruebas:** `tests/test_productos.py`  
**Datos de prueba:** `Data/productos_tests.csv`  
**Page Object:** `pages/producto_page.py`

#### Casos de Prueba: 33 Total

**Particiones Válidas (2 casos):**
- ✅ **PR1**: Valores mínimos permitidos (Precio: 0.01, Stock: 0)
- ✅ **PR2**: Valores máximos permitidos (Precio: 1000, Stock: 150, Descripción: 100 chars)

**Particiones Inválidas (31 casos):**
- ❌ **PR3-PR33**: Diversos casos que incluyen:
  - Nombre vacío
  - Nombre excede 20 caracteres
  - Nombre con caracteres especiales (@, números)
  - Precio = 0 o fuera del rango (0.01 - 1000)
  - Stock negativo o > 150
  - Descripción < 5 caracteres o > 100 caracteres

#### Validaciones Cubiertas:
- **Nombre**: Required, Length(4-20 caracteres)
- **Precio**: Range(0.01 - 1000)
- **Stock**: Range(0 - 150)
- **Descripción**: Required, Length(5-100 caracteres)
- **Categoría ID**: Required, > 0

---

## 🚀 Instalación y Configuración

### 1️⃣ Pre-requisitos

- **Python 3.8+** instalado
- **Google Chrome** instalado
- **Aplicación RestaurantQA** corriendo en `http://localhost:5020`

### 2️⃣ Instalación de Dependencias

```bash
# Navegar al directorio del proyecto
cd RestaurantQATest

# Crear entorno virtual (opcional pero recomendado)
python -m venv venv

# Activar entorno virtual
# En Linux/Mac:
source venv/bin/activate
# En Windows:
venv\Scripts\activate

# Instalar dependencias
pip install -r requirements.txt
```

### 3️⃣ Verificar Instalación

```bash
# Verificar que pytest está instalado
pytest --version

# Verificar que selenium está instalado
python -c "import selenium; print(selenium.__version__)"
```

---

## ▶️ Ejecución de Pruebas

### Ejecutar TODOS los tests

```bash
pytest
```

### Ejecutar tests de un módulo específico

```bash
# Solo tests de productos
pytest -m productos

# Solo tests de clientes
pytest -m clientes
```

### Ejecutar tests de smoke (críticos)

```bash
pytest -m smoke
```

### Ejecutar un archivo específico

```bash
pytest tests/test_productos.py
```

### Ejecutar con reporte HTML

```bash
pytest --html=reports/report.html --self-contained-html
```

### Ejecutar en modo verbose (más detalles)

```bash
pytest -v
```

### Ejecutar tests en paralelo (más rápido)

```bash
pytest -n auto
```

### Ejecutar con output detallado

```bash
pytest -vv -s
```

---

## 📊 Interpretación de Resultados

### Ejemplo de Salida Exitosa

```
tests/test_productos.py::test_registro_producto[PR1] ✅ PASSED
tests/test_productos.py::test_registro_producto[PR2] ✅ PASSED
tests/test_productos.py::test_registro_producto[PR3] ✅ PASSED

==================== 33 passed in 45.23s ====================
```

### Ejemplo de Salida con Fallo

```
tests/test_productos.py::test_registro_producto[PR5] ❌ FAILED

FAILURES
________________________ test_registro_producto[PR5] ________________________

    ❌ Caso PR5 FALLÓ: Se esperaba que el producto fuera ACEPTADO pero fue RECHAZADO.
       Datos: Nombre='Hamburguesa', Precio=25.5, Stock=50, Descripción='Rico'
       Partición: Válida - Valores intermedios
       Errores de validación: {'descripcion': 'La descripcion del producto no puede pasar de los 20 caracteres y no ser menos a 4.'}
```

---

## 📈 Reportes

### Reporte HTML

Después de ejecutar las pruebas, se genera un reporte HTML interactivo en:

```
reports/report.html
```

Abrir con navegador para ver:
- ✅ Tests pasados
- ❌ Tests fallidos
- ⚠️ Tests con warnings
- 📊 Gráficos y estadísticas
- 🕐 Tiempo de ejecución

### Logs Detallados

Los logs de ejecución se guardan en:

```
reports/pytest.log
```

---

## 🛠️ Mantenimiento de Tests

### Agregar Nuevos Casos de Prueba

1. **Editar el CSV de datos:**
   - Abrir `Data/productos_tests.csv`
   - Agregar una nueva fila con los datos del caso

2. **No se requiere modificar código:**
   - Las pruebas son parametrizadas
   - Automáticamente tomarán los nuevos casos

### Modificar Validaciones

Si cambian las validaciones del backend:

1. **Actualizar el Page Object:**
   - Editar `pages/producto_page.py`
   - Modificar localizadores si cambió el HTML

2. **Actualizar casos de prueba:**
   - Editar `Data/productos_tests.csv`
   - Ajustar valores esperados según nuevas validaciones

---

## 🏆 Mejores Prácticas Implementadas

### ✅ Page Object Model (POM)

- **Separación de responsabilidades:** Lógica de página vs. lógica de prueba
- **Reutilización:** Métodos comunes en `BasePage`
- **Mantenibilidad:** Cambios en UI solo afectan Page Objects

### ✅ Data-Driven Testing

- **CSV como fuente de datos:** Fácil de editar por no-programadores
- **Pruebas parametrizadas:** Un test, múltiples casos
- **Cobertura completa:** Todas las particiones equivalentes

### ✅ Asserts Claros

```python
assert producto_page.is_producto_registered(), \
    f"❌ Caso {case['caso']} FALLÓ: Se esperaba ACEPTADO pero fue RECHAZADO.\n" \
    f"   Datos: Nombre='{nombre}', Precio={precio}..."
```

### ✅ Configuración Centralizada

- **conftest.py:** Fixtures compartidas
- **pytest.ini:** Configuración de pytest
- **requirements.txt:** Dependencias versionadas

### ✅ Marcadores de Tests

```python
@pytest.mark.productos  # Filtrar por módulo
@pytest.mark.smoke      # Tests críticos
@pytest.mark.regression # Suite completa
```

---

## 🐛 Troubleshooting

### Error: `ModuleNotFoundError: No module named 'selenium'`

**Solución:**
```bash
pip install -r requirements.txt
```

### Error: `WebDriverException: Chrome not reachable`

**Solución:**
- Verificar que Google Chrome está instalado
- Actualizar webdriver: `pip install --upgrade webdriver-manager`

### Error: `Connection refused (localhost:5020)`

**Solución:**
- Verificar que la aplicación RestaurantQA está corriendo
- Comprobar el puerto en `conftest.py` fixture `base_url`

### Tests muy lentos

**Solución:**
```bash
# Ejecutar en paralelo
pytest -n auto
```

### No se generan reportes

**Solución:**
```bash
# Crear directorio de reportes
mkdir -p reports

# Ejecutar con reporte explícito
pytest --html=reports/report.html --self-contained-html
```

---

## 📚 Recursos Adicionales

### Documentación Oficial

- **Selenium:** https://selenium-python.readthedocs.io/
- **Pytest:** https://docs.pytest.org/
- **Page Object Pattern:** https://selenium-python.readthedocs.io/page-objects.html

### Comandos Útiles

```bash
# Ver todos los marcadores disponibles
pytest --markers

# Ver fixtures disponibles
pytest --fixtures

# Ejecutar solo tests que fallaron la última vez
pytest --lf

# Detener al primer fallo
pytest -x

# Mostrar 10 tests más lentos
pytest --durations=10
```

---

## 👥 Contribuciones

### Agregar un Nuevo Módulo de Pruebas

1. **Crear Page Object:**
   ```python
   # pages/nuevo_modulo_page.py
   from pages.base_page import BasePage
   
   class NuevoModuloPage(BasePage):
       # Definir localizadores y métodos
   ```

2. **Crear archivo de tests:**
   ```python
   # tests/test_nuevo_modulo.py
   import pytest
   from pages.nuevo_modulo_page import NuevoModuloPage
   
   @pytest.mark.nuevo_modulo
   def test_caso_1(driver, base_url):
       # Implementar test
   ```

3. **Agregar marcador en pytest.ini:**
   ```ini
   markers =
       nuevo_modulo: Tests del nuevo módulo
   ```

---

## 📝 Notas Importantes

### ⚠️ Consideraciones

1. **Base de datos:** Las pruebas insertarán datos reales en la BD
   - Usar BD de pruebas, no producción
   - Considerar limpiar datos después de cada ejecución

2. **Headless mode:** Por defecto, Chrome corre en modo headless
   - Para ver el navegador, editar `conftest.py`
   - Comentar línea: `opts.add_argument("--headless=new")`

3. **Timeouts:** Ajustar según velocidad de la máquina
   - Editar `driver.implicitly_wait(5)` en `conftest.py`

---

## 📞 Soporte

Para problemas o preguntas:
- Revisar sección de **Troubleshooting**
- Consultar logs en `reports/pytest.log`
- Verificar que la aplicación esté corriendo

---

## 🎓 Conceptos de Testing Aplicados

### Particiones Equivalentes

Dividir el dominio de entrada en clases donde se espera que el sistema se comporte de manera similar:

- **Partición Válida:** Datos que el sistema debe aceptar
- **Partición Inválida:** Datos que el sistema debe rechazar

### Valores Límite (Boundary Values)

Probar en los bordes de las particiones:

- **PR1:** Valores mínimos (0.01, 0, 5 chars)
- **PR2:** Valores máximos (1000, 150, 100 chars)

### Cobertura de Casos

- **2 casos válidos** (particiones aceptables)
- **31 casos inválidos** (particiones rechazables)
- **Cobertura total:** 100% de particiones identificadas

---

## ✨ Características Destacadas

✅ **Arquitectura POM** - Código mantenible y escalable  
✅ **Data-Driven Testing** - Casos de prueba en CSV  
✅ **Pruebas Parametrizadas** - DRY (Don't Repeat Yourself)  
✅ **Reportes HTML** - Visualización clara de resultados  
✅ **Logs Detallados** - Debugging facilitado  
✅ **Marcadores Personalizados** - Ejecución selectiva  
✅ **Fixtures Reutilizables** - Setup/Teardown automático  
✅ **Asserts Descriptivos** - Mensajes de error claros  

---

**Versión:** 1.0.0  
**Última actualización:** Octubre 2025  
**Licencia:** MIT  
