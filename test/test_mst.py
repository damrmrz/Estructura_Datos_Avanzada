import pytest
from mst import GraphMST

def test_prim_simple():
    """Test basico de Prim"""
    g = GraphMST(4)
    g.add_edge(0, 1, 10)
    g.add_edge(0, 2, 6)
    g.add_edge(0, 3, 5)
    g.add_edge(1, 3, 15)
    g.add_edge(2, 3, 4)
    
    edges, cost = g.prim_mst(0)
    
    assert cost == 19
    assert len(edges) == 3

def test_kruskal_simple():
    """Test basico de Kruskal"""
    g = GraphMST(4)
    g.add_edge(0, 1, 10)
    g.add_edge(0, 2, 6)
    g.add_edge(0, 3, 5)
    g.add_edge(1, 3, 15)
    g.add_edge(2, 3, 4)
    
    edges, cost = g.kruskal_mst()
    
    assert cost == 19
    assert len(edges) == 3

def test_prim_triangulo():
    """Test con triangulo"""
    g = GraphMST(3)
    g.add_edge(0, 1, 1)
    g.add_edge(1, 2, 2)
    g.add_edge(0, 2, 3)
    
    edges, cost = g.prim_mst(0)
    
    assert cost == 3

def test_kruskal_triangulo():
    """Test con triangulo"""
    g = GraphMST(3)
    g.add_edge(0, 1, 1)
    g.add_edge(1, 2, 2)
    g.add_edge(0, 2, 3)
    
    edges, cost = g.kruskal_mst()
    
    assert cost == 3

def test_mismo_resultado():
    """Verifica que Prim y Kruskal dan el mismo costo"""
    g = GraphMST(5)
    g.add_edge(0, 1, 2)
    g.add_edge(0, 3, 6)
    g.add_edge(1, 2, 3)
    g.add_edge(1, 3, 8)
    g.add_edge(1, 4, 5)
    g.add_edge(2, 4, 7)
    g.add_edge(3, 4, 9)
    
    edges_prim, cost_prim = g.prim_mst(0)
    edges_kruskal, cost_kruskal = g.kruskal_mst()
    
    assert cost_prim == cost_kruskal

def test_union_find_basico():
    """Test basico de Union-Find"""
    dsu = GraphMST.DSU(5)
    
    result = dsu.union(0, 1)
    
    assert result == True
    assert dsu.find(0) == dsu.find(1)

def test_union_mismo_conjunto():
    """Test que union del mismo conjunto devuelve False"""
    dsu = GraphMST.DSU(5)
    
    dsu.union(0, 1)
    result = dsu.union(0, 1)
    
    assert result == False

# En esta parte tome ayuda de la IA para diseñar tests de path compression
def test_path_compression():
    """Test de path compression en Union-Find"""
    dsu = GraphMST.DSU(5)
    
    dsu.union(0, 1)
    dsu.union(1, 2)
    dsu.union(2, 3)
    
    root = dsu.find(3)
    
    assert dsu.find(0) == root
    assert dsu.find(1) == root

def test_grafo_dos_nodos():
    """Test con 2 nodos"""
    g = GraphMST(2)
    g.add_edge(0, 1, 5)
    
    edges, cost = g.prim_mst(0)
    
    assert cost == 5
    assert len(edges) == 1

def test_numero_aristas():
    """Verifica que MST tiene V-1 aristas"""
    g = GraphMST(6)
    g.add_edge(0, 1, 1)
    g.add_edge(0, 2, 2)
    g.add_edge(1, 3, 3)
    g.add_edge(2, 3, 4)
    g.add_edge(3, 4, 5)
    g.add_edge(4, 5, 6)
    
    edges, cost = g.kruskal_mst()
    
    assert len(edges) == 5

if __name__ == "__main__":
    pytest.main([__file__, "-v"])