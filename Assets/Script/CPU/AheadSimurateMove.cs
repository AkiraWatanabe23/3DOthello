using Constants;
using UnityEngine;

[System.Serializable]
public class AheadSimurateMove : SearchBase
{
    [Tooltip("何手先まで")]
    [SerializeField] private int _aheadCount = 1;

    private GameManager _manager = default;
    private int _turn = 0;

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
        //置けるマスを列挙(bool[,])

        //相手ターンなら
        //4,シュミレーション
        //5,点数化( <<最小値>> を取得)...マスを保存しておく
        //6,count--;

        int x = 0;
        int y = 0;

        //探索開始時の盤面
        int[,] simurateBoard = (int[,])_manager.Board.Clone();
        _turn = GameManager.CurrentColor;

        while (_aheadCount > 0)
        {
            //探索が終わるまでループ
            if (_turn == Consts.WHITE)
            {
                int maxScore = ScoringMaximize(simurateBoard);
            }
            else if (_turn == Consts.BLACK)
            {
                int minScore = ScoringMinimize(simurateBoard);
            }

            _turn *= -1;
            _aheadCount--;
        }

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }

    public override int[,] FlipSimurate(int[,] board, int x, int y)
    {
        return board;
    }

    /// <summary> シュミレーションした盤面の最大値を返す
    ///            (自分のターンのシュミレーションに使う) </summary>
    private int ScoringMaximize(int[,] board)
    {
        int maxScore = int.MinValue;
        int score = Scoring(board);

        return maxScore;
    }

    /// <summary> シュミレーションした盤面の最小値を返す
    ///            (相手のターンのシュミレーションに使う) </summary>
    private int ScoringMinimize(int[,] board)
    {
        int minScore = int.MaxValue;
        int score = Scoring(board);

        return minScore;
    }

    /// <summary> 盤面に点数をつける </summary>
    private int Scoring(int[,] board)
    {
        int score = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (board[i, j] == GameManager.CurrentColor)
                {
                    score += board[i, j];
                }
                else if (board[i, j] == -GameManager.CurrentColor)
                {
                    score -= board[i, j];
                }
            }
        }

        return score;
    }
}
