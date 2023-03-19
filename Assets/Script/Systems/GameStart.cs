using UnityEngine;
using UnityEngine.Events;

public class GameStart : MonoBehaviour
{
    [SerializeField] private UnityEvent _event = default;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log("ゲーム開始");
            _event?.Invoke();
        }
    }
}
