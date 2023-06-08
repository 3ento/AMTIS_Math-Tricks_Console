using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MathTricks_AMTIS
{
    /*
     * TODO: 
     * Implement game sessions, see last bonus point on PDF file
     * Improve inital parameter input, as currently making mistakes in one fields restarts the whole thing, and the code is just ugly too
    */
    internal partial class Program
    {
        static void Main(string[] args)
        {
            int _height;
            int _width;
            int _player_count;

            // Initial size validation
            while (true) {
                try
                {
                    Console.WriteLine("Enter grid height: ");
                    _height = int.Parse(Console.ReadLine()!);
                    Console.WriteLine("Enter grid width: ");
                    _width = int.Parse(Console.ReadLine()!);
                    Console.WriteLine("Enter number of players (between 2 and 4): ");
                    _player_count = int.Parse(Console.ReadLine()!);

                    if (_player_count < 2 || _player_count > 4) {
                        Console.WriteLine("Players cannot be less than 2 or more than 4. Try again.");
                        continue;
                    }

                    if (_width < 4 && _height < 4)
                    {
                        Console.WriteLine("Grid must be at least 4x4. Try again.");
                        continue;
                    }
                    break;
                    
                }
                catch(FormatException) {
                    Console.WriteLine("Input must be an integer. Try again.");
                }
            }

            // Generate players
            for (int i = 0; i < _player_count; i++) {
                switch (i) {
                    case 0:
                        ProgramHelpers.players.Add(new Player("Green", ConsoleColor.Green, 0, 0));
                        break;
                    case 1:
                        ProgramHelpers.players.Add(new Player("Red", ConsoleColor.Red, _height - 1, _width - 1));
                        break;
                    case 2:
                        ProgramHelpers.players.Add(new Player("Blue", ConsoleColor.Blue, 0, _width - 1));
                        break;
                    case 3:
                        ProgramHelpers.players.Add(new Player("Yellow", ConsoleColor.DarkYellow, _height - 1, 0));
                        break;
                }

            }

            ProgramHelpers.GenerateGrid(_height, _width);
            ProgramHelpers.PrintGrid(_height, _width, ProgramHelpers.players[0]);

            // Gameplay loop
            bool game = true;
            while (game)
            {
                foreach (Player pl in ProgramHelpers.players) {
                    if (ProgramHelpers.TerminateGame(ProgramHelpers.players))
                    {
                        game = false;
                        break;
                    }

                    ProgramHelpers.PrintGrid(_height, _width, pl);
                    Console.WriteLine($"{pl._name}, make your move!");

                    pl.Move();
                    
                }
                ProgramHelpers.PrintGrid(_height, _width, ProgramHelpers.players[0]);
            }


            Console.WriteLine();
            //Print winner(s)
            foreach (Player pl in ProgramHelpers.GetWinners(ProgramHelpers.players)) {
                Console.WriteLine($"{pl._name} wins!");
            }

            Console.WriteLine();
            //Print scores
            foreach (Player pl in ProgramHelpers.players.OrderByDescending(x => x._score))
            {
                Console.WriteLine($"{pl._name}'s score: {pl._score}");
            }

        }
    }
}
 