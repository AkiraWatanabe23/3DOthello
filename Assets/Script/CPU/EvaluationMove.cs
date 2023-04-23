using Constants;

[System.Serializable]
public class EvaluationMove
{
    public string Evaluation(bool[,] movable)
    {
        int score = -100;
        int x = 0;
        int y = 0;

        //置けるマスの内、評価値の盤面の一番点数が高いマスを選択する
        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (movable[i, j] && score <= Consts.EVALUATION_BOARD[i - 1, j - 1])
                {
                    score = Consts.EVALUATION_BOARD[i - 1, j - 1];
                    x = i;
                    y = j;
                }
            }
        }

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }
}
