using UnityEngine;
using TMPro;

public class FactionUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text factionText;

    [Header("Базы фракций")]
    [SerializeField] private Base baseA;
    [SerializeField] private Base baseB;

    private void Update()
    {
        if (baseA == null || baseB == null) return;

        factionText.text =
            $"{baseA.FactionName}: {baseA.CollectedResources}\n" +
            $"{baseB.FactionName}: {baseB.CollectedResources}";
    }
}