using Constants;
using UnityEngine;

[System.Serializable]
public class Reversi
{
    /// <summary> 石を置ける位置の探索
    ///           全方向に探索を行い、ひっくり返せるか </summary>
    private int MovableCheck(int[,] board, int x, int y, int color)
    {
        int moveDir = 0;

        if (board[x, y] != Consts.EMPTY)
            return 0;

        //左
        if (board[x - 1, y] == -color)
        {
            int checkX = x - 2;
            int checkY = y;

            while (board[checkX, checkY] == -color)
                checkX--;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.LEFT;
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
                moveDir |= Consts.UPPER_LEFT;
        }

        //上
        if (board[x, y - 1] == -color)
        {
            int checkX = x;
            int checkY = y - 2;

            while (board[checkX, checkY] == -color)
                checkY--;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.UPPER;
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
                moveDir |= Consts.UPPER_RIGHT;
        }

        //右
        if (board[x + 1, y] == -color)
        {
            int checkX = x + 2;
            int checkY = y;

            while (board[checkX, checkY] == -color)
                checkX++;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.RIGHT;
        }

        //右下
        if (board[x + 1, y + 1] == -color)
        {
            int checkX = x + 2;
            int checkY = y + 2;

            while (board[checkX, checkY] == -color)
                checkX++;
                checkY++;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.LOWER_RIGHT;
        }

        //下
        if (board[x, y + 1] == -color)
        {
            int checkX = x;
            int checkY = y + 2;

            while (board[checkX, checkY] == -color)
                checkY++;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.LOWER;
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
                moveDir |= Consts.LOWER_LEFT;
        }
        return moveDir;
    }
}
