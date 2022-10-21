using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] _boardState = new int[8, 8];      //�}�X�̏���ۑ����Ă���
    bool[,] _boardSettable = new bool[8, 8]; //�u����}�X��true��Ԃ����ƂŁA�z�u�̉ۂ𔻒f����
    GameObject[,] _stones = new GameObject[8, 8];
    GameObject[,] _tiles = new GameObject[8, 8];
    [Tooltip("1...��, 2...��"), SerializeField] int _turn = 0;
    int _beFraTurn = 0;
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;
    [SerializeField] GameObject _settableTile;
    //�}�X����̈ړ���(�T���Ɏg��)
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
        //�����ݒ�(�����ɐ΂�z�u������Ԃ܂�)
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Tiles[i, j] = Instantiate(_settableTile, new Vector3(i, 0.1f, j), _settableTile.transform.rotation);
                if ((i == 3 && j == 4) || (i == 4 && j == 3)) //���΂̏����z�u
                {
                    _boardState[i, j] = (int)TileState.White;
                    Stones[i, j] = Instantiate(_white, new Vector3(i, 0.1f, j), _white.transform.rotation);
                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4)) //���΂̏����z�u
                {
                    _boardState[i, j] = (int)TileState.Black;
                    Stones[i, j] = Instantiate(_black, new Vector3(i, 0.1f, j), _black.transform.rotation);
                }
                else
                    _boardState[i, j] = (int)TileState.None;
            }
        }
        _turn = 2; //�I�Z���͏��荕����炵��
        _beFraTurn = 2;
        SettableDrawing();
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�����؂�ւ�����^�C�~���O�ŁA�΂�u����}�X�����邩�ǂ����𔻒肷��(�Ȃ������ꍇ�A�p�X�ɂȂ�)
        //���̕����̏����Œu����}�X�𖾎��I�ɂ���
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
            SettableDrawing();
            _beFraTurn = _turn;
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
                    TurnOverCheck(x, z);
                    _turn = _turn == 1 ? 2 : 1; //�^�[���̐؂�ւ�(���^�[���Ȃ獕�ɁA���^�[���Ȃ甒��)
                }
                else
                {
                    Debug.Log("���̃}�X�ɂ͒u���܂���");
                }
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

            if (_boardState[startX, startZ] == (int)TileState.None) //�u����}�X�͐΂�u���Ă��Ȃ��}�X
            {
                x += CheckSetX[i];
                z += CheckSetZ[i];
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
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
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
                if (!(0 <= x && x < 8 && 0 <= z && z < 8))
                    break;

                while (_boardState[x, z] == (int)TileState.White)
                {
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                    count++;
                }

                if (_boardState[x, z] == (int)TileState.Black && count != 0) //�΂����܂�Ă��邩
                {
                    _boardSettable[startX, startZ] = true;
                }
            }
        }
    }

    /// <summary>
    /// �I�񂾃}�X����ɁA�ǂ̐΂��Ђ�����Ԃ����𒲂ׂ�
    /// </summary>
    /// <param name="x">�΂�u���}�X��x���W</param>
    /// <param name="z">�΂�u���}�X��z���W</param>
    void TurnOverCheck(int x, int z)
    {
        Switchable.Clear();
        //�΂�u���}�X
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
        //�I�񂾃}�X�ɐ΂�u��
        Stones[x, z] = Instantiate(setting, new Vector3(x, 0.1f, z), setting.transform.rotation);
        //�Ђ�����Ԃ�
        for (int i = 0; i < 8; i++)
        {
            x = startX;
            z = startZ;

            if (_boardState[startX, startZ] == (int)TileState.None) //�I�񂾃}�X�͕K���΂�u���Ă��Ȃ��}�X�ł���
            {
                x += CheckSetX[i];
                z += CheckSetZ[i];
            }

            //���^�[��
            if (_turn == 1)
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //�T������}�X�̐i�s�������Ֆʂ͈̔͊O�Ȃ�T�����Ȃ�
                    break;

                while (_boardState[x, z] == (int)TileState.Black) //�T����ɂЂ�����Ԃ���΂�����Ԏ��s�����
                {
                    Switchable.Add(new SwitchColor(x, z));
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                }

                if (_boardState[x, z] == (int)TileState.White) //�΂����܂�Ă��邩
                {
                    TurnOver(setting);
                }
            }
            //���^�[��
            else
            {
                if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //�T������}�X�̐i�s�������Ֆʂ͈̔͊O�Ȃ�T�����Ȃ�
                    break;

                while (_boardState[x, z] == (int)TileState.White)
                {
                    Debug.Log(Switchable.Count);
                    Switchable.Add(new SwitchColor(x, z));
                    x += CheckSetX[i];
                    z += CheckSetZ[i];
                }

                if (_boardState[x, z] == (int)TileState.Black) //�΂����܂�Ă��邩
                {
                    TurnOver(setting);
                }
            }
        }
    }

    void TurnOver(GameObject setting)
    {
        //�Ђ�����Ԃ�(���ۂ�)����
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
