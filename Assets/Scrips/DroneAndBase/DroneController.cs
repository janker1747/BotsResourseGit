using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public class DroneController : MonoBehaviour
{
    [Header("Настройки дрона")]
    [SerializeField] private ParticleSystem _AddEffect;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Base _base;
    [SerializeField] private float _collectTime = 2f;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private LineRenderer _lineRenderer;

    private bool _drawPath = false;
    private Resource _currentTarget;
    private bool _hasResource = false;
    private bool _isBusy = false;

    private WaitForSeconds _waitCollect;
    private WaitUntil _waitUntilArrived;

    public bool IsBusy => _isBusy;

    private void Awake()
    {
        _startPosition = transform.position;

        _waitCollect = new WaitForSeconds(_collectTime);
        _waitUntilArrived = new WaitUntil(() => !_agent.pathPending && _agent.remainingDistance < 0.5f);
    }

    private void Update()
    {
        AvoidOtherDrones();

        if (_drawPath == true)
        DrawPath();
    }

    public void StartCollecting(Resource resource)
    {
        if (_isBusy) return;

        _isBusy = true;
        _currentTarget = resource;
        StartCoroutine(CollectRoutine());
    }

    private IEnumerator CollectRoutine()
    {
        _agent.SetDestination(_currentTarget.transform.position);
        yield return _waitUntilArrived;

        yield return _waitCollect;
        _currentTarget.Collect();

        _hasResource = true;

        _agent.SetDestination(_base.transform.position);
        yield return _waitUntilArrived;

        PlayUnloadEffect();
        _base.AddResource();
        _hasResource = false;
        _isBusy = false;
    }

    private void AvoidOtherDrones()
    {
        float avoidRadius = 2f;
        Collider[] hits = Physics.OverlapSphere(transform.position, avoidRadius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<DroneController>(out var other) && other != this)
            {
                Vector3 away = transform.position - other.transform.position;
                away.y = 0;

                _agent.Move(away.normalized * Time.deltaTime);
            }
        }
    }

    private void PlayUnloadEffect()
    {
        _AddEffect.Play();
    }

    public void initialisation(Vector3 position, Base home)
    {
        _base = home;
        _startPosition = position;
    }

    public void SetSpeed(float speed)
    {
        _agent.speed = speed;
    }

    public void EnablePathDrawing(bool enable)
    {
        _drawPath = enable;
        if (!enable && _lineRenderer != null)
            _lineRenderer.positionCount = 0;
    }

    private void DrawPath()
    {
        if (!_agent.hasPath) return;
        var corners = _agent.path.corners;
        if (corners.Length < 2) return;

        Vector3[] points = new Vector3[corners.Length];
        for (int i = 0; i < points.Length; i++)
            points[i] = corners[i] + Vector3.up * 0.05f;

        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }


}