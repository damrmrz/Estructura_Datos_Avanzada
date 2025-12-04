import heapq

class GraphMST:
    """
    Clase para calcular arboles generadores minimos (MST).
    Incluye algoritmos de Prim y Kruskal.
    """
    
    def __init__(self, vertices):
        self.V = vertices
        self.edges = []
        self.adj = {i: [] for i in range(vertices)}
    
    def add_edge(self, u, v, w):
        """
        Agrega una arista al grafo.
        u: nodo origen
        v: nodo destino
        w: peso de la arista
        """
        self.edges.append((u, v, w))
        self.adj[u].append((v, w))
        self.adj[v].append((u, w))
    
    def prim_mst(self, start_node=0):
        """
        Algoritmo de Prim para MST.
        Empieza desde start_node y va agregando aristas baratas.
        """
        visited = [False] * self.V
        pq = []
        
        visited[start_node] = True
        for neighbor, weight in self.adj[start_node]:
            heapq.heappush(pq, (weight, start_node, neighbor))
        
        mst_edges = []
        mst_cost = 0
        
        while pq:
            weight, u, v = heapq.heappop(pq)
            
            if visited[v]:
                continue
            
            visited[v] = True
            mst_edges.append((u, v, weight))
            mst_cost += weight
            
            for next_node, next_weight in self.adj[v]:
                if not visited[next_node]:
                    heapq.heappush(pq, (next_weight, v, next_node))
        
        return mst_edges, mst_cost
    
    # En esta parte tome ayuda de la IA para implementar Union-Find correctamente
    class DSU:
        """
        Estructura Union-Find (Disjoint Set Union).
        Sirve para detectar si agregar una arista crea un ciclo.
        """
        
        def __init__(self, n):
            self.parent = list(range(n))
            self.rank = [0] * n
        
        def find(self, i):
            """Encuentra la raiz del conjunto que contiene a i"""
            if self.parent[i] != i:
                self.parent[i] = self.find(self.parent[i])
            return self.parent[i]
        
        def union(self, i, j):
            """Une los conjuntos que contienen a i y j"""
            root_i = self.find(i)
            root_j = self.find(j)
            
            if root_i != root_j:
                if self.rank[root_i] < self.rank[root_j]:
                    self.parent[root_i] = root_j
                elif self.rank[root_i] > self.rank[root_j]:
                    self.parent[root_j] = root_i
                else:
                    self.parent[root_j] = root_i
                    self.rank[root_i] += 1
                return True
            return False
    
    def kruskal_mst(self):
        """
        Algoritmo de Kruskal para MST.
        Ordena aristas por peso y va agregando las que no crean ciclos.
        """
        mst_cost = 0
        mst_edges = []
        dsu = self.DSU(self.V)
        
        sorted_edges = sorted(self.edges, key=lambda item: item[2])
        
        for u, v, w in sorted_edges:
            if dsu.union(u, v):
                mst_edges.append((u, v, w))
                mst_cost += w
        
        return mst_edges, mst_cost


if __name__ == "__main__":
    print("=== PRUEBA DE PRIM Y KRUSKAL ===\n")
    
    g = GraphMST(4)
    g.add_edge(0, 1, 10)
    g.add_edge(0, 2, 6)
    g.add_edge(0, 3, 5)
    g.add_edge(1, 3, 15)
    g.add_edge(2, 3, 4)
    
    print("Grafo con 4 nodos:")
    print("0-1 peso 10")
    print("0-2 peso 6")
    print("0-3 peso 5")
    print("1-3 peso 15")
    print("2-3 peso 4")
    print()
    
    edges_p, cost_p = g.prim_mst()
    print("MST con Prim:")
    print("Aristas:", edges_p)
    print("Costo:", cost_p)
    print()
    
    edges_k, cost_k = g.kruskal_mst()
    print("MST con Kruskal:")
    print("Aristas:", edges_k)
    print("Costo:", cost_k)