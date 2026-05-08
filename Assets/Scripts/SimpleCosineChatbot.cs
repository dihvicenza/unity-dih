/*
Versione: 2.0
Data: 2026-05-08
Descrizione:
Semplice chatbot con similarità del coseno.

Il sistema:
- confronta la frase utente con un database
- usa cosine similarity
- trova la frase più simile
- restituisce una vera risposta

Obiettivo:
Comprendere il funzionamento base
di un chatbot AI semantico.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCosineChatbot : MonoBehaviour
{
    // =====================================================
    // INPUT UTENTE
    // =====================================================

    [TextArea]
    [SerializeField] string userInput = "parlami di unity";

    // =====================================================
    // DATABASE DOMANDE
    // =====================================================

    string[] database =
    {
        "unity sviluppo videogiochi realtà virtuale",
        "docente unity csharp game design",
        "sviluppo applicazioni vr ar immersive",
        "musei interattivi esperienza virtuale cultura",
        "programmazione gameplay sistemi arcade unity",
        "intelligenza artificiale chatbot locali ai",
        "formazione sviluppo videogame studenti",
        "progettazione ambienti 3d interattivi",
        "sviluppo simulazioni didattiche immersive",
        "consulenza tecnologica unity vr ai",
        "game development prototipazione gameplay",
        "realtà aumentata applicazioni professionali",
        "esperienze immersive museali interattive",
        "csharp unity programmazione professionale",
        "sviluppo configuratori 3d intelligenti",
        "retro gaming sviluppo creatività tecnologia",
        "sistemi ai offline rag chatbot",
        "digitalizzazione patrimonio culturale virtuale",
        "lezioni pratiche unity sviluppo giochi",
        "sviluppo software interattivo realtime"
    };

    // =====================================================
    // DATABASE RISPOSTE
    // =====================================================

    string[] responses =
    {
        "Mi occupo di sviluppo videogiochi e realtà virtuale con Unity.",
        "Insegno Unity, C# e Game Design in corsi professionali.",
        "Realizzo applicazioni immersive VR e AR.",
        "Sviluppo esperienze virtuali per musei e cultura.",
        "Creo sistemi gameplay arcade utilizzando Unity.",
        "Lavoro con chatbot AI e sistemi intelligenti locali.",
        "Formo studenti nello sviluppo di videogiochi.",
        "Progetto ambienti 3D interattivi realtime.",
        "Creo simulazioni immersive per formazione e didattica.",
        "Offro consulenza tecnologica su Unity, VR e AI.",
        "Sviluppo prototipi gameplay e sistemi interattivi.",
        "Realizzo applicazioni professionali in realtà aumentata.",
        "Creo esperienze museali immersive e interattive.",
        "Programmo in C# utilizzando Unity professionalmente.",
        "Sviluppo configuratori 3D intelligenti e dinamici.",
        "Sono appassionato di retrogaming e tecnologia creativa.",
        "Lavoro con sistemi AI offline e RAG locali.",
        "Digitalizzo contenuti culturali tramite tecnologie virtuali.",
        "Realizzo lezioni pratiche sullo sviluppo giochi con Unity.",
        "Sviluppo software interattivi realtime e immersivi."
    };

    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        Debug.Log("INPUT UTENTE:");
        Debug.Log(userInput);

        Debug.Log("----------------------");

        FindBestMatch(userInput);
    }

    // =====================================================
    // TROVA FRASE PIÙ SIMILE
    // =====================================================

    void FindBestMatch(string input)
    {
        float bestScore = -1f;

        int bestIndex = -1;

        // Confronta input con tutto il database
        for (int i = 0; i < database.Length; i++)
        {
            float similarity = CosineSimilarity(input, database[i]);

            //Debug.Log("Confronto con: " + database[i]);
            //Debug.Log("Similarity: " + similarity);

            // Salva il migliore
            if (similarity > bestScore)
            {
                bestScore = similarity;
                bestIndex = i;
            }

            //Debug.Log("----------------------");
        }

        // =====================================================
        // OUTPUT
        // =====================================================


        // Stampa SOLO la risposta finale
        if (bestIndex != -1 && bestScore > 0.1f)
        {
            Debug.Log(responses[bestIndex]);
        }
        else
        {
            Debug.Log("Mi dispiace, non ho trovato una risposta adatta.");
        }
    }

    // =====================================================
    // COSINE SIMILARITY
    // =====================================================

    float CosineSimilarity(string textA, string textB)
    {
        // Tokenizza parole
        string[] wordsA = textA.ToLower().Split(' ');
        string[] wordsB = textB.ToLower().Split(' ');

        // Vocabolario totale
        HashSet<string> vocabulary = new HashSet<string>();

        foreach (string word in wordsA)
        {
            vocabulary.Add(word);
        }

        foreach (string word in wordsB)
        {
            vocabulary.Add(word);
        }

        // Vettori
        List<int> vectorA = new List<int>();
        List<int> vectorB = new List<int>();

        // Costruzione vettori
        foreach (string word in vocabulary)
        {
            int countA = CountWord(wordsA, word);
            int countB = CountWord(wordsB, word);

            vectorA.Add(countA);
            vectorB.Add(countB);
        }

        // Dot Product
        float dotProduct = 0f;

        for (int i = 0; i < vectorA.Count; i++)
        {
            dotProduct += vectorA[i] * vectorB[i];
        }

        // Magnitudine
        float magnitudeA = 0f;
        float magnitudeB = 0f;

        for (int i = 0; i < vectorA.Count; i++)
        {
            magnitudeA += vectorA[i] * vectorA[i];
            magnitudeB += vectorB[i] * vectorB[i];
        }

        magnitudeA = Mathf.Sqrt(magnitudeA);
        magnitudeB = Mathf.Sqrt(magnitudeB);

        // Evita divisione per zero
        if (magnitudeA == 0 || magnitudeB == 0)
        {
            return 0;
        }

        // Formula cosine similarity
        return dotProduct / (magnitudeA * magnitudeB);
    }

    // =====================================================
    // CONTA PAROLA
    // =====================================================

    int CountWord(string[] words, string target)
    {
        int count = 0;

        foreach (string word in words)
        {
            if (word == target)
            {
                count++;
            }
        }

        return count;
    }
}