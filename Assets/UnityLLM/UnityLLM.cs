/*
Autore: Fabrizio Radica
Versione: 0.3
Data: 2026-05-08
Descrizione:
Manager principale per la gestione
della conversation history e richieste LLM.
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnityLLM : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private OllamaSettingsSO ollamaSettings;

    private UnityOllamaDriver ollamaDriver;

    private readonly List<LLMMessage> history =
        new List<LLMMessage>();

    private void Awake()
    {
        ollamaDriver =
            new UnityOllamaDriver(
                ollamaSettings
            );

        InitializeSystemPrompt();
    }

    private void InitializeSystemPrompt()
    {
        string finalSystemPrompt =
            ollamaSettings.systemPrompt;

        if (!string.IsNullOrWhiteSpace(
            ollamaSettings.positivePrompt))
        {
            finalSystemPrompt +=
                "\n\nPositive Prompt:\n" +
                ollamaSettings.positivePrompt;
        }

        if (!string.IsNullOrWhiteSpace(
            ollamaSettings.negativePrompt))
        {
            finalSystemPrompt +=
                "\n\nNegative Prompt:\n" +
                ollamaSettings.negativePrompt;
        }

        history.Add(
            new LLMMessage(
                "system",
                finalSystemPrompt
            )
        );
    }

    public async Task<string> AskAsync(
        string prompt)
    {
        history.Add(
            new LLMMessage(
                "user",
                prompt
            )
        );

        string response =
            await ollamaDriver.SendChatAsync(
                history
            );

        if (string.IsNullOrWhiteSpace(
            response))
        {
            return null;
        }

        history.Add(
            new LLMMessage(
                "assistant",
                response
            )
        );

        return response;
    }

    public void ClearHistory()
    {
        history.Clear();

        InitializeSystemPrompt();
    }

    public List<LLMMessage> GetHistory()
    {
        return history;
    }
}