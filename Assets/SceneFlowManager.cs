using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class SceneFlowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayableDirector introTimeline;
    [SerializeField] private GameObject player;
    [SerializeField] private DialogueLevels dialogue;

    private void Start()
    {
        StartCoroutine(SceneSequence());
    }

    IEnumerator SceneSequence()
    {
        /* 1. Cinematic intro
        if (introTimeline != null)
        {
            introTimeline.Play();
            yield return new WaitForSeconds((float)introTimeline.duration);
        }*/

        player.SetActive(true);

        // 3. Freeze game
        Time.timeScale = 0f;

        // 4. Start dialogue
        dialogue.StartDialogue();

        // 5. Wait until dialogue ends
        yield return new WaitUntil(() => dialogue.DialogueFinished);

        // 6. Wait 4s (real time, not affected by timescale)
        yield return new WaitForSecondsRealtime(2f);

        // 7. Resume game
        Time.timeScale = 1f;
    }
}