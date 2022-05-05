using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour
{
    [SerializeField] float MaxAngleX = 360f;
    [SerializeField] float MaxAngleY = 90f;
    [SerializeField] float CameraHeight = 1.2f;
    [SerializeField] AudioClip Music;
    
    public float GetXAngle()
    {
        return MaxAngleX;
    }
    public float GetYAngle()
    {
        return MaxAngleY;
    }

    public float GetCameraHeight()
    {
        return CameraHeight;
    }

    public AudioClip GetMusic()
    {
        return Music;
    }
}
