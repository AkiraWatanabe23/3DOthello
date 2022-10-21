using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 石をひっくり返す
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
    /// 選んだマスを基準に、どの石をひっくり返すかを調べる
    /// </summary>
    /// <param name="x">石を置くマスのx座標</param>
    /// <param name="z">石を置くマスのz座標</param>
    public void SwitchCheck(int x, int z)
    {
        Switchable.Clear();
        //石を置くマス
        int startX = x;
        int startZ = z;
        GameObject setting;

        setting = _board.Turn == 1 ? _white : _black;
        //ひっくり返す
        for (int i = 0; i < 8; i++)
        {
            x = startX;
            z = startZ;

            if (_board.BoardState[startX, startZ] == 0) //選んだマスは必ず石を置いていないマスである
            {
                x += _board.CheckSetX[i];
                z += _board.CheckSetZ[i];
            }

            if (!(0 <= x && x < 8 && 0 <= z && z < 8)) //探索するマスの進行方向が盤面の範囲外なら探索しない
                break;

            while (_board.BoardState[x, z] == (_board.Turn == 1 ? 2 : 1)) //探索先にひっくり返せる石がある間実行される
            {
                Switchable.Add(new SwitchColor(x, z));
                x += _board.CheckSetX[i];
                z += _board.CheckSetZ[i];
                if (_board.BoardState[x, z] == (_board.Turn == 1 ? 1 : 2)) //石が挟まれているか
                {
                    TurnOver(setting);
                    break;
                }
            }
        }
        //選んだマスに石を置く
        _board.BoardState[x, z] = _board.Turn == 1 ? 1 : 2;
        _board.Stones[x, z] = Instantiate(setting, new Vector3(startX, 0.1f, startZ), Quaternion.identity);
    }

    void TurnOver(GameObject setting)
    {
        //ひっくり返す処理
        for (int i = 0; i < Switchable.Count; i++)
        {
            SwitchColor switchPos = Switchable[i];
            Destroy(_board.Stones[switchPos.switchX, switchPos.switchZ]);
            _board.Stones[switchPos.switchX, switchPos.switchZ]
                = Instantiate(setting, new Vector3(switchPos.switchX, 0.1f, switchPos.switchZ), Quaternion.identity);
        }
        _board.Turn = _board.Turn == 1 ? 2 : 1; //ターンの切り替え(白→黒 or 黒→白)
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
