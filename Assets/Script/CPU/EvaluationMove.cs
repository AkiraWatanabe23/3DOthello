using Constants;

[System.Serializable]
public class EvaluationMove
{
    /// <summary> 盤面の評価値を示した盤面 </summary>
    private int[,] _evaluationBoard = new int[8, 8]
    { {  30, -12,  0, -1, -1,  0, -12,  30 },
      { -12, -15, -3, -3, -3, -3, -15, -12 },
      {   0,  -3,  0, -1, -1,  0,  -3,   0 },
      {  -1,  -3, -1, -1, -1, -1,  -3,  -1 },
      {  -1,  -3, -1, -1, -1, -1,  -3,  -1 },
      {   0,  -3,  0, -1, -1,  0,  -3,   0 },
      { -12, -15, -3, -3, -3, -3, -15, -12 },
      {  30, -12,  0, -1, -1,  0, -12,  30 } };

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
                if (movable[i, j] && score <= _evaluationBoard[i - 1, j - 1])
                {
                    score = _evaluationBoard[i - 1, j - 1];
                    x = i;
                    y = j;
                }
            }
        }

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }
}
