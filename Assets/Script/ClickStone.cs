using UnityEngine;
using UnityEngine.EventSystems;

public class ClickStone : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //GameObject go = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log(name);
    }
}
