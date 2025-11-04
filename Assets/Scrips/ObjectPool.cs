using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _initialCount = 10;

    private List<T> _objects = new List<T>();

    private void Awake()
    {
        for (int i = 0; i < _initialCount; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject()
    {
        T obj = Instantiate(_prefab, transform);
        obj.gameObject.SetActive(false);
        _objects.Add(obj);
        return obj;
    }

    public T GetObject(Vector3 position, Quaternion rotation = default)
    {
        foreach (var obj in _objects)
        {
            if (!obj.gameObject.activeSelf)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        T newObj = CreateObject();
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        newObj.gameObject.SetActive(true);
        return newObj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }
}
