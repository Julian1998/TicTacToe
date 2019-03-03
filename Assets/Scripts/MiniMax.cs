using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MiniMax : AI
{
    private string _difficulty;
    private GameController _gameController;
    private IPlayer _turn;
    private int _maxDepth;

    public MiniMax(string difficulty , GameController gameController)
    {
        _difficulty = difficulty;
        _gameController = gameController;
        _maxDepth = difficulty.Equals("Easy") ? 2 : 10;
    }
    
    public int Play(string[,] playground)
    {
        Node node = @miniMax(playground, 0, true, -1);
        Debug.Log("Value: " + node.value);
        return node.id;
    }
    
    private Node miniMax(string[,] board, int depth, bool maximizing, int id)
    {
        //Check for available moves
        int availableMoves = 9;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y] != "")
                {
                    availableMoves--;
                }
            }
        }
        if (depth == _maxDepth || _gameController.checkForWin(board) != 0 || availableMoves == 0)
        {
            return new Node(_gameController.checkForWin(board), id);
        }

        if (maximizing)
        {
            int value = -100;
            int newId = id;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[x, y] == "")
                    {
                        string[,] newBoard = board.Clone() as string[,];
                        newBoard[x, y] = "O";
                        int oldValue = value;
                        value = Mathf.Max(value, miniMax(newBoard, depth+1, false, 3*x+y).value);
                        if (oldValue != value)
                            newId = 3 * x + y;
                        if (value == 1)
                            break;
                    }
                }
                if (value == 1)
                    break;
            }
            return new Node(value, newId);
        }
        else
        {
            int value = 100;
            int newId = id;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[x, y] == "")
                    {
                        string[,] newBoard = board.Clone() as string[,];
                        newBoard[x, y] = "X";
                        int oldValue = value;
                        value = Mathf.Min(value, miniMax(newBoard, depth+1, true, 3*x+y).value);
                        if (oldValue != value)
                            newId = 3 * x + y;
                        if (value == -1)
                            break;
                    }
                }
                if (value == -1)
                    break;
            }
            return new Node(value, newId);
        }
    }

    private struct Node
    {
        public int value;
        public int id;

        public Node(int v, int i)
        {
            value = v;
            id = i;
        }
    }
}
