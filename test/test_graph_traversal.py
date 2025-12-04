"""
Tests para algoritmos de exploración y búsqueda en grafos.
Cubre BFS, DFS y aplicaciones avanzadas.
"""

def cargar_grafo_prueba_lineal():
    """Crea grafo lineal: A-B-C-D"""
    grafo = {}
    grafo['A'] = [('B', 1.0)]
    grafo['B'] = [('A', 1.0), ('C', 1.0)]
    grafo['C'] = [('B', 1.0), ('D', 1.0)]
    grafo['D'] = [('C', 1.0)]
    return grafo

def cargar_grafo_prueba_arbol():
    """Crea grafo árbol: A tiene hijos B y C, B tiene hijos D y E"""
    grafo = {}
    grafo['A'] = [('B', 1.0), ('C', 1.0)]
    grafo['B'] = [('A', 1.0), ('D', 1.0), ('E', 1.0)]
    grafo['C'] = [('A', 1.0)]
    grafo['D'] = [('B', 1.0)]
    grafo['E'] = [('B', 1.0)]
    return grafo

def cargar_grafo_desconectado():
    """Crea grafo con 3 componentes: {A-B}, {C-D}, {E-F}"""
    grafo = {}
    grafo['A'] = [('B', 1.0)]
    grafo['B'] = [('A', 1.0)]
    grafo['C'] = [('D', 1.0)]
    grafo['D'] = [('C', 1.0)]
    grafo['E'] = [('F', 1.0)]
    grafo['F'] = [('E', 1.0)]
    return grafo

# ========================================
# IMPLEMENTACIONES AUXILIARES PARA TESTS
# ========================================

def bfs(grafo, inicio):
    """BFS simple para tests"""
    if inicio not in grafo:
        raise ValueError(f"El nodo {inicio} no existe en el grafo")
    
    visitados = set()
    resultado = []
    cola = [inicio]
    visitados.add(inicio)
    
    while cola:
        actual = cola.pop(0)
        resultado.append(actual)
        
        for vecino, peso in grafo.get(actual, []):
            if vecino not in visitados:
                visitados.add(vecino)
                cola.append(vecino)
    
    return resultado

def bfs_distancias(grafo, inicio):
    """BFS con cálculo de distancias"""
    distancias = {inicio: 0}
    cola = [inicio]
    
    while cola:
        actual = cola.pop(0)
        
        for vecino, peso in grafo.get(actual, []):
            if vecino not in distancias:
                distancias[vecino] = distancias[actual] + 1
                cola.append(vecino)
    
    return distancias

def bfs_camino_mas_corto(grafo, inicio, fin):
    """BFS que reconstruye camino más corto"""
    padre = {inicio: None}
    visitados = {inicio}
    cola = [inicio]
    encontrado = False
    
    while cola and not encontrado:
        actual = cola.pop(0)
        
        if actual == fin:
            encontrado = True
            break
        
        for vecino, peso in grafo.get(actual, []):
            if vecino not in visitados:
                visitados.add(vecino)
                padre[vecino] = actual
                cola.append(vecino)
    
    if not encontrado:
        return None
    
    # Reconstruir camino
    camino = []
    nodo = fin
    while nodo is not None:
        camino.append(nodo)
        nodo = padre.get(nodo)
    
    camino.reverse()
    return camino

def dfs_recursivo(grafo, inicio):
    """DFS recursivo simple"""
    if inicio not in grafo:
        raise ValueError(f"El nodo {inicio} no existe en el grafo")
    
    visitados = set()
    resultado = []
    
    def dfs_helper(nodo):
        visitados.add(nodo)
        resultado.append(nodo)
        
        for vecino, peso in grafo.get(nodo, []):
            if vecino not in visitados:
                dfs_helper(vecino)
    
    dfs_helper(inicio)
    return resultado

def dfs_iterativo(grafo, inicio):
    """DFS iterativo con pila"""
    if inicio not in grafo:
        raise ValueError(f"El nodo {inicio} no existe en el grafo")
    
    visitados = set()
    resultado = []
    pila = [inicio]
    
    while pila:
        actual = pila.pop()
        
        if actual in visitados:
            continue
        
        visitados.add(actual)
        resultado.append(actual)
        
        vecinos = [v for v, p in grafo.get(actual, [])]
        vecinos.reverse()
        
        for vecino in vecinos:
            if vecino not in visitados:
                pila.append(vecino)
    
    return resultado

def encontrar_componentes_conectadas(grafo):
    """Encuentra todas las componentes conectadas"""
    visitados = set()
    componentes = []
    
    def dfs_componente(nodo, componente):
        visitados.add(nodo)
        componente.append(nodo)
        
        for vecino, peso in grafo.get(nodo, []):
            if vecino not in visitados:
                dfs_componente(vecino, componente)
    
    for nodo in grafo.keys():
        if nodo not in visitados:
            componente = []
            dfs_componente(nodo, componente)
            componentes.append(componente)
    
    return componentes

# ========================================
# TESTS
# ========================================

def test_1_bfs_grafo_lineal():
    """Test 1: BFS en grafo lineal"""
    grafo = cargar_grafo_prueba_lineal()
    resultado = bfs(grafo, 'A')
    
    esperado = ['A', 'B', 'C', 'D']
    return resultado == esperado

def test_2_bfs_distancias():
    """Test 2: BFS cálculo de distancias"""
    grafo = cargar_grafo_prueba_arbol()
    distancias = bfs_distancias(grafo, 'A')
    
    return (distancias['A'] == 0 and 
            distancias['B'] == 1 and 
            distancias['C'] == 1 and 
            distancias['D'] == 2 and 
            distancias['E'] == 2)

def test_3_bfs_camino_mas_corto():
    """Test 3: BFS camino más corto"""
    grafo = cargar_grafo_prueba_arbol()
    camino = bfs_camino_mas_corto(grafo, 'A', 'D')
    
    return (camino is not None and 
            len(camino) == 3 and 
            camino[0] == 'A' and 
            camino[-1] == 'D')

def test_4_dfs_recursivo():
    """Test 4: DFS recursivo"""
    grafo = cargar_grafo_prueba_arbol()
    resultado = dfs_recursivo(grafo, 'A')
    
    return len(resultado) == 5 and resultado[0] == 'A'

def test_5_dfs_iterativo():
    """Test 5: DFS iterativo"""
    grafo = cargar_grafo_prueba_arbol()
    resultado = dfs_iterativo(grafo, 'A')
    
    return len(resultado) == 5 and resultado[0] == 'A'

def test_6_componentes_conectadas():
    """Test 6: Componentes conectadas"""
    grafo = cargar_grafo_desconectado()
    componentes = encontrar_componentes_conectadas(grafo)
    
    return len(componentes) == 3

def test_7_bfs_nodo_inexistente():
    """Test 7: BFS con nodo inexistente debe lanzar excepción"""
    grafo = cargar_grafo_prueba_lineal()
    
    try:
        bfs(grafo, 'Z')
        return False  # No debería llegar aquí
    except ValueError:
        return True  # Debe lanzar excepción

def test_8_dfs_nodo_inexistente():
    """Test 8: DFS con nodo inexistente debe lanzar excepción"""
    grafo = cargar_grafo_prueba_lineal()
    
    try:
        dfs_recursivo(grafo, 'Z')
        return False  # No debería llegar aquí
    except ValueError:
        return True  # Debe lanzar excepción

def test_9_camino_inexistente():
    """Test 9: BFS debe retornar None si no hay camino"""
    grafo = cargar_grafo_desconectado()
    camino = bfs_camino_mas_corto(grafo, 'A', 'C')
    
    return camino is None

def test_10_grafo_un_nodo():
    """Test 10: BFS/DFS con grafo de un solo nodo"""
    grafo = {'A': []}
    resultado_bfs = bfs(grafo, 'A')
    resultado_dfs = dfs_recursivo(grafo, 'A')
    
    return (resultado_bfs == ['A'] and 
            resultado_dfs == ['A'])

# ========================================
# EJECUCIÓN DE TESTS
# ========================================

def ejecutar_todos_los_tests():
    """Ejecuta todos los tests y muestra resultados"""
    print("\n" + "="*60)
    print("  EJECUCIÓN DE TESTS DE BFS Y DFS - PYTHON")
    print("="*60 + "\n")
    
    tests = [
        ("Test 1: BFS en grafo lineal", test_1_bfs_grafo_lineal),
        ("Test 2: BFS cálculo de distancias", test_2_bfs_distancias),
        ("Test 3: BFS camino más corto", test_3_bfs_camino_mas_corto),
        ("Test 4: DFS recursivo", test_4_dfs_recursivo),
        ("Test 5: DFS iterativo", test_5_dfs_iterativo),
        ("Test 6: Componentes conectadas", test_6_componentes_conectadas),
        ("Test 7: BFS nodo inexistente", test_7_bfs_nodo_inexistente),
        ("Test 8: DFS nodo inexistente", test_8_dfs_nodo_inexistente),
        ("Test 9: Camino inexistente", test_9_camino_inexistente),
        ("Test 10: Grafo de un nodo", test_10_grafo_un_nodo)
    ]
    
    pasados = 0
    total = len(tests)
    
    for nombre, test_func in tests:
        try:
            if test_func():
                print(f"{nombre}: [✓ PASS]")
                pasados += 1
            else:
                print(f"{nombre}: [✗ FAIL]")
        except Exception as e:
            print(f"{nombre}: [✗ FAIL] - Error: {e}")
    
    print("\n" + "="*60)
    print(f"RESULTADO: {pasados}/{total} tests pasados")
    print("="*60 + "\n")

if __name__ == "__main__":
    ejecutar_todos_los_tests()