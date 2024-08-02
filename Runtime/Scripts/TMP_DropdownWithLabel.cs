using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TMP_DropdownWithLabel : MonoBehaviour
{
    public TMP_Text label;
    public TMP_Dropdown dropdown;

    public void AddCleanListenerOnValueChanged(UnityAction<int> action)
    {
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(action);
    }
}
