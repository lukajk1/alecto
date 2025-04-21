using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePage;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text dialogueBody;

    private float wordInterval;
    private Coroutine dialogueCoroutine;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"more than one instance of {this} in scene");
        }
        Instance = this;
    }
    private void Start()
    {
        dialoguePage.SetActive(false);
    }

    public void Open(string speakerName, string[] dialogue, AudioSource voice, float pitch, float wordInterval, Color nameColor, NPC npc)
    {
        this.speakerName.text = speakerName;
        this.speakerName.color = nameColor;
        voice.pitch = pitch;
        this.wordInterval = wordInterval;

        if (dialogueCoroutine == null)
        {
            dialoguePage.SetActive(true);
            dialogueCoroutine = StartCoroutine(animateDialogue(speakerName, dialogue, voice, npc));
        }
    }

    private void Close()
    {
        dialoguePage.SetActive(false);
    }

    private IEnumerator animateDialogue(string speakerName, string[] dialogue, AudioSource voice, NPC npc)
    {
        Game.IsInDialogue = true;

        foreach (string dialogueItem in dialogue)
        {
            string[] words = dialogueItem.Split(' ');
            dialogueBody.text = "";

            foreach (string word in words)
            {
                dialogueBody.text += word + " ";
                voice.Play();
                yield return new WaitForSeconds(wordInterval);
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        Close();
        npc.OnClose();
        Game.IsInDialogue = false;
        dialogueCoroutine = null;
    }
}
