# AMTIS_Math-Tricks_Console
[Az Moga Tuk I Sega](https://az-moga.bg/) (translated "I can, right here, right now") game from competitive category "*Programming*". <sub>03.06 2023</sub>

## Game description: 
"MathTricks" is played by two players on a board with dimensions (NxM) divided into cells (the players decide the size of the playing field at the beginning of the game).

Each player has one pull, which they move around the playing field on each turn - one cell in any direction (horizontal, vertical, diagonal). 
Each cell on the board represents one of the following mathematical operations:
- *Addition by a number* (e.g. +5) - The number from the cell is added to the player's total;
- *Difference by a number* (e.g. -11) - subtract from the player's total accumulated so far the number from the cell;
- *Multiplication by a number* (e.g. x2) - the player's total accumulated so far is multiplied by the number in the cell;
- *Division by a number* (e.g. /2) - the player's total accumulated so far is divided by the number in the cell.
- *0 (zero) is a valid number in the game* - it can be placed on the board for both for both addition and multiplication (effectively zeroing the player's cumulative total to to date),

### Mandatory conditions for the game board:
- The game board is at least 4x4 squares;
- The game board can be rectangular;
- The game board is different for each game that is started;
- Both players start the game from positions 0,0 and N,M, with values 0;

### Move restrictions:
- A player may not move his checker outside the playing area;
- A player cannot move his checker to a non-adjacent cell;
- A player may not place his checker on a cell through which a checker has already passed (the
any player).

### Winner:
- The player who at the end of the game has collected a greater total than his
opponent.
! The rule applies even if the player with the higher total is "closed" by
his opponent and thus end the game.

### End of Game:
- The game ends when a player does not have a valid move to move his pool at
their turn.

### Gameplay:
- The initial input to the program is two positive integers, determining the amount of
the size of the game board (NxM);
- On each turn, your program expects to receive the coordinates of a cell on which the player
moves its pool;
Moves alternate until a player is left without a new cell to step on
