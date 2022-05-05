using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HotSpotTeatro : MonoBehaviour
{
    [SerializeField] GameObject AmbientToShow;
    GameObject AmbientToHide;
    [SerializeField] string AmbientToShowName;
    TextMeshProUGUI TxtName;
    Transform CameraParentPosition;

    Button button;
    // public bool isClick = true;

    void Start()
    {
        TxtName = transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (!string.IsNullOrEmpty(AmbientToShowName) && TxtName != null)
            TxtName.text = AmbientToShowName;
        else
            TxtName.text = "";

        AmbientToHide = transform.parent.parent.gameObject;

        CameraParentPosition = Camera.main.transform.parent;
        Transform parent = transform.parent;
        parent.LookAt(CameraParentPosition);
        parent.Rotate(0, 180, 0);

        button = transform.parent.GetChild(2).GetComponent<Button>();
        button.onClick.AddListener(Btnclick);

        float posY = gameObject.GetComponent<RectTransform>().anchoredPosition.y + 0.15f;

        LeanTween.moveY(gameObject.GetComponent<RectTransform>(), posY, 1.2f).setEase(LeanTweenType.easeInQuad).setDelay(1f).setLoopPingPong(-1);
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 1f, 1.2f).setDelay(1f).setLoopPingPong(-1);
    }

    void Btnclick()
    {
        bool isClick = button.GetComponent<ButtonHover>().isClick;
        // Debug.Log(isClick);
        if (!isClick)
            return;
        float angleX = AmbientToShow.GetComponent<Ambient>().GetXAngle();
        float angleY = AmbientToShow.GetComponent<Ambient>().GetYAngle();
        float cameraHeight = AmbientToShow.GetComponent<Ambient>().GetCameraHeight();
        AudioClip music = AmbientToShow.GetComponent<Ambient>().GetMusic();

        Transform canvas = transform.parent;

        GameManager.Instance.EnterTheater(AmbientToShow, AmbientToHide, angleX, angleY, cameraHeight, canvas, music);
    }
}
