using System;
using System.Collections.Generic;

[Serializable]
public class OllamaChatRequest
{
    public string model;

    public List<LLMMessage> messages;

    public bool stream;

    public OllamaOptions options;
}

[Serializable]
public class OllamaOptions
{
    public float temperature;
    public int top_k;
    public float top_p;
    public float repeat_penalty;
    public int num_predict;
}



/*
Autore: Fabrizio Radica
Versione: 0.3
Data: 2026-05-08
Descrizione:
Classi serializzabili per parsing
della risposta JSON di Ollama.
*/

[Serializable]
public class OllamaChatResponse
{
    public string model;
    public string created_at;
    public OllamaResponseMessage message;
    public bool done;
    public string done_reason;
}

[Serializable]
public class OllamaResponseMessage
{
    public string role;
    public string content;
    // FAB v0.3
    // Supporto modelli thinking
    public string thinking;
}