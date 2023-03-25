using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private SelectType _type = SelectType.RANDOM;

    private GameManager _manager = default;
    private readonly RandomMove _random = new();
    private readonly PieceCountMove _count = new();

    public SelectType Type { get => _type; protected set => _type = value; }

    private void Start()
    {
        _manager = GetComponent<GameManager>();
    }

    public string TypeCheck()
    {
        string input = "";

        switch (_type)
        {
            case SelectType.RANDOM:
                input = _random.Random(_manager.MovablePos);
                break;
            case SelectType.PIECE_COUNT:
                input = _count.PieceCount(_manager.MovablePos);
                break;
        }
        return input;
    }
}

public enum SelectType
{
    /// <summary> 石を置けるマスからランダムに選ぶ </summary>
    RANDOM,
    /// <summary> 一番多くの石を獲れるマスを選ぶ </summary>
    PIECE_COUNT,
}
