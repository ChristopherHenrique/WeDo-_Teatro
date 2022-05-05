using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class BtnBilheteria : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Serializable]
    public class ButtonPressEvent : UnityEvent { }

    public ButtonPressEvent OnPress = new ButtonPressEvent();

    private bool canClick = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canClick)
        {
            OnPress.Invoke();
            canClick = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        canClick = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Mouse enter");
        GameManager.Instance.ChangeMouseCursor(GameManager.POINTERCURSOR);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Mouse exit");
        GameManager.Instance.ChangeMouseCursor(GameManager.DEFAULTCURSOR);
    }
}
