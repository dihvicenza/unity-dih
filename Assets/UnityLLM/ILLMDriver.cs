/*
Autore: Fabrizio Radica
Versione: 0.4
Data: 2026-05-11
Descrizione:
Interfaccia comune per i driver LLM.
Permette di intercambiare backend Ollama / LMStudio.
*/

using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILLMDriver
{
    Task<string> SendChatAsync(List<LLMMessage> messages);
}
