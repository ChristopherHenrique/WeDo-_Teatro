using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    [DllImport("__Internal")]
    private static extern void SetPointerCursor(string cursorType);

    [DllImport("__Internal")]
    private static extern string SetDefaultCursor(string cursorType);
    [DllImport("__Internal")]
    private static extern string OpenFrame(string frame);
    [DllImport("__Internal")]
    private static extern string CloseFrame(string frame);

    [DllImport("__Internal")]
    private static extern void PrintMessage(string message);
    [DllImport("__Internal")]
    public static extern bool CheckIsMobile();

    [DllImport("__Internal")]
    private static extern void OpenURL(string url);
    [DllImport("__Internal")]
    private static extern void OpenInput();
    [DllImport("__Internal")]
    private static extern void CloseInput();
    [DllImport("__Internal")]
    private static extern void OpenLink(string url);
    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    [DllImport("__Internal")]
    private static extern bool checkSSH();



    public const string DEFAULTCURSOR = "default";
    public const string POINTERCURSOR = "pointer";
    public const string FRAMEBILHETERIA = "bilheteria";
    public const string FRAMEPALCO = "palco";
    const string HOMEURL = "https://www.wedoentretenimento.com/";
    const string INSTAGRAMURL = "https://instagram.com/bendittapipoca?igshid=yo9kkler83o1";
    const string URLBILHETERIA = "https://site.bileto.sympla.com.br/teatrowedo/";

    const string HOME = "home";
    const string PIPOQUEIRO = "pipoqueiro";

    const string ACCESSPLAYERPREFSKEY = "acesso";

    const string ADMINPASSWORD = "my_password";

    public bool CanEnter { get; private set; } = false;

    Transform cameraParent;

    [SerializeField] Transform Ambients;
    [SerializeField] Image ImgFade;

    [SerializeField] GameObject Framebg;

    [SerializeField] Button BtnHome;

    [SerializeField] GameObject OrientationWarning;
    [SerializeField] GameObject TermsAndConditions;
    [SerializeField] GameObject SensibilityWarning;
    [SerializeField] Button BtnAcceptTerms;
    [SerializeField] Button BtnSensibility;
    [SerializeField] Button BtnInstagram;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TicketInput ticketInput;

    string currentFrame = "";

    Button BtnCloseFrame;


    public enum Frame
    {
        bilheteria,
        palco,
        loja
    }

    void Start()
    {
        TermsAndConditions.transform.localScale = Vector3.zero;
        SensibilityWarning.transform.localScale = Vector3.zero;
        BtnAcceptTerms.interactable = false;
        cameraParent = Camera.main.transform.parent;
        Color tempColor = ImgFade.color;
        tempColor.a = 0f;
        ImgFade.color = tempColor;
        foreach (Transform element in Ambients)
        {
            if (element.gameObject.activeInHierarchy)
            {
                float angleX = element.GetComponent<Ambient>().GetXAngle();
                float angleY = element.GetComponent<Ambient>().GetYAngle();
                float cameraHeight = element.GetComponent<Ambient>().GetCameraHeight();
                cameraParent.GetComponent<VRCameraController>().LimitAngle(angleX, angleY);
                cameraParent.GetComponent<VRCameraController>().SetCameraHeight(cameraHeight);
                AudioClip music = element.GetComponent<Ambient>().GetMusic();
                SoundManager.Instance.ChangeMusic(music);
            }
        }
        ChangeMouseCursor(DEFAULTCURSOR);
        Framebg.SetActive(false);

        ImgFade.raycastTarget = true;

        BtnCloseFrame = Framebg.GetComponent<Button>();
        BtnCloseFrame.onClick.AddListener(delegate { BtnFrameClose(currentFrame); });

        BtnHome.onClick.AddListener(() =>
        {
            Application.OpenURL(HOMEURL);
        });
        BtnSensibility.onClick.AddListener(BtnWarningClick);
        BtnAcceptTerms.onClick.AddListener(AcceptTermsAndConditions);

        BtnInstagram.onClick.AddListener(() =>
        {
            OpenLink(INSTAGRAMURL);
        });

        Intro();
    }

    void saveKey(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }
    string loadKey(string key)
    {
        if(PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);
        else
            return "";
    }

    public void retrieveAccess(List<string> ids) 
    {
        // saveKey(ACCESSPLAYERPREFSKEY, "1226858");
        string idEvento = loadKey(ACCESSPLAYERPREFSKEY);
        // string idEvento = "1226858";
        foreach(string element in ids)
        {
            if(idEvento.Equals(element))
                CanEnter = true;
        }
    }
    public void saveAccess(string idEvento)
    {
        saveKey(ACCESSPLAYERPREFSKEY, idEvento);
    }

    public void ClickBilheteria()
    {
        // Debug.Log("Clique bilheteria");
        // OpenLink(URLBILHETERIA);
#if UNITY_WEBGL && !UNITY_EDITOR 
        OpenLink(URLBILHETERIA);
#endif

    }

    private void BtnWarningClick()
    {
        LeanTween.scale(SensibilityWarning, Vector3.zero, 0.3f).setOnComplete(() =>
        {
            SensibilityWarning.SetActive(false);
            LeanTween.scale(TermsAndConditions, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
            {

                BtnAcceptTerms.interactable = true;
                scrollRect.enabled = true;
            });
            // cameraParent.GetComponent<VRCameraController>().rotationEnabled = true;
            // ImgFade.raycastTarget = false;
        });
    }

    void Intro()
    {
        scrollRect.enabled = false;

        Color tempColor = Color.black;
        tempColor.a = 0.75f;
        ImgFade.color = tempColor;
        BtnSensibility.interactable = false;
        LeanTween.scale(SensibilityWarning, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
                    {
                        BtnSensibility.interactable = true;
                    });
    }


    void FixedUpdate()
    {
        CheckOrientation();
    }

    void CheckOrientation()
    {
        if (Screen.width < Screen.height)
        {
            if (!OrientationWarning.activeInHierarchy)
            {
                OrientationWarning.SetActive(true);
                cameraParent.GetComponent<VRCameraController>().rotationEnabled = false;
            }
        }
        else
        {
            if (OrientationWarning.activeInHierarchy)
            {
                OrientationWarning.SetActive(false);
                cameraParent.GetComponent<VRCameraController>().rotationEnabled = true;
            }
        }
    }

    void AcceptTermsAndConditions()
    {
        Color tempColorNoAlpha = Color.black;
        tempColorNoAlpha.a = 0f;
        ImgFade.color = tempColorNoAlpha;
        LeanTween.scale(TermsAndConditions, Vector3.zero, 0.3f).setOnComplete(() =>
        {
            TermsAndConditions.SetActive(false);
            cameraParent.GetComponent<VRCameraController>().rotationEnabled = true;
            ImgFade.raycastTarget = false;
        });
    }

    public void ChangeAmbient(GameObject ambientToShow, GameObject ambientToHide, float angleX, float angleY, float cameraHeight, Transform canvas, AudioClip music)
    {
        ImgFade.raycastTarget = true;
        cameraParent.GetComponent<VRCameraController>().rotationEnabled = false;
        Transform camera = Camera.main.transform;
        Vector3 rotation = canvas.eulerAngles;
        LeanTween.rotateLocal(cameraParent.gameObject, rotation, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeOutCirc).setOnComplete(() =>
        {
            float distance = (Vector3.Distance(camera.position, canvas.position)) * 0.8f;
            Debug.Log(distance);

            LeanTween.moveLocalZ(camera.gameObject, distance, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCirc).setOnComplete(() =>
            {
                LeanTween.alpha(ImgFade.GetComponent<RectTransform>(), 1, 0.2f).setOnComplete(() =>
                {
                    cameraParent.GetComponent<VRCameraController>().resetCamera();
                    cameraParent.GetComponent<VRCameraController>().LimitAngle(angleX, angleY);
                    cameraParent.GetComponent<VRCameraController>().SetCameraHeight(cameraHeight);
                    ambientToHide.SetActive(false);
                    ambientToShow.SetActive(true);
                    ChangeMouseCursor(DEFAULTCURSOR);
                    LeanTween.alpha(ImgFade.GetComponent<RectTransform>(), 0, 0.2f).setOnComplete(() =>
                    {
                        cameraParent.GetComponent<VRCameraController>().rotationEnabled = true;
                        ImgFade.raycastTarget = false;
                        SoundManager.Instance.ChangeMusic(music);
                    });
                });
            });
        });
    }

    public void EnterTheater(GameObject ambientToShow, GameObject ambientToHide, float angleX, float angleY, float cameraHeight, Transform canvas, AudioClip music)
    {
        if (!CanEnter)
        {
            AmbientNav ambientNav = new AmbientNav(ambientToShow, ambientToHide, angleX, angleY, cameraHeight, canvas, music);
            // ticketInput.ShowInput(ambientNav);
            cameraParent.GetComponent<VRCameraController>().rotationEnabled = false;
            ImgFade.raycastTarget = true;
            Color tempColorAlpha = Color.black;
            tempColorAlpha.a = 0.6f;
            ImgFade.color = tempColorAlpha;

            AmbientNav tempNav = new AmbientNav(ambientToShow, ambientToHide, angleX, angleY, cameraHeight, canvas, music);
            ticketInput.setTempNav(tempNav);
            ticketInput.ShowEventSelection();
        }
        else
            ChangeAmbient(ambientToShow, ambientToHide, angleX, angleY, cameraHeight, canvas, music);
    }

    public void ShowHtmlInput()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLInput.captureAllKeyboardInput = false;
        OpenInput();
#elif UNITY_EDITOR
        ticketInput.ShowInput();
#endif
    }

    public void SetCanEnter(bool status)
    {
        CanEnter = status;
    }

    public void ChangeMouseCursor(string cursor)
    {
        try
        {
            SetPointerCursor(cursor);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void FrameControlOpen(string frame, Transform canvas)
    {
        currentFrame = frame;

        cameraParent.GetComponent<VRCameraController>().rotationEnabled = false;
        ImgFade.raycastTarget = true;
        Transform camera = Camera.main.transform;
        Vector3 rotation = canvas.eulerAngles;
        // Debug.Log("Camera X == " + cameraParent.eulerAngles.x);
        // Debug.Log("Camera Y == " + cameraParent.eulerAngles.y);
        LeanTween.rotateLocal(cameraParent.gameObject, rotation, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeOutCirc).setOnComplete(() =>
        {
            try
            {
                float newX = cameraParent.eulerAngles.x;
                float newY = cameraParent.eulerAngles.y;
                if (newY > 180f)
                    newY -= 360f;
                if (newY < -180f)
                    newY += 360f;
                if (newX > 180f)
                    newX -= 360f;
                if (newX < -180f)
                    newX += 360f;

                newX *= -1;
                cameraParent.GetComponent<VRCameraController>().SaveNewRotation(newY, newX);
                OpenFrame(frame);
                Framebg.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        });
    }

    public void FrameControlClose(string frame)
    {
        try
        {
            ImgFade.raycastTarget = false;
            cameraParent.GetComponent<VRCameraController>().rotationEnabled = true;
            Framebg.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void BtnFrameClose(string frame)
    {
        try
        {
            CloseFrame(frame);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void CloseInputWindow(string ticketSent)
    {
#if UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = true;
#endif
        if (String.IsNullOrEmpty(ticketSent))
        {
            Color tempColorNoAlpha = Color.black;
            tempColorNoAlpha.a = 0f;
            ImgFade.color = tempColorNoAlpha;
            ImgFade.raycastTarget = false;
            cameraParent.GetComponent<VRCameraController>().rotationEnabled = true;
        }
    }

    public void hideFrameBG()
    {
        ImgFade.raycastTarget = false;
    }

    public void RetrieveTicket(string ticket)
    {
        // Debug.Log(ticket);
        HttpManager.Instance.PostCheckin(ticket);
    }

    public void PopupBlocked()
    {
        Debug.Log("Popup bloqueada");
        ticketInput.ShowMessage("A janela da bilheteria foi bloqueada.\nPor favor, desabilite o bloqueador de popups do seu navegador.");
    }

    public Boolean isSSH()
    {
        return checkSSH();
    }
}
