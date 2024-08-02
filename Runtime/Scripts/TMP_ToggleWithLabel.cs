using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TMP_ToggleWithLabel : MonoBehaviour
{
    public TMP_Text label;
    public Toggle toggle;

    public void AddCleanListenerOnChanged(UnityAction<bool> action)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(action);
    }
}
