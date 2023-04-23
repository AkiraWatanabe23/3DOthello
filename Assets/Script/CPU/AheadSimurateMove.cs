using Constants;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AheadSimurateMove : SearchBase
{
    [Tooltip("何手先まで")]
    [SerializeField] private int _aheadCount = 1;

    private GameManager _manager = default;
    /// <summary> ターンを確認する </summary>
    private int _turn = 0;
    /// <summary> シュミレーションで取得した点数を保存する </summary>
    private List<int> _scoreList = new();

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

        int[] pos = new int[2];

        //探索開始時の盤面
        int[,] simurateBoard = (int[,])_manager.Board.Clone();
        _turn = GameManager.CurrentColor;

        while (_aheadCount > 0)
        {
            //探索が終わるまでループ
            if (_turn == Consts.WHITE)
            {
                pos = ScoringMaximize(simurateBoard);
            }
            else if (_turn == Consts.BLACK)
            {
                pos = ScoringMinimize(simurateBoard);
            }

            _turn *= -1;
            _aheadCount--;
        }
        int x = pos[0];
        int y = pos[1];

        return Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1];
    }

    public override int[,] FlipSimurate(int[,] board, int x, int y)
    {
        return board;
    }

    /// <summary> シュミレーションした最大値のマスを返す
    ///            (自分のターンのシュミレーションに使う) </summary>
    private int[] ScoringMaximize(int[,] board)
    {
        //探索→点数化(Listに追加)→最大値(最小値)を取得→マスを返す
        int x = 0;
        int y = 0;
        int score = Scoring(board);

        return new int[] { x, y };
    }

    /// <summary> シュミレーションした最小値のマスを返す
    ///            (相手のターンのシュミレーションに使う) </summary>
    private int[] ScoringMinimize(int[,] board)
    {
        int x = 0;
        int y = 0;
        int score = Scoring(board);

        return new int[] { x, y };
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
