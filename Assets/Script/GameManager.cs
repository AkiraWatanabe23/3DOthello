using Constants;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputField _input = default;

    private Reversi _reversi = new();
    private TurnOverCheck _checking = new();
    private ObjectPool _pool = default;

    private int[,] _board = new int[10, 10];
    private int _turnCount = 0;
    private int _currentColor = Consts.BLACK;

    private bool[,] _movablePos = new bool[10, 10];
    private int[,] _movableDir = new int[10, 10];

    public int[,] Board { get => _board; set => _board = value; }
    public int TurnCount { get => _turnCount; set => _turnCount = value; }
    public int CurrentColor { get => _currentColor; set => _currentColor = value; }
    public bool[,] MovablePos { get => _movablePos; protected set => _movablePos = value; }

    private void Awake()
    {
        _board[4, 4] = Consts.WHITE;
        _board[5, 5] = Consts.WHITE;
        _board[4, 5] = Consts.BLACK;
        _board[5, 4] = Consts.BLACK;

        _turnCount = 0;
        _currentColor = Consts.BLACK;

        ResetMovables();
    }

    public void ResetMovables()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _movablePos[i, j] = false;
            }
        }

        for (int x = 1; x < Consts.BOARD_SIZE + 1; x++)
        {
            for (int y = 1; y < Consts.BOARD_SIZE + 1; y++)
            {
                int moveDir = _checking.MovableCheck(_board, x, y, _currentColor);
                _movableDir[x, y] = moveDir;

                if (moveDir != 0)
                {
                    _movablePos[x, y] = true;
                    Debug.Log("初期化");
                }
            }
        }
    }

    /// <summary> 盤面の表示更新 </summary>
    public void Display()
    {
        for (int x = 1; x < Consts.BOARD_SIZE + 1; x++)
        {
            for (int y = 1; y < Consts.BOARD_SIZE + 1; y++)
            {
                if (_board[x, y] == Consts.WHITE)
                {
                    _pool.SetWhite(new Vector3(x, 0.1f, y));
                }
                else if (_board[x, y] == Consts.BLACK)
                {
                    _pool.SetBlack(new Vector3(x, 0.1f, y));
                }
            }
        }
    }
}
