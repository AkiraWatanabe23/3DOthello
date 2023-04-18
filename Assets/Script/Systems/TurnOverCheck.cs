using Constants;
using UnityEngine;

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
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkX--;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LEFT;
                PieceCountMove.FlipCount.Add(new int[3] {checkX, checkY, count});
            }
        }

        //����
        if (board[x - 1, y - 1] == -color)
        {
            int checkX = x - 2;
            int checkY = y - 2;
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkX--;
                checkY--;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.UPPER_LEFT;
                PieceCountMove.FlipCount.Add(new int[3] { checkX, checkY, count });
            }
        }

        //��
        if (board[x, y - 1] == -color)
        {
            int checkX = x;
            int checkY = y - 2;
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkY--;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.UPPER;
                PieceCountMove.FlipCount.Add(new int[3] { checkX, checkY, count });
            }
        }

        //�E��
        if (board[x + 1, y - 1] == -color)
        {
            int checkX = x + 2;
            int checkY = y - 2;
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkX++;
                checkY--;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.UPPER_RIGHT;
                PieceCountMove.FlipCount.Add(new int[3] { checkX, checkY, count });
            }
        }

        //�E
        if (board[x + 1, y] == -color)
        {
            int checkX = x + 2;
            int checkY = y;
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkX++;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.RIGHT;
                PieceCountMove.FlipCount.Add(new int[3] { checkX, checkY, count });
            }
        }

        //�E��
        if (board[x + 1, y + 1] == -color)
        {
            int checkX = x + 2;
            int checkY = y + 2;
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkX++;
                checkY++;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LOWER_RIGHT;
                PieceCountMove.FlipCount.Add(new int[3] { checkX, checkY, count });
            }
        }

        //��
        if (board[x, y + 1] == -color)
        {
            int checkX = x;
            int checkY = y + 2;
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkY++;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LOWER;
                PieceCountMove.FlipCount.Add(new int[3] { checkX, checkY, count });
            }
        }

        //����
        if (board[x - 1, y + 1] == -color)
        {
            int checkX = x - 2;
            int checkY = y + 2;
            //�Ђ�����Ԃ��΂̐�
            int count = 0;

            while (board[checkX, checkY] == -color)
            {
                checkX--;
                checkY++;

                count++;
            }

            if (board[checkX, checkY] == color)
            {
                moveDir |= Consts.LOWER_LEFT;
                PieceCountMove.FlipCount.Add(new int[3] { checkX, checkY, count });
            }
        }
        return moveDir;
    }

    /// <summary> �΂̔z�u </summary>
    public bool SetStone(bool[,] pos, int x, int y)
    {
        if (x < 1 || Consts.BOARD_SIZE < x)
            return false;
        if (y < 1 || Consts.BOARD_SIZE < y)
            return false;
        if (pos[x, y] == false)
        {
            Debug.Log("�I�������}�X�ɂ͒u���܂���");
            return false;
        }

        return true;
    }

    /// <summary> �΂�u���A�Ֆʂɔ��f���� </summary>
    public int[,] FlipStone(int[,] board, int movable, int x, int y, int color)
    {
        board[x, y] = color;

        int setDir = movable;

        //�r�b�g���Z���s���A���ׂ��������̃t���O�������Ă��邩�𒲂ׂ�
        //��
        if ((setDir & Consts.LEFT) == Consts.LEFT)
        {
            int checkX = x - 1;

            while (board[checkX, y] == -color)
            {
                board[checkX, y] = color;
                checkX--;
            }
        }

        //����
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

        //��
        if ((setDir & Consts.UPPER) == Consts.UPPER)
        {
            int checkY = y - 1;

            while (board[x, checkY] == -color)
            {
                board[x, checkY] = color;
                checkY--;
            }
        }

        //�E��
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

        //�E
        if ((setDir & Consts.RIGHT) == Consts.RIGHT)
        {
            int checkX = x + 1;

            while (board[checkX, y] == -color)
            {
                board[checkX, y] = color;
                checkX++;
            }
        }

        //�E��
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

        //��
        if ((setDir & Consts.LOWER) == Consts.LOWER)
        {
            int checkY = y + 1;

            while (board[x, checkY] == -color)
            {
                board[x, checkY] = color;
                checkY++;
            }
        }

        //����
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
