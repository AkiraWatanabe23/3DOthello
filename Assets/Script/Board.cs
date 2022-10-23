using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];      //マスの情報を保存しておく
    bool[,] _boardSettable = new bool[8, 8]; //置けるマスはtrueを返すことで、配置の可否を判断する
    GameObject[,] _stones = new GameObject[8, 8];
    GameObject[,] _tiles = new GameObject[8, 8];
    int _beFraTurn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;
    [SerializeField] GameObject _settableTile;
    //マスからの移動差(探索に使う)
    int[] _checkSetX = new[] { -1, -1, 0, 1, 1, 1, 0, -1 };
    int[] _checkSetZ = new[] { 0, 1, 1, 1, 0, -1, -1, -1 };
    TurnOverCheck _check;
    public int[,] BoardState { get => _boardState; set => _boardState = value; }
    /// <summary> 1...白, 2...黒 </summary>
    public int Turn { get; set; }
    public GameObject[,] Stones { get => _stones; set => _stones = value; }
    public GameObject[,] Tiles { get => _tiles; set => _tiles = value; }
    public int[] CheckSetX { get => _checkSetX; set => _checkSetX = value; }
    public int[] CheckSetZ { get => _checkSetZ; set => _checkSetZ = value; }

    // Start is called before the first frame update
    void Start()
    {
        _check = GetComponent<TurnOverCheck>();
        //初期設定(中央に石を配置した状態まで)
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Tiles[j, i] = Instantiate(_settableTile, new Vector3(j, 0.1f, i), Quaternion.identity);
                if ((i == 3 && j == 4) || (i == 4 && j == 3)) //白石の初期配置
                {
                    BoardState[j, i] = (int)TileState.White;
                    Stones[j, i] = Instantiate(_white, new Vector3(j, 0.1f, i), Quaternion.identity);
                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4)) //黒石の初期配置
                {
                    BoardState[j, i] = (int)TileState.Black;
                    Stones[j, i] = Instantiate(_black, new Vector3(j, 0.1f, i), Quaternion.identity);
                }
                else
                    BoardState[j, i] = (int)TileState.None;
            }
        }
        Turn = 2; //オセロは初手黒かららしい
        _beFraTurn = 2;
        SettableDrawing();
    }

    // Update is called once per frame
    void Update()
    {
        //ターンが切り替わったタイミングで、石を置けるマスがあるかどうかを判定する(なかった場合、パスになる)
        //この部分の処理で置けるマスを明示的にする
        if (Turn != _beFraTurn)
        {
            //ここで配置可能マスの探索リセット
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _boardSettable[i, j] = false;
                    Tiles[i, j].GetComponent<MeshRenderer>().enabled = false;
                }
            }
            SettableDrawing();
            _beFraTurn = Turn;
        }

        //空いているマスに石を置く処理
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && 
                hit.collider.gameObject.CompareTag("Tile"))
            {
                int x = (int)hit.collider.gameObject.transform.position.x;
                int z = (int)hit.collider.gameObject.transform.position.z;

                if (_boardSettable[x, z] == true)
                    _check.SwitchCheck(x, z);
                else
                    Debug.Log("このマスには置けません");
            }
        }
    }

    /// <summary>
    /// 石を置くことが出来るマスの明示的な表示
    /// </summary>
    void SettableDrawing()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                SettableCheck(j, i);
                if (_boardSettable[j, i] == true)
                {
                    Tiles[j, i].GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// 空いているマスに石を置くことが出来るかの判定
    /// </summary>
    /// <param name="x">選んだマスのx座標</param>
    /// <param name="z">選んだマスのz座標</param>
    void SettableCheck(int x, int z)
    {
        for (int n = 0; n < 8; n++)
        {
            //ひっくり返せる石がいくつあったか
            int count = 0;
            //探索を始めるマス
            int startX = x;
            int startZ = z;

            x = startX;
            z = startZ;

            if (BoardState[startX, startZ] == (int)TileState.None) //置けるマスは石を置いていないマス
            {
                x += CheckSetX[n];
                z += CheckSetZ[n];
            }
            else //石があるマスは探索外
                break;

            //白ターン
            if (Turn == 1)
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //探索するマスの進行方向が盤面の範囲外なら探索しない
                    break;

                while (BoardState[x, z] == (int)TileState.Black) //探索先にひっくり返せる石がある間実行される
                {
                    x += CheckSetX[n];
                    z += CheckSetZ[n];
                    count++;
                }

                if (BoardState[x, z] == (int)TileState.White && count != 0) //石が挟まれているか
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
            //黒ターン
            else
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8))
                    break;

                while (BoardState[x, z] == (int)TileState.White)
                {
                    x += CheckSetX[n];
                    z += CheckSetZ[n];
                    count++;
                }

                if (BoardState[x, z] == (int)TileState.Black && count != 0) //石が挟まれているか
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
        }
    }

    public enum TileState
    {
        None = 0,
        White,
        Black,
    }
}
