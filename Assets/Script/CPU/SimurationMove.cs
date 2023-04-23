using Constants;
using System;
using UnityEngine;

[System.Serializable]
public class SimurationMove : SearchBase
{
    private GameManager _manager = default;

    public void Start(GameManager manager)
    {
        _manager = manager;
    }

    public string Simuration(bool[,] movable)
    {
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
                    Debug.Log($"SimurateScore：{simurateScore}, {pos}");
                    if (score < simurateScore)
                    {
                        score = simurateScore;
                        x = i;
                        y = j;
                    }
                }
            }
        }

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }

    /// <summary> 実際に石を置き、盤面がどうなるかのシュミレーション
    ///            (盤面をつくり、点数をつけて返す) </summary>
    public override int SetSimurate(string pos)
    {
        int x = Array.IndexOf(Consts.INPUT_ALPHABET, pos[0]) + 1;
        int y = Array.IndexOf(Consts.INPUT_NUMBER, pos[1]) + 1;

        int[,] simurateBoard = (int[,])_manager.Board.Clone();

        //TODO：配置シュミレーション
        simurateBoard = FlipSimurate(simurateBoard, x, y);

        //点数化
        int checkScore = 0;
        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                //置いてある石が味方のものなら
                if (simurateBoard[i, j] == GameManager.CurrentColor)
                {
                    checkScore += Consts.EVALUATION_BOARD[i - 1, j - 1];
                }
                //相手のものなら
                else if (simurateBoard[i, j] == -GameManager.CurrentColor)
                {
                    checkScore -= Consts.EVALUATION_BOARD[i - 1, j - 1];
                }
            }
        }
        Array.Clear(simurateBoard, 0, simurateBoard.Length);

        return checkScore;
    }

    public override int[,] FlipSimurate(int[,] board, int x, int y)
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
                Debug.Log(board[checkX, y]);
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
