using Constants;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
    private GameManager _manager = new();
    private Reversi _reversi = new();
    private TurnOverCheck _checking = new();
    private InputField _input = default;

    private void Start()
    {
        _input = GetComponent<InputField>();
    }

    public void InputMove()
    {
        string input = _input.text;
        Debug.Log(input + "を選択しました");

        if (input == "e")
        {
            Debug.Log("中断");
            //ゲームを中断する
        }

        if (_reversi.InputCorrect(input))
        {
            int x = Array.IndexOf(Consts.INPUT_ALPHABET, input[0]) + 1;
            int y = Array.IndexOf(Consts.INPUT_NUMBER, input[1]) + 1;

            if (_checking.SetStone(_manager.MovablePos, x, y))
            {
                Debug.Log("true");

                _manager.Board = _checking.FlipStone(_manager.Board, x, y, _manager.CurrentColor);

                _manager.Display();
                _manager.TurnCount++;
                _manager.CurrentColor = -_manager.CurrentColor;

                _manager.ResetMovables();
            }
        }
        else
        {
            Debug.LogWarning("不正な入力です。正しい形式(例. f5)で入力してください。");
            _input.text = "";
        }
    }
}
