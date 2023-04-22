using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Tooltip("敵の動き方")]
    [SerializeField] private SelectType _moveType = SelectType.RANDOM;

    private GameManager _manager = default;
    private readonly RandomMove _random = new();
    private readonly PieceCountMove _count = new();
    private readonly EvaluationMove _evaluation = new();
    private readonly SimurationMove _simuration = new();
    private readonly AheadSimurateMove _ahead = new();

    private void Start()
    {
        _manager = GetComponent<GameManager>();

        switch (_moveType)
        {
            case SelectType.PIECE_COUNT:
                _count.Start(_manager);
                break;
            case SelectType.SIMURATE:
                _simuration.Start(_manager);
                break;
            case SelectType.AHEAD:
                _ahead.Start(_manager);
                break;
        }
    }

    public string TypeCheck()
    {
        string input = "";

        //相手の行動選択(今後増える予定有)
        switch (_moveType)
        {
            case SelectType.RANDOM:
                input = _random.Random(_manager.MovablePos);
                break;
            case SelectType.PIECE_COUNT:
                input = _count.PieceCount(_manager.MovablePos);
                break;
            case SelectType.EVALUATION_FUNC:
                input = _evaluation.Evaluation(_manager.MovablePos);
                break;
            case SelectType.SIMURATE:
                input = _simuration.Simuration(_manager.MovablePos);
                break;
            case SelectType.AHEAD:
                input = _ahead.AheadSimurate(_manager.MovablePos);
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
    /// <summary> 評価関数を用いて、マスを選ぶ </summary>
    EVALUATION_FUNC,
    /// <summary> 配置シュミレーションを行い、評価関数に基づいて選ぶ </summary>
    SIMURATE,
    /// <summary> 指定した手数分だけ先読みして指す </summary>
    AHEAD,
}
