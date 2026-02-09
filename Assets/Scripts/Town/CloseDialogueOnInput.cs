using UnityEngine;

public class CloseDialogueOnInput : MonoBehaviour
{
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    private void Update()
    {
        if (DialogueUIManager.Instance == null) return;
        if (!DialogueUIManager.Instance.IsOpen) return;

        if (Input.GetKeyDown(closeKey) || Input.GetMouseButtonDown(0))
        {
            DialogueUIManager.Instance.Hide();
        }
    }
}