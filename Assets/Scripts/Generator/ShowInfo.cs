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
        string tag = gameObject.tag;
        string pref = $"Level_{tag[2]}_Completed"; // p
        Debug.Log($"Player entered trigger with tag {tag}. Checking PlayerPrefs for key '{pref}' with value {PlayerPrefs.GetInt(pref, 0)}");
        if (other.CompareTag("Player") && PlayerPrefs.GetInt(pref, 0) == 0)
        {
            textInfo.text = $"Fragment non possédé";
            textInfo.gameObject.SetActive(true);
        }
        else {
            if (PlayerPrefs.GetInt("LV"+tag[2]+"Valide", 0) == 1){
                textInfo.text = $"Fragment déjà déposé";
                textInfo.gameObject.SetActive(true);
            }
            else{
                textInfo.text = $"Fragment possédé";
                textInfo.gameObject.SetActive(true);
            }

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
