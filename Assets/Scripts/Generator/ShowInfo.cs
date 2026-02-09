using UnityEngine;
using TMPro;

public class ShowInfo : MonoBehaviour
{
    public TMP_Text textInfo;

    void Start()
    {
        textInfo.gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var tag = gameObject.tag;
        string pref = $"{tag}Valide";
        if (other.CompareTag("Player") && PlayerPrefs.GetInt(pref, 0) == 0)
        {
            textInfo.text = $"Fragment non possédé";
            textInfo.gameObject.SetActive(true);
        }
        else {
            textInfo.text = $"Déposer le fragment";
            textInfo.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textInfo.gameObject.SetActive(false);
        }
    
    }
}
