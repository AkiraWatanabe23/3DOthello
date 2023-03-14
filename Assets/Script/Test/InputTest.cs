using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
    private InputField _input = default;

    private void Start()
    {
        _input = GetComponent<InputField>();
    }

    public void InputMove()
    {
        Debug.Log(_input.text);
    }
}
