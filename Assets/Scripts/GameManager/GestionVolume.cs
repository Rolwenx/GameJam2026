using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GestionVolume : MonoBehaviour
{

    private const string PREF_VOLUME = "MusicVolume";
    public Slider sliderVolume;
    public AudioSource musicAudioSource;
    public TMP_Text volumeValueText; // Texte pour afficher la valeur du volume
    public string effect = "<sketchy>";

    void Start()
    {
        if (!PlayerPrefs.HasKey(PREF_VOLUME)){
            PlayerPrefs.SetFloat(PREF_VOLUME, 1f); // volume par défaut à 100%
            PlayerPrefs.Save();
        }
        sliderVolume.value = PlayerPrefs.GetFloat(PREF_VOLUME); // initialiser le slider à la valeur actuelle du volume
        volumeValueText.text = effect +(sliderVolume.value * 100).ToString("0"); // afficher la valeur en pourcentage
    }
    

    public void ChangeVolume()
    {
        if (musicAudioSource != null && sliderVolume != null)
            musicAudioSource.volume = sliderVolume.value; // slider en 0..1
            volumeValueText.text = effect +(musicAudioSource.volume * 100).ToString("0"); // afficher la valeur en pourcentage
        PlayerPrefs.SetFloat(PREF_VOLUME, sliderVolume.value); // sauvegarder la valeur du volume
        PlayerPrefs.Save();
    }
}
