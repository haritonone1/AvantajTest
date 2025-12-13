using System.Collections.Generic;
using UnityEngine;

public sealed class InteractCanvas : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private InteractItemView[] items;

    public void UpdateUI(IReadOnlyList<InteractionUIData> interactions)
    {
        if (interactions == null || interactions.Count == 0)
        {
            root.SetActive(false);
            return;
        }

        root.SetActive(true);

        for (int i = 0; i < items.Length; i++)
        {
            if (i < interactions.Count)
            {
                items[i].Show(
                    interactions[i].Key,
                    interactions[i].Description
                );
            }
            else
            {
                items[i].Hide();
            }
        }
    }
}