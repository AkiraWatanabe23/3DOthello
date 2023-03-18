using UnityEngine;
using UnityEngine.EventSystems;

public class ClickInput : MonoBehaviour, IPointerClickHandler
{
    private InputTest _input = default;

    private void Start()
    {
        _input = GameObject.Find("System").GetComponent<InputTest>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var go = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log(go.transform.position);
        _input.BlackInput(go.transform.position);
    }
}
