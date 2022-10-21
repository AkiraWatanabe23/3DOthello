using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];      //マスの情報を保存しておく
    bool[,] _boardSettable = new bool[8, 8]; //置けるマスはtrueを返すことで、配置の可否を判断する
    GameObject[,] _stones = new GameObject[8, 8];
    GameObject[,] _tiles = new GameObject[8, 8];
    [Tooltip("1...白, 2...黒"), SerializeField] int _turn = 0;
    int _beFraTurn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;
    [SerializeField] GameObject _settableTile;
    //マスからの移動差(探索に使う)
    int[] _checkSetX = new[] { -1, -1, 0, 1, 1, 1, 0, -1 };
    int[] _checkSetZ = new[] { 0, 1, 1, 1, 0, -1, -1, -1 };
    List<SwitchColor> _switchable = new();
    public GameObject[,] Stones { get => _stones; set => _stones = value; }
    public GameObject[,] Tiles { get => _tiles; set => _tiles = value; }
    public int[] CheckSetX { get => _checkSetX; set => _checkSetX = value; }
    public int[] CheckSetZ { get => _checkSetZ; set => _checkSetZ = value; }
    public List<SwitchColor> Switchable { get => _switchable; set => _switchable = value; }

    // Start is called before the first frame update
    void Start()
    {
        //初期設定(中央に石を配置した状態まで)
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Tiles[i, j] = Instantiate(_settableTile, new Vector3(i, 0.1f, j), _settableTile.transform.rotation);
                if ((i == 3 && j == 4) || (i == 4 && j == 3)) //白石の初期配置
                {
                    _boardState[i, j] = (int)TileState.White;
                    Stones[i, j] = Instantiate(_white, new Vector3(i, 0.1f, j), _white.transform.rotation);
                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4)) //黒石の初期配置
                {
                    _boardState[i, j] = (int)TileState.Black;
                    Stones[i, j] = Instantiate(_black, new Vector3(i, 0.1f, j), _black.transform.rotation);
                }
                else
                    _boardState[i, j] = (int)TileState.None;
            }
        }
        _turn = 2; //オセロは初手黒かららしい
        _beFraTurn = 2;
        SettableDrawing();
    }

    // Update is called once per frame
    void Update()
    {
        //ターンが切り替わったタイミングで、石を置けるマスがあるかどうかを判定する(なかった場合、パスになる)
        //この部分の処理で置けるマスを明示的にする
        if (_turn != _beFraTurn)
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
            _beFraTurn = _turn;
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
                {
                    TurnOverCheck(x, z);
                    _turn = _turn == 1 ? 2 : 1; //ターンの切り替え(白ターンなら黒に、黒ターンなら白に)
                }
                else
                {
                    Debug.Log("このマスには置けません");
                }
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
                if (_boardSettable[i, j] == true)
                {
                    Tiles[i, j].GetComponent<MeshRenderer>().enabled = true;
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
        for (int i = 0; i < 8; i++)
        {
            //ひっくり返せる石がいくつあったか
            int count = 0;
            //探索を始めるマス
            int startX = x;
            int startZ = z;

            x = startX;
            z = startZ;

            if (_boardState[startX, startZ] == (int)TileState.None) //置けるマスは石を置いていないマス
            {
                x += CheckSetX[i];
                z += CheckSetZ[i];
            }
            else //石があるマスは探索外
                break;

            //白ターン
            if (_turn == 1)
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //探索するマスの進行方向が盤面の範囲外なら探索しない
                    break;

                while (_boardState[x, z] == (int)TileState.Black) //探索先にひっくり返せる石がある間実行される
                {
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                    count++;
                }

                if (_boardState[x, z] == (int)TileState.White && count != 0) //石が挟まれているか
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
            //黒ターン
            else
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8))
                    break;

                while (_boardState[x, z] == (int)TileState.White)
                {
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                    count++;
                }

                if (_boardState[x, z] == (int)TileState.Black && count != 0) //石が挟まれているか
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
        }
    }

    /// <summary>
    /// 選んだマスを基準に、どの石をひっくり返すかを調べる
    /// </summary>
    /// <param name="x">石を置くマスのx座標</param>
    /// <param name="z">石を置くマスのz座標</param>
    void TurnOverCheck(int x, int z)
    {
        Switchable.Clear();
        //石を置くマス
        int startX = x;
        int startZ = z;

        GameObject setting;
        if (_turn == 1)
        {
            _boardState[x, z] = (int)TileState.White;
            setting = _white;
        }
        else
        {
            _boardState[x, z] = (int)TileState.Black;
            setting = _black;
        }
        //選んだマスに石を置く
        Stones[x, z] = Instantiate(setting, new Vector3(x, 0.1f, z), setting.transform.rotation);
        //ひっくり返す
        for (int i = 0; i < 8; i++)
        {
            x = startX;
            z = startZ;

            if (_boardState[startX, startZ] == (int)TileState.None) //選んだマスは必ず石を置いていないマスである
            {
                x += CheckSetX[i];
                z += CheckSetZ[i];
            }

            //白ターン
            if (_turn == 1)
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //探索するマスの進行方向が盤面の範囲外なら探索しない
                    break;

                while (_boardState[x, z] == (int)TileState.Black) //探索先にひっくり返せる石がある間実行される
                {
                    Switchable.Add(new SwitchColor(x, z));
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                }

                if (_boardState[x, z] == (int)TileState.White) //石が挟まれているか
                {
                    TurnOver(setting);
                }
            }
            //黒ターン
            else
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //探索するマスの進行方向が盤面の範囲外なら探索しない
                    break;

                while (_boardState[x, z] == (int)TileState.White)
                {
                    Debug.Log(Switchable.Count);
                    Switchable.Add(new SwitchColor(x, z));
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                }

                if (_boardState[x, z] == (int)TileState.Black) //石が挟まれているか
                {
                    TurnOver(setting);
                }
            }
        }
    }

    void TurnOver(GameObject setting)
    {
        //ひっくり返す(っぽい)処理
        for (int i = 0; i < Switchable.Count; i++)
        {
            Debug.Log("aaa");
            SwitchColor switchPos = Switchable[i];
            Destroy(Stones[switchPos.switchX, switchPos.switchZ]);
            Stones[switchPos.switchX, switchPos.switchZ]
                = Instantiate(setting, new Vector3(switchPos.switchX, 0.1f, switchPos.switchZ), setting.transform.rotation);
        }
    }

    public struct SwitchColor
    {
        public int switchX;
        public int switchZ;
        public SwitchColor(int x, int z)
        {
            switchX = x;
            switchZ = z;
        }
    }

    enum TileState
    {
        None = 0,
        White,
        Black,
    }
}
