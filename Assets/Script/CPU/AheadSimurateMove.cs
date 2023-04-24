using Constants;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AheadSimurateMove : SearchBase
{
    [Tooltip("何手先まで探索するか")]
    [SerializeField] private int _aheadCount = 1;

    private GameManager _manager = default;
    /// <summary> ターンを確認する </summary>
    private int _turn = 0;
    /// <summary> シュミレーションで取得した点数を保存する </summary>
    private List<int> _scoreList = new();

    /// <summary> 判定用に使う盤面 </summary>
    private int[,] _simurateBoard = new int[10, 10];
    /// <summary> どの方向にひっくり返るか保存 </summary>
    private int[,] _searchDir = new int[10, 10];
    /// <summary> 置けるマスを保存 </summary>
    private bool[,] _searchPos = new bool[10, 10];

    public void Start(GameManager manager)
    {
        _manager = manager;
    }

    public string AheadSimurate(bool[,] movable)
    {
        //0...x, 1...y
        int[] pos = new int[2];

        //探索開始時の情報を保存
        _simurateBoard = (int[,])_manager.Board.Clone();
        _searchDir = (int[,])_manager.MovableDir.Clone();
        _searchPos = (bool[,])movable.Clone();
        _turn = GameManager.CurrentColor;

        while (_aheadCount > 0)
        {
            //探索が終わるまでループ
            if (_turn == Consts.WHITE)
            {
                pos = ScoringMaximize(_simurateBoard);
            }
            else if (_turn == Consts.BLACK)
            {
                pos = ScoringMinimize(_simurateBoard);
            }

            _turn *= -1;
            _aheadCount--;
            SearchMovable();
        }
        int x = pos[0];
        int y = pos[1];

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }

    /// <summary> シュミレーションした最大値のマスを返す
    ///            (自分のターンのシュミレーションに使う) </summary>
    private int[] ScoringMaximize(int[,] board)
    {
        //探索→点数化(Listに追加)→最大値(最小値)を取得→マスを返す
        int x = 0;
        int y = 0;
        int score = Scoring(board);

        return new int[] { x, y };
    }

    /// <summary> シュミレーションした最小値のマスを返す
    ///            (相手のターンのシュミレーションに使う) </summary>
    private int[] ScoringMinimize(int[,] board)
    {
        int x = 0;
        int y = 0;
        int score = Scoring(board);

        return new int[] { x, y };
    }

    /// <summary> 盤面に点数をつける </summary>
    private int Scoring(int[,] board)
    {
        int score = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (board[i, j] == GameManager.CurrentColor)
                {
                    score += board[i, j];
                }
                else if (board[i, j] == -GameManager.CurrentColor)
                {
                    score -= board[i, j];
                }
            }
        }
        return score;
    }

    public override int[,] FlipSimurate(int[,] board, int x, int y)
    {
        int setDir = _searchDir[x, y];
        int searchTurn = _turn;
        board[x, y] = searchTurn;

        //ビット演算を行い、調べたい方向のフラグが立っているかを調べる
        //左
        if ((setDir & Consts.LEFT) == Consts.LEFT)
        {
            int checkX = x - 1;

            while (board[checkX, y] == -searchTurn)
            {
                Debug.Log(board[checkX, y]);
                board[checkX, y] = searchTurn;
                checkX--;
            }
        }

        //左上
        if ((setDir & Consts.UPPER_LEFT) == Consts.UPPER_LEFT)
        {
            int checkX = x - 1;
            int checkY = y - 1;

            while (board[checkX, checkY] == -searchTurn)
            {
                board[checkX, checkY] = searchTurn;
                checkX--;
                checkY--;
            }
        }

        //上
        if ((setDir & Consts.UPPER) == Consts.UPPER)
        {
            int checkY = y - 1;

            while (board[x, checkY] == -searchTurn)
            {
                board[x, checkY] = searchTurn;
                checkY--;
            }
        }

        //右上
        if ((setDir & Consts.UPPER_RIGHT) == Consts.UPPER_RIGHT)
        {
            int checkX = x + 1;
            int checkY = y - 1;

            while (board[checkX, checkY] == -searchTurn)
            {
                board[checkX, checkY] = searchTurn;
                checkX++;
                checkY--;
            }
        }

        //右
        if ((setDir & Consts.RIGHT) == Consts.RIGHT)
        {
            int checkX = x + 1;

            while (board[checkX, y] == -searchTurn)
            {
                board[checkX, y] = searchTurn;
                checkX++;
            }
        }

        //右下
        if ((setDir & Consts.LOWER_RIGHT) == Consts.LOWER_RIGHT)
        {
            int checkX = x + 1;
            int checkY = y + 1;

            while (board[checkX, checkY] == -searchTurn)
            {
                board[checkX, checkY] = searchTurn;
                checkX++;
                checkY++;
            }
        }

        //下
        if ((setDir & Consts.LOWER) == Consts.LOWER)
        {
            int checkY = y + 1;

            while (board[x, checkY] == -searchTurn)
            {
                board[x, checkY] = searchTurn;
                checkY++;
            }
        }

        //左下
        if ((setDir & Consts.LOWER_LEFT) == Consts.LOWER_LEFT)
        {
            int checkX = x - 1;
            int checkY = y + 1;

            while (board[checkX, checkY] == -searchTurn)
            {
                board[checkX, checkY] = searchTurn;
                checkX--;
                checkY++;
            }
        }
        return board;
    }

    private void SearchMovable()
    {
        //判定用の配列をリセット
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _searchPos[i, j] = false;
            }
        }

        //再探索
        for (int x = 1; x < Consts.BOARD_SIZE + 1; x++)
        {
            for (int y = 1; y < Consts.BOARD_SIZE + 1; y++)
            {
                _searchDir[x, y] = _manager.Checking.MovableCheck(_simurateBoard, x, y, _turn);

                if (_searchDir[x, y] != 0)
                {
                    _searchPos[x, y] = true;
                }
            }
        }
    }

    /// <summary> 点数とマスを保存する </summary>
    private struct GridScoring
    {
        public int Parent;
        public int Score;
        public int PosX;
        public int PosY;

        public GridScoring(int parent, int score, int x, int y)
        {
            Parent = parent;
            Score = score;
            PosX = x;
            PosY = y;
        }
    }
}
