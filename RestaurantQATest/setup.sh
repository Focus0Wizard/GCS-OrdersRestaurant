#!/bin/bash
# ============================================
# Script de Instalación y Ejecución Rápida
# Proyecto: RestaurantQA - Tests de Productos
# ============================================

echo "╔═══════════════════════════════════════════════════════════╗"
echo "║   INSTALACIÓN DE PRUEBAS AUTOMATIZADAS - PRODUCTOS      ║"
echo "╚═══════════════════════════════════════════════════════════╝"
echo ""

# Verificar Python
echo "📋 Verificando Python..."
if ! command -v python3 &> /dev/null; then
    echo "❌ Python 3 no está instalado. Por favor instálalo primero."
    exit 1
fi

PYTHON_VERSION=$(python3 --version)
echo "✅ $PYTHON_VERSION encontrado"
echo ""

# Crear entorno virtual
echo "🔧 Creando entorno virtual..."
if [ -d "venv" ]; then
    echo "⚠️  El entorno virtual ya existe. Eliminando..."
    rm -rf venv
fi

python3 -m venv venv
echo "✅ Entorno virtual creado"
echo ""

# Activar entorno virtual
echo "🔌 Activando entorno virtual..."
source venv/bin/activate
echo "✅ Entorno virtual activado"
echo ""

# Instalar dependencias
echo "📦 Instalando dependencias..."
pip install --upgrade pip > /dev/null 2>&1
pip install -r requirements.txt

if [ $? -eq 0 ]; then
    echo "✅ Dependencias instaladas correctamente"
else
    echo "❌ Error al instalar dependencias"
    exit 1
fi
echo ""

# Verificar instalación
echo "🔍 Verificando instalación..."
pytest --version > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo "✅ pytest instalado correctamente"
else
    echo "❌ pytest no se instaló correctamente"
    exit 1
fi

python3 -c "import selenium" > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo "✅ selenium instalado correctamente"
else
    echo "❌ selenium no se instaló correctamente"
    exit 1
fi
echo ""

# Crear directorio de reportes
echo "📁 Creando directorio de reportes..."
mkdir -p reports
echo "✅ Directorio de reportes creado"
echo ""

# Resumen
echo "╔═══════════════════════════════════════════════════════════╗"
echo "║              ✅ INSTALACIÓN COMPLETADA                    ║"
echo "╚═══════════════════════════════════════════════════════════╝"
echo ""
echo "📊 RESUMEN:"
echo "   • Python: $PYTHON_VERSION"
echo "   • Entorno virtual: venv/"
echo "   • Dependencias: instaladas"
echo "   • Reportes: reports/"
echo ""
echo "🚀 PRÓXIMOS PASOS:"
echo ""
echo "   1. Activar entorno virtual:"
echo "      source venv/bin/activate"
echo ""
echo "   2. Iniciar la aplicación (en otra terminal):"
echo "      cd ../RestaurantQA && dotnet run"
echo ""
echo "   3. Ejecutar las pruebas:"
echo "      pytest tests/test_productos.py -v"
echo ""
echo "   4. Ver reporte HTML:"
echo "      xdg-open reports/report.html"
echo ""
echo "📚 DOCUMENTACIÓN:"
echo "   • Guía rápida: cat QUICKSTART_PRODUCTOS.md"
echo "   • README completo: cat README_PRODUCTOS.md"
echo "   • Análisis técnico: cat PARTICIONES_PRODUCTOS.md"
echo ""
echo "✨ ¡Listo para comenzar a probar!"
echo ""
