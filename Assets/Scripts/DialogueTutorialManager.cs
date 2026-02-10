using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class DialogueTutorialManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerName;
    public event Action OnDialogueFinished;

    private string[] lines;
    private int index;
    private bool active;

    // ✅ AJOUT: Typewriter
    [SerializeField] private float charDelay = 0.02f;
    private Coroutine typeCoroutine;
    private bool isTyping;

    private void LateUpdate()
    {
        if (!active || target == null) return;
        transform.position = target.position + offset;
    }

    public void StartDialogue(string speaker, string[] newLines)

    {
        Time.timeScale = 0f;

        speakerName.text = speaker;
        lines = newLines;
        index = 0;
        active = true;

        panel.SetActive(true);

        // ✅ AJOUT: au lieu d'afficher direct
        PlayLine(lines[index]);
    }

    public void StartIndication(string speaker, string[] newLines)
    {

        speakerName.text = speaker;
        lines = newLines;
        index = 0;
        active = true;

        panel.SetActive(true);

        // ✅ AJOUT: au lieu d'afficher direct
        PlayLine(lines[index]);
    }

    private void Update()
    {
        if (!active) return;

        if (Input.GetMouseButtonDown(0))
        {
            // ✅ AJOUT: si on clique pendant le typewriter -> afficher instant
            if (isTyping)
            {
                FinishTypingInstant();
                return;
            }

            index++;

            if (index >= lines.Length)
            {
                EndDialogue();
            }
            else
            {
                // ✅ AJOUT: au lieu d'afficher direct
                PlayLine(lines[index]);
            }
        }
    }

    private void EndDialogue()
    {
        active = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
        OnDialogueFinished?.Invoke();
    }

    // ✅ AJOUT: fonctions typewriter
    private void PlayLine(string line)
    {
        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);

        typeCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;

        dialogueText.text = line;

        // IMPORTANT avec certaines fonts TMP
        dialogueText.ForceMeshUpdate();

        dialogueText.maxVisibleCharacters = 0;

        int total = dialogueText.textInfo.characterCount;
        for (int i = 0; i <= total; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            // Realtime car Time.timeScale peut être 0
            yield return new WaitForSecondsRealtime(charDelay);
        }

        isTyping = false;
        typeCoroutine = null;
    }

    private void FinishTypingInstant()
    {
        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);

        dialogueText.ForceMeshUpdate();
        dialogueText.maxVisibleCharacters = int.MaxValue;

        isTyping = false;
        typeCoroutine = null;
    }
}
