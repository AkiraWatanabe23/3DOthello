using Constants;
using UnityEngine;

[System.Serializable]
public class Reversi
{
    private int[,] _board = new int[10, 10];
    private int _turnCount = 0;
    private int _currentColor = Consts.BLACK;

    private bool[,] _movablePos = new bool[10, 10];
    private int[,] _movableDir = new int[10, 10];

    public void Awake(int[,] board, int turn, int current)
    {
        _board = board;
        _turnCount = turn;
        _currentColor = current;
        ResetMovables();
    }

    /// <summary> 判定用Listの初期化 </summary>
    private void ResetMovables()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                _movablePos[x, y] = false;
            }
        }

        for (int x = 1; x <= Consts.BOARD_SIZE; x++)
        {
            for (int y = 1; y <= Consts.BOARD_SIZE; y++)
            {
                int moveDir = MovableCheck(x, y, _currentColor);
                _movableDir[x, y] = moveDir;

                if (moveDir != 0)
                    _movablePos[x, y] = true;
            }
        }
    }

    private int MovableCheck(int x, int y, int color)
    {
        int moveDir = 0;

        if (_board[x, y] != Consts.EMPTY)
            return 0;

        //左
        if (_board[x - 1, y] == -color)
        {
            int checkX = x - 2;
            int checkY = y;

            while (_board[checkX, checkY] == -color)
            {
                checkX--;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.LEFT;
        }

        //左上
        if (_board[x - 1, y - 1] == -color)
        {
            int checkX = x - 2;
            int checkY = y - 2;

            while (_board[checkX, checkY] == -color)
            {
                checkX--;
                checkY--;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.UPPER_LEFT;
        }

        //上
        if (_board[x, y - 1] == -color)
        {
            int checkX = x;
            int checkY = y - 2;

            while (_board[checkX, checkY] == -color)
            {
                checkY--;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.UPPER;
        }

        //右上
        if (_board[x + 1, y - 1] == -color)
        {
            int checkX = x + 2;
            int checkY = y - 2;

            while (_board[checkX, checkY] == -color)
            {
                checkX++;
                checkY--;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.UPPER_RIGHT;
        }

        //右
        if (_board[x + 1, y] == -color)
        {
            int checkX = x + 2;
            int checkY = y;

            while (_board[checkX, checkY] == -color)
            {
                checkX++;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.RIGHT;
        }

        //右下
        if (_board[x + 1, y + 1] == -color)
        {
            int checkX = x + 2;
            int checkY = y + 2;

            while (_board[checkX, checkY] == -color)
            {
                checkX++;
                checkY++;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.LOWER_RIGHT;
        }

        //下
        if (_board[x, y + 1] == -color)
        {
            int checkX = x;
            int checkY = y + 2;

            while (_board[checkX, checkY] == -color)
            {
                checkY++;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.LOWER;
        }

        //左下
        if (_board[x - 1, y + 1] == -color)
        {
            int checkX = x - 2;
            int checkY = y + 2;

            while (_board[checkX, checkY] == -color)
            {
                checkX--;
                checkY++;
            }

            if (_board[checkX, checkY] == color)
                moveDir |= Consts.LOWER_LEFT;
        }
        return moveDir;
    }
}
