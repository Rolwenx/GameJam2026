using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Character Dialogue")]
    public GameObject followerPanel;
    public TextMeshProUGUI followerText;

    [Header("Game Indication")]
    public GameObject indicationPanel;
    public TextMeshProUGUI indicationText;

    void Awake()
    {
        HideFollower();
        HideIndication();
    }

    // --- FOLLOWER ---
    public void ShowFollower(string text)
    {
        followerPanel.SetActive(true);
        followerText.text = text;
    }

    public void HideFollower()
    {
        followerPanel.SetActive(false);
    }

    // --- INDICATION ---
    public void ShowIndication(string text)
    {
        indicationPanel.SetActive(true);
        indicationText.text = text;
    }

    public void HideIndication()
    {
        indicationPanel.SetActive(false);
    }
}