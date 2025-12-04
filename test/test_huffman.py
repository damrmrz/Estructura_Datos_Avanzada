import unittest
from huffman import Huffman

class TestHuffman(unittest.TestCase):
    
    def setUp(self):
        """Se ejecuta antes de cada test"""
        self.huff = Huffman()
    
    def test_texto_simple(self):
        """Probar comprension de texto simple"""
        texto = "ABRACADABRA"
        self.huff.construir_arbol(texto)
        
        codificado = self.huff.codificar(texto)
        decodificado = self.huff.decodificar(codificado)
        
        self.assertEqual(texto, decodificado)
    
    def test_texto_vacio(self):
        """Probar con texto vacio"""
        texto = ""
        self.huff.construir_arbol(texto)
        self.assertEqual(len(self.huff.codigos), 0)
    
    def test_un_solo_caracter(self):
        """Probar con texto de un solo caracter repetido"""
        texto = "AAAA"
        self.huff.construir_arbol(texto)
        
        self.assertEqual(len(self.huff.codigos), 1)
        self.assertEqual(self.huff.codigos['A'], '0')
        
        codificado = self.huff.codificar(texto)
        decodificado = self.huff.decodificar(codificado)
        self.assertEqual(texto, decodificado)
    
    def test_dos_caracteres(self):
        """Probar con dos caracteres diferentes"""
        texto = "AABB"
        self.huff.construir_arbol(texto)
        
        self.assertEqual(len(self.huff.codigos), 2)
        
        codificado = self.huff.codificar(texto)
        decodificado = self.huff.decodificar(codificado)
        self.assertEqual(texto, decodificado)
    
    def test_codigo_prefijo(self):
        """Verificar que ningun codigo es prefijo de otro"""
        texto = "ABRACADABRA"
        self.huff.construir_arbol(texto)
        
        codigos = list(self.huff.codigos.values())
        for i, cod1 in enumerate(codigos):
            for j, cod2 in enumerate(codigos):
                if i != j:
                    # Ningun codigo debe ser prefijo de otro
                    self.assertFalse(cod2.startswith(cod1))
    
    def test_frecuencia_afecta_longitud(self):
        """Caracteres mas frecuentes deben tener codigos mas cortos"""
        texto = "AAAAAABBBC"  # A:6, B:3, C:1
        self.huff.construir_arbol(texto)
        
        len_A = len(self.huff.codigos['A'])
        len_B = len(self.huff.codigos['B'])
        len_C = len(self.huff.codigos['C'])
        
        # A debe tener codigo mas corto o igual que B
        self.assertLessEqual(len_A, len_B)
        # B debe tener codigo mas corto o igual que C
        self.assertLessEqual(len_B, len_C)
    
    def test_texto_largo(self):
        """Probar con texto mas largo"""
        texto = "Este es un texto de prueba para verificar la compresion de Huffman" * 10
        self.huff.construir_arbol(texto)
        
        codificado = self.huff.codificar(texto)
        decodificado = self.huff.decodificar(codificado)
        
        self.assertEqual(texto, decodificado)
    
    def test_compresion_efectiva(self):
        """Verificar que realmente se comprime"""
        texto = "ABRACADABRA"
        self.huff.construir_arbol(texto)
        
        bits_original = len(texto) * 8
        bits_huffman = len(self.huff.codificar(texto))
        
        # Debe haber compresion
        self.assertLess(bits_huffman, bits_original)
    
    def test_todos_ascii(self):
        """Probar con caracteres ASCII variados"""
        texto = "Hello World! 123"
        self.huff.construir_arbol(texto)
        
        codificado = self.huff.codificar(texto)
        decodificado = self.huff.decodificar(codificado)
        
        self.assertEqual(texto, decodificado)
    
    def test_codigos_unicos(self):
        """Verificar que cada caracter tiene un codigo unico"""
        texto = "ABCDEF"
        self.huff.construir_arbol(texto)
        
        codigos = list(self.huff.codigos.values())
        self.assertEqual(len(codigos), len(set(codigos)))
    
    def test_reversibilidad_completa(self):
        """Probar que codificar->decodificar es reversible"""
        textos = [
            "A",
            "AB",
            "ABC",
            "AAABBBCCC",
            "The quick brown fox jumps over the lazy dog",
            "12345",
            "   ",  # espacios
        ]
        
        for texto in textos:
            if texto:  # Saltar texto vacio
                huff = Huffman()
                huff.construir_arbol(texto)
                codificado = huff.codificar(texto)
                decodificado = huff.decodificar(codificado)
                self.assertEqual(texto, decodificado, 
                               f"Fallo con texto: '{texto}'")

if __name__ == '__main__':
    unittest.main()