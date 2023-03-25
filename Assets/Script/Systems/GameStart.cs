using Constants;
using UnityEngine;
using UnityEngine.Events;

public class GameStart : MonoBehaviour
{
    [SerializeField] private UnityEvent _event = default;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("ゲーム開始");
            _event?.Invoke();
        }
    }

    /// <summary> 先手を決める </summary>
    public void SelectFirst(int move)
    {
        if (move == 0)
            GameManager.CurrentColor = Consts.BLACK;
        else if (move == 1)
            GameManager.CurrentColor = Consts.WHITE;
    }
}
