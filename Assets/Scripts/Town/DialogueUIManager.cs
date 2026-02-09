using UnityEngine;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager Instance { get; private set; }

    [SerializeField] private GameObject rootPanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text lineText;

    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    private void Update()
    {
        if (!IsOpen) return;

        if (Input.GetKeyDown(closeKey) || Input.GetMouseButtonDown(0))
        {
            Hide();
        }
    }

    public void Show(string speaker, string line)
    {
        if (rootPanel != null) rootPanel.SetActive(true);
        if (speakerText != null) speakerText.text = speaker;
        if (lineText != null) lineText.text = line;
        IsOpen = true;
    }

    public void Hide()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
        IsOpen = false;
    }
}