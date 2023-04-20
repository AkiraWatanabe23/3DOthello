using Constants;
using System;
using UnityEngine;

[System.Serializable]
public class SimurationMove
{
    private GameManager _manager = default;

    private int[,] _simurateBoard = new int[10, 10];
    /// <summary> 一時保存用の配列 </summary>
    private int[,] _savedBoard = new int[10, 10];
    /// <summary> 盤面の評価値を示した盤面 </summary>
    private int[,] _evaluationBoard = new int[8, 8]
    { { 9, 1, 5, 3, 3, 5, 1, 9 },
      { 1, 1, 3, 4, 4, 3, 1, 1 },
      { 5, 3, 4, 6, 6, 4, 3, 5 },
      { 3, 4, 6, 6, 6, 6, 4, 3 },
      { 3, 4, 6, 6, 6, 6, 4, 3 },
      { 5, 3, 4, 6, 6, 4, 3, 5 },
      { 1, 1, 3, 4, 4, 3, 1, 1 },
      { 9, 1, 5, 3, 3, 5, 1, 9 } };

    public void Start(GameManager manager)
    {
        _manager = manager;
        Array.Clear(_simurateBoard, 0, _simurateBoard.Length);
    }

    public string Simuration(bool[,] movable)
    {
        //行動の流れ↓
        //1, 置けるマスを取得
        //2, ループ
        //       各マスに置き、盤面を点数化
        //       最大値を取得
        //3, 最大値のマスを返す
        _savedBoard = _manager.Board;

        int x = 0;
        int y = 0;
        int score = int.MinValue;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (movable[i, j])
                {
                    string pos = Consts.INPUT_ALPHABET[i - 1].ToString() + Consts.INPUT_NUMBER[j - 1];
                    int simurateScore = SetSimurate(pos);
                    if (score < simurateScore)
                    {
                        score = simurateScore;
                        x = i;
                        y = j;
                    }
                }
            }
        }

        _manager.Board = _savedBoard;
        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }

    /// <summary> 実際に石を置き、盤面がどうなるかのシュミレーション
    ///            (盤面をつくり、点数をつけて返す) </summary>
    private int SetSimurate(string pos)
    {
        int x = Array.IndexOf(Consts.INPUT_ALPHABET, pos[0]) + 1;
        int y = Array.IndexOf(Consts.INPUT_NUMBER, pos[1]) + 1;

        //TODO：配置シュミレーション
        _simurateBoard = FlipSimurate(_manager.Board, x, y);

        //点数化
        int score = 0;
        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                //置いてある石が味方のものなら
                if (_simurateBoard[i, j] == GameManager.CurrentColor)
                {
                    score += _evaluationBoard[i - 1, j - 1];
                }
                //相手のものなら
                else if (_simurateBoard[i, j] == -GameManager.CurrentColor)
                {
                    score -= _evaluationBoard[i - 1, j - 1];
                }
            }
        }
        Array.Clear(_simurateBoard, 0, _simurateBoard.Length);
        return score;
    }

    private int[,] FlipSimurate(int[,] board, int x, int y)
    {
        int setDir = _manager.MovableDir[x, y];
        int turn = GameManager.CurrentColor;
        board[x, y] = turn;

        //ビット演算を行い、調べたい方向のフラグが立っているかを調べる
        //左
        if ((setDir & Consts.LEFT) == Consts.LEFT)
        {
            int checkX = x - 1;

            while (board[checkX, y] == -turn)
            {
                board[checkX, y] = turn;
                checkX--;
            }
        }

        //左上
        if ((setDir & Consts.UPPER_LEFT) == Consts.UPPER_LEFT)
        {
            int checkX = x - 1;
            int checkY = y - 1;

            while (board[checkX, checkY] == -turn)
            {
                board[checkX, checkY] = turn;
                checkX--;
                checkY--;
            }
        }

        //上
        if ((setDir & Consts.UPPER) == Consts.UPPER)
        {
            int checkY = y - 1;

            while (board[x, checkY] == -turn)
            {
                board[x, checkY] = turn;
                checkY--;
            }
        }

        //右上
        if ((setDir & Consts.UPPER_RIGHT) == Consts.UPPER_RIGHT)
        {
            int checkX = x + 1;
            int checkY = y - 1;

            while (board[checkX, checkY] == -turn)
            {
                board[checkX, checkY] = turn;
                checkX++;
                checkY--;
            }
        }

        //右
        if ((setDir & Consts.RIGHT) == Consts.RIGHT)
        {
            int checkX = x + 1;

            while (board[checkX, y] == -turn)
            {
                board[checkX, y] = turn;
                checkX++;
            }
        }

        //右下
        if ((setDir & Consts.LOWER_RIGHT) == Consts.LOWER_RIGHT)
        {
            int checkX = x + 1;
            int checkY = y + 1;

            while (board[checkX, checkY] == -turn)
            {
                board[checkX, checkY] = turn;
                checkX++;
                checkY++;
            }
        }

        //下
        if ((setDir & Consts.LOWER) == Consts.LOWER)
        {
            int checkY = y + 1;

            while (board[x, checkY] == -turn)
            {
                board[x, checkY] = turn;
                checkY++;
            }
        }

        //左下
        if ((setDir & Consts.LOWER_LEFT) == Consts.LOWER_LEFT)
        {
            int checkX = x - 1;
            int checkY = y + 1;

            while (board[checkX, checkY] == -turn)
            {
                board[checkX, checkY] = turn;
                checkX--;
                checkY++;
            }
        }

        return board;
    }
}
