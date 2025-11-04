using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private ResourcePool _resourcePool;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private GameObject _spawnPlane;
    [SerializeField] private float _spawnHeightOffset = 0.5f;
    [SerializeField] private float _spawnAreaMargin = 0f;

    private SpawnUIController _controller;
    private Vector3 _planeCenter;
    private Vector3 _planeSize;

    public Action<Resource> OnResourceSpawned;

    private void Awake()
    {
        _controller = GetComponent<SpawnUIController>();
    }

    private void Start()
    {
        if (_spawnPlane == null) return;

        MeshRenderer renderer = _spawnPlane.GetComponent<MeshRenderer>();
        if (renderer == null) return;

        _planeCenter = renderer.bounds.center;
        _planeSize = renderer.bounds.size;

        StartCoroutine(SpawnLoop());
    }

    private void OnEnable()
    {
        _controller.IntervalChanged += SetSpawnInterval;
    }

    private void OnDisable()
    {
        _controller.IntervalChanged -= SetSpawnInterval;
    }

    private void SetSpawnInterval(float newInterval)
    {
        _spawnInterval = newInterval;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnResource();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnResource()
    {
        float halfX = _planeSize.x / 2 - _spawnAreaMargin;
        float halfZ = _planeSize.z / 2 - _spawnAreaMargin;

        float randomX = UnityEngine.Random.Range(_planeCenter.x - halfX, _planeCenter.x + halfX);
        float randomZ = UnityEngine.Random.Range(_planeCenter.z - halfZ, _planeCenter.z + halfZ);

        Vector3 spawnPos = new Vector3(randomX, _planeCenter.y + _spawnHeightOffset, randomZ);

        Resource resource = _resourcePool.GetObject(spawnPos);
        resource.OnCollect += ReturnResourse;

        OnResourceSpawned?.Invoke(resource);
    }

    private void ReturnResourse(Resource resource)
    {
        _resourcePool.ReturnToPool(resource);
        resource.OnCollect -= ReturnResourse;
    }
}
