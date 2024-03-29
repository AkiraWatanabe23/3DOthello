using Constants;
using UnityEngine;

[System.Serializable]
public class TurnOverCheck
{
    //turn over ... ひっくり返す

    /// <summary> 石を置ける位置の探索
    ///           全方向に探索を行い、ひっくり返せるか </summary>
    public int MovableCheck(int[,] board, int x, int y, int color)
    {
        int moveDir = 0;

        //既に石が置かれていたら不可
        if (board[x, y] != Consts.EMPTY)
            return 0;

        //探索を行える↓
        //左
        if (board[x - 1, y] == -color)
        {
            int checkX = x - 2;
            int checkY = y;

            while (board[checkX, checkY] == -color)
            {
                checkX--;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LEFT;
            }
        }

        //左上
        if (board[x - 1, y - 1] == -color)
        {
            int checkX = x - 2;
            int checkY = y - 2;

            while (board[checkX, checkY] == -color)
            {
                checkX--;
                checkY--;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.UPPER_LEFT;
            }
        }

        //上
        if (board[x, y - 1] == -color)
        {
            int checkX = x;
            int checkY = y - 2;

            while (board[checkX, checkY] == -color)
            {
                checkY--;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.UPPER;
            }
        }

        //右上
        if (board[x + 1, y - 1] == -color)
        {
            int checkX = x + 2;
            int checkY = y - 2;

            while (board[checkX, checkY] == -color)
            {
                checkX++;
                checkY--;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.UPPER_RIGHT;
            }
        }

        //右
        if (board[x + 1, y] == -color)
        {
            int checkX = x + 2;
            int checkY = y;

            while (board[checkX, checkY] == -color)
            {
                checkX++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.RIGHT;
            }
        }

        //右下
        if (board[x + 1, y + 1] == -color)
        {
            int checkX = x + 2;
            int checkY = y + 2;

            while (board[checkX, checkY] == -color)
            {
                checkX++;
                checkY++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LOWER_RIGHT;
            }
        }

        //下
        if (board[x, y + 1] == -color)
        {
            int checkX = x;
            int checkY = y + 2;

            while (board[checkX, checkY] == -color)
            {
                checkY++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LOWER;
            }
        }

        //左下
        if (board[x - 1, y + 1] == -color)
        {
            int checkX = x - 2;
            int checkY = y + 2;

            while (board[checkX, checkY] == -color)
            {
                checkX--;
                checkY++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LOWER_LEFT;
            }
        }
        return moveDir;
    }

    /// <summary> 石の配置 </summary>
    public bool SetStone(bool[,] pos, int x, int y)
    {
        if (x < 1 || Consts.BOARD_SIZE < x)
            return false;
        if (y < 1 || Consts.BOARD_SIZE < y)
            return false;
        if (pos[x, y] == false)
        {
            Debug.Log("選択したマスには置けません");
            return false;
        }

        return true;
    }

    /// <summary> 石を置き、盤面に反映する </summary>
    public int[,] FlipStone(int[,] board, int movable, int x, int y, int color)
    {
        board[x, y] = color;

        int setDir = movable;

        //ビット演算を行い、調べたい方向のフラグが立っているかを調べる
        //左
        if ((setDir & Consts.LEFT) == Consts.LEFT)
        {
            int checkX = x - 1;

            while (board[checkX, y] == -color)
            {
                board[checkX, y] = color;
                checkX--;
            }
        }

        //左上
        if ((setDir & Consts.UPPER_LEFT) == Consts.UPPER_LEFT)
        {
            int checkX = x - 1;
            int checkY = y - 1;

            while (board[checkX, checkY] == -color)
            {
                board[checkX, checkY] = color;
                checkX--;
                checkY--;
            }
        }

        //上
        if ((setDir & Consts.UPPER) == Consts.UPPER)
        {
            int checkY = y - 1;

            while (board[x, checkY] == -color)
            {
                board[x, checkY] = color;
                checkY--;
            }
        }

        //右上
        if ((setDir & Consts.UPPER_RIGHT) == Consts.UPPER_RIGHT)
        {
            int checkX = x + 1;
            int checkY = y - 1;

            while (board[checkX, checkY] == -color)
            {
                board[checkX, checkY] = color;
                checkX++;
                checkY--;
            }
        }

        //右
        if ((setDir & Consts.RIGHT) == Consts.RIGHT)
        {
            int checkX = x + 1;

            while (board[checkX, y] == -color)
            {
                board[checkX, y] = color;
                checkX++;
            }
        }

        //右下
        if ((setDir & Consts.LOWER_RIGHT) == Consts.LOWER_RIGHT)
        {
            int checkX = x + 1;
            int checkY = y + 1;

            while (board[checkX, checkY] == -color)
            {
                board[checkX, checkY] = color;
                checkX++;
                checkY++;
            }
        }

        //下
        if ((setDir & Consts.LOWER) == Consts.LOWER)
        {
            int checkY = y + 1;

            while (board[x, checkY] == -color)
            {
                board[x, checkY] = color;
                checkY++;
            }
        }

        //左下
        if ((setDir & Consts.LOWER_LEFT) == Consts.LOWER_LEFT)
        {
            int checkX = x - 1;
            int checkY = y + 1;

            while (board[checkX, checkY] == -color)
            {
                board[checkX, checkY] = color;
                checkX--;
                checkY++;
            }
        }
        return board;
    }
}
