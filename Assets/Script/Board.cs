using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];      //マスの情報を保存しておく
    bool[,] _boardSettable = new bool[8, 8]; //置けるマスはtrueを返すことで、配置の可否を判断する
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
    public GameObject[,] Tiles { get => _tiles; set => _tiles = value; }
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
                    Instantiate(_white, new Vector3(i, 0.1f, j), _white.transform.rotation);
                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4)) //黒石の初期配置
                {
                    _boardState[i, j] = (int)TileState.Black;
                    Instantiate(_black, new Vector3(i, 0.1f, j), _black.transform.rotation);
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
                    TurnOver(x, z);
                    _turn = _turn == 1 ? 2 : 1; //ターンの切り替え
                }
                else
                {
                    Debug.Log("このマスには置けません");
                }
            }
        }
    }

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
    /// 空いているマスに石を置くことが出来るかを判定する
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
            Switchable.Clear();

            if (_boardState[startX, startZ] == (int)TileState.None) //置けるマスは石を置いていないマス
            {
                x += _checkSetX[i];
                z += _checkSetZ[i];
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
                    Switchable.Add(new SwitchColor(x, z));
                    x += _checkSetX[i];
                    z += _checkSetZ[i];
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
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //探索するマスの進行方向が盤面の範囲外なら探索しない
                    break;

                while (_boardState[x, z] == (int)TileState.White)
                {
                    Switchable.Add(new SwitchColor(x, z));
                    x += _checkSetX[i];
                    z += _checkSetZ[i];
                    count++;
                }

                if (_boardState[x, z] == (int)TileState.Black && count != 0) //石が挟まれているか
                {
                    _boardSettable[startX, startZ] = true;
                }
                Debug.Log(Switchable.Count);
            }
        }
    }

    void TurnOver(int x, int z)
    {
        GameObject setting = null;
        SettableCheck(x, z);

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
        Instantiate(setting, new Vector3(x, 0.1f, z), setting.transform.rotation);
        //ひっくり返す(っぽい処理)
        for (int i = 0; i < Switchable.Count; i++)
        {
            SwitchColor switchPos = Switchable[i];
            if (Physics.Raycast(new Vector3(switchPos.switchX, 1f, switchPos.switchZ), Vector3.down, out RaycastHit hit, 5))
            {
                Destroy(hit.collider.gameObject);
                Debug.Log("aaa");
            }
            Instantiate(setting, new Vector3(switchPos.switchX, 0.1f, switchPos.switchZ), setting.transform.rotation);
        }
        Switchable.Clear();
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
