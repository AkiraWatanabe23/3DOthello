using Constants;

[System.Serializable]
public class TurnOverCheck
{
    //turn over ... �Ђ�����Ԃ�

    /// <summary> �΂�u����ʒu�̒T��
    ///           �S�����ɒT�����s���A�Ђ�����Ԃ��邩 </summary>
    public int MovableCheck(int[,] board, int x, int y, int color)
    {
        int moveDir = 0;

        //���ɐ΂��u����Ă�����s��
        if (board[x, y] != Consts.EMPTY)
            return 0;

        //�T�����s���遫
        //��
        if (board[x - 1, y] == -color)
        {
            int checkX = x - 2;
            int checkY = y;

            while (board[checkX, checkY] == -color)
                checkX--;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.LEFT;
        }

        //����
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

        //��
        if (board[x, y - 1] == -color)
        {
            int checkX = x;
            int checkY = y - 2;

            while (board[checkX, checkY] == -color)
                checkY--;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.UPPER;
        }

        //�E��
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

        //�E
        if (board[x + 1, y] == -color)
        {
            int checkX = x + 2;
            int checkY = y;

            while (board[checkX, checkY] == -color)
                checkX++;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.RIGHT;
        }

        //�E��
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

        //��
        if (board[x, y + 1] == -color)
        {
            int checkX = x;
            int checkY = y + 2;

            while (board[checkX, checkY] == -color)
                checkY++;

            if (board[checkX, checkY] == color)
                moveDir |= Consts.LOWER;
        }

        //����
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

    /// <summary> �΂̔z�u </summary>
    public bool SetStone(bool[,] pos, int x, int y)
    {
        if (x < 1 || Consts.BOARD_SIZE < x)
            return false;
        if (y < 1 || Consts.BOARD_SIZE > y)
            return false;
        if (pos[x, y] == false)
            return false;

        return true;
    }

    /// <summary> �΂�u���A�Ֆʂɔ��f���� </summary>
    public void FlipStone(int[,] board, int x, int y, int color)
    {
        board[x, y] = color;
    }
}
