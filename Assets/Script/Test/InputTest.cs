﻿using Constants;
using System;
using System.Collections;
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

    public void WhiteInput()
    {
        if (_manager.CurrentColor == Consts.WHITE)
        {
            _inputPlace = _enemy.TypeCheck();
            Debug.Log(_inputPlace);
        }
        StartCoroutine(InputMove());
    }

    public void BlackInput(Vector3 pos)
    {
        if (_manager.CurrentColor == Consts.BLACK)
        {
            //Consts.INPUT_ALPHABET[x - 1].ToString() + Consts.INPUT_NUMBER[y - 1]
            _inputPlace = Consts.INPUT_ALPHABET[(int)pos.x - 1].ToString() + Consts.INPUT_NUMBER[9 - (int)pos.z - 1];
            Debug.Log(_inputPlace + "を選択しました");
        }

        if (_inputPlace == "e")
        {
            Debug.Log("中断");
            //ゲームを中断する
        }
        StartCoroutine(InputMove());
    }

    private IEnumerator InputMove()
    {
        if (_manager.CurrentColor == Consts.WHITE)
        {
            yield return new WaitForSeconds(1f);
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
                    Debug.Log("ゲームを続行します");
                }
                else
                {
                    Debug.Log("ゲームを終了します");
                    _manager.WinningCheck();
                }
            }
        }
        else
        {
            Debug.LogWarning("不正な入力です。正しい形式(例. f5)で入力してください。");
            _input.text = "";
        }

        if (_manager.Skip())
        {
            _manager.CurrentColor = -_manager.CurrentColor;
            _manager.ResetMovables();
            Debug.Log("パスしました");
        }
        else
        {
            Debug.Log("パスしない");
        }
        Debug.Log($"Turn : {_manager.CurrentColor}");

        if (_manager.CurrentColor == Consts.WHITE)
        {
            WhiteInput();
        }
    }
}
