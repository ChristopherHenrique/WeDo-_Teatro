using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoyerClick : MonoBehaviour
{
    [SerializeField] GameObject FoyerToRotate;
    [SerializeField] Quaternion rotation;
    Button button;

    void Start() 
    {
        button.onClick.AddListener(Btnclick);
    }

    void Btnclick()
    {
        bool isClick = button.GetComponent<ButtonHover>().isClick;
        Debug.Log(isClick);
        if (!isClick)
            return;
        
        FoyerToRotate.transform.rotation = rotation;
        
    }
}
