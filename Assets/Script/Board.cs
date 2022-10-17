using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];
    [SerializeField] int _turn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;
    RaycastHit _hit;
    //前後左右方向のマスからの移動差(マスの探索に使う)
    int[] ZnumVer = new int[] { -1, 1 };
    int[] XnumVer = new int[] { -1, 1 };
    int[] ZnumHor = new int[] { -1, 1 };
    int[] XnumHor = new int[] { -1, 1, -1, 1 };

    // Start is called before the first frame update
    void Start()
    {
        //初期設定(中央に石を配置した状態まで)
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i == 3 && j == 4) || (i == 4 && j == 3))
                {
                    _boardState[i, j] = (int)TileState.White;
                    Instantiate(_white, new Vector3(i, 0.1f, j), _white.transform.rotation);
                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4))
                {
                    _boardState[i, j] = (int)TileState.Black;
                    Instantiate(_black, new Vector3(i, 0.1f, j), _black.transform.rotation);
                }
                else
                    _boardState[i, j] = (int)TileState.None;
            }
        }
        _turn = 2; //オセロは初手黒かららしい
    }

    // Update is called once per frame
    void Update()
    {
        //空いているマスに石を置く処理(置けるマスか置けないマスかの判断をする処理も)
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                int x = (int)_hit.collider.gameObject.transform.position.x;
                int z = (int)_hit.collider.gameObject.transform.position.z;

                if (SettableCheck(x, z) == true)
                {
                    if (_boardState[x, z] == 0)
                    {
                        if (_turn == 1)
                        {
                            _boardState[x, z] = (int)TileState.White;
                            Instantiate(_white, new Vector3(x, 0.1f, z), _white.transform.rotation);
                            _turn = 2;
                        }
                        else
                        {
                            _boardState[x, z] = (int)TileState.Black;
                            Instantiate(_black, new Vector3(x, 0.1f, z), _black.transform.rotation);
                            _turn = 1;
                        }
                    }
                }
                else
                {
                    Debug.Log("このマスには置けません");
                }
            }
        }
    }

    bool SettableCheck(int x, int z)
    {
        //選ばれたマスの全方向を探索し、置けるマスかどうか(そこに置いたらひっくり返せるか)を判定する
        //前後
        //白ターン
        if (_turn == 1)
        {
            while (_boardState[x, z] == (int)TileState.Black)
            {

            }
        }
        //黒ターン
        else
        {

        }
        for (int i = 0; i < ZnumVer.Length; i++)
        {
            if ((i == 0 && z != 0) || (i == 1 && z != 7)) //IndexOutOfRange防止
            {

            }
        }
        //左右
        for (int i = 0; i < XnumVer.Length; i++)
        {
            if ((i == 0 && x != 0) || (i == 1 && x != 7))
            {

            }
        }
        //斜め
        for (int i = 0; i < XnumHor.Length; i++)
        {
            if (i <= 1) //前
            {
                if ((i == 0 && x != 0 && z != 0) ||
                    (i == 1 && x != 7 && z != 0))
                {

                }
            }
            else //後ろ
            {
                if ((i == 2 && x != 0 && z != 7) ||
                    (i == 3 && x != 7 && z != 7))
                {

                }
            }
        }
        return false;
    }

    enum TileState
    {
        None = 0,
        White,
        Black,
        UnSettable = -1 //ここのマスには置けない
    }
}
