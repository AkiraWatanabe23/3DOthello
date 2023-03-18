using Constants;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
    [SerializeField] private InputField _input = default;

    private string _inputPlace = "";
    private GameManager _manager = default;
    private EnemyMove _enemy = default;

    private readonly Reversi _reversi = new();
    private readonly TurnOverCheck _checking = new();

    private void Start()
    {
        _manager = GetComponent<GameManager>();
        _enemy = GetComponent<EnemyMove>();
    }

    public void InputMove()
    {
        if (_manager.CurrentColor == Consts.BLACK)
        {
            _inputPlace = _input.text;
            Debug.Log(_inputPlace + "を選択しました");
        }
        else if (_manager.CurrentColor == Consts.WHITE)
        {
            _inputPlace = _enemy.TypeCheck();
        }

        if (_inputPlace == "e")
        {
            Debug.Log("中断");
            //ゲームを中断する
        }

        if (_reversi.InputCorrect(_inputPlace))
        {
            int x = Array.IndexOf(Consts.INPUT_ALPHABET, _inputPlace[0]) + 1;
            int y = Array.IndexOf(Consts.INPUT_NUMBER, _inputPlace[1]) + 1;

            if (_checking.SetStone(_manager.MovablePos, x, y))
            {
                _manager.Board =
                    _checking.FlipStone(_manager.Board, _manager.MovableDir[x, y], x, y, _manager.CurrentColor);

                _manager.Display();

                //ゲームがまだ終了していなければ
                if (!_manager.GameFinish())
                {
                    _manager.TurnCount++;
                    _manager.CurrentColor = -_manager.CurrentColor;

                    _manager.ResetMovables();
                }
                else
                {
                    Debug.Log("ゲームを終了します");
                }
            }
        }
        else
        {
            Debug.LogWarning("不正な入力です。正しい形式(例. f5)で入力してください。");
            _input.text = "";
        }
    }
}
