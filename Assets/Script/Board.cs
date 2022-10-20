using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];      //�}�X�̏���ۑ����Ă���
    bool[,] _boardSettable = new bool[8, 8]; //�u����}�X��true��Ԃ����ƂŁA�z�u�̉ۂ𔻒f����
    GameObject[,] _tiles = new GameObject[8, 8];
    [SerializeField] int _turn = 0;
    int _beFraTurn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;
    [SerializeField] GameObject _settableTile;
    RaycastHit _hit;
    //�}�X����̈ړ���(�T���Ɏg��)
    int[] _checkSetX = new[] { -1, -1, 0, 1, 1, 1, 0, -1 };
    int[] _checkSetZ = new[] { 0, 1, 1, 1, 0, -1, -1, -1 };
    public GameObject[,] Tiles { get => _tiles; set => _tiles = value; }

    // Start is called before the first frame update
    void Start()
    {
        //�����ݒ�(�����ɐ΂�z�u������Ԃ܂�)
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Tiles[i, j] = Instantiate(_settableTile, new Vector3(i, 0.1f, j), _settableTile.transform.rotation);
                if ((i == 3 && j == 4) || (i == 4 && j == 3)) //���΂̏����z�u
                {
                    _boardState[i, j] = (int)TileState.White;
                    Instantiate(_white, new Vector3(i, 0.1f, j), _white.transform.rotation);
                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4)) //���΂̏����z�u
                {
                    _boardState[i, j] = (int)TileState.Black;
                    Instantiate(_black, new Vector3(i, 0.1f, j), _black.transform.rotation);
                }
                else
                    _boardState[i, j] = (int)TileState.None;
            }
        }
        _turn = 2; //�I�Z���͏��荕����炵��
        _beFraTurn = 2;
        DrawingSettable();
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�����؂�ւ�����^�C�~���O�ŁA�΂�u����}�X�����邩�ǂ����𔻒肷��(�Ȃ������ꍇ�A�p�X�ɂȂ�)
        if (_turn != _beFraTurn)
        {
            //�����Ŕz�u�\�}�X�̒T�����Z�b�g
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _boardSettable[i, j] = false;
                    Tiles[i, j].GetComponent<MeshRenderer>().enabled = false;
                }
            }
            DrawingSettable();
        }
        _beFraTurn = _turn;

        //�󂢂Ă���}�X�ɐ΂�u������(��L�̏����Œu����}�X�𖾎��I�ɕ\������\��)
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                int x = (int)_hit.collider.gameObject.transform.position.x;
                int z = (int)_hit.collider.gameObject.transform.position.z;

                if (_boardSettable[x, z] == true)
                {
                    if (_turn == 1)
                    {
                        _boardState[x, z] = (int)TileState.White;
                        Instantiate(_white, new Vector3(x, 0.1f, z), _white.transform.rotation);
                    }
                    else
                    {
                        _boardState[x, z] = (int)TileState.Black;
                        Instantiate(_black, new Vector3(x, 0.1f, z), _black.transform.rotation);
                    }
                    _turn = _turn == 1 ? 2 : 1; //�^�[���̐؂�ւ�
                }
                else
                {
                    Debug.Log("���̃}�X�ɂ͒u���܂���");
                }
            }
        }
    }

    /// <summary>
    /// �󂢂Ă���}�X�ɐ΂�u�����Ƃ��o���邩�𔻒肷��
    /// </summary>
    /// <param name="x">�I�񂾃}�X��x���W</param>
    /// <param name="z">�I�񂾃}�X��z���W</param>
    void SettableCheck(int x, int z)
    {
        for (int i = 0; i < 8; i++)
        {
            //�Ђ�����Ԃ���΂�������������
            int count = 0;
            //�T�����n�߂�}�X
            int startX = x;
            int startZ = z;
            //�T����������̏��
            int checkX = _checkSetX[i];
            int checkZ = _checkSetZ[i];

            if (_boardState[startX, startZ] == (int)TileState.None) //�u����}�X�͐΂�u���Ă��Ȃ��}�X
            {
                x += checkX;
                z += checkZ;
            }
            else //�΂�����}�X�͒T���O
                break;

            //���^�[��
            if (_turn == 1)
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //�T������}�X�̐i�s�������Ֆʂ͈̔͊O�Ȃ�T�����Ȃ�
                    break;

                while (_boardState[x, z] == (int)TileState.Black) //�T����ɂЂ�����Ԃ���΂�����Ԏ��s�����
                {
                    x += checkX;
                    z += checkZ;
                    count++;
                }

                if (_boardState[x, z] == (int)TileState.White && count != 0) //�΂����܂�Ă��邩
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
            //���^�[��
            else
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //�T������}�X�̐i�s�������Ֆʂ͈̔͊O�Ȃ�T�����Ȃ�
                    break;

                while (_boardState[x, z] == (int)TileState.White)
                {
                    x += checkX;
                    z += checkZ;
                    count++;
                }

                if (_boardState[x, z] == (int)TileState.Black && count != 0) //�΂����܂�Ă��邩
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
        }
    }

    void DrawingSettable()
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

    enum TileState
    {
        None = 0,
        White,
        Black,
    }
}
