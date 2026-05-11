using TMPro;
using UnityEngine;

public class ChatUI : MonoBehaviour
{
    [SerializeField] UnityLLM llm;
    [SerializeField] TMP_Text chat;
    [SerializeField] TMP_Text chatresponse;

    bool isWaiting;
    public async void Send()
    {
        if (isWaiting) return;

        isWaiting = true;
        chatresponse.text = "Thinking...";

        try
        {
            string response = await llm.AskAsync(chat.text);
            chatresponse.text = response;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            chatresponse.text = "Errore richiesta LLM";
        }

        isWaiting = false;
    }
}