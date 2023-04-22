using Constants;
using UnityEngine;

[System.Serializable]
public class AheadSimurateMove
{
    [Tooltip("何手先まで")]
    [SerializeField] private int _aheadCount = 0;

    public string AheadSimurate(bool[,] movable)
    {
        int x = 0;
        int y = 0;

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }
}
