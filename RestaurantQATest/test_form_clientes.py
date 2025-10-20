import csv
import pytest
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

DATA_FILE = "RestaurantQATest/data/clientes_tests.csv"

def load_cases():
    with open(DATA_FILE, newline='', encoding='utf-8') as f:
        return list(csv.DictReader(f))

@pytest.mark.parametrize("case", load_cases())
def test_registro_cliente(driver, base_url, case):
    driver.get(f"{base_url}/Clientes/Create")

    driver.find_element(By.NAME, "Cliente.Nombre").clear()
    driver.find_element(By.NAME, "Cliente.Nombre").send_keys(case["nombre"] or "")
    driver.find_element(By.NAME, "Cliente.Apellido").clear()
    driver.find_element(By.NAME, "Cliente.Apellido").send_keys(case["apellido"] or "")
    driver.find_element(By.NAME, "Cliente.Telefono").clear()
    driver.find_element(By.NAME, "Cliente.Telefono").send_keys(case["telefono"] or "")
    driver.find_element(By.NAME, "Cliente.Correo").clear()
    driver.find_element(By.NAME, "Cliente.Correo").send_keys(case["correo"] or "")

    driver.find_element(By.CSS_SELECTOR, "form button[type='submit']").click()

    wait = WebDriverWait(driver, 5)
    try:
        success = wait.until(EC.visibility_of_element_located((By.CSS_SELECTOR, ".alert-success")))
        resultado = "Aceptado"
    except:
        resultado = "Rechazado"

    assert resultado == case["esperado"], f"Fallo en {case['caso']} → Esperado {case['esperado']} pero fue {resultado}"
