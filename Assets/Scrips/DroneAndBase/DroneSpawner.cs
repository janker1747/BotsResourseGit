using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneSpawner : MonoBehaviour
{
    [Header("Настройки спавна дронов")]
    [SerializeField] private DronPool _pool;
    [SerializeField] private Base _base;
    [SerializeField, Range(1, 5)] private int _initialDroneCount = 5;
    [SerializeField] private Slider _slider; 
    [SerializeField] private float _spawnRadius = 3f;

    private readonly List<DroneController> _drones = new List<DroneController>();

    private void Start()
    {
        for (int i = 0; i < _initialDroneCount; i++)
        {
            SpawnDrone();
        }

        _slider.minValue = 1;
        _slider.maxValue = 5;
        _slider.wholeNumbers = true;
        _slider.onValueChanged.AddListener(SetDroneCount);

        SetDroneCount(_slider.value);
    }

    public void SetDroneCount(float count)
    {
        int targetCount = Mathf.RoundToInt(count);

        while (_drones.Count < targetCount)
        {
            SpawnDrone();
        }

        while (_drones.Count > targetCount)
        {
            DroneController last = _drones[_drones.Count - 1];
            _drones.RemoveAt(_drones.Count - 1);
            last.gameObject.SetActive(false);
        }
    }

    public DroneController SpawnDrone()
    {
        Vector2 offset = Random.insideUnitCircle * _spawnRadius;
        Vector3 spawnPosition = _base.transform.position + new Vector3(offset.x, 0, offset.y);

        DroneController newDrone = _pool.GetObject(spawnPosition);

        newDrone.initialisation(spawnPosition, _base);

        _drones.Add(newDrone);
        return newDrone;
    }

    public void RemoveDrone()
    {
        if (_drones.Count == 0)
            return;

        DroneController drone = _drones[_drones.Count - 1];
        _drones.RemoveAt(_drones.Count - 1);

        if (drone != null)
        {
            drone.gameObject.SetActive(false);
        }
    }

    public void ClearAllDrones()
    {
        foreach (var drone in _drones)
        {
            if (drone != null)
                Destroy(drone.gameObject);
        }

        _drones.Clear();
    }

    public List<DroneController> GetDrones()
    {
        return _drones;
    }

}
