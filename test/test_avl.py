import unittest
from avl import AVL

class TestAVL(unittest.TestCase):
    
    def setUp(self):
        """Se ejecuta antes de cada test"""
        self.avl = AVL()
    
    def test_arbol_vacio(self):
        """Probar operaciones en arbol vacio"""
        self.assertIsNone(self.avl.raiz)
        self.assertEqual(self.avl.inorden(), [])
    
    def test_insercion_simple(self):
        """Probar insercion sin rotaciones"""
        self.avl.insertar(50)
        self.assertEqual(self.avl.raiz.valor, 50)
        self.assertEqual(self.avl.factor_balance(self.avl.raiz), 0)
    
    def test_rotacion_LL(self):
        """Probar rotacion simple derecha (caso LL)"""
        # Insertar 30, 20, 10 causa rotacion LL
        self.avl.insertar(30)
        self.avl.insertar(20)
        self.avl.insertar(10)
        
        # Despues de la rotacion, 20 debe ser la raiz
        self.assertEqual(self.avl.raiz.valor, 20)
        self.assertEqual(self.avl.raiz.izquierdo.valor, 10)
        self.assertEqual(self.avl.raiz.derecho.valor, 30)
        self.assertEqual(self.avl.factor_balance(self.avl.raiz), 0)
    
    def test_rotacion_RR(self):
        """Probar rotacion simple izquierda (caso RR)"""
        # Insertar 10, 20, 30 causa rotacion RR
        self.avl.insertar(10)
        self.avl.insertar(20)
        self.avl.insertar(30)
        
        # Despues de la rotacion, 20 debe ser la raiz
        self.assertEqual(self.avl.raiz.valor, 20)
        self.assertEqual(self.avl.raiz.izquierdo.valor, 10)
        self.assertEqual(self.avl.raiz.derecho.valor, 30)
        self.assertEqual(self.avl.factor_balance(self.avl.raiz), 0)
    
    def test_rotacion_LR(self):
        """Probar rotacion doble izquierda-derecha (caso LR)"""
        # Insertar 30, 20, 25 causa rotacion LR
        self.avl.insertar(30)
        self.avl.insertar(20)
        self.avl.insertar(25)
        
        # Despues de la rotacion doble, 25 debe ser la raiz
        self.assertEqual(self.avl.raiz.valor, 25)
        self.assertEqual(self.avl.raiz.izquierdo.valor, 20)
        self.assertEqual(self.avl.raiz.derecho.valor, 30)
        self.assertEqual(self.avl.factor_balance(self.avl.raiz), 0)
    
    def test_rotacion_RL(self):
        """Probar rotacion doble derecha-izquierda (caso RL)"""
        # Insertar 10, 30, 20 causa rotacion RL
        self.avl.insertar(10)
        self.avl.insertar(30)
        self.avl.insertar(20)
        
        # Despues de la rotacion doble, 20 debe ser la raiz
        self.assertEqual(self.avl.raiz.valor, 20)
        self.assertEqual(self.avl.raiz.izquierdo.valor, 10)
        self.assertEqual(self.avl.raiz.derecho.valor, 30)
        self.assertEqual(self.avl.factor_balance(self.avl.raiz), 0)
    
    def test_insercion_ordenada_mantiene_balance(self):
        """Insertar elementos ordenados debe mantener el arbol balanceado"""
        valores = [10, 20, 30, 40, 50, 25]
        for v in valores:
            self.avl.insertar(v)
        
        # Verificar que todos los nodos estan balanceados
        def verificar_balance(nodo):
            if nodo is None:
                return True
            fb = self.avl.factor_balance(nodo)
            if abs(fb) > 1:
                return False
            return verificar_balance(nodo.izquierdo) and verificar_balance(nodo.derecho)
        
        self.assertTrue(verificar_balance(self.avl.raiz))
    
    def test_altura_correcta(self):
        """Verificar que las alturas se calculan correctamente"""
        self.avl.insertar(50)
        self.assertEqual(self.avl.altura(self.avl.raiz), 1)
        
        self.avl.insertar(30)
        self.avl.insertar(70)
        self.assertEqual(self.avl.altura(self.avl.raiz), 2)
    
    def test_factor_balance_correcto(self):
        """Verificar calculo de factor de balance"""
        self.avl.insertar(50)
        self.avl.insertar(30)
        self.assertEqual(self.avl.factor_balance(self.avl.raiz), 1)
        
        self.avl.insertar(70)
        self.assertEqual(self.avl.factor_balance(self.avl.raiz), 0)
    
    def test_no_duplicados(self):
        """Verificar que no se permiten duplicados"""
        self.avl.insertar(50)
        self.avl.insertar(50)
        valores = [v for v, fb in self.avl.inorden()]
        self.assertEqual(valores, [50])
    
    def test_inorden_ordenado(self):
        """El recorrido inorden debe producir elementos ordenados"""
        valores = [50, 30, 70, 20, 40, 60, 80, 10, 25, 35, 45]
        for v in valores:
            self.avl.insertar(v)
        
        resultado = [v for v, fb in self.avl.inorden()]
        self.assertEqual(resultado, sorted(valores))
    
    def test_secuencia_compleja(self):
        """Probar secuencia compleja de inserciones con multiples rotaciones"""
        valores = [10, 20, 30, 40, 50, 25]
        for v in valores:
            self.avl.insertar(v)
        
        # Verificar que el arbol esta balanceado
        def verificar_balance(nodo):
            if nodo is None:
                return True
            fb = self.avl.factor_balance(nodo)
            if abs(fb) > 1:
                return False
            return verificar_balance(nodo.izquierdo) and verificar_balance(nodo.derecho)
        
        self.assertTrue(verificar_balance(self.avl.raiz))
        
        # Verificar que todos los elementos estan presentes
        resultado = [v for v, fb in self.avl.inorden()]
        self.assertEqual(resultado, sorted(valores))
    
    def test_altura_logaritmica(self):
        """Verificar que la altura se mantiene logaritmica"""
        # Insertar 100 elementos ordenados
        for i in range(1, 101):
            self.avl.insertar(i)
        
        # La altura debe ser aprox 1.44 * log2(n)
        import math
        altura_esperada = int(1.44 * math.log2(100) + 1)
        altura_real = self.avl.altura(self.avl.raiz)
        
        # La altura real debe estar cerca de la esperada
        self.assertLessEqual(altura_real, altura_esperada + 5)

if __name__ == '__main__':
    unittest.main()