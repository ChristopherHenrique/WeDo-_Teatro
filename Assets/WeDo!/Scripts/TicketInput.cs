using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using System.Globalization;

public class TicketInput : MonoBehaviour
{
    [SerializeField] Button btnTicket;
    [SerializeField] Button btnConfirm;
    [SerializeField] Button btnConfirmWelcome;
    [SerializeField] Button btnConfirmEvento;
    [SerializeField] TMP_InputField userInput;

    [SerializeField] GameObject inputWindow;
    [SerializeField] GameObject messageWindow;
    [SerializeField] GameObject welcomeWindow;
    [SerializeField] GameObject eventWindow;
    [SerializeField] Image ImgFade;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TextMeshProUGUI txtEventos;

    Transform cameraParent;
    CanvasGroup canvas;

    [SerializeField] TextMeshProUGUI TxtMessage;

    public AmbientNav tempAmbient { get; private set; }

    Evento[] eventos;
    public Evento currentEvento { get; private set; }

    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        btnTicket.onClick.AddListener(InputTicket);
        btnConfirm.onClick.AddListener(BtnConfirmClick);
        btnConfirmWelcome.onClick.AddListener(BtnWelcomeClick);
        btnConfirmEvento.onClick.AddListener(btnEventClick);
        this.transform.localScale = Vector3.zero;
        canvas.blocksRaycasts = false;
        btnTicket.interactable = false;
        userInput.interactable = false;
        btnConfirm.interactable = false;
        cameraParent = Camera.main.transform.parent;
    }

    public void GenerateEventList(Evento[] eventos)
    {
        this.eventos = eventos;
        List<Evento> listEventos = new List<Evento>();
        listEventos = eventos.OfType<Evento>().ToList<Evento>();
        // Debug.Log(listEventos.Count);

        List<String> listOptions = new List<string>();
        foreach (Evento element in listEventos)
        {
            
            string a = element.name + ", " + element.date;
             Debug.Log(a);
            listOptions.Add(a);
        }

        Debug.Log(listOptions.Count);
        if (listOptions.Count > 0)
        {
            txtEventos.text = "Escolha seu evento na lista abaixo";
            dropdown.gameObject.SetActive(true);
            dropdown.ClearOptions();
            dropdown.AddOptions(listOptions);
        }
        else
        {
            txtEventos.text = "Nenhum evento cadastrado no momento.";
            dropdown.gameObject.SetActive(false);
        }
    }

    void BtnConfirmClick()
    {
        HideWindow(true);
    }
    void BtnWelcomeClick()
    {
        HideWindow(true, true);
    }

    void btnEventClick()
    {
        if (dropdown.options.Count > 0)
        {
            dropdown.interactable = false;
            btnConfirmEvento.interactable = false;

            int option = dropdown.value;
            currentEvento = eventos[option];
            string id = currentEvento.event_id.ToString();
            HttpManager.Instance.id_evento = id;
            HideWindow(false, false, GameManager.Instance.ShowHtmlInput);
        }
        else
        {
            HideWindow(true);
        }
    }

    void InputTicket()
    {
        btnTicket.interactable = false;
        userInput.interactable = false;
        string ticket = userInput.text;
        HttpManager.Instance.PostCheckin(ticket);
        HideWindow();
    }

    void HideWindow(bool hideAll = false, bool isEnterTheater = false, System.Action callback = null)
    {
        canvas.blocksRaycasts = false;
        btnTicket.interactable = false;
        userInput.interactable = false;
        btnConfirm.interactable = false;
        LeanTween.scale(this.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
                    {
                        if (hideAll)
                        {
                            Color tempColorNoAlpha = Color.black;
                            tempColorNoAlpha.a = 0f;
                            ImgFade.color = tempColorNoAlpha;
                            ImgFade.raycastTarget = false;
                            if (!isEnterTheater)
                                cameraParent.GetComponent<VRCameraController>().rotationEnabled = true;
                            else
                            {
                                GameManager.Instance.ChangeAmbient(tempAmbient.ambientToShow, tempAmbient.ambientToHide, tempAmbient.angleX, tempAmbient.angleY, tempAmbient.cameraHeight, tempAmbient.canvas, tempAmbient.music);
                            }

                        }
                        callback?.Invoke();
                    });
    }

    public void ShowInput()
    {
        userInput.text = "";
        // tempAmbient = ambientNav;
        cameraParent.GetComponent<VRCameraController>().rotationEnabled = false;
        inputWindow.SetActive(true);
        messageWindow.SetActive(false);
        welcomeWindow.SetActive(false);
        eventWindow.SetActive(false);

        ImgFade.raycastTarget = true;
        Color tempColor = Color.black;
        tempColor.a = 0.75f;
        ImgFade.color = tempColor;

        LeanTween.scale(this.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
                    {
                        btnTicket.interactable = true;
                        userInput.interactable = true;
                        canvas.blocksRaycasts = true;
                    });

    }

    public void setTempNav(AmbientNav ambientNav)
    {
        tempAmbient = ambientNav;
    }

    void setMessage(string message)
    {
        TxtMessage.text = message;
    }

    public void ShowMessage(string message)
    {
        if (cameraParent.GetComponent<VRCameraController>().rotationEnabled == true)
        {
            cameraParent.GetComponent<VRCameraController>().rotationEnabled = false;
            ImgFade.raycastTarget = true;
            Color tempColor = Color.black;
            tempColor.a = 0.75f;
            ImgFade.color = tempColor;
        }

        setMessage(message);
        btnConfirm.interactable = false;
        inputWindow.SetActive(false);
        messageWindow.SetActive(true);
        welcomeWindow.SetActive(false);
        eventWindow.SetActive(false);
        LeanTween.scale(this.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
                    {
                        btnConfirm.interactable = true;
                        canvas.blocksRaycasts = true;
                    });
    }

    public void ShowWelcomeMessage()
    {
        btnConfirmWelcome.interactable = false;
        inputWindow.gameObject.SetActive(false);
        messageWindow.gameObject.SetActive(false);
        eventWindow.SetActive(false);
        welcomeWindow.SetActive(true);

        LeanTween.scale(this.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
                    {
                        btnConfirmWelcome.interactable = true;
                        canvas.blocksRaycasts = true;
                    });
    }

    public void ShowEventSelection()
    {
        btnConfirmEvento.interactable = false;
        dropdown.interactable = false;
        inputWindow.SetActive(false);
        messageWindow.gameObject.SetActive(false);
        eventWindow.SetActive(true);
        welcomeWindow.SetActive(false);

        LeanTween.scale(this.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
                            {
                                btnConfirmEvento.interactable = true;
                                dropdown.interactable = true;
                                canvas.blocksRaycasts = true;
                            });
    }
}
