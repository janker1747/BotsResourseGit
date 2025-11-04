using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DroneUIController : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Toggle pathToggle;

    private WaitForSeconds forSeconds;

    private DroneController[] drones;

    private void Start()
    {
        forSeconds = new WaitForSeconds(1f);
        StartCoroutine(InitDrones());
    }

    private IEnumerator InitDrones()
    {
        yield return forSeconds; 
        drones = FindObjectsOfType<DroneController>();

        speedSlider.onValueChanged.AddListener(OnSpeedChanged);
        pathToggle.onValueChanged.AddListener(OnPathToggle);
    }


    private void OnSpeedChanged(float value)
    {
        foreach (var drone in drones)
        {
            drone.SetSpeed(value);
        }
    }

    private void OnPathToggle(bool enabled)
    {
        foreach (var drone in drones)
        {
            drone.EnablePathDrawing(enabled);
        }
    }
}