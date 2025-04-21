using UnityEngine;

public class NPCData : MonoBehaviour
{
    public enum NPC
    {
        Mo
    }

    public static string GetName(NPC npc)
    {
        switch (npc)
        {
            case NPC.Mo:
                return "Mo";
            default:
                return "(Name was not found)";
        }
    }

    public static string[][] GetDialogue(NPC npc)
    {
        switch (npc)
        {
            case NPC.Mo:
                return new string[][]
                    {
                        new string[] { "sup." },
                        new string[] { "thanks for playing!" }
                    };
            default:
                return new string[][]
                    {
                        new string[] { "(Dialogue was not found. If you're seeing this something has gone horribly wrong)" }
                    };

        }
    }
}
