using Constants;

[System.Serializable]
public class TestMove
{
    public string TestSearch(bool[,] movable)
    {
        int x = 0;
        int y = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (movable[i, j])
                {

                }
            }
        }

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }
}
