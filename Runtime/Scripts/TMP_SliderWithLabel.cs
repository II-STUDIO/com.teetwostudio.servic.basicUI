using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TMP_SliderWithLabel : MonoBehaviour
{
    public TMP_Text label;
    public Slider slider;

    public void AddCleanListenerOnChanged(UnityAction<float> onChanged)
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(onChanged);
    }
}
