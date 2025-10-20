# 🎯 INSTRUCCIONES DE USO - PRUEBAS DE PRODUCTOS

## ⚡ INSTALACIÓN RÁPIDA (Opción 1)

```bash
# Ejecutar script automático
./setup.sh
```

## 🔧 INSTALACIÓN MANUAL (Opción 2)

```bash
# 1. Crear entorno virtual
python3 -m venv venv

# 2. Activar entorno virtual
source venv/bin/activate

# 3. Instalar dependencias
pip install -r requirements.txt

# 4. Crear directorio de reportes
mkdir -p reports
```

---

## ▶️ EJECUTAR PRUEBAS

### Antes de ejecutar las pruebas:

```bash
# IMPORTANTE: Iniciar la aplicación en otra terminal
cd ../RestaurantQA
dotnet run

# La aplicación debe estar corriendo en http://localhost:5020
```

### Ejecutar todos los tests de productos:

```bash
pytest tests/test_productos.py -v
```

### Ejecutar solo tests con marca 'smoke':

```bash
pytest -m smoke
```

### Generar reporte HTML:

```bash
pytest --html=reports/report.html --self-contained-html
```

### Ejecutar con más detalles:

```bash
pytest tests/test_productos.py -vv -s
```

### Ejecutar en paralelo (más rápido):

```bash
pytest -n auto
```

---

## 📊 VERIFICAR RESULTADOS

### Ver reporte HTML:

```bash
# Linux
xdg-open reports/report.html

# macOS
open reports/report.html

# Windows
start reports/report.html
```

### Ver logs:

```bash
cat reports/pytest.log
```

---

## 📚 LEER DOCUMENTACIÓN

### Guía Rápida (5 minutos):

```bash
cat QUICKSTART_PRODUCTOS.md
```

### Documentación Completa:

```bash
cat README_PRODUCTOS.md
```

### Análisis Técnico de Particiones:

```bash
cat PARTICIONES_PRODUCTOS.md
```

### Resumen de Implementación:

```bash
cat RESUMEN_IMPLEMENTACION.md
```

---

## 🔍 CASOS DE PRUEBA

### Ver casos de prueba:

```bash
cat Data/productos_tests.csv
```

### Ejecutar un caso específico:

```bash
# Ejecutar solo el caso PR1
pytest tests/test_productos.py -k "PR1"

# Ejecutar casos PR1 a PR5
pytest tests/test_productos.py::test_registro_producto
```

---

## 🎓 ESTRUCTURA DEL CÓDIGO

### Page Objects (pages/):

```
base_page.py       → Métodos comunes para todas las páginas
producto_page.py   → Métodos específicos del formulario de productos
```

### Tests (tests/):

```
test_productos.py  → 33 casos de prueba automatizados
```

### Configuración:

```
conftest.py        → Fixtures de pytest (driver, base_url, setup/teardown)
pytest.ini         → Configuración de pytest
requirements.txt   → Dependencias del proyecto
```

---

## 📈 RESUMEN DE CASOS

```
Total:      33 casos
Válidos:     3 casos (PR1, PR2, PR5)
Inválidos:  30 casos (PR3-PR4, PR6-PR33)
```

### Particiones cubiertas:

- ✅ Nombre: Vacío, corto, largo, caracteres especiales
- ✅ Precio: Cero, bajo límite, sobre límite
- ✅ Stock: Negativo, sobre límite
- ✅ Descripción: Vacía, corta, larga

---

## 🐛 TROUBLESHOOTING

### Error: "ModuleNotFoundError: No module named 'selenium'"

```bash
pip install -r requirements.txt
```

### Error: "Connection refused"

```bash
# Verificar que la app esté corriendo
cd ../RestaurantQA && dotnet run
```

### Error: "ChromeDriver"

```bash
pip install --upgrade webdriver-manager
```

---

## ✨ PRÓXIMOS PASOS

1. ✅ Instalar dependencias → `./setup.sh` o manual
2. ✅ Iniciar aplicación → `dotnet run` en otra terminal
3. ✅ Ejecutar pruebas → `pytest tests/test_productos.py -v`
4. ✅ Ver resultados → `xdg-open reports/report.html`
5. ✅ Leer documentación → `cat README_PRODUCTOS.md`

---

## 📞 NECESITAS AYUDA?

1. **Lee la documentación:**
   - QUICKSTART_PRODUCTOS.md → Inicio rápido
   - README_PRODUCTOS.md → Guía completa
   - PARTICIONES_PRODUCTOS.md → Análisis técnico

2. **Revisa los logs:**
   - reports/pytest.log → Logs detallados
   - reports/report.html → Reporte visual

3. **Verifica el código:**
   - pages/ → Page Objects con docstrings
   - tests/ → Tests con comentarios explicativos

---

**¡Listo para comenzar a probar! 🚀**
