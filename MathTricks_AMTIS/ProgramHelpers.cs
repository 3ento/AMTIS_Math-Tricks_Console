using MathTricks_AMTIS;
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
        for (int i = 0; i < _width; i++)
        {
            grid.Add(new List<Node>());
            for (int j = 0; j < _height; j++)
            {
                int[] value_operation_arr = GenerateNodeValues();

                int value = value_operation_arr[0];
                int operation = value_operation_arr[1];
                
                // prevent divide by 0
                if (value == 0 && operation == 3)
                {
                    operation = 2;
                }

                grid[i].Add(new Node(i, j, value, operation));
            }
            
        }
        foreach (Player pl in players)
        {
            pl.taken_nodes.Add(grid[pl.starting_pos_x][pl.starting_pos_y]);
            grid[pl.starting_pos_x][pl.starting_pos_y].Occupy(pl);

            grid[pl.starting_pos_x][pl.starting_pos_y]._value = 0;
            grid[pl.starting_pos_x][pl.starting_pos_y]._operation = "";

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

    public static void PrintGrid(int _width, int _height, Player current_player)
    {
        Console.Clear();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
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
        int max_score = players.OrderByDescending(x => x._score).First()._score;
        List<Player> winners = new List<Player>();

        foreach (Player pl in players) {
            if (pl._score == max_score) { 
                winners.Add(pl);
            }
        }

        return winners;
    }

    public static int[] GenerateNodeValues() {
        double sample = new Random().NextDouble();
        Random selector = new Random();
        int value = 1;
        int operation = 0;
        int idx;

        List<int> super_rare_values = new List<int>() { 9, 10, -9, -10, 0 };

        List<int> rare_values = new List<int>() { 2, 7, 8, -2, -7, -8 };

        List<int> common_values = new List<int>() { 3, 4, 5, 6, -3, -4, -5, -6 };

        if (0.1 >= sample)
        {
            idx = selector.Next(0, 6);
            value = rare_values[idx];
            operation = selector.Next(2, 4);
        }
        else if (0.2 <= sample && sample <= 0.8)
        {
            idx = selector.Next(0, 8);
            value = common_values[idx];
            operation = selector.Next(0, 2);
        }
        else if (0.9 < sample) {
            idx = selector.Next(0, 5);
            value = super_rare_values[idx];
            operation = selector.Next(2, 4);
        }

        return new int[2] {value, operation};
    }

    public static bool TerminateGame(List<Player> players) {
        foreach (Player pl in players) {
            if (pl.GetAvailableSurroundingMoves().Count == 0) {
                return true;
            }
        }
        return false;
    }
}