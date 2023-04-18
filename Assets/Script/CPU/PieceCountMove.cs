using Constants;
using System.Collections.Generic;

[System.Serializable]
/// <summary>
/// 一番多くの石をひっくり返せるマスを選択する
/// </summary>
public class PieceCountMove
{
    /// <summary> いくつの石がひっくり返せるか
    ///           0...x, 1...y, 2...count </summary>
    public static List<int[]> FlipCount = new();

    public void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            FlipCount.Add(new int[3] {0, 0, 0});
        }
    }

    public string PieceCount(bool[,] movable)
    {
        //一番多くの石を獲れるマスを選ぶ
        int x = 0;
        int y = 0;

        int maxCount = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (movable[i, j] && maxCount < FlipCount[i][2])
                {
                    x = FlipCount[i][0];
                    y = FlipCount[i][1];
                    maxCount = FlipCount[i][2];
                }
            }
        }

        return Consts.INPUT_ALPHABET[x].ToString() + Consts.INPUT_NUMBER[y];
    }
}
