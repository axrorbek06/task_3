using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace task3
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter an odd number of non-repeating strings for moves (e.g., Rock Paper Scissors):");
            var moves = GetMovesFromUser();

            if (moves.Length < 3 || moves.Length % 2 == 0 || HasDuplicates(moves))
            {
                Console.WriteLine("Incorrect input. Please provide an odd number of non-repeating strings.");
                return;
            }

            var key = GenerateKey();
            var computerMove = GetComputerMove(moves);
            var hmac = CalculateHMAC(key, computerMove);
            ShowHMAC(hmac);

            DisplayMenu(moves);

            int userMove;
            while (true)
            {
                Console.Write("Enter your move (number): ");
                var input = Console.ReadLine();
                if (input == "?")
                {
                    TableGenerator.GenerateTable(moves);
                    continue;

                }
                else if (int.TryParse(input, out userMove) && userMove >= 0 && userMove <= moves.Length)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a valid move number or '?' for help.");
            }

            if (userMove == 0)
            {
                Console.WriteLine("Exiting the game.");
                return;
            }

            var userMoveString = moves[userMove - 1];
            Console.WriteLine($"Your move: {userMoveString}");
            Console.WriteLine($"Computer move: {computerMove}");

            var result = GetResult(userMove, moves.Length, computerMove, moves);
            if (result == GameResult.Win)
                Console.WriteLine("You win!");
            else if (result == GameResult.Lose)
                Console.WriteLine("You lose!");
            else
                Console.WriteLine("It's a draw!");

            Console.WriteLine($"HMAC key: {ByteArrayToHexString(key)}");

            Main();
        }

        static string[] GetMovesFromUser()
        {
            var input = Console.ReadLine();
            return input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        static bool HasDuplicates(string[] array)
        {
            var set = new HashSet<string>();
            foreach (var item in array)
            {
                if (!set.Add(item))
                    return true;
            }
            return false;
        }

        static byte[] GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[32]; // 256 bits
                rng.GetBytes(key);
                return key;
            }
        }

        static string GetComputerMove(string[] moves)
        {
            var random = new Random();
            var randomIndex = random.Next(0, moves.Length);
            return moves[randomIndex];
        }

        static byte[] CalculateHMAC(byte[] key, string data)
        {
            using (var hmac = new HMACSHA256(key))
            {
                var dataBytes = Encoding.UTF8.GetBytes(data);
                return hmac.ComputeHash(dataBytes);
            }
        }

        static void ShowHMAC(byte[] hmac)
        {
            Console.WriteLine($"HMAC: {ByteArrayToHexString(hmac)}");
        }

        static string ByteArrayToHexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        static void DisplayMenu(string[] moves)
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < moves.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {moves[i]}");
            }
            Console.WriteLine("0 - exit");
            Console.WriteLine("? - help");
        }

        static GameResult GetResult(int userMove, int totalMoves, string computerMove, string[] moves)
        {
            int movesInCircle = totalMoves / 2;
            int computerMoveIndex = Array.IndexOf(moves, computerMove);
            int userMoveIndex = userMove - 1;

            int distance = (userMoveIndex - computerMoveIndex + totalMoves) % totalMoves;
            if (distance == 0)
                return GameResult.Draw;
            else if (distance <= movesInCircle)
                return GameResult.Win;
            else
                return GameResult.Lose;
        }

        enum GameResult
        {
            Win,
            Lose,
            Draw
        }
    }
}
