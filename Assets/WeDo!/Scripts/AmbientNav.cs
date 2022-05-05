using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbientNav
{
    public GameObject ambientToShow { get; private set; }
    public GameObject ambientToHide { get; private set; }
    public float angleX { get; private set; }
    public float angleY { get; private set; }
    public float cameraHeight { get; private set; }
    public Transform canvas { get; private set; }
    public AudioClip music { get; private set; }

    public AmbientNav()
    {

    }

    public AmbientNav(GameObject ambientToShow, GameObject ambientToHide, float angleX, float angleY, float cameraHeight, Transform canvas, AudioClip music)
    {
        this.ambientToShow = ambientToShow;
        this.ambientToHide = ambientToHide;
        this.angleX = angleX;
        this.angleY = angleY;
        this.cameraHeight = cameraHeight;
        this.canvas = canvas;
        this.music = music;
    }
}
