using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGrafos
{
    /// <summary>
    /// Clase que implementa algoritmos de exploración y búsqueda en grafos.
    /// Incluye BFS, DFS (recursivo e iterativo), y aplicaciones prácticas.
    /// </summary>
    public class GraphTraversal
    {
        private Grafo<string> grafo;
        
        public GraphTraversal(Grafo<string> grafo)
        {
            this.grafo = grafo;
        }
        
        // ========================================
        // BÚSQUEDA EN AMPLITUD (BFS)
        // ========================================
        
        /// <summary>
        /// Realiza BFS desde un nodo inicial y retorna el orden de visita.
        /// Complejidad: O(V + E)
        /// </summary>
        public List<string> BFS(string inicio)
        {
            if (!grafo.ObtenerVertices().Contains(inicio))
            {
                throw new ArgumentException("El nodo " + inicio + " no existe en el grafo");
            }
            
            HashSet<string> visitados = new HashSet<string>();
            List<string> resultado = new List<string>();
            Queue<string> cola = new Queue<string>();
            
            cola.Enqueue(inicio);
            visitados.Add(inicio);
            
            while (cola.Count > 0)
            {
                string actual = cola.Dequeue();
                resultado.Add(actual);
                
                foreach (Arista<string> arista in grafo.ObtenerVecinos(actual))
                {
                    if (!visitados.Contains(arista.Destino))
                    {
                        visitados.Add(arista.Destino);
                        cola.Enqueue(arista.Destino);
                    }
                }
            }
            
            return resultado;
        }
        
        /// <summary>
        /// BFS que retorna distancias desde el nodo inicial.
        /// Útil para encontrar caminos más cortos en grafos no ponderados.
        /// </summary>
        public Dictionary<string, int> BFSDistancias(string inicio)
        {
            Dictionary<string, int> distancias = new Dictionary<string, int>();
            Queue<string> cola = new Queue<string>();
            
            cola.Enqueue(inicio);
            distancias[inicio] = 0;
            
            while (cola.Count > 0)
            {
                string actual = cola.Dequeue();
                
                foreach (Arista<string> arista in grafo.ObtenerVecinos(actual))
                {
                    if (!distancias.ContainsKey(arista.Destino))
                    {
                        distancias[arista.Destino] = distancias[actual] + 1;
                        cola.Enqueue(arista.Destino);
                    }
                }
            }
            
            return distancias;
        }
        
        /// <summary>
        /// BFS que reconstruye el camino más corto de inicio a fin.
        /// Retorna null si no hay camino.
        /// </summary>
        public List<string> BFSCaminoMasCorto(string inicio, string fin)
        {
            Dictionary<string, string> padre = new Dictionary<string, string>();
            HashSet<string> visitados = new HashSet<string>();
            Queue<string> cola = new Queue<string>();
            
            cola.Enqueue(inicio);
            visitados.Add(inicio);
            padre[inicio] = null;
            
            bool encontrado = false;
            
            while (cola.Count > 0 && !encontrado)
            {
                string actual = cola.Dequeue();
                
                if (actual == fin)
                {
                    encontrado = true;
                    break;
                }
                
                foreach (Arista<string> arista in grafo.ObtenerVecinos(actual))
                {
                    if (!visitados.Contains(arista.Destino))
                    {
                        visitados.Add(arista.Destino);
                        padre[arista.Destino] = actual;
                        cola.Enqueue(arista.Destino);
                    }
                }
            }
            
            if (!encontrado)
            {
                return null;
            }
            
            // Reconstruir camino
            List<string> camino = new List<string>();
            string nodo = fin;
            
            while (nodo != null)
            {
                camino.Add(nodo);
                nodo = padre[nodo];
            }
            
            camino.Reverse();
            return camino;
        }
        
        // ========================================
        // BÚSQUEDA EN PROFUNDIDAD (DFS)
        // ========================================
        
        /// <summary>
        /// DFS recursivo desde un nodo inicial.
        /// </summary>
        public List<string> DFSRecursivo(string inicio)
        {
            if (!grafo.ObtenerVertices().Contains(inicio))
            {
                throw new ArgumentException("El nodo " + inicio + " no existe en el grafo");
            }
            
            HashSet<string> visitados = new HashSet<string>();
            List<string> resultado = new List<string>();
            
            DFSRecursivoHelper(inicio, visitados, resultado);
            
            return resultado;
        }
        
        private void DFSRecursivoHelper(string nodo, HashSet<string> visitados, List<string> resultado)
        {
            visitados.Add(nodo);
            resultado.Add(nodo);
            
            foreach (Arista<string> arista in grafo.ObtenerVecinos(nodo))
            {
                if (!visitados.Contains(arista.Destino))
                {
                    DFSRecursivoHelper(arista.Destino, visitados, resultado);
                }
            }
        }
        
        /// <summary>
        /// DFS iterativo usando pila explícita.
        /// Más seguro para grafos muy profundos (evita stack overflow).
        /// </summary>
        public List<string> DFSIterativo(string inicio)
        {
            if (!grafo.ObtenerVertices().Contains(inicio))
            {
                throw new ArgumentException("El nodo " + inicio + " no existe en el grafo");
            }
            
            HashSet<string> visitados = new HashSet<string>();
            List<string> resultado = new List<string>();
            Stack<string> pila = new Stack<string>();
            
            pila.Push(inicio);
            
            while (pila.Count > 0)
            {
                string actual = pila.Pop();
                
                if (visitados.Contains(actual))
                {
                    continue;
                }
                
                visitados.Add(actual);
                resultado.Add(actual);
                
                // Apilar vecinos en orden inverso
                List<Arista<string>> vecinos = grafo.ObtenerVecinos(actual).ToList();
                
                for (int i = vecinos.Count - 1; i >= 0; i--)
                {
                    if (!visitados.Contains(vecinos[i].Destino))
                    {
                        pila.Push(vecinos[i].Destino);
                    }
                }
            }
            
            return resultado;
        }
        
        // ========================================
        // APLICACIONES AVANZADAS
        // ========================================
        
        /// <summary>
        /// Encuentra todas las componentes conectadas en un grafo no dirigido.
        /// </summary>
        public List<List<string>> EncontrarComponentesConectadas()
        {
            HashSet<string> visitados = new HashSet<string>();
            List<List<string>> componentes = new List<List<string>>();
            
            foreach (string nodo in grafo.ObtenerVertices())
            {
                if (!visitados.Contains(nodo))
                {
                    List<string> componente = new List<string>();
                    DFSParaComponente(nodo, visitados, componente);
                    componentes.Add(componente);
                }
            }
            
            return componentes;
        }
        
        private void DFSParaComponente(string nodo, HashSet<string> visitados, List<string> componente)
        {
            visitados.Add(nodo);
            componente.Add(nodo);
            
            foreach (Arista<string> arista in grafo.ObtenerVecinos(nodo))
            {
                if (!visitados.Contains(arista.Destino))
                {
                    DFSParaComponente(arista.Destino, visitados, componente);
                }
            }
        }
        
        /// <summary>
        /// Calcula la distancia máxima entre dos nodos cualesquiera del grafo.
        /// </summary>
        public int DistanciaMaxima()
        {
            int maxDistancia = 0;
            
            foreach (string vertice in grafo.ObtenerVertices())
            {
                Dictionary<string, int> distancias = BFSDistancias(vertice);
                
                foreach (int distancia in distancias.Values)
                {
                    if (distancia > maxDistancia)
                    {
                        maxDistancia = distancia;
                    }
                }
            }
            
            return maxDistancia;
        }
        
        /// <summary>
        /// Encuentra el nodo más central (menor distancia promedio a todos los demás).
        /// </summary>
        public string EncontrarNodoCentral()
        {
            string nodoCentral = null;
            double menorDistanciaPromedio = double.MaxValue;
            
            foreach (string vertice in grafo.ObtenerVertices())
            {
                Dictionary<string, int> distancias = BFSDistancias(vertice);
                
                if (distancias.Count == 0)
                {
                    continue;
                }
                
                double suma = 0;
                foreach (int dist in distancias.Values)
                {
                    suma += dist;
                }
                
                double promedio = suma / distancias.Count;
                
                if (promedio < menorDistanciaPromedio)
                {
                    menorDistanciaPromedio = promedio;
                    nodoCentral = vertice;
                }
            }
            
            return nodoCentral;
        }
    }
}