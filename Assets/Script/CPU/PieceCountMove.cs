using Constants;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 一番多くの石をひっくり返せるマスを選択する
/// </summary>
public class PieceCountMove
{
    private GameManager _manager = default;

    public void Start(GameManager manager)
    {
        _manager = manager;
    }

    public string PieceCount(bool[,] movable)
    {
        //一番多くの石を獲れるマスを選ぶ
        int x = 0;
        int y = 0;

        int stoneCount = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                var simurateBoard = (int[,])_manager.Board.Clone();
                if (movable[i, j])
                {
                    simurateBoard = FlipSimurate(simurateBoard, i, j);
                    int simurate = SimurateStoneCount(simurateBoard, GameManager.CurrentColor);
                    if (stoneCount <= simurate)
                    {
                        x = i;
                        y = j;
                        stoneCount = simurate;
                    }
                }
            }
        }

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
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

    private int SimurateStoneCount(int[,] board, int turn)
    {
        int count = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (board[i, j] == turn)
                {
                    count++;
                }
            }
        }
        return count;
    }
}
