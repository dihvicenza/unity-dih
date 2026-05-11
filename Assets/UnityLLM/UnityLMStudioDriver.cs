/*
Autore: Fabrizio Radica
Versione: 0.4
Data: 2026-05-11
Descrizione:
Driver per la comunicazione con LMStudio
tramite API stile OpenAI (/v1/chat/completions).
Gestisce esclusivamente la richiesta HTTP.
*/

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class UnityLMStudioDriver : ILLMDriver
{
    private readonly LMStudioSettingsSO settings;

    public UnityLMStudioDriver(LMStudioSettingsSO settings)
    {
        this.settings = settings;
    }

    public async Task<string> SendChatAsync(List<LLMMessage> messages)
    {
        string url =
            settings.baseUrl + settings.endpoint;

        OpenAIChatRequest requestData =
            new OpenAIChatRequest
            {
                model = settings.modelName,
                messages = messages,

                // FAB v0.4
                // Stream OFF per semplicità
                stream = false,

                temperature =
                    settings.temperature,

                top_p =
                    settings.top_p,

                max_tokens =
                    settings.max_tokens,

                frequency_penalty =
                    settings.frequency_penalty,

                presence_penalty =
                    settings.presence_penalty
            };

        string jsonBody =
            JsonUtility.ToJson(requestData);

        Debug.Log(jsonBody);

        using UnityWebRequest request =
            new UnityWebRequest(url, "POST");

        byte[] bodyRaw =
            Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler =
            new UploadHandlerRaw(bodyRaw);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        // FAB v0.4
        // LMStudio locale non richiede auth.
        // Header presente solo per compatibilità
        // con altri endpoint OpenAI-style.
        if (!string.IsNullOrWhiteSpace(
            settings.apiKey))
        {
            request.SetRequestHeader(
                "Authorization",
                "Bearer " + settings.apiKey
            );
        }

        request.timeout =
            settings.timeout;

        UnityWebRequestAsyncOperation operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        string jsonResponse =
            request.downloadHandler.text;

        Debug.Log(jsonResponse);

        OpenAIChatResponse response =
            JsonUtility.FromJson<OpenAIChatResponse>(
                jsonResponse
            );

        if (response == null)
        {
            Debug.LogError(
                "Response NULL"
            );

            return null;
        }

        if (response.choices == null ||
            response.choices.Count == 0)
        {
            Debug.LogError(
                "Choices NULL/empty"
            );

            return null;
        }

        OpenAIResponseMessage message =
            response.choices[0].message;

        if (message == null)
        {
            Debug.LogError(
                "Message NULL"
            );

            return null;
        }

        if (!string.IsNullOrWhiteSpace(
            message.content))
        {
            return message.content;
        }

        return null;
    }
}
