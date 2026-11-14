
namespace ConnectFour;

public class GameState
{
    private const int Rows = 6;
    private const int Columns = 7;

    private readonly int[,] board = new int[Rows, Columns];

    public int PlayerTurn { get; private set; } = 1;

    public int CurrentTurn { get; private set; } = 0;

    private bool gameOver = false;

    public enum WinState
    {
        No_Winner,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    public int GetCell(int row, int col)
    {
        return board[row, col];
    }

    public bool IsColumnFull(int col)
    {
        return board[0, col] != 0;
    }

    public void ResetBoard()
    {
        Array.Clear(board, 0, board.Length);

        PlayerTurn = 1;
        CurrentTurn = 0;
        gameOver = false;
    }

    public int PlayPiece(byte col)
    {
        if (gameOver)
        {
            throw new ArgumentException("The game is over. Reset to play again.");
        }

        if (col >= Columns)
        {
            throw new ArgumentException("Invalid column.");
        }

        int row = Rows - 1;

        while (row >= 0 && board[row, col] != 0)
        {
            row--;
        }

        if (row < 0)
        {
            throw new ArgumentException("This column is full. Choose another column.");
        }

        board[row, col] = PlayerTurn;

        CurrentTurn++;

        return Rows - row;
    }

    public WinState CheckForWin()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                int player = board[row, col];

                if (player == 0)
                {
                    continue;
                }

                if (
                    CheckDirection(row, col, 0, 1, player) ||
                    CheckDirection(row, col, 1, 0, player) ||
                    CheckDirection(row, col, 1, 1, player) ||
                    CheckDirection(row, col, 1, -1, player)
                )
                {
                    gameOver = true;

                    return player == 1
                        ? WinState.Player1_Wins
                        : WinState.Player2_Wins;
                }
            }
        }

        if (CurrentTurn == Rows * Columns)
        {
            gameOver = true;
            return WinState.Tie;
        }

        PlayerTurn = PlayerTurn == 1 ? 2 : 1;

        return WinState.No_Winner;
    }

    private bool CheckDirection(
        int row,
        int col,
        int rowDirection,
        int colDirection,
        int player)
    {
        for (int i = 0; i < 4; i++)
        {
            int newRow = row + i * rowDirection;
            int newCol = col + i * colDirection;

            if (
                newRow < 0 || newRow >= Rows ||
                newCol < 0 || newCol >= Columns ||
                board[newRow, newCol] != player
            )
            {
                return false;
            }
        }

        return true;
    }
}
