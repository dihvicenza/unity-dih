/*
Autore: Fabrizio Radica
Versione: 0.3
Data: 2026-05-08
Descrizione:
ScriptableObject per la configurazione
del driver Ollama.
*/

using UnityEngine;

[CreateAssetMenu(
    fileName = "OllamaSettings",
    menuName = "RadicaDesign/LLM/Ollama Settings"
)]
public class OllamaSettingsSO : ScriptableObject
{
    [Header("Connection")]
    public string baseUrl = "http://localhost:11434";

    public string endpoint = "/api/chat";

    [Header("Model")]
    public string modelName = "gemma4:e2b";

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

    [Range(1, 100)]
    public int top_k = 40;

    [Range(0f, 1f)]
    public float top_p = 0.9f;

    [Range(0f, 5f)]
    public float repeat_penalty = 1.1f;

    // FAB v0.3
    // Evita truncation Gemma
    public int num_predict = 1024;

    [Header("Request")]
    public bool stream = false;

    public int timeout = 120;
}