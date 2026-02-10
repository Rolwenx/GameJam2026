using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GestionVolume : MonoBehaviour
{

    private const string PREF_VOLUME = "MusicVolume";
    private const string PREF_SFX = "SfxVolume";

    public Slider sliderVolume;
    public AudioSource musicAudioSource;
    public TMP_Text volumeValueText; // Texte pour afficher la valeur du volume
    public string effect = "<sketchy>";


    public Slider sliderEffet;
    AudioSource[] allAudioSources;
    public TMP_Text effetValueText; // Texte pour afficher la valeur des effets

    void Start()
    {
        // Créer les prefs si elles n'existent pas
        if (!PlayerPrefs.HasKey(PREF_VOLUME))
            PlayerPrefs.SetFloat(PREF_VOLUME, 1f);

        if (!PlayerPrefs.HasKey(PREF_SFX))
            PlayerPrefs.SetFloat(PREF_SFX, 1f);

        PlayerPrefs.Save();

        // Lire valeurs
        float musicV = PlayerPrefs.GetFloat(PREF_VOLUME, 1f);
        float sfxV   = PlayerPrefs.GetFloat(PREF_SFX, 1f);

        // Init sliders + textes
        if (sliderVolume != null) sliderVolume.value = musicV;
        if (volumeValueText != null) volumeValueText.text = effect + (musicV * 100f).ToString("0");

        if (sliderEffet != null) sliderEffet.value = sfxV;
        if (effetValueText != null) effetValueText.text = effect + (sfxV * 100f).ToString("0");

        // Appliquer volumes
        if (musicAudioSource != null)
            musicAudioSource.volume = musicV; // Ambiante seulement

        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio == null) continue;

            // Effets = tout sauf Ambiante (et sauf la musicAudioSource)
            if (!audio.gameObject.name.Contains("Ambiante") && audio != musicAudioSource)
                audio.volume = sfxV;
        }
    }

    

    public void ChangeVolume()
    {
        if (musicAudioSource != null && sliderVolume != null)
            musicAudioSource.volume = sliderVolume.value; // slider en 0..1
            volumeValueText.text = effect +(musicAudioSource.volume * 100).ToString("0"); // afficher la valeur en pourcentage
        PlayerPrefs.SetFloat(PREF_VOLUME, sliderVolume.value); // sauvegarder la valeur du volume
        PlayerPrefs.Save();
    }

    public void ChangeVolumeEffets()
    {
        if (sliderEffet == null) return;

        float v = sliderEffet.value;

        if (allAudioSources == null || allAudioSources.Length == 0)
            allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in allAudioSources)
        {
            if (audio == null) continue;

            if (!audio.gameObject.name.Contains("Ambiante") && audio != musicAudioSource)
                audio.volume = v;
        }

        if (effetValueText != null)
            effetValueText.text = effect + (v * 100f).ToString("0");

        PlayerPrefs.SetFloat(PREF_SFX, v);
        PlayerPrefs.Save();
    }

}
