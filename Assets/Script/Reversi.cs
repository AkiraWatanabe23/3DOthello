using Constants;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Reversi
{
    /// <summary> 入力された手が正しいか判定 </summary>
    public bool InputCorrect(Vector3 pos)
    {
        if (Consts.INPUT_NUMBER.Contains((char)pos.x))
        {
            return true;
        }

        return false;
    }
}
