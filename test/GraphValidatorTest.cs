using System;
using System.Collections.Generic;

namespace SistemaGrafos
{
    public static class GraphValidatorTests
    {
        public static void EjecutarTodos()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("  EJECUCIÓN DE TESTS UNITARIOS");
            Console.WriteLine(new string('=', 60));

            int pasados = 0;
            int total = 0;

            // Test 1: Secuencia válida básica
            total++;
            if (Test1_SecuenciaValidaBasica())
            {
                Console.WriteLine("Test 1: [✓ PASS] Secuencia válida básica");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 1: [✗ FAIL] Secuencia válida básica");
            }

            // Test 2: Secuencia con suma impar
            total++;
            if (Test2_SumaImpar())
            {
                Console.WriteLine("Test 2: [✓ PASS] Rechazo de suma impar");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 2: [✗ FAIL] Rechazo de suma impar");
            }

            // Test 3: Grado máximo inválido
            total++;
            if (Test3_GradoMaximoInvalido())
            {
                Console.WriteLine("Test 3: [✓ PASS] Rechazo de grado máximo > n-1");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 3: [✗ FAIL] Rechazo de grado máximo > n-1");
            }

            // Test 4: Grafo vacío
            total++;
            if (Test4_GrafoVacio())
            {
                Console.WriteLine("Test 4: [✓ PASS] Grafo vacío es válido");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 4: [✗ FAIL] Grafo vacío es válido");
            }

            // Test 5: Consistencia de grafo
            total++;
            if (Test5_ConsistenciaGrafo())
            {
                Console.WriteLine("Test 5: [✓ PASS] Validación de consistencia");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 5: [✗ FAIL] Validación de consistencia");
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("RESULTADO: " + pasados + "/" + total + " tests pasados");
            Console.WriteLine(new string('=', 60) + "\n");
        }

        private static bool Test1_SecuenciaValidaBasica()
        {
            List<int> secuencia = new List<int> { 3, 2, 2, 1 };
            return GraphValidator.IsGraphicalSequence(secuencia) == true;
        }

        private static bool Test2_SumaImpar()
        {
            List<int> secuencia = new List<int> { 5, 3, 2, 2, 1 };
            return GraphValidator.IsGraphicalSequence(secuencia) == false;
        }

        private static bool Test3_GradoMaximoInvalido()
        {
            List<int> secuencia = new List<int> { 3, 2, 1 };
            return GraphValidator.IsGraphicalSequence(secuencia) == false;
        }

        private static bool Test4_GrafoVacio()
        {
            List<int> secuencia = new List<int> { 0, 0, 0, 0 };
            return GraphValidator.IsGraphicalSequence(secuencia) == true;
        }

        private static bool Test5_ConsistenciaGrafo()
        {
            Grafo<string> grafo = new Grafo<string>(dirigido: false);
            grafo.AgregarArista("A", "B", 1.0);
            grafo.AgregarArista("B", "C", 1.0);
            grafo.AgregarArista("C", "A", 1.0);

            return GraphValidator.ValidateConsistency(grafo) == true;
        }
    }
}