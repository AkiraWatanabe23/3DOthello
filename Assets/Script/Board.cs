using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];      //�}�X�̏���ۑ����Ă���
    bool[,] _boardSettable = new bool[8, 8]; //�u����}�X��true��Ԃ����ƂŁA�z�u�̉ۂ𔻒f����
    GameObject[,] _stones = new GameObject[8, 8];
    GameObject[,] _tiles = new GameObject[8, 8];
    int _beFraTurn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;
    [SerializeField] GameObject _settableTile;
    //�}�X����̈ړ���(�T���Ɏg��)
    int[] _checkSetX = new[] { -1, -1, 0, 1, 1, 1, 0, -1 };
    int[] _checkSetZ = new[] { 0, 1, 1, 1, 0, -1, -1, -1 };
    TurnOverCheck _check;
    public int[,] BoardState { get => _boardState; set => _boardState = value; }
    /// <summary> 1...��, 2...�� </summary>
    public int Turn { get; set; }
    public GameObject[,] Stones { get => _stones; set => _stones = value; }
    public GameObject[,] Tiles { get => _tiles; set => _tiles = value; }
    public int[] CheckSetX { get => _checkSetX; set => _checkSetX = value; }
    public int[] CheckSetZ { get => _checkSetZ; set => _checkSetZ = value; }

    // Start is called before the first frame update
    void Start()
    {
        _check = GetComponent<TurnOverCheck>();
        //�����ݒ�(�����ɐ΂�z�u������Ԃ܂�)
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Tiles[i, j] = Instantiate(_settableTile, new Vector3(i, 0.1f, j), _settableTile.transform.rotation);
                if ((i == 3 && j == 4) || (i == 4 && j == 3)) //���΂̏����z�u
                {
                    BoardState[i, j] = (int)TileState.White;
                    Stones[i, j] = Instantiate(_white, new Vector3(i, 0.1f, j), _white.transform.rotation);
                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4)) //���΂̏����z�u
                {
                    BoardState[i, j] = (int)TileState.Black;
                    Stones[i, j] = Instantiate(_black, new Vector3(i, 0.1f, j), _black.transform.rotation);
                }
                else
                    BoardState[i, j] = (int)TileState.None;
            }
        }
        Turn = 2; //�I�Z���͏��荕����炵��
        _beFraTurn = 2;
        SettableDrawing();
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�����؂�ւ�����^�C�~���O�ŁA�΂�u����}�X�����邩�ǂ����𔻒肷��(�Ȃ������ꍇ�A�p�X�ɂȂ�)
        //���̕����̏����Œu����}�X�𖾎��I�ɂ���
        if (Turn != _beFraTurn)
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
            SettableDrawing();
            _beFraTurn = Turn;
        }

        //�󂢂Ă���}�X�ɐ΂�u������
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && 
                hit.collider.gameObject.CompareTag("Tile"))
            {
                int x = (int)hit.collider.gameObject.transform.position.x;
                int z = (int)hit.collider.gameObject.transform.position.z;

                if (_boardSettable[x, z] == true)
                {
                    _check.SwitchCheck(x, z);
                    Turn = Turn == 1 ? 2 : 1; //�^�[���̐؂�ւ�(������ or ������)
                }
                else
                    Debug.Log("���̃}�X�ɂ͒u���܂���");
            }
        }
    }

    /// <summary>
    /// �΂�u�����Ƃ��o����}�X�̖����I�ȕ\��
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
    /// �󂢂Ă���}�X�ɐ΂�u�����Ƃ��o���邩�̔���
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

            x = startX;
            z = startZ;

            if (BoardState[startX, startZ] == (int)TileState.None) //�u����}�X�͐΂�u���Ă��Ȃ��}�X
            {
                x += CheckSetX[i];
                z += CheckSetZ[i];
            }
            else //�΂�����}�X�͒T���O
                break;

            //���^�[��
            if (Turn == 1)
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //�T������}�X�̐i�s�������Ֆʂ͈̔͊O�Ȃ�T�����Ȃ�
                    break;

                while (BoardState[x, z] == (int)TileState.Black) //�T����ɂЂ�����Ԃ���΂�����Ԏ��s�����
                {
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                    count++;
                }

                if (BoardState[x, z] == (int)TileState.White && count != 0) //�΂����܂�Ă��邩
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
            //���^�[��
            else
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8))
                    break;

                while (BoardState[x, z] == (int)TileState.White)
                {
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                    count++;
                }

                if (BoardState[x, z] == (int)TileState.Black && count != 0) //�΂����܂�Ă��邩
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
