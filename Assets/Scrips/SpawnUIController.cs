using UnityEngine;
using TMPro;
using System;

public class SpawnUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField intervalInput;

    public Action<float> IntervalChanged;

    private void Start()
    {
        intervalInput.onEndEdit.AddListener(OnIntervalChanged);
    }

    private void OnIntervalChanged(string value)
    {
        if (float.TryParse(value, out float newInterval) && newInterval > 0f)
        {
            IntervalChanged?.Invoke(newInterval);
            Debug.Log("Новый интервал спавна: " + newInterval);
        }
        else
        {
            Debug.LogWarning("Некорректный ввод интервала");
        }
    }
}