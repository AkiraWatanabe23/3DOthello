using Constants;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Reversi _reversi = new();
    [SerializeField] private TurnOverCheck _checking = new();

    private int[,] _board = new int[10, 10];
    private int _turnCount = 0;
    private int _currentColor = Consts.BLACK;

    private bool[,] _movablePos = new bool[10, 10];
    private int[,] _movableDir = new int[10, 10];

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

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void ResetMovables()
    {
        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
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
                }
            }
        }
    }

    /// <summary> 盤面の表示更新 </summary>
    private void Display()
    {

    }
}
