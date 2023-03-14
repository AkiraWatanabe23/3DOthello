using Constants;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Reversi
{
    /// <summary> 入力された手が正しいか判定 </summary>
    public bool InputCorrect(string input)
    {
        if (Consts.INPUT_ALPHABET.Contains(input[0]) &&
            Consts.INPUT_NUMBER.Contains(input[1]))
        {
            return true;
        }

        return false;
    }
}
