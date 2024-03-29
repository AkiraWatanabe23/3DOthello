﻿using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private TurnOverCheck _checking = new();
    private ObjectPool _pool = default;
    private UIManager _uiManager = default;

    //実際の盤面等を示す値
    private int[,] _board = new int[10, 10];
    private int _turnCount = 0;

    //判定用の配列
    private bool[,] _movablePos = new bool[10, 10];
    private int[,] _movableDir = new int[10, 10];

    public static int CurrentColor = Consts.BLACK;

    public TurnOverCheck Checking => _checking;
    public int[,] Board { get => _board; set => _board = value; }
    public int TurnCount { get => _turnCount; set => _turnCount = value; }
    public bool[,] MovablePos { get => _movablePos; protected set => _movablePos = value; }
    public int[,] MovableDir { get => _movableDir; protected set => _movableDir = value; }

    private void Awake()
    {
        _board[4, 4] = Consts.WHITE;
        _board[5, 5] = Consts.WHITE;
        _board[4, 5] = Consts.BLACK;
        _board[5, 4] = Consts.BLACK;

        _turnCount = 0;

        ResetMovables();
        _pool = GetComponent<ObjectPool>();

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                _pool.SetBoard(new Vector3(i, 0, j));

                if (_board[i, j] == Consts.WHITE)
                    _pool.SetWhite(new Vector3(i, 0.1f, 9 - j));
                else if (_board[i, j] == Consts.BLACK)
                    _pool.SetBlack(new Vector3(i, 0.1f, 9 - j));
            }
        }

        _uiManager = GetComponent<UIManager>();
        StoneCount();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //現在のシーンを再読み込み
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary> 置けるマスの探索 </summary>
    public void ResetMovables()
    {
        //判定用の配列をリセット
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _movablePos[i, j] = false;
            }
        }

        //再探索
        for (int x = 1; x < Consts.BOARD_SIZE + 1; x++)
        {
            for (int y = 1; y < Consts.BOARD_SIZE + 1; y++)
            {
                _movableDir[x, y] = _checking.MovableCheck(_board, x, y, CurrentColor);

                if (_movableDir[x, y] != 0)
                {
                    _movablePos[x, y] = true;
                }
            }
        }
    }

    /// <summary> 盤面の表示更新 </summary>
    public void Display()
    {
        for (int x = 1; x < Consts.BOARD_SIZE + 1; x++)
        {
            for (int y = 1; y < Consts.BOARD_SIZE + 1; y++)
            {
                if (_board[x, y] == Consts.WHITE)
                {
                    var piece = Consts.FindWithVector(new Vector3(x, 0.1f, 9 - y));
                    if (piece != null)
                        piece.SetActive(false);

                    _pool.SetWhite(new Vector3(x, 0.1f, 9 - y));
                }
                else if (_board[x, y] == Consts.BLACK)
                {
                    var piece = Consts.FindWithVector(new Vector3(x, 0.1f, 9 - y));
                    if (piece != null)
                        piece.SetActive(false);

                    _pool.SetBlack(new Vector3(x, 0.1f, 9 - y));
                }
            }
        }
    }

    /// <summary> パスをするかの判定 </summary>
    public bool Skip()
    {
        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                //置くことができるマスがあればパスしない
                if (_movablePos[i, j])
                    return false;
            }
        }

        if (GameFinish())
        {
            WinningCheck();
            return false;
        }

        CurrentColor *= -1;
        ResetMovables();

        return true;
    }

    /// <summary> ゲームが終了しているかの判定 </summary>
    public bool GameFinish()
    {
        if (_turnCount >= Consts.MAX_TURNS)
        {
            Debug.Log("マスが埋まった");
            return true;
        }

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                //まだ打てる手があれば続行する
                if (_movablePos[i, j])
                {
                    Debug.Log("まだ打てる");
                    return false;
                }

                if (_checking.MovableCheck(_board, i, j, -CurrentColor) != 0)
                {
                    Debug.Log("まだ打てる");
                    return false;
                }
            }
        }

        Debug.Log("何もできない");
        return true;
    }

    public void WinningCheck()
    {
        int blackCount = 0;
        int whiteCount = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (_board[i, j] == Consts.BLACK)
                    blackCount++;
                else if (_board[i, j] == Consts.WHITE)
                    whiteCount++;
            }
        }

        int diff = blackCount - whiteCount;
        if (diff > 0)
        {
            Debug.Log("黒の勝ち");
        }
        else if (diff < 0)
        {
            Debug.Log("白の勝ち");
        }
        else
        {
            Debug.Log("引き分け");
        }
        Debug.Log($"黒：{blackCount}, 白：{whiteCount}");
        _uiManager.GameFinish();
    }

    public void StoneCount()
    {
        int blackCount = 0;
        int whiteCount = 0;

        for (int i = 1; i < Consts.BOARD_SIZE + 1; i++)
        {
            for (int j = 1; j < Consts.BOARD_SIZE + 1; j++)
            {
                if (_board[i, j] == Consts.BLACK)
                    blackCount++;
                else if (_board[i, j] == Consts.WHITE)
                    whiteCount++;
            }
        }

        _uiManager.BlackCount = blackCount;
        _uiManager.WhiteCount = whiteCount;
    }
}
