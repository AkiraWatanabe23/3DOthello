﻿using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private SelectType _type = SelectType.RANDOM;

    private GameManager _manager = default;
    private readonly RandomMove _random = new();
    private readonly PieceCountMove _count = new();
    private readonly EvaluationMove _evaluation = new();
    private readonly SimurationMove _simuration = new();

    public SelectType Type { get => _type; protected set => _type = value; }

    private void Start()
    {
        _manager = GetComponent<GameManager>();
        _simuration.Start(_manager);
    }

    public string TypeCheck()
    {
        string input = "";

        //相手の行動選択(今後増える予定有)
        switch (_type)
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
}
