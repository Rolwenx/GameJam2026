using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueLevels : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;

    [Header("Dialogue")]
    public string[] lines;
    public GameObject dialogueCanvas;
    public float textSpeed = 0.05f;

    private int index;
    private bool isTyping;

     public bool DialogueFinished { get; private set; }


    public void StartDialogue()
    {
        DialogueFinished = false;
        index = 0;
        textComponent.text = string.Empty;
        dialogueCanvas.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("hi");
            EndDialogue();
        }

    }

    void EndDialogue()
    {
        DialogueFinished = true;
        dialogueCanvas.SetActive(false);
    }

   
}