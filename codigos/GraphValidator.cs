using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGrafos
{
    public static class GraphValidator
    {
        public static bool IsGraphicalSequence(List<int> degrees)
        {
            if (degrees == null || degrees.Count == 0) return true;

            var seq = new List<int>(degrees);
            seq.Sort((a, b) => b.CompareTo(a));

            int sum = 0;
            foreach (int d in seq) sum += d;

            if (sum % 2 != 0 || seq[0] >= seq.Count) return false;

            while (seq.Count > 0)
            {
                seq.Sort((a, b) => b.CompareTo(a));

                int d1 = seq[0];
                seq.RemoveAt(0);

                if (d1 == 0) return true;
                if (d1 > seq.Count) return false;

                for (int i = 0; i < d1; i++)
                {
                    seq[i]--;
                    if (seq[i] < 0) return false;
                }
            }
            return true;
        }

        public static bool ValidateConsistency<T>(Grafo<T> grafo) where T : IComparable<T>
        {
            if (grafo.EsDirigido) return true;

            int totalDegree = 0;
            foreach (T v in grafo.ObtenerVertices())
            {
                totalDegree += grafo.GradoSalida(v);
            }

            return totalDegree % 2 == 0;
        }

        public static List<int> ExtractDegreeSequence<T>(Grafo<T> grafo) where T : IComparable<T>
        {
            List<int> degrees = new List<int>();

            foreach (T v in grafo.ObtenerVertices())
            {
                degrees.Add(grafo.GradoSalida(v));
            }

            degrees.Sort((a, b) => b.CompareTo(a));
            return degrees;
        }
    }
}