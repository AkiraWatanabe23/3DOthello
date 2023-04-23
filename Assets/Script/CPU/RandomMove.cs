using Constants;
using System.Collections.Generic;

[System.Serializable]
/// <summary>
/// ランダムにマスを選択する
/// </summary>
public class RandomMove
{
    public string Random(bool[,] movable)
    {
        //石を置けるマスからランダムに選ぶ
        List<int[]> grids = new();
        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (movable[i, j])
                    grids.Add(new int[] { i, j });
            }
        }
        int randomIndex = UnityEngine.Random.Range(0, grids.Count);
        var x = grids[randomIndex][0];
        var y = grids[randomIndex][1];

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }
}
