using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "RadicaDesign/LLM/NPCData")]
public class NPCData : ScriptableObject
{
        public int ID;
        public string Name;

        [TextArea(5, 10)]
        public string prompt;
}
