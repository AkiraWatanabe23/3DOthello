using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �΂��Ђ�����Ԃ�
/// </summary>
public class TurnOverCheck : MonoBehaviour
{
    [SerializeField] GameObject _white;
    [SerializeField] GameObject _black;
    Board _board;
    List<SwitchColor> _switchable = new();
    public List<SwitchColor> Switchable { get => _switchable; set => _switchable = value; }

    // Start is called before the first frame update
    void Start()
    {
        _board = GetComponent<Board>();
    }

    /// <summary>
    /// �I�񂾃}�X����ɁA�ǂ̐΂��Ђ�����Ԃ����𒲂ׂ�
    /// </summary>
    /// <param name="x">�΂�u���}�X��x���W</param>
    /// <param name="z">�΂�u���}�X��z���W</param>
    public void SwitchCheck(int x, int z)
    {
        Switchable.Clear();
        //�΂�u���}�X
        int startX = x;
        int startZ = z;
        GameObject setting;

        setting = _board.Turn == 1 ? _white : _black;
        //�Ђ�����Ԃ�
        for (int i = 0; i < 8; i++)
        {
            x = startX;
            z = startZ;

            if (_board.BoardState[startX, startZ] == 0) //�I�񂾃}�X�͕K���΂�u���Ă��Ȃ��}�X�ł���
            {
                x += _board.CheckSetX[i];
                z += _board.CheckSetZ[i];
            }

            if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //�T������}�X�̐i�s�������Ֆʂ͈̔͊O�Ȃ�T�����Ȃ�
                break;

            while (_board.BoardState[x, z] == (_board.Turn == 1 ? 2 : 1)) //�T����ɂЂ�����Ԃ���΂�����Ԏ��s�����
            {
                Switchable.Add(new SwitchColor(x, z));
                x += _board.CheckSetX[i];
                z += _board.CheckSetZ[i];
                if (_board.BoardState[x, z] == (_board.Turn == 1 ? 1 : 2)) //�΂����܂�Ă��邩
                {
                    TurnOver(setting);
                    break;
                }
            }
        }
        //�I�񂾃}�X�ɐ΂�u��
        _board.BoardState[x, z] = _board.Turn == 1 ? 1 : 2;
        _board.Stones[x, z] = Instantiate(setting, new Vector3(startX, 0.1f, startZ), Quaternion.identity);
    }

    void TurnOver(GameObject setting)
    {
        //�Ђ�����Ԃ�����
        for (int i = 0; i < Switchable.Count; i++)
        {
            SwitchColor switchPos = Switchable[i];
            Destroy(_board.Stones[switchPos.switchX, switchPos.switchZ]);
            _board.Stones[switchPos.switchX, switchPos.switchZ]
                = Instantiate(setting, new Vector3(switchPos.switchX, 0.1f, switchPos.switchZ), Quaternion.identity);
        }
        _board.Turn = _board.Turn == 1 ? 2 : 1; //�^�[���̐؂�ւ�(������ or ������)
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
}
