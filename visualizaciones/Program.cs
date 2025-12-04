using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGrafos
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=" + new string('=', 70));
            Console.WriteLine("  PROYECTO INTEGRADOR - SEMANAS 3, 4, 5 Y 6");
            Console.WriteLine("  Sistema de Analisis y Optimizacion de Grafos Urbanos");
            Console.WriteLine("=" + new string('=', 70));
            Console.WriteLine();

            // SEMANA 3: CREACION DEL GRAFO
            Console.WriteLine("--- SEMANA 3: MODELADO DEL GRAFO ---\n");
            
            Grafo<string> redCiudades = CargarGrafoNoDirigido();
            
            Console.WriteLine("Grafo cargado exitosamente:");
            Console.WriteLine("  Vertices: " + redCiudades.NumeroVertices);
            Console.WriteLine("  Aristas: " + redCiudades.NumeroAristas);
            Console.WriteLine();

            // SEMANA 4: VALIDACION DE PROPIEDADES
            Console.WriteLine("\n--- SEMANA 4: VALIDACION DE PROPIEDADES ---\n");
            
            List<int> secuencia = GraphValidator.ExtractDegreeSequence(redCiudades);
            Console.WriteLine("Secuencia de grados: [" + string.Join(", ", secuencia) + "]");
            
            bool esGrafica = GraphValidator.IsGraphicalSequence(secuencia);
            Console.WriteLine("Es secuencia grafica? " + (esGrafica ? "SI" : "NO"));
            
            bool esConsistente = GraphValidator.ValidateConsistency(redCiudades);
            Console.WriteLine("Es consistente (suma par)? " + (esConsistente ? "SI" : "NO"));
            
            int sumaGrados = 0;
            foreach (int grado in secuencia)
            {
                sumaGrados += grado;
            }
            Console.WriteLine("Suma de grados: " + sumaGrados);

            // SEMANA 5: ALGORITMOS DE EXPLORACION
            Console.WriteLine("\n--- SEMANA 5: ALGORITMOS BFS Y DFS ---\n");
            
            GraphTraversal traversal = new GraphTraversal(redCiudades);
            
            string ciudadInicio = "Ciudad A";
            Console.WriteLine("=== BFS desde " + ciudadInicio + " ===");
            
            List<string> ordenBFS = traversal.BFS(ciudadInicio);
            Console.WriteLine("Orden de visita BFS:");
            Console.WriteLine("  " + string.Join(" -> ", ordenBFS));
            Console.WriteLine();
            
            Dictionary<string, int> distancias = traversal.BFSDistancias(ciudadInicio);
            Console.WriteLine("Distancias desde " + ciudadInicio + ":");
            foreach (string ciudad in distancias.Keys.OrderBy(x => distancias[x]))
            {
                Console.WriteLine("  " + ciudad + ": " + distancias[ciudad] + " saltos");
            }
            Console.WriteLine();
            
            Console.WriteLine("=== Analisis de Conectividad ===");
            List<List<string>> componentes = traversal.EncontrarComponentesConectadas();
            Console.WriteLine("Numero de componentes conectadas: " + componentes.Count);
            
            if (componentes.Count == 1)
            {
                Console.WriteLine("El grafo esta completamente conectado");
            }

            // SEMANA 6: ALGORITMOS DE OPTIMIZACION
            Console.WriteLine("\n--- SEMANA 6: ALGORITMOS DIJKSTRA Y FLOYD-WARSHALL ---\n");
            
            WeightedGraph grafoPonderado = CargarGrafoPonderado();
            
            Console.WriteLine("=== Dijkstra desde Ciudad A ===");
            
            try
            {
                var (distanciasDijkstra, padres) = grafoPonderado.Dijkstra("Ciudad A");
                
                Console.WriteLine("Distancias ponderadas desde Ciudad A:");
                foreach (string ciudad in distanciasDijkstra.Keys.OrderBy(x => distanciasDijkstra[x]))
                {
                    double dist = distanciasDijkstra[ciudad];
                    if (double.IsPositiveInfinity(dist))
                    {
                        Console.WriteLine("  " + ciudad + ": No alcanzable");
                    }
                    else
                    {
                        Console.WriteLine("  " + ciudad + ": " + dist.ToString("F2") + " km");
                    }
                }
                Console.WriteLine();
                
                // Reconstruir camino mas corto
                string destino = "Ciudad E";
                List<string> camino = grafoPonderado.ReconstruirCamino("Ciudad A", destino, padres);
                
                if (camino != null)
                {
                    Console.WriteLine("Camino mas corto de Ciudad A a " + destino + ":");
                    Console.WriteLine("  " + string.Join(" -> ", camino));
                    Console.WriteLine("  Distancia total: " + distanciasDijkstra[destino].ToString("F2") + " km");
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Dijkstra: " + ex.Message);
            }
            
            Console.WriteLine("=== Floyd-Warshall (Todos los pares) ===");
            
            try
            {
                var (distanciasFW, siguienteFW) = grafoPonderado.FloydWarshall();
                
                Console.WriteLine("Matriz de distancias (muestra):");
                List<string> vertices = grafoPonderado.ObtenerVertices().Take(3).ToList();
                
                foreach (string origen in vertices)
                {
                    foreach (string destino in vertices)
                    {
                        double dist = distanciasFW[(origen, destino)];
                        string distStr = double.IsPositiveInfinity(dist) ? "INF" : dist.ToString("F1");
                        Console.Write(origen.Substring(7) + "->" + destino.Substring(7) + ": " + distStr + " | ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Floyd-Warshall: " + ex.Message);
            }
            
            // Analisis avanzado
            Console.WriteLine("=== Analisis Avanzado ===");
            
            string verticeCentral = grafoPonderado.EncontrarVerticeMasCentral();
            Console.WriteLine("Vertice mas central: " + verticeCentral);
            
            double distanciaMax = grafoPonderado.CalcularDistanciaMaxima();
            Console.WriteLine("Distancia maxima en el grafo: " + distanciaMax.ToString("F2") + " km");
            Console.WriteLine();

            // PRUEBA DE LOS 10 CASOS OFICIALES (SEMANA 4)
            Console.WriteLine("\n--- CASOS DE PRUEBA HAVEL-HAKIMI ---\n");
            PruebaCasosOficiales();

            // EJECUCION DE TESTS UNITARIOS
            Console.WriteLine("\n\n--- EJECUCION DE TESTS UNITARIOS ---");
            
            GraphValidatorTests.EjecutarTodos();
            GraphTraversalTests.EjecutarTodos();
            WeightedGraphTests.EjecutarTodos();

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("  FIN DEL PROGRAMA");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        static Grafo<string> CargarGrafoNoDirigido()
        {
            Grafo<string> grafo = new Grafo<string>(dirigido: false);

            Console.WriteLine("Cargando red de ciudades...");
            
            grafo.AgregarArista("Ciudad A", "Ciudad B", 50.5);
            grafo.AgregarArista("Ciudad A", "Ciudad C", 80.0);
            grafo.AgregarArista("Ciudad A", "Ciudad D", 95.0);
            grafo.AgregarArista("Ciudad B", "Ciudad D", 30.0);
            grafo.AgregarArista("Ciudad C", "Ciudad D", 45.5);
            grafo.AgregarArista("Ciudad C", "Ciudad E", 70.0);
            grafo.AgregarArista("Ciudad D", "Ciudad E", 25.0);

            Console.WriteLine("7 aristas cargadas\n");
            
            return grafo;
        }

        static WeightedGraph CargarGrafoPonderado()
        {
            WeightedGraph grafo = new WeightedGraph();
            
            Console.WriteLine("Creando grafo ponderado para optimizacion...");
            
            // Mismo grafo pero con estructura para Dijkstra/Floyd-Warshall
            grafo.AgregarArista("Ciudad A", "Ciudad B", 50.5);
            grafo.AgregarArista("Ciudad A", "Ciudad C", 80.0);
            grafo.AgregarArista("Ciudad A", "Ciudad D", 95.0);
            grafo.AgregarArista("Ciudad B", "Ciudad D", 30.0);
            grafo.AgregarArista("Ciudad C", "Ciudad D", 45.5);
            grafo.AgregarArista("Ciudad C", "Ciudad E", 70.0);
            grafo.AgregarArista("Ciudad D", "Ciudad E", 25.0);
            
            // Aristas inversas para simular no dirigido en grafo dirigido
            grafo.AgregarArista("Ciudad B", "Ciudad A", 50.5);
            grafo.AgregarArista("Ciudad C", "Ciudad A", 80.0);
            grafo.AgregarArista("Ciudad D", "Ciudad A", 95.0);
            grafo.AgregarArista("Ciudad D", "Ciudad B", 30.0);
            grafo.AgregarArista("Ciudad D", "Ciudad C", 45.5);
            grafo.AgregarArista("Ciudad E", "Ciudad C", 70.0);
            grafo.AgregarArista("Ciudad E", "Ciudad D", 25.0);
            
            Console.WriteLine("Grafo ponderado creado (14 aristas dirigidas)\n");
            
            return grafo;
        }

        static void PruebaCasosOficiales()
        {
            var casosPrueba = new List<(List<int> secuencia, bool esperado, string razon)>
            {
                (new List<int> {4, 3, 3, 2, 2, 2, 1, 1}, true, "Suma=18 (par), max=4<=7"),
                (new List<int> {3, 2, 2, 1}, true, "Ejemplo del documento"),
                (new List<int> {4, 3, 3, 2, 2, 2}, true, "n=6, suma=16 (par)"),
                (new List<int> {0, 0, 0, 0}, true, "Grafo vacio"),
                (new List<int> {3, 3, 3, 3}, true, "Grafo completo K4"),
                (new List<int> {3, 3, 3, 1}, false, "Genera negativos"),
                (new List<int> {5, 5, 4, 3, 2, 1}, false, "Estructura imposible"),
                (new List<int> {3, 2, 1}, false, "max > n-1"),
                (new List<int> {6, 1, 1, 1, 1, 1, 1}, false, "Imposible"),
                (new List<int> {5, 3, 2, 2, 1}, false, "Suma impar")
            };

            int aprobados = 0;
            int total = casosPrueba.Count;

            for (int i = 0; i < casosPrueba.Count; i++)
            {
                var (secuencia, esperado, razon) = casosPrueba[i];
                bool resultado = GraphValidator.IsGraphicalSequence(secuencia);
                bool correcto = resultado == esperado;

                string estado = correcto ? "[PASS]" : "[FAIL]";
                Console.WriteLine("Caso " + (i + 1) + ": " + estado);
                Console.WriteLine("  Secuencia: [" + string.Join(", ", secuencia) + "]");
                Console.WriteLine("  Esperado: " + esperado + " | Obtenido: " + resultado);
                Console.WriteLine("  Razon: " + razon);
                Console.WriteLine();

                if (correcto) aprobados++;
            }

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("RESULTADOS FINALES: " + aprobados + "/" + total + " casos correctos");

            if (aprobados == total)
            {
                Console.WriteLine("TODOS LOS TESTS PASARON EXITOSAMENTE");
            }
            else
            {
                Console.WriteLine("ADVERTENCIA: Algunos tests fallaron");
            }
            Console.WriteLine(new string('=', 60));
        }
    }
}

