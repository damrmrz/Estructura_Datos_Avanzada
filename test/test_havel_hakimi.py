def es_secuencia_grafica(grados):
    """
    Valida secuencia gráfica con Havel-Hakimi.
    Complejidad: O(n² log n) por reordenamiento en cada iteración.
    """
    if not grados:
        return True
    
    # Crear copia para no modificar original
    seq = sorted(grados, reverse=True)
    
    # Verificar suma par y máx grado
    suma_total = sum(seq)
    if suma_total % 2 != 0 or seq[0] >= len(seq):
        return False
    
    while seq:
        seq.sort(reverse=True)
        
        d1 = seq.pop(0)
        
        if d1 == 0:
            return True
        
        if d1 > len(seq):
            return False
        
        # Restar 1 de los siguientes d1
        for i in range(d1):
            seq[i] -= 1
            if seq[i] < 0:
                return False
    
    return True

def validar_consistencia(grafo):
    """
    Verifica consistencia: suma de grados debe ser par en grafo no dirigido.
    """
    if len(grafo) == 0:
        return True
    
    grado_total = 0
    for vertice in grafo:
        if vertice in grafo:
            grado_total += len(grafo[vertice])
    
    # En grafo no dirigido, suma de grados = 2 * |aristas|
    return grado_total % 2 == 0

def extraer_secuencia_grados(grafo):
    """
    Extrae secuencia de grados del grafo.
    Útil para validar el mapa urbano como secuencia gráfica.
    """
    grados = []
    for vertice in grafo:
        if vertice in grafo:
            grados.append(len(grafo[vertice]))
    
    grados.sort(reverse=True)
    return grados

# === CASOS DE PRUEBA OFICIALES ===

def ejecutar_pruebas():
    """Ejecuta los 10 casos de prueba oficiales."""
    print("\n" + "="*60)
    print("  PRUEBAS DE HAVEL-HAKIMI - 10 CASOS OFICIALES")
    print("="*60 + "\n")
    
    casos_prueba = [
        # CASOS VÁLIDOS
        ([4, 3, 3, 2, 2, 2, 1, 1], True, "Suma=18 (par), max=4≤7, converge a ceros"),
        ([3, 2, 2, 1], True, "Ejemplo del documento, converge correctamente"),
        ([4, 3, 3, 2, 2, 2], True, "n=6, suma=16 (par), max=4≤5"),
        ([0, 0, 0, 0], True, "Grafo vacío (sin aristas)"),
        ([3, 3, 3, 3], True, "Grafo completo K₄ (todos conectados)"),
        
        # CASOS INVÁLIDOS
        ([3, 3, 3, 1], False, "Reduce a [2,2,0] → [1,-1] (negativo en paso 2)"),
        ([5, 5, 4, 3, 2, 1], False, "Suma=20 (par), pero estructura imposible"),
        ([3, 2, 1], False, "Early exit: max=3 > n-1=2 (falla chequeo inicial)"),
        ([6, 1, 1, 1, 1, 1, 1], False, "n=7, estructura imposible"),
        ([5, 3, 2, 2, 1], False, "Suma=13 (impar) → imposible en grafo no dirigido")
    ]
    
    aprobados = 0
    total = len(casos_prueba)
    
    for i in range(len(casos_prueba)):
        secuencia, esperado, razon = casos_prueba[i]
        resultado = es_secuencia_grafica(secuencia)
        correcto = resultado == esperado
        
        estado = "[✓ PASS]" if correcto else "[✗ FAIL]"
        print(f"Caso {i + 1}: {estado}")
        print(f"  Secuencia: {secuencia}")
        print(f"  Esperado: {esperado} | Obtenido: {resultado}")
        print(f"  Razón: {razon}")
        print()
        
        if correcto:
            aprobados += 1
    
    print("="*60)
    print(f"RESULTADOS FINALES: {aprobados}/{total} casos correctos")
    
    if aprobados == total:
        print("¡TODOS LOS TESTS PASARON EXITOSAMENTE!")
    else:
        print("ADVERTENCIA: Algunos tests fallaron. Revisar implementación.")
    print("="*60 + "\n")

if __name__ == "__main__":
    ejecutar_pruebas()