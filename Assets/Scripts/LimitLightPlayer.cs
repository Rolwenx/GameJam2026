using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LimitLightPlayer : MonoBehaviour
{
    [Header("UI")]
    public Slider sliderLight;
    [SerializeField] private Image fillImage; // l'Image de "Fill" (Slider/Fill Area/Fill)
    public TMP_Text percentageText; // le TMP_Text qui affiche le pourcentage de lumière restante

    [Header("Light")]
    public float maxLight = 100f;
    public float currentLight = 100f;

    [Header("Drain")]
    public float drainPerSecond = 1.5f;

    void Start()
    {
        currentLight = Mathf.Clamp(currentLight, 0f, maxLight);
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currentLight = Mathf.Clamp(currentLight - drainPerSecond * Time.deltaTime, 0f, maxLight);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        float t = currentLight / maxLight; // 0..1
        sliderLight.value = t;

        if (fillImage != null)
            fillImage.color = GetColorForPercent(t);

        if (percentageText != null)
            percentageText.text = Mathf.RoundToInt(t * 100f).ToString();
    }

    Color GetColorForPercent(float t)
    {
        // t = current/max
        if (t >= 0.70f) return Color.green;                 // au-dessus de 70% : vert
        if (t >= 0.50f) return Color.yellow;                // 70% -> 50% : jaune
        if (t >= 0.20f) return new Color(1f, 0.55f, 0f);    // 50% -> 20% : orange
        return Color.red;                                   // en dessous de 20% : rouge
    }
}
