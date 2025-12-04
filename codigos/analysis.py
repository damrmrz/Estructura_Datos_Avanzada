import os

def cargar_grafo(ruta_archivo, es_dirigido=True):
    """Carga un grafo desde un archivo de texto."""
    grafo = {}
    
    if not os.path.exists(ruta_archivo):
        print(f"Error: El archivo '{ruta_archivo}' no existe.")
        return grafo
    
    try:
        archivo = open(ruta_archivo, 'r', encoding='utf-8')
        lineas = archivo.readlines()
        archivo.close()
        
        for linea in lineas:
            linea = linea.strip()
            
            # Ignorar lineas vacias
            if not linea:
                continue
            
            partes = linea.split()
            if len(partes) < 2:
                continue
            
            origen = partes[0]
            destino = partes[1]
            
            # Obtener el peso si existe
            if len(partes) >= 3:
                try:
                    peso = float(partes[2])
                except:
                    peso = 1.0
            else:
                peso = 1.0
            
            # Agregar vertice origen si no existe
            if origen not in grafo:
                grafo[origen] = []
            
            # Agregar la arista
            grafo[origen].append((destino, peso))
            
            # Si es no dirigido, agregar la arista inversa
            if not es_dirigido:
                if destino not in grafo:
                    grafo[destino] = []
                grafo[destino].append((origen, peso))
                    
    except Exception as e:
        print(f"Error al leer el archivo: {e}")
    
    return grafo

def obtener_vecinos(grafo, vertice):
    """Obtiene los vecinos de un vertice."""
    if vertice in grafo:
        return grafo[vertice]
    else:
        return []

def existe_arista(grafo, desde, hasta):
    """Verifica si existe una arista entre dos vertices."""
    if desde not in grafo:
        return False
    
    vecinos = grafo[desde]
    for vecino, peso in vecinos:
        if vecino == hasta:
            return True
    return False

def calcular_grado_salida(grafo, vertice):
    """Calcula cuantas aristas salen de un vertice."""
    if vertice in grafo:
        return len(grafo[vertice])
    else:
        return 0

def calcular_grado_entrada(grafo, vertice):
    """Calcula cuantas aristas llegan a un vertice."""
    contador = 0
    
    for v in grafo:
        vecinos = grafo[v]
        for vecino, peso in vecinos:
            if vecino == vertice:
                contador = contador + 1
    
    return contador

def obtener_todos_vertices(grafo):
    """Obtiene una lista de todos los vertices del grafo."""
    vertices = []
    
    # Agregar todos los vertices que tienen aristas salientes
    for v in grafo:
        if v not in vertices:
            vertices.append(v)
    
    # Agregar vertices que solo tienen aristas entrantes
    for v in grafo:
        vecinos = grafo[v]
        for vecino, peso in vecinos:
            if vecino not in vertices:
                vertices.append(vecino)
    
    return vertices

def contar_aristas(grafo, es_dirigido=True):
    """Cuenta el total de aristas en el grafo."""
    total = 0
    
    for v in grafo:
        total = total + len(grafo[v])
    
    if not es_dirigido:
        total = total // 2
    
    return total

def mostrar_grafo(grafo, tipo_grafo, es_dirigido=True):
    """Muestra la informacion del grafo."""
    print("\n" + "="*60)
    print(f"  GRAFO {tipo_grafo}")
    print("="*60)
    
    if len(grafo) == 0:
        print("El grafo esta vacio")
        return
    
    vertices = obtener_todos_vertices(grafo)
    vertices.sort()
    
    num_vertices = len(vertices)
    num_aristas = contar_aristas(grafo, es_dirigido)
    
    print(f"\nNumero de vertices: {num_vertices}")
    print(f"Numero de aristas: {num_aristas}")
    
    print("\nInformacion de cada vertice:")
    for vertice in vertices:
        grado_salida = calcular_grado_salida(grafo, vertice)
        grado_entrada = calcular_grado_entrada(grafo, vertice)
        
        if es_dirigido:
            print(f"\n  Vertice: {vertice}")
            print(f"    Grado de salida: {grado_salida}")
            print(f"    Grado de entrada: {grado_entrada}")
        else:
            print(f"\n  Vertice: {vertice}")
            print(f"    Grado: {grado_salida}")
        
        vecinos = obtener_vecinos(grafo, vertice)
        if len(vecinos) > 0:
            print(f"    Vecinos:", end=" ")
            for i in range(len(vecinos)):
                vecino, peso = vecinos[i]
                if i < len(vecinos) - 1:
                    print(f"{vecino}({peso:.1f})", end=", ")
                else:
                    print(f"{vecino}({peso:.1f})")

def encontrar_vertice_mas_conectado(grafo):
    """Encuentra el vertice con mas conexiones."""
    vertices = obtener_todos_vertices(grafo)
    
    if len(vertices) == 0:
        return None
    
    vertice_max = vertices[0]
    grado_max = 0
    
    for vertice in vertices:
        grado_total = calcular_grado_salida(grafo, vertice) + calcular_grado_entrada(grafo, vertice)
        
        if grado_total > grado_max:
            grado_max = grado_total
            vertice_max = vertice
    
    return vertice_max, grado_max

def buscar_camino(grafo, inicio, fin):
    """Busca un camino entre dos vertices usando BFS."""
    if inicio not in grafo:
        return []
    
    visitados = []
    cola = []
    
    # Inicializar con el vertice inicial
    cola.append((inicio, [inicio]))
    visitados.append(inicio)
    
    while len(cola) > 0:
        vertice_actual, camino = cola.pop(0)
        
        if vertice_actual == fin:
            return camino
        
        vecinos = obtener_vecinos(grafo, vertice_actual)
        for vecino, peso in vecinos:
            if vecino not in visitados:
                visitados.append(vecino)
                nuevo_camino = camino.copy()
                nuevo_camino.append(vecino)
                cola.append((vecino, nuevo_camino))
    
    return []

def probar_conexiones(grafo, es_dirigido=True):
    """Hace algunas pruebas de conectividad en el grafo."""
    print("\n" + "="*60)
    print("  PRUEBAS DE CONECTIVIDAD")
    print("="*60)
    
    if len(grafo) == 0:
        print("El grafo esta vacio")
        return
    
    vertices = obtener_todos_vertices(grafo)
    vertices.sort()
    
    if len(vertices) >= 2:
        v1 = vertices[0]
        v2 = vertices[1]
        
        print(f"\nVerificando si existe arista de {v1} a {v2}:")
        if existe_arista(grafo, v1, v2):
            print(f"  SI existe la arista {v1} -> {v2}")
        else:
            print(f"  NO existe la arista {v1} -> {v2}")
        
        if not es_dirigido:
            print(f"\nVerificando si existe arista de {v2} a {v1}:")
            if existe_arista(grafo, v2, v1):
                print(f"  SI existe la arista {v2} -> {v1}")
            else:
                print(f"  NO existe la arista {v2} -> {v1}")
    
    if len(vertices) >= 2:
        inicio = vertices[0]
        fin = vertices[len(vertices) - 1]
        
        print(f"\nBuscando camino de {inicio} a {fin}:")
        camino = buscar_camino(grafo, inicio, fin)
        
        if len(camino) > 0:
            print(f"  Camino encontrado: ", end="")
            for i in range(len(camino)):
                if i < len(camino) - 1:
                    print(f"{camino[i]} -> ", end="")
                else:
                    print(f"{camino[i]}")
        else:
            print(f"  No existe camino")
    
    vertice_max, grado_max = encontrar_vertice_mas_conectado(grafo)
    print(f"\nVertice mas conectado: {vertice_max} con grado total de {grado_max}")

# Programa principal
print("="*60)
print("  ANALIZADOR DE GRAFOS")
print("="*60)

# Obtener la ruta de los archivos
directorio_actual = os.path.dirname(os.path.abspath(__file__))
directorio_datos = os.path.join(directorio_actual, "..", "datos")

print(f"\nBuscando archivos en: {os.path.abspath(directorio_datos)}")

# Archivo 1: Grafo no dirigido
archivo1 = os.path.join(directorio_datos, "edges_undirected.txt")
print(f"\nArchivo 1: {archivo1}")
print(f"Existe? {os.path.exists(archivo1)}")

if os.path.exists(archivo1):
    print("\nCargando grafo no dirigido...")
    grafo1 = cargar_grafo(archivo1, es_dirigido=False)
    mostrar_grafo(grafo1, "NO DIRIGIDO (Ciudades)", es_dirigido=False)
    probar_conexiones(grafo1, es_dirigido=False)

# Archivo 2: Grafo dirigido
archivo2 = os.path.join(directorio_datos, "edges_directed.txt")
print(f"\n\nArchivo 2: {archivo2}")
print(f"Existe? {os.path.exists(archivo2)}")

if os.path.exists(archivo2):
    print("\nCargando grafo dirigido...")
    grafo2 = cargar_grafo(archivo2, es_dirigido=True)
    mostrar_grafo(grafo2, "DIRIGIDO (Red Social)", es_dirigido=True)
    probar_conexiones(grafo2, es_dirigido=True)

print("\n" + "="*60)
print("  FIN DEL PROGRAMA")
print("="*60)