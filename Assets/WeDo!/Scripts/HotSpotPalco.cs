using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotSpotPalco : MonoBehaviour
{
    [SerializeField] string Interacao = "";
    TextMeshProUGUI TxtName;
    Transform CameraParentPosition;

    Button button;

    [SerializeField] GameManager.Frame frame;
    [SerializeField] bool CanRotate = true;

    void Start()
    {
        TxtName = transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (TxtName != null)
            TxtName.text = Interacao;

        button = transform.parent.GetChild(2).GetComponent<Button>();
        button.onClick.AddListener(Btnclick);



        CameraParentPosition = Camera.main.transform.parent;
        Transform parent = transform.parent;
        parent.LookAt(CameraParentPosition);
        parent.Rotate(0, 180, 0);

    }

    void Btnclick()
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        if (!button.GetComponent<ButtonHover>().isClick)
            return;
        string a = frame.ToString();
        Transform canvas = transform.parent;
        // Debug.Log(a);

        // if (frame == GameManager.Frame.bilheteria)
        //     GameManager.Instance.ClickBilheteria();
        // else
            GameManager.Instance.FrameControlOpen(a, canvas);
    #endif

    }
}
