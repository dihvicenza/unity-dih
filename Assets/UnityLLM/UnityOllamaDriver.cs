/*
Autore: Fabrizio Radica
Versione: 0.3
Data: 2026-05-08
Descrizione:
Driver base per la comunicazione con Ollama.
Gestisce esclusivamente la richiesta HTTP.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class UnityOllamaDriver : ILLMDriver
{
    private readonly OllamaSettingsSO settings;

    public UnityOllamaDriver(OllamaSettingsSO settings)
    {
        this.settings = settings;
    }

    public async Task<string> SendChatAsync(List<LLMMessage> messages)
    {
        string url = settings.baseUrl + settings.endpoint;

        OllamaChatRequest requestData = new OllamaChatRequest
            {
                model = settings.modelName,
                messages = messages,

                // FAB v0.3
                // Stream OFF per semplicità
                stream = false,

                options = new OllamaOptions
                {
                    temperature =
                        settings.temperature,

                    top_k =
                        settings.top_k,

                    top_p =
                        settings.top_p,

                    repeat_penalty =
                        settings.repeat_penalty,

                    num_predict =
                        settings.num_predict
                }
            };

        string jsonBody = JsonUtility.ToJson(requestData);

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

        request.timeout = settings.timeout;

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

        OllamaChatResponse response =
            JsonUtility.FromJson<OllamaChatResponse>(
                jsonResponse
            );

        if (response == null)
        {
            Debug.LogError(
                "Response NULL"
            );

            return null;
        }

        if (response.message == null)
        {
            Debug.LogError(
                "Message NULL"
            );

            return null;
        }

        // FAB v0.3
        // Risposta standard
        if (!string.IsNullOrWhiteSpace(
            response.message.content))
        {
            return response.message.content;
        }

        // FAB v0.3
        // Debug modelli thinking
        if (!string.IsNullOrWhiteSpace(
            response.message.thinking))
        {
            Debug.LogWarning(
                "Il modello ha restituito THINKING."
            );

            Debug.Log(
                response.message.thinking
            );

            return null;
        }

        return null;
    }
}
