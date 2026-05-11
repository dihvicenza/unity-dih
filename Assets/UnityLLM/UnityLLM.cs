/*
Autore: Fabrizio Radica
Versione: 0.4
Data: 2026-05-11
Descrizione:
Manager principale per la gestione
della conversation history e richieste LLM.
Supporta driver Ollama e LMStudio selezionabili
da inspector.
*/

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public class UnityLLM : MonoBehaviour
{
    public enum DriverType
    {
        Ollama,
        LMStudio
    }

    [Header("Driver")]
    [SerializeField]
    private DriverType driverType = DriverType.Ollama;

    [Header("Settings")]
    [SerializeField]
    private OllamaSettingsSO ollamaSettings;

    [SerializeField]
    private LMStudioSettingsSO lmStudioSettings;

    private ILLMDriver driver;

    private string activeSystemPrompt;
    private string activePositivePrompt;
    private string activeNegativePrompt;

    private readonly List<LLMMessage> history = new List<LLMMessage>();
 

    private void Awake()
    {
        BuildDriver();
        InitializeSystemPrompt();
    }

    private void BuildDriver()
    {
        switch (driverType)
        {
            case DriverType.LMStudio:

                if (lmStudioSettings == null)
                {
                    Debug.LogError(
                        "LMStudio Settings non assegnati."
                    );
                    return;
                }

                driver =
                    new UnityLMStudioDriver(
                        lmStudioSettings
                    );

                activeSystemPrompt = lmStudioSettings.systemPrompt;

                activePositivePrompt = lmStudioSettings.positivePrompt;

                activeNegativePrompt = lmStudioSettings.negativePrompt;

                break;

            case DriverType.Ollama:
            default:

                if (ollamaSettings == null)
                {
                    Debug.LogError(
                        "Ollama Settings non assegnati."
                    );
                    return;
                }

                driver =
                    new UnityOllamaDriver(
                        ollamaSettings
                    );

                activeSystemPrompt =
                    ollamaSettings.systemPrompt;

                activePositivePrompt =
                    ollamaSettings.positivePrompt;

                activeNegativePrompt =
                    ollamaSettings.negativePrompt;

                break;
        }
    }

    private void InitializeSystemPrompt()
    {
        string finalSystemPrompt =
            activeSystemPrompt;

        if (!string.IsNullOrWhiteSpace(
            activePositivePrompt))
        {
            finalSystemPrompt +=
                "\n\nPositive Prompt:\n" +
                activePositivePrompt;
        }

        if (!string.IsNullOrWhiteSpace(
            activeNegativePrompt))
        {
            finalSystemPrompt +=
                "\n\nNegative Prompt:\n" +
                activeNegativePrompt;
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
        if (driver == null)
        {
            Debug.LogError(
                "Driver non inizializzato."
            );
            return null;
        }

        history.Add(
            new LLMMessage(
                "user",
                prompt
            )
        );

        string response =
            await driver.SendChatAsync(
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
