using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TMP_ButtonWithLabel : MonoBehaviour
{
    public TMP_Text label;
    public Button button;

    public void AddCleanListener(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
