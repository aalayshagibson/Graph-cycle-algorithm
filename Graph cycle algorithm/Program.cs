using System;
using System.Collections.Generic;

namespace MinCycleGraph
{
    class Program
    {
        const int N = 5;
        const double INF = double.PositiveInfinity;

        static readonly double[,] W = new double[N, N]
        {
            { 0,   3,   INF, INF, 1   },
            { INF, 0,   6,   INF, 3   },
            { 1,   INF, 0,   INF, INF },
            { -4,  INF, 5,   0,   INF },
            { INF, INF, 2,   2,   0   }
        };

        static bool[] visited = new bool[N];
        static List<int> path = new List<int>();

        static double minWeight = INF;
        static List<List<int>> minCycles = new List<List<int>>();

        static void Main(string[] args)
        {
            for (int start = 0; start < N; start++)
            {
                Array.Fill(visited, false);
                path.Clear();

                visited[start] = true;
                path.Add(start);

                DFS(start, start, 0.0);
            }

            if (double.IsPositiveInfinity(minWeight))
            {
                Console.WriteLine("No cycles found in the graph.");
            }
            else
            {
                Console.WriteLine("Minimum cycle weight: " + minWeight);
                Console.WriteLine("Minimum-weight cycles (vertices are 1-based):");
                foreach (var cycle in minCycles)
                {
                    for (int i = 0; i < cycle.Count; i++)
                    {
                        int v = cycle[i] + 1; 
                        if (i < cycle.Count - 1)
                            Console.Write(v + " -> ");
                        else
                            Console.Write(v);
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        static void DFS(int start, int current, double weightSoFar)
        {
            for (int v = 0; v < N; v++)
            {
                double w = W[current, v];

                if (double.IsPositiveInfinity(w))
                    continue;

                if (v == start && path.Count >= 2)
                {
                    double total = weightSoFar + w;

                    int minVertex = path[0];
                    foreach (int node in path)
                    {
                        if (node < minVertex)
                            minVertex = node;
                    }
                    if (minVertex != start)
                        continue;

                    if (total < minWeight)
                    {
                        minWeight = total;
                        minCycles.Clear();

                        var cycleCopy = new List<int>(path);
                        cycleCopy.Add(start);
                        minCycles.Add(cycleCopy);
                    }
                    else if (Math.Abs(total - minWeight) < 1e-9)
                    {
                        var cycleCopy = new List<int>(path);
                        cycleCopy.Add(start);
                        minCycles.Add(cycleCopy);
                    }
                }
                else if (!visited[v] && v != start)
                {
                    visited[v] = true;
                    path.Add(v);

                    DFS(start, v, weightSoFar + w);

                    path.RemoveAt(path.Count - 1);
                    visited[v] = false;
                }
            }
        }
    }
}
