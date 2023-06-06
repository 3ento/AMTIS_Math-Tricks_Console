namespace MathTricks_AMTIS
{
    
        public class Player
        {
            public string _name;
            public int _score = 0;
            public List<Node> taken_nodes = new List<Node>();
            public ConsoleColor _color;

            public Player(string name, ConsoleColor color, int starting_pos_x, int starting_pos_y)
            {
                _name = name;
                _color = color;
                taken_nodes.Add(ProgramHelpers.grid[starting_pos_x][starting_pos_y]);
                ProgramHelpers.grid[starting_pos_x][starting_pos_y].Occupy(this);
            }

            public Node GetCurrentPosition()
            {
                return this.taken_nodes.Last();
            }

            public void ChangeScore(int score, int operation)
            {
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
                    if (ProgramHelpers.isNodeAvailableForMove(x, y))
                    {
                        available_moves.Add(ProgramHelpers.grid[x][y]);
                    }
                }

                return available_moves;
            }

            public void ShiftPosition(Node new_position)
            {
                taken_nodes.Add(new_position);
                taken_nodes.Last().Occupy(this);
                this.ChangeScore(taken_nodes.Last()._value, taken_nodes.Last()._numeral_operation);
            }

            public void Move()
            {
                ConsoleKey input = Console.ReadKey().Key;

                int new_x;
                int new_y;
                switch (input)
                {
                    case ConsoleKey.D:
                    case ConsoleKey.NumPad6:
                        new_x = taken_nodes.Last()._x;
                        new_y = taken_nodes.Last()._y + 1;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.A:
                    case ConsoleKey.NumPad4:
                        new_x = taken_nodes.Last()._x;
                        new_y = taken_nodes.Last()._y - 1;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.NumPad2:
                        new_x = taken_nodes.Last()._x + 1;
                        new_y = taken_nodes.Last()._y;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.W:
                    case ConsoleKey.NumPad8:
                        new_x = taken_nodes.Last()._x - 1;
                        new_y = taken_nodes.Last()._y;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;

                    case ConsoleKey.Q:
                    case ConsoleKey.NumPad7:
                        new_x = taken_nodes.Last()._x - 1;
                        new_y = taken_nodes.Last()._y - 1;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;
                    case ConsoleKey.E:
                    case ConsoleKey.NumPad9:
                        new_x = taken_nodes.Last()._x - 1;
                        new_y = taken_nodes.Last()._y + 1;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;
                    case ConsoleKey.Z:
                    case ConsoleKey.NumPad1:
                        new_x = taken_nodes.Last()._x + 1;
                        new_y = taken_nodes.Last()._y - 1;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;
                    case ConsoleKey.C:
                    case ConsoleKey.NumPad3:
                        new_x = taken_nodes.Last()._x + 1;
                        new_y = taken_nodes.Last()._y + 1;
                        ProgramHelpers.
                                                MoveAttempt(new_x, new_y, this);

                        break;
                    default:
                        Console.WriteLine("Invalid input. Please use WASDQEZC or NumPad.");
                        Move();

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
    
}
 