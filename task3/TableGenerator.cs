

namespace task3
{
    using System;

    class TableGenerator
    {
        public static void GenerateTable(string[] moves)
        {
            int n = moves.Length;
            string[,] table = new string[n + 1, n + 1];

            for (int i = 0; i < n; i++)
            {
                table[0, i + 1] = table[i + 1, 0] = moves[i];
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int distance = (j - i + n) % n;
                    if (distance == 0)
                        table[i, j] = "Draw";
                    else if (distance <= n / 2)
                        table[i, j] = "Win";
                    else
                        table[i, j] = "Lose";
                }
            }

            PrintTable(table, n + 1, n + 1);
        }

        private static void PrintTable(string[,] table, int rows, int cols)
        {
            Console.WriteLine("Help table:");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(table[i, j].PadRight(10));
                }
                Console.WriteLine();
            }
        }
    }

}
