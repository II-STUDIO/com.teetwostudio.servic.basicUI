using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TMP_InputWithLabel : MonoBehaviour
{
    public TMP_Text label;
    public TMP_InputField input;

    public void AddCleanListenerOnChanged(UnityAction<string> action)
    {
        input.onValueChanged.RemoveAllListeners();
        input.onValueChanged.AddListener(action);
    }

    public void AddCleanListenerOnSummit(UnityAction<string> action)
    {
        input.onSubmit.RemoveAllListeners();
        input.onSubmit.AddListener(action);
    }
}
