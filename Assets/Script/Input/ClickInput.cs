using UnityEngine;
using UnityEngine.EventSystems;

public class ClickInput : MonoBehaviour, IPointerClickHandler
{
    private PlayerInput _input = default;

    private void Start()
    {
        _input = GameObject.Find("System").GetComponent<PlayerInput>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var go = eventData.pointerCurrentRaycast.gameObject;
        _input.BlackInput(go.transform.position);
    }
}
