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
    //�O�㍶�E�����̃}�X����̈ړ���(�}�X�̒T���Ɏg��)
    int[] ZnumVer = new int[] { -1, 1 };
    int[] XnumVer = new int[] { -1, 1 };
    int[] ZnumHor = new int[] { -1, 1 };
    int[] XnumHor = new int[] { -1, 1, -1, 1 };
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
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�����؂�ւ�����^�C�~���O�ŁA�΂�u����}�X�����邩�ǂ����𔻒肷��(�Ȃ������ꍇ�A�p�X�ɂȂ�)
        if (_turn != _beFraTurn)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SettableCheck(j, i);
                }
            }
        }
        _beFraTurn = _turn;

        //�󂢂Ă���}�X�ɐ΂�u������(��L�̏����Œu����}�X�𖾎��I�ɕ\������\��)
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                int x = (int)_hit.collider.gameObject.transform.position.x;
                int z = (int)_hit.collider.gameObject.transform.position.z;

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
    }

    /// <summary>
    /// �I�΂ꂽ(�N���b�N���ꂽ)�}�X�ɐ΂�u�����Ƃ��o���邩�𔻒肷��
    /// </summary>
    /// <param name="x">�I�񂾃}�X��x���W</param>
    /// <param name="z">�I�񂾃}�X��z���W</param>
    bool SettableCheck(int x, int z)
    {
        //�I�΂ꂽ�}�X�̑S������T�����A�u����}�X���ǂ���(�����ɒu������Ђ�����Ԃ�΂����邩)�𔻒肷��
        //���^�[��
        if (_turn == 1)
        {
            while (_boardState[x, z] == (int)TileState.Black) //�T����ɂЂ�����Ԃ���\���̂���΂�����Ԏ��s�����
            {
                z--;
            }
            if (_boardState[x, z] == (int)TileState.White) //�΂����܂�Ă��邩
            {
                _boardSettable[x, z] = true;
            }
        }
        //���^�[��
        else
        {
            while (_boardState[x, z] == (int)TileState.White)
            {

            }
        }

        //�O��
        for (int i = 0; i < ZnumVer.Length; i++)
        {
            if ((i == 0 && z != 0) || (i == 1 && z != 7)) //IndexOutOfRange�h�~
            {

            }
        }
        //���E
        for (int i = 0; i < XnumVer.Length; i++)
        {
            if ((i == 0 && x != 0) || (i == 1 && x != 7))
            {

            }
        }

        //�΂�
        for (int i = 0; i < XnumHor.Length; i++)
        {
            if (i <= 1) //�O
            {
                if ((i == 0 && x != 0 && z != 0) ||
                    (i == 1 && x != 7 && z != 0))
                {

                }
            }
            else //���
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
    }
}
