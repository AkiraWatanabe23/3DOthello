using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];
    int _start = 0; //�ŏ��ɂǂ������炩���߂�
    [SerializeField] int _turn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;

    // Start is called before the first frame update
    void Start()
    {
        //�����ݒ�(�����ɐ΂�z�u������Ԃ܂�)
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
        //�ŏ��ɂǂ������炩���߂�
        _start = Random.Range(1,10);
        if (_start % 2 == 1)
        {
            //������n�߂�
            _turn = 1;
            Debug.Log("������ł�");
        }
        else
        {
            //������n�߂�
            _turn = 2;
            Debug.Log("������ł�");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�󂢂Ă���}�X�ɐ΂�u������
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
