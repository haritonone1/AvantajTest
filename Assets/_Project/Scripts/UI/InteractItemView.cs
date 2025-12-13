using TMPro;
using UnityEngine;

public sealed class InteractItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text descriptionText;

    public void Show(string key, string description)
    {
        keyText.text = key;
        descriptionText.text = description;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}