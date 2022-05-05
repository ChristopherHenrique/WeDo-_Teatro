using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotspotPipoca : MonoBehaviour
{

    Transform CameraParentPosition;
    // Start is called before the first frame update
    void Start()
    {
        CameraParentPosition = Camera.main.transform.parent;
        Transform parent = transform.parent;
        // parent.LookAt(CameraParentPosition);
        // parent.Rotate(0, 180, 0);

        float posY = gameObject.GetComponent<RectTransform>().anchoredPosition.y + 0.15f;

        LeanTween.moveY(gameObject.GetComponent<RectTransform>(), posY, 1.2f).setEase(LeanTweenType.easeInQuad).setDelay(1f).setLoopPingPong(-1);
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 1f, 1.2f).setDelay(1f).setLoopPingPong(-1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
