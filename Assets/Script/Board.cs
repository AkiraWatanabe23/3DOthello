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
    RaycastHit _hit;

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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                int x = (int)_hit.collider.gameObject.transform.position.x;
                int z = (int)_hit.collider.gameObject.transform.position.z;

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
                else
                {
                    Debug.Log("�����ɂ͒u���Ȃ�");
                }
            }
        }
    }

    enum TileState
    {
        None = 0,
        White,
        Black
    }
}
