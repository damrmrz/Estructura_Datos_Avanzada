using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGrafos
{
    public static class GraphTraversalTests
    {
        public static void EjecutarTodos()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("  EJECUCIÓN DE TESTS DE BFS Y DFS");
            Console.WriteLine(new string('=', 60));

            int pasados = 0;
            int total = 0;

            // Test 1: BFS en grafo lineal
            total++;
            if (Test1_BFS_GrafoLineal())
            {
                Console.WriteLine("Test 1: [✓ PASS] BFS en grafo lineal");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 1: [✗ FAIL] BFS en grafo lineal");
            }

            // Test 2: BFS distancias
            total++;
            if (Test2_BFS_Distancias())
            {
                Console.WriteLine("Test 2: [✓ PASS] BFS cálculo de distancias");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 2: [✗ FAIL] BFS cálculo de distancias");
            }

            // Test 3: BFS camino más corto
            total++;
            if (Test3_BFS_CaminoMasCorto())
            {
                Console.WriteLine("Test 3: [✓ PASS] BFS camino más corto");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 3: [✗ FAIL] BFS camino más corto");
            }

            // Test 4: DFS recursivo
            total++;
            if (Test4_DFS_Recursivo())
            {
                Console.WriteLine("Test 4: [✓ PASS] DFS recursivo");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 4: [✗ FAIL] DFS recursivo");
            }

            // Test 5: DFS iterativo
            total++;
            if (Test5_DFS_Iterativo())
            {
                Console.WriteLine("Test 5: [✓ PASS] DFS iterativo");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 5: [✗ FAIL] DFS iterativo");
            }

            // Test 6: Componentes conectadas
            total++;
            if (Test6_ComponentesConectadas())
            {
                Console.WriteLine("Test 6: [✓ PASS] Componentes conectadas");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 6: [✗ FAIL] Componentes conectadas");
            }

            // Test 7: BFS nodo inexistente
            total++;
            if (Test7_BFS_NodoInexistente())
            {
                Console.WriteLine("Test 7: [✓ PASS] BFS maneja nodo inexistente");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 7: [✗ FAIL] BFS maneja nodo inexistente");
            }

            // Test 8: DFS nodo inexistente
            total++;
            if (Test8_DFS_NodoInexistente())
            {
                Console.WriteLine("Test 8: [✓ PASS] DFS maneja nodo inexistente");
                pasados++;
            }
            else
            {
                Console.WriteLine("Test 8: [✗ FAIL] DFS maneja nodo inexistente");
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("RESULTADO: " + pasados + "/" + total + " tests pasados");
            Console.WriteLine(new string('=', 60) + "\n");
        }

        private static bool Test1_BFS_GrafoLineal()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);
                grafo.AgregarArista("B", "C", 1.0);
                grafo.AgregarArista("C", "D", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                List<string> resultado = traversal.BFS("A");

                return resultado.Count == 4 && 
                       resultado[0] == "A" && 
                       resultado[1] == "B" && 
                       resultado[2] == "C" && 
                       resultado[3] == "D";
            }
            catch
            {
                return false;
            }
        }

        private static bool Test2_BFS_Distancias()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);
                grafo.AgregarArista("A", "C", 1.0);
                grafo.AgregarArista("B", "D", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                Dictionary<string, int> distancias = traversal.BFSDistancias("A");

                return distancias["A"] == 0 && 
                       distancias["B"] == 1 && 
                       distancias["C"] == 1 && 
                       distancias["D"] == 2;
            }
            catch
            {
                return false;
            }
        }

        private static bool Test3_BFS_CaminoMasCorto()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);
                grafo.AgregarArista("A", "C", 1.0);
                grafo.AgregarArista("B", "D", 1.0);
                grafo.AgregarArista("C", "D", 1.0);
                grafo.AgregarArista("D", "E", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                List<string> camino = traversal.BFSCaminoMasCorto("A", "E");

                return camino != null && 
                       camino.Count == 4 && 
                       camino[0] == "A" && 
                       camino[camino.Count - 1] == "E";
            }
            catch
            {
                return false;
            }
        }

        private static bool Test4_DFS_Recursivo()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);
                grafo.AgregarArista("A", "C", 1.0);
                grafo.AgregarArista("B", "D", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                List<string> resultado = traversal.DFSRecursivo("A");

                return resultado.Count == 4 && resultado[0] == "A";
            }
            catch
            {
                return false;
            }
        }

        private static bool Test5_DFS_Iterativo()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);
                grafo.AgregarArista("A", "C", 1.0);
                grafo.AgregarArista("B", "D", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                List<string> resultado = traversal.DFSIterativo("A");

                return resultado.Count == 4 && resultado[0] == "A";
            }
            catch
            {
                return false;
            }
        }

        private static bool Test6_ComponentesConectadas()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);
                grafo.AgregarArista("C", "D", 1.0);
                grafo.AgregarArista("E", "F", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                List<List<string>> componentes = traversal.EncontrarComponentesConectadas();

                return componentes.Count == 3;
            }
            catch
            {
                return false;
            }
        }

        private static bool Test7_BFS_NodoInexistente()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                traversal.BFS("Z");

                return false; // No debería llegar aquí
            }
            catch (ArgumentException)
            {
                return true; // Debe lanzar excepción
            }
            catch
            {
                return false;
            }
        }

        private static bool Test8_DFS_NodoInexistente()
        {
            try
            {
                Grafo<string> grafo = new Grafo<string>(dirigido: false);
                grafo.AgregarArista("A", "B", 1.0);

                GraphTraversal traversal = new GraphTraversal(grafo);
                traversal.DFSRecursivo("Z");

                return false; // No debería llegar aquí
            }
            catch (ArgumentException)
            {
                return true; // Debe lanzar excepción
            }
            catch
            {
                return false;
            }
        }
    }
}