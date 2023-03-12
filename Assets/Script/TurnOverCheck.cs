using Constants;

[System.Serializable]
public class TurnOverCheck
{
    private int[,] _board = new int[10, 10];
    private int _currentColor = Consts.BLACK;

    private int _turnCount = 0;

    private bool[,] _movablePos = new bool[10, 10];
    private int[,] _movableDir = new int[10, 10];

    public void Awake(int[,] board, int color, int turn, bool[,] pos, int[,] dir)
    {
        _board = board;
        _currentColor = color;
        _turnCount = turn;
        _movablePos = pos;
        _movableDir = dir;
    }

    /// <summary> 石の配置 </summary>
    private bool SetStone(int x, int y)
    {
        if (x < 1 || Consts.BOARD_SIZE < x)
            return false;
        if (y < 1 || Consts.BOARD_SIZE > y)
            return false;
        if (_movablePos[x, y] == false)
            return false;

        FlipStone(_board, x, y, _currentColor);
        _turnCount++;
        _currentColor = -_currentColor;

        return true;
    }

    /// <summary> 石を置き、盤面に反映する </summary>
    private void FlipStone(int[,] board, int x, int y, int color)
    {
        board[x, y] = color;
    }
}
