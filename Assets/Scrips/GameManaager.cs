using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DroneSpawner _blueTeam;
    [SerializeField] private DroneSpawner _redTeam;
    [SerializeField] private Spawner _resourceSpawners; 

    private List<Resource> _resources = new List<Resource>();
    private List<DroneController> _blue;
    private List<DroneController> _red;

    private bool _blueTurn = true;

    private void Awake()
    {
        _resourceSpawners.OnResourceSpawned += AddResource;
    }

    private void Start()
    {
        StartCoroutine(InitTeams());
    }

    private IEnumerator InitTeams()
    {
        yield return new WaitForSeconds(1f);

        _blue = _blueTeam.GetDrones();
        _red = _redTeam.GetDrones();

        StartCoroutine(AssignDronesRoutine());
    }

    private IEnumerator AssignDronesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (_blueTurn)
            {
                TryAssignDrone(_blue);
            }
            else
            {
                TryAssignDrone(_red);
            }

            _blueTurn = !_blueTurn;
        }
    }

    private void TryAssignDrone(List<DroneController> team)
    {
        foreach (var drone in team)
        {
            if (!drone.IsBusy)
            {
                Resource res = FindFreeResource();
                if (res != null)
                {
                    drone.StartCollecting(res);

                    _resources.Remove(res);
                }
                break;
            }
        }
    }

    private Resource FindFreeResource()
    {
        return _resources.Count > 0 ? _resources[Random.Range(0, _resources.Count)] : null;
    }

    private void AddResource(Resource res)
    {
        if (!_resources.Contains(res))
            _resources.Add(res);
    }
}