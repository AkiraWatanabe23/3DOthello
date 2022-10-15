using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];
    int _start = 0; //最初にどっちからか決める
    [SerializeField] int _turn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;

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
        //最初にどっちからか決める
        _start = Random.Range(1,10);
        if (_start % 2 == 1)
        {
            //白から始める
            _turn = 1;
            Debug.Log("白からです");
        }
        else
        {
            //黒から始める
            _turn = 2;
            Debug.Log("黒からです");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //空いているマスに石を置く処理
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

    enum TileState
    {
        None = 0,
        White,
        Black
    }
}
