using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spotAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveY( gameObject, 1f, 1f).setEase( LeanTweenType.easeInQuad ).setDelay(1f).setLoopPingPong(-1);
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0.5f, 1f).setDelay(1f).setLoopPingPong(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
