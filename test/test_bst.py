import unittest
from bst import BST

class TestBST(unittest.TestCase):
    
    def setUp(self):
        """Se ejecuta antes de cada test"""
        self.bst = BST()
    
    def test_arbol_vacio(self):
        """Probar operaciones en arbol vacio"""
        self.assertIsNone(self.bst.raiz)
        self.assertFalse(self.bst.buscar(10))
        self.assertIsNone(self.bst.minimo())
        self.assertIsNone(self.bst.maximo())
        self.assertEqual(self.bst.inorden(), [])
    
    def test_insercion_simple(self):
        """Probar insercion basica"""
        self.bst.insertar(50)
        self.assertTrue(self.bst.buscar(50))
        self.assertEqual(self.bst.inorden(), [50])
    
    def test_insercion_multiple(self):
        """Probar insercion de multiples elementos"""
        valores = [50, 30, 70, 20, 40, 60, 80]
        for v in valores:
            self.bst.insertar(v)
        
        self.assertEqual(self.bst.inorden(), [20, 30, 40, 50, 60, 70, 80])
        for v in valores:
            self.assertTrue(self.bst.buscar(v))
    
    def test_no_permite_duplicados(self):
        """Verificar que no se permiten duplicados"""
        self.bst.insertar(50)
        self.bst.insertar(50)
        self.assertEqual(self.bst.inorden(), [50])
    
    def test_busqueda_elementos_inexistentes(self):
        """Buscar elementos que no existen"""
        valores = [50, 30, 70]
        for v in valores:
            self.bst.insertar(v)
        
        self.assertFalse(self.bst.buscar(10))
        self.assertFalse(self.bst.buscar(100))
        self.assertFalse(self.bst.buscar(40))
    
    def test_minimo_maximo(self):
        """Probar busqueda de minimo y maximo"""
        valores = [50, 30, 70, 20, 40, 60, 80]
        for v in valores:
            self.bst.insertar(v)
        
        self.assertEqual(self.bst.minimo(), 20)
        self.assertEqual(self.bst.maximo(), 80)
    
    def test_eliminacion_hoja(self):
        """CASO 1: Eliminar nodo hoja (sin hijos)"""
        valores = [50, 30, 70, 20, 40, 60, 80]
        for v in valores:
            self.bst.insertar(v)
        
        self.bst.eliminar(20)
        self.assertFalse(self.bst.buscar(20))
        self.assertEqual(self.bst.inorden(), [30, 40, 50, 60, 70, 80])
    
    def test_eliminacion_un_hijo_derecho(self):
        """CASO 2a: Eliminar nodo con solo hijo derecho"""
        self.bst.insertar(50)
        self.bst.insertar(30)
        self.bst.insertar(70)
        self.bst.insertar(60)
        
        self.bst.eliminar(70)
        self.assertFalse(self.bst.buscar(70))
        self.assertTrue(self.bst.buscar(60))
        self.assertEqual(self.bst.inorden(), [30, 50, 60])
    
    def test_eliminacion_un_hijo_izquierdo(self):
        """CASO 2b: Eliminar nodo con solo hijo izquierdo"""
        self.bst.insertar(50)
        self.bst.insertar(30)
        self.bst.insertar(70)
        self.bst.insertar(60)
        
        self.bst.eliminar(30)
        self.assertFalse(self.bst.buscar(30))
        self.assertEqual(self.bst.inorden(), [50, 60, 70])
    
    def test_eliminacion_dos_hijos(self):
        """CASO 3: Eliminar nodo con dos hijos"""
        valores = [50, 30, 70, 20, 40, 60, 80]
        for v in valores:
            self.bst.insertar(v)
        
        self.bst.eliminar(70)
        self.assertFalse(self.bst.buscar(70))
        self.assertEqual(self.bst.inorden(), [20, 30, 40, 50, 60, 80])
    
    def test_eliminacion_raiz_hoja(self):
        """Eliminar raiz cuando es el unico nodo"""
        self.bst.insertar(50)
        self.bst.eliminar(50)
        self.assertIsNone(self.bst.raiz)
        self.assertEqual(self.bst.inorden(), [])
    
    def test_eliminacion_raiz_dos_hijos(self):
        """Eliminar raiz con dos hijos"""
        valores = [50, 30, 70, 20, 40, 60, 80]
        for v in valores:
            self.bst.insertar(v)
        
        self.bst.eliminar(50)
        self.assertFalse(self.bst.buscar(50))
        self.assertTrue(self.bst.buscar(60))
        self.assertEqual(self.bst.inorden(), [20, 30, 40, 60, 70, 80])
    
    def test_eliminacion_elemento_inexistente(self):
        """Eliminar elemento que no existe no debe causar error"""
        valores = [50, 30, 70]
        for v in valores:
            self.bst.insertar(v)
        
        self.bst.eliminar(100)
        self.assertEqual(self.bst.inorden(), [30, 50, 70])
    
    def test_recorrido_preorden(self):
        """Probar recorrido preorden"""
        valores = [50, 30, 70, 20, 40, 60, 80]
        for v in valores:
            self.bst.insertar(v)
        
        self.assertEqual(self.bst.preorden(), [50, 30, 20, 40, 70, 60, 80])
    
    def test_recorrido_postorden(self):
        """Probar recorrido postorden"""
        valores = [50, 30, 70, 20, 40, 60, 80]
        for v in valores:
            self.bst.insertar(v)
        
        self.assertEqual(self.bst.postorden(), [20, 40, 30, 60, 80, 70, 50])
    
    def test_insercion_ordenada_degenerada(self):
        """Insertar elementos ordenados crea arbol degenerado"""
        valores = [10, 20, 30, 40, 50]
        for v in valores:
            self.bst.insertar(v)
        
        self.assertEqual(self.bst.inorden(), valores)
        self.assertEqual(self.bst.minimo(), 10)
        self.assertEqual(self.bst.maximo(), 50)

if __name__ == '__main__':
    unittest.main()