using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientZoom : MonoBehaviour
{
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.3f;
    Vector3 originalPos;
    Transform transformPosition;
    bool isZoomedIn = false;
    [SerializeField] Transform CameraZoom;

}
