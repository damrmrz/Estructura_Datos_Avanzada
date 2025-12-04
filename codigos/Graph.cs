using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SistemaGrafos
{
    public class Grafo<T> where T : IComparable<T>
    {
        private readonly Dictionary<T, List<Arista<T>>> listaAdyacencia;
        private readonly bool esDirigido;
        
        public Grafo(bool dirigido = true)
        {
            listaAdyacencia = new Dictionary<T, List<Arista<T>>>();
            esDirigido = dirigido;
        }
        
        public bool EsDirigido => esDirigido;
        
        public void AgregarVertice(T vertice)
        {
            if (!listaAdyacencia.ContainsKey(vertice))
            {
                listaAdyacencia[vertice] = new List<Arista<T>>();
                Console.WriteLine("Vertice '" + vertice + "' agregado");
            }
        }
        
        public void AgregarArista(T origen, T destino, double peso = 1.0)
        {
            AgregarVertice(origen);
            AgregarVertice(destino);
            
            listaAdyacencia[origen].Add(new Arista<T>(destino, peso));
            
            if (!esDirigido)
            {
                listaAdyacencia[destino].Add(new Arista<T>(origen, peso));
            }
            
            string tipo = esDirigido ? "->" : "<->";
            Console.WriteLine("Arista " + origen + " " + tipo + " " + destino + " (peso: " + peso + ") agregada");
        }
        
        public bool EliminarArista(T origen, T destino)
        {
            if (!listaAdyacencia.ContainsKey(origen))
                return false;
            
            bool eliminada = listaAdyacencia[origen].RemoveAll(a => 
                EqualityComparer<T>.Default.Equals(a.Destino, destino)) > 0;
            
            if (!esDirigido && listaAdyacencia.ContainsKey(destino))
            {
                listaAdyacencia[destino].RemoveAll(a => 
                    EqualityComparer<T>.Default.Equals(a.Destino, origen));
            }
            
            return eliminada;
        }
        
        public bool ExisteArista(T origen, T destino)
        {
            return listaAdyacencia.TryGetValue(origen, out List<Arista<T>> aristas) && 
                    aristas.Any(a => EqualityComparer<T>.Default.Equals(a.Destino, destino));
        }
        
        public IEnumerable<Arista<T>> ObtenerVecinos(T vertice)
        {
            return listaAdyacencia.TryGetValue(vertice, out List<Arista<T>> vecinos) 
                ? vecinos 
                : Enumerable.Empty<Arista<T>>();
        }
        
        public int GradoSalida(T vertice)
        {
            return listaAdyacencia.TryGetValue(vertice, out List<Arista<T>> aristas) 
                ? aristas.Count 
                : 0;
        }
        
        public int GradoEntrada(T vertice)
        {
            return listaAdyacencia.Values
                .SelectMany(aristas => aristas)
                .Count(a => EqualityComparer<T>.Default.Equals(a.Destino, vertice));
        }
        
        public int Grado(T vertice)
        {
            return esDirigido ? GradoSalida(vertice) + GradoEntrada(vertice) : GradoSalida(vertice);
        }
        
        public IEnumerable<T> ObtenerVertices()
        {
            return listaAdyacencia.Keys;
        }
        
        public int NumeroVertices 
        { 
            get { return listaAdyacencia.Count; }
        }
        
        public int NumeroAristas
        {
            get
            {
                int total = listaAdyacencia.Values.Sum(aristas => aristas.Count);
                return esDirigido ? total : total / 2;
            }
        }
        
        public void MostrarGrafo()
        {
            string tipo = esDirigido ? "Dirigido" : "No Dirigido";
            string separador = new string('=', 50);
            Console.WriteLine("\n" + separador);
            Console.WriteLine("  GRAFO " + tipo.ToUpper());
            Console.WriteLine("  Vertices: " + NumeroVertices + " | Aristas: " + NumeroAristas);
            Console.WriteLine(separador);
            
            foreach (T vertice in listaAdyacencia.Keys.OrderBy(v => v))
            {
                string conexiones = string.Join(", ", 
                    listaAdyacencia[vertice].Select(a => a.Destino + "(" + a.Peso.ToString("F1") + ")"));
                
                Console.WriteLine("  " + vertice + ": [" + conexiones + "]");
            }
            Console.WriteLine(separador + "\n");
        }
        
        public void ExportarArchivo(string nombreArchivo, bool incluirPesos = true)
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(nombreArchivo))
                {
                    HashSet<string> aristasEscritas = new HashSet<string>();
                    
                    foreach (T vertice in listaAdyacencia.Keys.OrderBy(v => v))
                    {
                        foreach (Arista<T> arista in listaAdyacencia[vertice])
                        {
                            string clave;
                            
                            if (!esDirigido)
                            {
                                string v1 = vertice.ToString();
                                string v2 = arista.Destino.ToString();
                                clave = string.Compare(v1, v2) <= 0 
                                    ? v1 + "-" + v2
                                    : v2 + "-" + v1;
                                
                                if (aristasEscritas.Contains(clave))
                                    continue;
                                
                                aristasEscritas.Add(clave);
                            }
                            
                            string linea = incluirPesos 
                                ? vertice + " " + arista.Destino + " " + arista.Peso.ToString("F1")
                                : vertice + " " + arista.Destino;
                            
                            escritor.WriteLine(linea);
                        }
                    }
                }
                
                Console.WriteLine("Archivo '" + nombreArchivo + "' exportado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar: " + ex.Message);
            }
        }
        
        public List<T> BusquedaProfundidad(T inicio)
        {
            HashSet<T> visitados = new HashSet<T>();
            List<T> resultado = new List<T>();
            DFSRecursivo(inicio, visitados, resultado);
            return resultado;
        }
        
        private void DFSRecursivo(T vertice, HashSet<T> visitados, List<T> resultado)
        {
            visitados.Add(vertice);
            resultado.Add(vertice);
            
            foreach (Arista<T> arista in listaAdyacencia[vertice])
            {
                if (!visitados.Contains(arista.Destino))
                {
                    DFSRecursivo(arista.Destino, visitados, resultado);
                }
            }
        }
        
        public List<T> BusquedaAmplitud(T inicio)
        {
            HashSet<T> visitados = new HashSet<T>();
            List<T> resultado = new List<T>();
            Queue<T> cola = new Queue<T>();
            
            cola.Enqueue(inicio);
            visitados.Add(inicio);
            
            while (cola.Count > 0)
            {
                T vertice = cola.Dequeue();
                resultado.Add(vertice);
                
                foreach (Arista<T> arista in listaAdyacencia[vertice])
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
    }
    
    public class Arista<T>
    {
        public T Destino { get; private set; }
        public double Peso { get; private set; }
        
        public Arista(T destino, double peso = 1.0)
        {
            Destino = destino;
            Peso = peso;
        }
    }
}