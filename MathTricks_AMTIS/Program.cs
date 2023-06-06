using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MathTricks_AMTIS
{
    /*
     * TODO: 
     * Make a better number generation algorithm, as the current one is highly unfair and lacks negative numbers
     * (?) Implement up to 4 players
     * Implement game sessions, see last bonus point on PDF file
    */
    internal partial class Program
    {
        static void Main(string[] args)
        {
            int _height;
            int _width;

            // Initial size validation
            while (true) {
                try
                {
                    Console.WriteLine("Enter grid height: ");
                    _height = int.Parse(Console.ReadLine()!);
                    Console.WriteLine("Enter grid width: ");
                    _width = int.Parse(Console.ReadLine()!);

                    if (_width >= 4 && _height >= 4)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Grid must be at least 4x4. Try again.");
                    }
                }
                catch(FormatException) {
                    Console.WriteLine("Input must be an integer. Try again.");
                }
            }

            ProgramHelpers.GenerateGrid(_height, _width);

            Player pl1 = new Player("Green", ConsoleColor.Green, 0, 0);
            Player pl2 = new Player("Red", ConsoleColor.Red, _height-1, _width-1);

            ProgramHelpers.players.Add(pl1);
            ProgramHelpers.players.Add(pl2);

            ProgramHelpers.PrintGrid(_height, _width, 0);

            // Gameplay loop
            int counter = 1;
            while (true)
            {
                if (pl1.GetAvailableSurroundingMoves().Count == 0 || pl2.GetAvailableSurroundingMoves().Count == 0)
                {
                    break;
                }

                if (counter % 2 != 0)
                {
                    Console.WriteLine("Player 1, make your move!");

                    pl1.Move();
                    ProgramHelpers.PrintGrid(_height, _width, counter);
                }
                else {
                    Console.WriteLine("Player 2, make your move!");

                    pl2.Move();
                    ProgramHelpers.PrintGrid(_height, _width, counter);
                }
                counter++;
            }


            Console.WriteLine();
            //Print winner(s)
            foreach (Player pl in ProgramHelpers.GetWinners(ProgramHelpers.players)) {
                Console.WriteLine($"{pl._name} wins!");
            }
            //Print scores
            foreach (Player pl in ProgramHelpers.players.OrderByDescending(x => x._score))
            {
                Console.WriteLine($"{pl._name}'s score: {pl._score}");
            }

        }
    }
}
 