using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    Player _player;
    Computer _computer;
    AI _ai;
    private IPlayer _turn;
    int _gameOver;
    String[,] _playground = new String[3,3];
    
    [Header("Game Logic")]
    public int selectCounter;
    public string difficulty;

    [Header("UI")]
    public Text statusText;
    
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs
        difficulty = PlayerPrefs.GetString("difficulty", "Hard");
        
        //References
        _player = new Player();
        _computer = new Computer();
        _ai = new MiniMax(difficulty, this);
        
        //Randomly select start player
        _turn = (((int) Random.Range(0f, 2f)) == 0 ? _player : (IPlayer) _computer);

        //Initalize
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                _playground[x, y] = "";
            }
        }
        statusText.text = _turn.Name + "...";
        _gameOver = 0;
        selectCounter = 0;
    }

    void Update()
    {
        //AI Move
        if (_turn.Name.Equals(_computer.Name) && selectCounter < 9 && _gameOver == 0)
        {
            int buttonId = _ai.Play(_playground);
            selectButtonById(buttonId);
        }
    }

    public void selectButton(Button button)
    {
        if (_turn.GetType() == _player.GetType())
        {
            int buttonId = Int32.Parse(button.name);
            int x = buttonId / 3;
            int y = buttonId % 3;
        
            @select(button, x, y);
        }
    }

    private void selectButtonById(int id)
    {
        if (id >= 9 || id < 0)
        {
            Debug.Log("Button Identifier wrong: " + id);
        }
        else
        {
            Button button = GameObject.Find(id.ToString()).GetComponent<Button>();
            int x = id / 3;
            int y = id % 3;

            @select(button, x, y);
        }
    }

    private void select(Button button, int x, int y)
    {
        if (_playground[x, y] == "" && _gameOver == 0)
        {
            //Create UI selection
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = _turn.Sign;
            Color newColor;
            ColorUtility.TryParseHtmlString(_turn.HexColor, out newColor);
            buttonText.color = newColor;
            
            //Game Logic
            selectCounter++;
            _playground[x, y] = buttonText.text;

            _gameOver = checkForWin(_playground);

            if (_gameOver == 0 && selectCounter < 9)
            {
                changePlayer(ref _turn);
            }
            else if (_gameOver == 0 && selectCounter >= 9)
            {
                statusText.text = "Draw";
            }
            else
            {
                statusText.text = _gameOver == -1 ? _player.Status : _computer.Status;
            }
            
        }
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void changePlayer(ref IPlayer _turn)
    {
        if (_turn.Name.Equals(_player.Name))
        {
            _turn = _computer;
        }
        else
        {
            _turn = _player;
        }

        statusText.text = _turn.Name + "...";
    }

    /* Checks for Win
     *
     * Returns int
     *
     * -1 = Player
     *  0 = None
     *  1 = Computer
     */
    public int checkForWin(string[,] board)
    {
        for(int i = 0; i < 3; i++)
        {
            //Horizontal
            if (board[0, i] == board[1, i] && board[0, i] == board[2, i])
            {
                if (board[0, i] == _player.Sign)
                {
                    return -1;
                }
                if (board[0, i] == _computer.Sign)
                {
                    return 1;
                }
            }
            //Vertical
            if (board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2])
            {
                if (board[i, 0] == _player.Sign)
                {
                    return -1;
                }
                if (board[i, 0] == _computer.Sign)
                {
                    return 1;
                }
            }
        }
        //Diagonal
        if (board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2])
        {
            if (board[0, 0] == _player.Sign)
            {
                return -1;
            }
            if(board[0, 0] == _computer.Sign)
            {
                return 1;
            }
        }
        if (board[2, 0] == board[1, 1] && board[2, 0] == board[0, 2])
        {
            if (board[2, 0] == _player.Sign)
            {
                return -1;
            }
            if(board[2, 0] == _computer.Sign)
            {
                return 1;
            }
        }

        return 0;
    }

    public int getSelectCounter()
    {
        return selectCounter;
    }

    public IPlayer getTurn()
    {
        return _turn;
    }
}
