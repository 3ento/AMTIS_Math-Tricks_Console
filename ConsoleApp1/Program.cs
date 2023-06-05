using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ConsoleApp1 
{
    /*
     * TODO: 
     * Organize code by seperating big classes and functions like Player
     * Figure out why diagonal moves don't work sometimes despite being recognized as valid moves by GetAvailableSurroundingMoves()
     * Make a better number generation algorithym, as the current one is highly unfair and lacks negative numbers
     * (?) Implement up to 4 players
     * Implement game sessions, see last bonus point on PDF file
     * Find a way to evenly print the grid, as rn it's kinda hard to see what node is above/below you as opposed to diagonally to you
     * Implement better keybinds, hopefully some sort of list works but if not, just WASD/Arrow Keys(idk how diags would work tho)/Numpad
    */
    internal class Program
    {
        public static Dictionary<int, string> _operationList = new Dictionary<int, string>()
            {
                {0,"+"}, {1,"-"}, {2,"*"}, {3, "/"}, {4, ""}
            };

        public static List<List<Node>> grid = new List<List<Node>>();

        public static List<Player> players = new List<Player>();

        // temp, probably wouldn't work
        public static Dictionary<string, List<ConsoleKey>> keybinds = new Dictionary<string, List<ConsoleKey>>() {
            {"up", new List<ConsoleKey>() {ConsoleKey.W, ConsoleKey.UpArrow, ConsoleKey.NumPad8 } }
        };

        public static void MoveAttempt(int new_x, int new_y, Player pl) {
            while (true)
            {
                if (isNodeAvailableForMove(new_x, new_y) && pl.GetAvailableSurroundingMoves().Contains(grid[new_x][new_y]))
                {
                    pl.ShiftPosition(grid[new_x][new_y]);
                    break;
                }
                else
                {
                    Console.WriteLine("This move is illegal, please pick one of the available positions: ");

                    foreach (Node node in pl.GetAvailableSurroundingMoves())
                    {
                        Console.WriteLine(node);
                    }

                    pl.Move();
                    break;
                }
            }
        }

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

                    int value = rnd.Next(0, 11);
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

                        Console.Write(grid[i][j]._operation + grid[i][j]._value + " ");
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
                    Console.Write(grid[i][j]._operation + grid[i][j]._value + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
            Console.WriteLine($"{current_player._name}'s current score: {current_player._score}");
        }

        public static bool isNodeAvailableForMove(int x, int y)
        {
            if (x < 0 || x >= grid.Count() || y < 0 || y >= grid[0].Count())
            {
                return false;
            }
            return grid[x][y].available;
        }

        public class Player {
            public string _name;
            public int _score = 0;
            public List<Node> taken_nodes = new List<Node>();
            public ConsoleColor _color;

            public Player(string name, ConsoleColor color, int starting_pos_x, int starting_pos_y) { 
                _name = name;
                _color = color;
                taken_nodes.Add(grid[starting_pos_x][starting_pos_y]);
                grid[starting_pos_x][starting_pos_y].Occupy(this);
            }

            public Node GetCurrentPosition() {
                return this.taken_nodes.Last();
            }

            public void ChangeScore(int score, int operation) { 
                switch (operation)
                {
                    case 0:
                        _score += score;
                        break;
                    case 1:
                        _score -= score;
                        break;
                    case 2: 
                        _score *= score;
                        break;
                    case 3: 
                        _score /= score; 
                        break;
                }
            }

            public List<Node> GetAvailableSurroundingMoves()
            {
                Node current_position = GetCurrentPosition();

                List<int[]> surrounding_moves = new List<int[]>();
                List<Node> available_moves = new List<Node>();

                surrounding_moves.Add(new int[2] { current_position._x, current_position._y + 1 });
                surrounding_moves.Add(new int[2] { current_position._x, current_position._y - 1 });
                surrounding_moves.Add(new int[2] { current_position._x + 1, current_position._y });
                surrounding_moves.Add(new int[2] { current_position._x - 1, current_position._y });
                surrounding_moves.Add(new int[2] { current_position._x - 1, current_position._y - 1 });
                surrounding_moves.Add(new int[2] { current_position._x - 1, current_position._y + 1 });
                surrounding_moves.Add(new int[2] { current_position._x + 1, current_position._y + 1 });
                surrounding_moves.Add(new int[2] { current_position._x + 1, current_position._y - 1 });

                foreach (int[] node in surrounding_moves)
                {
                    int x = node[0];
                    int y = node[1];
                    if (isNodeAvailableForMove(x, y))
                    {
                        available_moves.Add(grid[x][y]);
                    }
                }

                return available_moves;
            }

            public void ShiftPosition(Node new_position) {
                taken_nodes.Add(new_position);
                taken_nodes.Last().Occupy(this);
                this.ChangeScore(taken_nodes.Last()._value, taken_nodes.Last()._numeral_operation);
            }

            public void Move() {
                ConsoleKey input = Console.ReadKey().Key;

                int new_x;
                int new_y;
                switch (input)
                {
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        new_x = taken_nodes.Last()._x;
                        new_y = taken_nodes.Last()._y + 1;

                        MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        new_x = taken_nodes.Last()._x;
                        new_y = taken_nodes.Last()._y - 1;

                        MoveAttempt(new_x, new_y, this);

                        break;
                        
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        new_x = taken_nodes.Last()._x + 1;
                        new_y = taken_nodes.Last()._y;

                        MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        new_x = taken_nodes.Last()._x - 1;
                        new_y = taken_nodes.Last()._y;

                        MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.Q:
                        new_x = taken_nodes.Last()._x - 1;
                        new_y = taken_nodes.Last()._y - 1;

                        MoveAttempt(new_x, new_y, this);

                        break;
                    case ConsoleKey.E:
                        new_x = taken_nodes.Last()._x - 1;
                        new_y = taken_nodes.Last()._y + 1;

                        MoveAttempt(new_x, new_y, this);

                        break;
                    case ConsoleKey.Z:
                        new_x = taken_nodes.Last()._x + 1;
                        new_y = taken_nodes.Last()._y - 1;

                        MoveAttempt(new_x, new_y, this);

                        break;
                    case ConsoleKey.C:
                        new_x = taken_nodes.Last()._x + 1;
                        new_y = taken_nodes.Last()._x + 1;

                        MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.H:
                        Player pl = players[0];
                        foreach (Node node in GetAvailableSurroundingMoves()) { 
                            Console.WriteLine(node);
                        }
                        break;

                }
            }
            
            public override string ToString()
            {
                return $"Name: {this._name} " +
                    $"\nScore: {_score} " +
                    $"\nCurrPosVal: {GetCurrentPosition()}\n";
            }
        }

        public class Node { 
            public int _x; public int _y;
            public bool available;
            public Player owner = null;

            public int _value;
            public string _operation;
            public int _numeral_operation;

            public Node(int x, int y, int value, int operation) {
                _x = x;
                _y = y;
                _value = value;
                _operation = _operationList[operation];
                _numeral_operation = operation;
                available = true;
                owner = null;
            }

            public void Occupy(Player player) { 
                available = false;
                owner = player;
            }

            public override string ToString()
            {
                return $"{_operation}{_value}| Position: ({_x} {_y})";
            }
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enter grid height: ");
            int _width = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter grid width: ");
            int _height = int.Parse(Console.ReadLine()!);

            GenerateGrid(_width, _height);

            Player pl1 = new Player("Green", ConsoleColor.Green, 0, 0);
            Player pl2 = new Player("Red", ConsoleColor.Red, _width-1, _height-1);

            players.Add(pl1);
            players.Add(pl2);

            PrintGrid(_width, _height, 0);

            
            int counter = 1;
            while (true)
            {
                if (counter % 2 != 0)
                {
                    Console.WriteLine("Player 1, make your move!");

                    pl1.Move();
                    PrintGrid(_width, _height, counter);

                    if (pl1.GetAvailableSurroundingMoves().Count == 0)
                    {
                        break;
                    }
                }
                else {
                    Console.WriteLine("Player 2, make your move!");

                    pl2.Move();
                    PrintGrid(_width, _height, counter);

                    if (pl2.GetAvailableSurroundingMoves().Count == 0)
                    {
                        break;
                    }
                }
                counter++;
            }

            Console.WriteLine();
            Console.WriteLine($"{players.Select(x => x).Where(x => x._score == players.Select(x => x._score).Max()).Select(x => x._name).ToList()[0]} wins!");
            Console.WriteLine($"{pl1._name}'s score: {pl1._score}");
            Console.WriteLine($"{pl2._name}'s score: {pl2._score}");

        }
    }
}
 