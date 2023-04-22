using Constants;
using UnityEngine;

[System.Serializable]
public class AheadSimurateMove : SearchBase
{
    [Tooltip("何手先まで")]
    [SerializeField] private int _aheadCount = 1;

    private GameManager _manager = default;

    public void Start(GameManager manager)
    {
        _manager = manager;
    }

    public string AheadSimurate(bool[,] movable)
    {
        //実行手順
        //自分のターンなら
        //1,シュミレーション
        //2,点数化( <<最大値>> を取得)...マスを保存しておく
        //3,count--;

        //探索を終えていなかったら
        //シュミレーションのターンを切り替える
        //相手ターンなら
        //4,シュミレーション
        //5,点数化( <<最小値>> を取得)...マスを保存しておく
        //6,count--;

        int x = 0;
        int y = 0;
        //探索開始時の盤面
        int[,] simurateBoard = (int[,])_manager.Board.Clone();

        while (_aheadCount > 0)
        {
            //探索が終わるまでループ
            _aheadCount--;
        }

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }

    public override int[,] FlipSimurate(int[,] board, int x, int y)
    {
        return board;
    }

    private int ScoringMaximize()
    {
        int maxScore = int.MinValue;

        return maxScore;
    }

    private int ScoringMinimize()
    {
        int minScore = int.MaxValue;

        return minScore;
    }
}
