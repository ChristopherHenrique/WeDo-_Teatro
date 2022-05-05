using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    GameObject hotspot;
    bool CanCount;
    float timer = 0f;
    public bool isClick { get; private set; } = false;
    void Start()
    {
        hotspot = transform.parent.GetChild(0).gameObject;
    }
    void FixedUpdate()
    {
        if (CanCount)
        {
            timer += Time.deltaTime;
            if (timer > 0.4f)
            {
                // hotspot.GetComponent<HotSpotController>().isClick = false;
                isClick = false;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        CanCount = true;
        // Debug.Log("Pointer down");
        // hotspot.GetComponent<HotSpotController>().isClick = true;
        isClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CanCount = false;
        timer = 0f;
        // Debug.Log("Pointer up");
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
