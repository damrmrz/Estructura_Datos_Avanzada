using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGrafos
{
    public class WeightedGraph
    {
        private Dictionary<string, List<(string destino, double peso)>> listaAdyacencia;
        private int numeroVertices;
        
        public WeightedGraph()
        {
            listaAdyacencia = new Dictionary<string, List<(string, double)>>();
            numeroVertices = 0;
        }
        
        public void AgregarVertice(string vertice)
        {
            if (!listaAdyacencia.ContainsKey(vertice))
            {
                listaAdyacencia[vertice] = new List<(string, double)>();
                numeroVertices++;
            }
        }
        
        public void AgregarArista(string origen, string destino, double peso)
        {
            AgregarVertice(origen);
            AgregarVertice(destino);
            listaAdyacencia[origen].Add((destino, peso));
        }
        
        public IEnumerable<string> ObtenerVertices()
        {
            return listaAdyacencia.Keys;
        }
        
        // ALGORITMO DE DIJKSTRA (AYUDA DE IA)
        // Complejidad: O((V + E) log V) con heap binario
        
        /* NOTA: La implementación de la cola de prioridad con SortedSet fue optimizada
         * con asistencia de IA para manejar correctamente la comparación de tuplas
         * (distancia, nodo) y evitar duplicados cuando dos nodos tienen la misma distancia.
         * El comparador personalizado resuelve conflictos usando el nombre del nodo como
         * criterio secundario, garantizando ordenamiento estable.
         */
        public (Dictionary<string, double> distancias, Dictionary<string, string> padres) 
            Dijkstra(string origen)
        {
            if (!listaAdyacencia.ContainsKey(origen))
            {
                throw new ArgumentException("El nodo " + origen + " no existe en el grafo");
            }
            
            Dictionary<string, double> distancias = new Dictionary<string, double>();
            Dictionary<string, string> padres = new Dictionary<string, string>();
            HashSet<string> visitados = new HashSet<string>();
            
            // Cola de prioridad: (distancia, nodo)
            SortedSet<(double distancia, string nodo)> colaPrioridad = 
                new SortedSet<(double, string)>(Comparer<(double, string)>.Create((a, b) =>
                {
                    int comp = a.Item1.CompareTo(b.Item1);
                    return comp != 0 ? comp : a.Item2.CompareTo(b.Item2);
                }));
            
            // Inicializar distancias a infinito
            foreach (string vertice in listaAdyacencia.Keys)
            {
                distancias[vertice] = double.PositiveInfinity;
                padres[vertice] = null;
            }
            
            distancias[origen] = 0;
            colaPrioridad.Add((0, origen));
            
            while (colaPrioridad.Count > 0)
            {
                var actual = colaPrioridad.Min;
                colaPrioridad.Remove(actual);
                
                string nodoActual = actual.nodo;
                double distanciaActual = actual.distancia;
                
                if (visitados.Contains(nodoActual))
                {
                    continue;
                }
                
                visitados.Add(nodoActual);
                
                // Relajación de aristas
                foreach (var (vecino, peso) in listaAdyacencia[nodoActual])
                {
                    if (!visitados.Contains(vecino))
                    {
                        double nuevaDistancia = distanciaActual + peso;
                        
                        if (nuevaDistancia < distancias[vecino])
                        {
                            distancias[vecino] = nuevaDistancia;
                            padres[vecino] = nodoActual;
                            colaPrioridad.Add((nuevaDistancia, vecino));
                        }
                    }
                }
            }
            
            return (distancias, padres);
        }
        
        public List<string> ReconstruirCamino(string origen, string destino, 
            Dictionary<string, string> padres)
        {
            if (!padres.ContainsKey(destino) || padres[destino] == null && destino != origen)
            {
                return null;
            }
            
            List<string> camino = new List<string>();
            string nodo = destino;
            
            while (nodo != null)
            {
                camino.Add(nodo);
                nodo = padres[nodo];
            }
            
            camino.Reverse();
            return camino;
        }
        
        // ALGORITMO DE FLOYD-WARSHALL
        // Complejidad: O(V³)
        
        public (Dictionary<(string, string), double> distancias, 
                Dictionary<(string, string), string> siguiente) FloydWarshall()
        {
            List<string> vertices = listaAdyacencia.Keys.ToList();
            int n = vertices.Count;
            
            Dictionary<(string, string), double> dist = 
                new Dictionary<(string, string), double>();
            Dictionary<(string, string), string> siguiente = 
                new Dictionary<(string, string), string>();
            
            // Inicialización
            foreach (string i in vertices)
            {
                foreach (string j in vertices)
                {
                    if (i == j)
                    {
                        dist[(i, j)] = 0;
                        siguiente[(i, j)] = null;
                    }
                    else
                    {
                        dist[(i, j)] = double.PositiveInfinity;
                        siguiente[(i, j)] = null;
                    }
                }
            }
            
            // Agregar aristas existentes
            foreach (string u in vertices)
            {
                foreach (var (v, peso) in listaAdyacencia[u])
                {
                    dist[(u, v)] = peso;
                    siguiente[(u, v)] = v;
                }
            }
            
            // Algoritmo principal
            foreach (string k in vertices)
            {
                foreach (string i in vertices)
                {
                    foreach (string j in vertices)
                    {
                        if (dist[(i, k)] + dist[(k, j)] < dist[(i, j)])
                        {
                            dist[(i, j)] = dist[(i, k)] + dist[(k, j)];
                            siguiente[(i, j)] = siguiente[(i, k)];
                        }
                    }
                }
            }
            
            // Detectar ciclos negativos
            foreach (string i in vertices)
            {
                if (dist[(i, i)] < 0)
                {
                    throw new InvalidOperationException(
                        "Ciclo negativo detectado en el vertice " + i);
                }
            }
            
            return (dist, siguiente);
        }
        
        public List<string> ReconstruirCaminoFW(string origen, string destino,
            Dictionary<(string, string), string> siguiente)
        {
            if (siguiente[(origen, destino)] == null)
            {
                return null;
            }
            
            List<string> camino = new List<string>();
            string actual = origen;
            
            while (actual != destino)
            {
                camino.Add(actual);
                actual = siguiente[(actual, destino)];
                
                if (actual == null)
                {
                    return null;
                }
            }
            
            camino.Add(destino);
            return camino;
        }
        
        // ANALISIS AVANZADO
        
        public double DistanciaPromedio(string vertice)
        {
            var (distancias, _) = Dijkstra(vertice);
            
            double suma = 0;
            int contador = 0;
            
            foreach (var dist in distancias.Values)
            {
                if (!double.IsInfinity(dist) && dist > 0)
                {
                    suma += dist;
                    contador++;
                }
            }
            
            return contador > 0 ? suma / contador : double.PositiveInfinity;
        }
        
        public string EncontrarVerticeMasCentral()
        {
            string verticeMasCentral = null;
            double menorDistanciaPromedio = double.PositiveInfinity;
            
            foreach (string vertice in listaAdyacencia.Keys)
            {
                double distPromedio = DistanciaPromedio(vertice);
                
                if (distPromedio < menorDistanciaPromedio)
                {
                    menorDistanciaPromedio = distPromedio;
                    verticeMasCentral = vertice;
                }
            }
            
            return verticeMasCentral;
        }
        
        public double CalcularDistanciaMaxima()
        {
            double maxDistancia = 0;
            
            foreach (string vertice in listaAdyacencia.Keys)
            {
                var (distancias, _) = Dijkstra(vertice);
                
                foreach (double dist in distancias.Values)
                {
                    if (!double.IsInfinity(dist) && dist > maxDistancia)
                    {
                        maxDistancia = dist;
                    }
                }
            }
            
            return maxDistancia;
        }
        
        public void ExportarArchivo(string nombreArchivo)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(nombreArchivo))
                {
                    foreach (string vertice in listaAdyacencia.Keys)
                    {
                        foreach (var (destino, peso) in listaAdyacencia[vertice])
                        {
                            writer.WriteLine($"{vertice} {destino} {peso:F2}");
                        }
                    }
                }
                
                Console.WriteLine($"Archivo '{nombreArchivo}' exportado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al exportar: {ex.Message}");
            }
        }
        
        public static WeightedGraph CargarDesdeArchivo(string nombreArchivo)
        {
            WeightedGraph grafo = new WeightedGraph();
            
            try
            {
                string[] lineas = System.IO.File.ReadAllLines(nombreArchivo);
                
                foreach (string linea in lineas)
                {
                    string[] partes = linea.Split(' ');
                    
                    if (partes.Length >= 3)
                    {
                        string origen = partes[0];
                        string destino = partes[1];
                        
                        if (double.TryParse(partes[2], out double peso))
                        {
                            grafo.AgregarArista(origen, destino, peso);
                        }
                    }
                }
                
                Console.WriteLine($"Grafo cargado desde '{nombreArchivo}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar: {ex.Message}");
            }
            
            return grafo;
        }
    }
}