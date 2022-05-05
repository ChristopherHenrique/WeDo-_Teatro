using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;


public class HttpManager : BaseManager<HttpManager>
{
    const string BASE_URL = "http://apiwedo.com/";
    public string id_evento = "";

    const string GET_URL = "consult/event/";
    const string TICKET = "/ticket/";

    const string POST_URL = "checkin";
    const string LOCALPOST = "http://localhost:3001/checkin";

    const string GET_EVENTOS = "event/list";
    const string LOCALURL = "http://localhost:3001/";

    [SerializeField] TicketInput ticketInput;

    void Start()
    {
        StartCoroutine(RetrieveEventList());
    }

    IEnumerator RetrieveEventList()
    {
        string url = BASE_URL + GET_EVENTOS;
        // Debug.Log(url);
#if UNITY_WEBGL && !UNITY_EDITOR
        if(GameManager.Instance.isSSH())
            url = url.Replace("http", "https");
#endif

        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        unityWebRequest.timeout = 5;

        yield return unityWebRequest.SendWebRequest();


        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sucesso!");
            string response = unityWebRequest.downloadHandler.text;
             Debug.Log(response);
            try
            {
                response = "{\"Items\":" + response + "}";
                Evento[] eventos = JsonHelper.FromJson<Evento>(response);
                List<string> ids = new List<string>();
                foreach (Evento element in eventos)
                {
                    ids.Add(element.event_id.ToString());
                    Debug.Log(element.event_id);
                }
                GameManager.Instance.retrieveAccess(ids);
                ticketInput.GenerateEventList(eventos);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Evento[] eventos = new Evento[0];
                ticketInput.GenerateEventList(eventos);
            }
        }
        else
        {
            Debug.Log("Ocorreu um erro");
            Debug.Log(unityWebRequest.error);
            // ticketInput.ShowMessage("Ocorreu um erro de conexão, tente novamente.");
            Evento[] eventos = new Evento[0];
            ticketInput.GenerateEventList(eventos);
        }

        unityWebRequest.Dispose();
    }


    public void GetTicketData(string ticket)
    {
        string url = BASE_URL + GET_URL + id_evento + TICKET + ticket;
#if UNITY_WEBGL && !UNITY_EDITOR
        if(GameManager.Instance.isSSH())
            url = url.Replace("http", "https");
#endif
        StartCoroutine(GetRequest(url, ticket));
    }

    public void PostCheckin(string ticket)
    {
        string url = BASE_URL + POST_URL;
        Debug.Log(url);
#if UNITY_WEBGL && !UNITY_EDITOR
        if(GameManager.Instance.isSSH())
            url = url.Replace("http", "https");
#endif
        StartCoroutine(postRequest(url, ticket));
    }

    void TreatCheckin(Response response, string ticket)
    {
        switch (response.status)
        {
            case 200:
                DateTime horaEvento = DateTime.Parse(ticketInput.currentEvento.date.Substring(ticketInput.currentEvento.date.Length - 5));
                Debug.Log(ticketInput.currentEvento.date.Substring(ticketInput.currentEvento.date.Length - 5));
                DateTime horaAtual = DateTime.Now;
                TimeSpan timeSpan = horaEvento - horaAtual;
                if (timeSpan.TotalHours < 1)
                {
                    Debug.Log("Ingresso validado com sucesso! Tenha um bom espetáculo!");
                    // GameManager.Instance.SetCanEnter(true);
                    // ticketInput.ShowWelcomeMessage();
                    // PostCheckin(ticket);
                }
                else
                {
                    ticketInput.ShowMessage("A entrada no evento ainda não foi liberada");
                }
                break;
            case 201:
                Debug.Log("Este ingresso já foi utilizado.");
                ticketInput.ShowMessage(response.message);
                break;
            case 400:
                Debug.Log("Token não encontrado");
                ticketInput.ShowMessage(response.message);
                break;
            case 401:
                Debug.Log("Evento não encontrado");
                ticketInput.ShowMessage(response.message);
                break;
            default:
                ticketInput.ShowMessage("Ocorreu um erro, tente novamente mais tarde.");
                break;
        }
    }


    void TreatPostCheckin(Response response)
    {

        switch (response.status)
        {
            case 200:
                DateTime horaEvento = DateTime.Parse(ticketInput.currentEvento.date.Substring(ticketInput.currentEvento.date.Length -5));
                Debug.Log(ticketInput.currentEvento.date.Substring(ticketInput.currentEvento.date.Length - 5));
                DateTime horaAtual = DateTime.Now;
                TimeSpan timeSpan = horaEvento - horaAtual;
                if (timeSpan.TotalHours < 1)
                {
                    Debug.Log("Bem-vindo a sala");
                    GameManager.Instance.saveAccess(id_evento);
                    GameManager.Instance.SetCanEnter(true);
                    ticketInput.ShowWelcomeMessage();
                    // Debug.Log("Ingresso validado com sucesso! Tenha um bom espetáculo!");
                    // GameManager.Instance.SetCanEnter(true);
                    // ticketInput.ShowWelcomeMessage();
                    // PostCheckin(ticket);
                }
                else
                {
                    ticketInput.ShowMessage("A entrada no evento ainda não foi liberada");
                }

                break;
            case 400:
                Debug.Log("Participante com check-in já realizado.");
                ticketInput.ShowMessage(response.message);
                break;
            case 401:
                Debug.Log("Erro de autorização");
                ticketInput.ShowMessage(response.message);
                Debug.Log(response.message);
                break;
            case 404:
                Debug.Log("Participante não encontrado ao tentar fazer o checkin.");
                ticketInput.ShowMessage(response.message);
                break;
            default:
                ticketInput.ShowMessage("Ocorreu um erro, tente novamente mais tarde.");
                break;
        }
    }

    IEnumerator GetRequest(string url, string ticket)
    {
        // Debug.Log(url);
#if UNITY_WEBGL && !UNITY_EDITOR
        if(GameManager.Instance.isSSH())
            url = url.Replace("http", "https");
#endif

        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);

        unityWebRequest.SetRequestHeader("ticket", ticket);
        unityWebRequest.SetRequestHeader("evento", id_evento);

        yield return unityWebRequest.SendWebRequest();


        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sucesso!");
            string response = unityWebRequest.downloadHandler.text;
            // Debug.Log(response);
            try
            {
                Response resposta = JsonUtility.FromJson<Response>(response);
                TreatCheckin(resposta, ticket);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                ticketInput.ShowMessage(response);
            }
        }
        else
        {
            Debug.Log("Ocorreu um erro");
            Debug.Log(unityWebRequest.error);
            ticketInput.ShowMessage("Ocorreu um erro de conexão, tente novamente.");
        }

        unityWebRequest.Dispose();

        // Dictionary<string, string> headers = unityWebRequest.GetResponseHeaders();
        // foreach (KeyValuePair<string, string> element in headers)
        // {
        //     Debug.Log("Key = " + element.Key + ", Value = " + element.Value);
        // }

    }

    IEnumerator postRequest(string url, string ticket)
    {
        // Debug.Log(url);
#if UNITY_WEBGL && !UNITY_EDITOR
        // if(GameManager.Instance.isSSH())
        //     url = url.Replace("http", "https");
#endif

        string json = $"{{\"ticket\": \"{ticket}\", \"event\": \"{id_evento}\"}}";
        // Debug.Log(json);

        UnityWebRequest unityWebRequest = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");

        yield return unityWebRequest.SendWebRequest();

        bodyRaw = null;
        string response = unityWebRequest.downloadHandler.text;
        Debug.Log(response);

        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sucesso!");
            Response resposta = JsonUtility.FromJson<Response>(response);
            // Debug.Log(resposta.message);
            TreatPostCheckin(resposta);
        }
        else
        {
            Debug.Log("Ocorreu um erro");
            Debug.Log(unityWebRequest.error);
            ticketInput.ShowMessage("Ocorreu um erro de conexão, tente novamente.");
        }


        unityWebRequest.Dispose();

    }
}
