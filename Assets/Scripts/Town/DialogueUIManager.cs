using UnityEngine;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text lineText;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void Show(string speaker, string line)
    {
        dialoguePanel.SetActive(true);
        speakerText.text = speaker;
        lineText.text = line;
        IsOpen = true;
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
        IsOpen = false;
    }
}