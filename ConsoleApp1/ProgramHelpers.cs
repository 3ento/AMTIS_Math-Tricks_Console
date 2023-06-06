using ConsoleApp1;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
internal static class ProgramHelpers
{
    public static Dictionary<int, string> _operationList = new Dictionary<int, string>()
        {
            {0,"+"}, {1,"-"}, {2,"*"}, {3, "/"}, {4, ""}
        };

    public static List<List<Node>> grid = new List<List<Node>>();

    public static List<Player> players = new List<Player>();

    public static void GenerateGrid(int _width, int _height)
    {
        Random rnd = new Random();
        for (int i = 0; i < _width; i++)
        {
            grid.Add(new List<Node>());
            for (int j = 0; j < _height; j++)
            {
                // sets starting pos colors
                if (i == 0 && j == 0 || i == _width - 1 && j == _height - 1)
                {
                    grid[i].Add(new Node(i, j, 0, 4));
                    continue;
                }

                int value = rnd.Next(-10, 11);
                int operation = rnd.Next(0, 4);

                // prevent divide by 0
                if (value == 0 && operation == 3)
                {
                    operation = 2;
                }

                grid[i].Add(new Node(i, j, value, operation));
            }
        }
    }

    public static bool isNodeAvailableForMove(int x, int y)
    {
        if (x < 0 || x >= grid.Count() || y < 0 || y >= grid[0].Count())
        {
            return false;
        }
        return grid[x][y].available;
    }

    public static void MoveAttempt(int new_x, int new_y, Player pl)
    {
        while (true)
        {
            if (isNodeAvailableForMove(new_x, new_y) && pl.GetAvailableSurroundingMoves().Contains(grid[new_x][new_y]))
            {
                pl.ShiftPosition(grid[new_x][new_y]);
                break;
            }
            else
            {
                Console.WriteLine($"({pl.GetCurrentPosition()._x} {pl.GetCurrentPosition()._y}) can't go to ({new_x} {new_y}), please pick one of the available positions: ");

                foreach (Node node in pl.GetAvailableSurroundingMoves())
                {
                    Console.WriteLine($"{node} at position ({node._x}, {node._y})");
                }

                pl.Move();
                break;
            }
        }
    }

    public static void PrintGrid(int _width, int _height, int turn)
    {
        Console.Clear();
        Player current_player;

        if (turn % 2 == 0)
        {
            current_player = players[0];
        }
        else
        {
            current_player = players[1];
        }


        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                // sets starting pos colors
                if (i == 0 && j == 0 || i == _width - 1 && j == _height - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (i == _width - 1 && j == _height - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write(string.Format("{0,4} ", grid[i][j]));
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }


                ConsoleColor color;
                if (grid[i][j].owner != null)
                {
                    if (grid[i][j] == grid[i][j].owner.GetCurrentPosition() && grid[i][j].owner == current_player)
                    {

                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    color = grid[i][j].owner._color;
                }
                else
                {
                    color = ConsoleColor.White;
                }

                Console.ForegroundColor = color;

                Console.Write(string.Format("{0,4} ", grid[i][j]));
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }
        Console.WriteLine($"{current_player._name}'s current score: {current_player._score}");
    }

    public static List<Player> GetWinners(List<Player> players) 
    {
        int max_score = players.Select(x => x).Where(x => x._score == players.Select(x => x._score).Max()).Select(x => x._score).ToList()[0];
        List<Player> winners = new List<Player>();

        foreach (Player pl in players) {
            if (pl._score == max_score) { 
                winners.Add(pl);
            }
        }

        return winners;
    }
}