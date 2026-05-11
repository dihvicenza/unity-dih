/*
Autore: Fabrizio Radica
Versione: 0.4
Data: 2026-05-11
Descrizione:
ScriptableObject per la configurazione
del driver LMStudio (API stile OpenAI).
*/

using UnityEngine;

[CreateAssetMenu(
    fileName = "LMStudioSettings",
    menuName = "RadicaDesign/LLM/LMStudio Settings"
)]
public class LMStudioSettingsSO : ScriptableObject
{
    [Header("Connection")]
    public string baseUrl = "http://localhost:1234";

    public string endpoint = "/v1/chat/completions";

    // FAB v0.4
    // Lasciare vuoto per LMStudio locale.
    // Necessario solo se si punta ad endpoint
    // OpenAI-compatibili che richiedono auth.
    public string apiKey = "";

    [Header("Model")]
    public string modelName = "local-model";

    [Header("Prompts")]

    [TextArea(5, 20)]
    public string systemPrompt;

    [TextArea(3, 10)]
    public string positivePrompt;

    [TextArea(3, 10)]
    public string negativePrompt;

    [Header("Generation Parameters")]

    [Range(0f, 2f)]
    public float temperature = 0.7f;

    [Range(0f, 1f)]
    public float top_p = 0.95f;

    [Range(0f, 100f)]
    public float top_k = 65;

    [Range(-2f, 2f)]
    public float frequency_penalty = 0f;

    [Range(-2f, 2f)]
    public float presence_penalty = 0f;

    public int max_tokens = 1024;

    [Header("Request")]
    public bool stream = false;

    public int timeout = 120;
}
