/*
Autore: Fabrizio Radica
Versione: 0.3
Data: 2026-05-08
Descrizione:
Classe dati serializzabile per rappresentare
un messaggio compatibile con Ollama /api/chat.
*/

using System;

[Serializable]
public class LLMMessage
{
    public string role;
    public string content;

    public LLMMessage(string role, string content)
    {
        this.role = role;
        this.content = content;
    }
}