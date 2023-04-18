using Constants;

[System.Serializable]
/// <summary>
/// 一番多くの石をひっくり返せるマスを選択する
/// </summary>
public class PieceCountMove
{
    public string PieceCount(bool[,] movable)
    {
        //一番多くの石を獲れるマスを選ぶ
        int x = 0;
        int y = 0;
        
        return Consts.INPUT_ALPHABET[x].ToString() + Consts.INPUT_NUMBER[y];
    }
}
