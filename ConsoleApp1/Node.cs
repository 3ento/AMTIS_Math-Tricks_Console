namespace ConsoleApp1
{
    
        public class Node { 
            public int _x; public int _y;
            public bool available;
            public Player owner = null!;

            public int _value;
            public string _operation;
            public int _numeral_operation;

            public Node(int x, int y, int value, int operation) {
                _x = x;
                _y = y;
                _value = value;
                _operation = ProgramHelpers._operationList[operation];
                _numeral_operation = operation;
                available = true;
                owner = null!;
            }

            public void Occupy(Player player) { 
                available = false;
                owner = player;
            }

            public override string ToString()
            {
                //return $"{_operation}{_value}| Position: ({_x} {_y})";
                return $"{_operation}{_value}";
            }
        }
    
}
 