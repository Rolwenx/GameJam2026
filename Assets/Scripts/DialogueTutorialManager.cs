using UnityEngine;
using System;
using TMPro;

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
        dialogueText.text = lines[index];
    }

    public void StartIndication(string speaker, string[] newLines)
    {

        speakerName.text = speaker;
        lines = newLines;
        index = 0;
        active = true;

        panel.SetActive(true);
        dialogueText.text = lines[index];
    }

    private void Update()
    {
        if (!active) return;

        if (Input.GetMouseButtonDown(0))
        {
            index++;

            if (index >= lines.Length)
            {
                EndDialogue();
            }
            else
            {
                dialogueText.text = lines[index];
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
}