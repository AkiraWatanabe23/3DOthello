using Constants;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private SelectType _type = SelectType.RANDOM;

    private GameManager _manager = default;

    public SelectType Type { get => _type; protected set => _type = value; }

    private void Start()
    {
        _manager = GetComponent<GameManager>();
    }

    public string TypeCheck()
    {
        string input = "";
        //if (_manager.Skip())
        //    return;

        switch (_type)
        {
            case SelectType.RANDOM:
                input = Random();
                break;
            case SelectType.PIECE_COUNT:
                PieceCount();
                break;
        }
        return input;
    }

    private string Random()
    {
        //石を置けるマスからランダムに選ぶ
        List<int[]> grids = new();
        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (_manager.MovablePos[i, j])
                    grids.Add(new int[] { i, j });
            }
        }
        int randomIndex = UnityEngine.Random.Range(0, grids.Count);
        var x = grids[randomIndex][0];
        var y = grids[randomIndex][1];

        return Consts.INPUT_ALPHABET[x].ToString() + Consts.INPUT_NUMBER[y];
    }

    private void PieceCount()
    {
        //一番多くの石を獲れるマスを選ぶ
    }
}

public enum SelectType
{
    /// <summary> 石を置けるマスからランダムに選ぶ </summary>
    RANDOM,
    /// <summary> 一番多くの石を獲れるマスを選ぶ </summary>
    PIECE_COUNT,
}
