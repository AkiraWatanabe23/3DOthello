using Constants;
using System;
using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string _inputPlace = "";
    private GameManager _manager = default;
    private EnemyMove _enemy = default;

    private readonly TurnOverCheck _checking = new();

    private void Start()
    {
        _manager = GetComponent<GameManager>();
        _enemy = GetComponent<EnemyMove>();

        if (GameManager.CurrentColor == Consts.WHITE)
            WhiteInput();
    }

    public void WhiteInput()
    {
        if (GameManager.CurrentColor == Consts.WHITE)
        {
            _inputPlace = _enemy.TypeCheck();
            Debug.Log($"相手が {_inputPlace} を選択しました");
        }

        StartCoroutine(InputMove());
    }

    public void BlackInput(Vector3 pos)
    {
        if (GameManager.CurrentColor == Consts.BLACK)
        {
            _inputPlace = Consts.INPUT_ALPHABET[(int)pos.x - 1].ToString() + Consts.INPUT_NUMBER[9 - (int)pos.z - 1];
            Debug.Log(_inputPlace + "を選択しました");
        }

        if (_inputPlace == "e")
        {
            Debug.Log("中断");
            //ゲームを中断する
            _manager.WinningCheck();
        }
        StartCoroutine(InputMove());
    }

    private IEnumerator InputMove()
    {
        if (GameManager.CurrentColor == Consts.WHITE)
        {
            yield return new WaitForSeconds(1f);
        }

        try
        {
            Debug.Log(_inputPlace);
            int x = Array.IndexOf(Consts.INPUT_ALPHABET, _inputPlace[0]) + 1;
            int y = Array.IndexOf(Consts.INPUT_NUMBER, _inputPlace[1]) + 1;

            if (_checking.SetStone(_manager.MovablePos, x, y))
            {
                _manager.Board =
                    _checking.FlipStone(_manager.Board, _manager.MovableDir[x, y], x, y, GameManager.CurrentColor);

                _manager.Display();

                //ゲームがまだ終了していなければ
                if (!_manager.GameFinish())
                {
                    _manager.TurnCount++;
                    GameManager.CurrentColor *= -1;

                    _manager.ResetMovables();
                    _manager.StoneCount();
                    Debug.Log("ゲームを続行します");
                }
                else
                {
                    Debug.Log("ゲームを終了します");
                    _manager.WinningCheck();
                }
            }
        }
        catch (IndexOutOfRangeException range)
        {
            Debug.LogWarning($"{range}：不正な入力です。");
        }

        if (_manager.Skip())
            Debug.Log("パスしました");

        Debug.Log($"Turn : {GameManager.CurrentColor}");

        if (GameManager.CurrentColor == Consts.WHITE)
        {
            WhiteInput();
        }
    }
}
