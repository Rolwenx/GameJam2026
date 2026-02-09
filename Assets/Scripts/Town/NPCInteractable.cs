using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private NPCDialogueProfile dialogueProfile;
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange;

    private void Start()
    {
        if (interactIcon != null)
            interactIcon.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (DialogueUIManager.Instance != null && DialogueUIManager.Instance.IsOpen) return;

        if (Input.GetKeyDown(interactKey))
        {
            Talk();
        }
    }

    private void Talk()
    {
        if (dialogueProfile == null) return;

        GameProgressState state = GameStateManager.Instance != null
            ? GameStateManager.Instance.CurrentState
            : GameProgressState.None;

        string line = dialogueProfile.GetRandomLine(state);
        DialogueUIManager.Instance.Show(dialogueProfile.npcName, line);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (interactIcon != null) interactIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (interactIcon != null) interactIcon.SetActive(false);
    }
}