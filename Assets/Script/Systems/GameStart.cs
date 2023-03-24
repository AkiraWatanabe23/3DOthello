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
}
