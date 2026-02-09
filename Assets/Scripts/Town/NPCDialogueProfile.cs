using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "City/NPC Dialogue Profile", fileName = "NPCDialogueProfile")]
public class NPCDialogueProfile : ScriptableObject
{
    [Serializable]
    public class DialogueSet
    {
        public GameProgressState state;
        [TextArea(2, 5)]
        public List<string> lines = new List<string>();
    }

    public string npcName = "Villager";
    public List<DialogueSet> dialogueSets = new List<DialogueSet>();

    private string lastLine;

    public string GetRandomLine(GameProgressState state)
    {
        DialogueSet set = dialogueSets.Find(s => s.state == state);

        // fallback sur None si pas trouvé
        if (set == null || set.lines.Count == 0)
        {
            set = dialogueSets.Find(s => s.state == GameProgressState.None);
            if (set == null || set.lines.Count == 0)
                return "...";
        }

        if (set.lines.Count == 1)
        {
            lastLine = set.lines[0];
            return lastLine;
        }

        string chosen = set.lines[UnityEngine.Random.Range(0, set.lines.Count)];
        if (chosen == lastLine)
            chosen = set.lines[UnityEngine.Random.Range(0, set.lines.Count)];

        lastLine = chosen;
        return chosen;
    }
}