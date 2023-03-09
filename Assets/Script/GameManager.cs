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
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (x == 0 || x == 9 ||
                    y == 0 || y == 9)
                {
                    _board[x, y] = Consts.WALL;
                }
            }
        }
        _board[4, 4] = Consts.WHITE;
        _board[5, 5] = Consts.WHITE;
        _board[4, 5] = Consts.BLACK;
        _board[5, 4] = Consts.BLACK;

        _turnCount = 0;
        _currentColor = Consts.BLACK;

        _reversi.Awake(_board, _turnCount, _currentColor);
        _checking.Awake(_board, _currentColor, _turnCount, _movablePos, _movableDir);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
