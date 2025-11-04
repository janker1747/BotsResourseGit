using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _isBusy = false;

    public Action<Resource> OnCollect;
    public bool IsBusy => _isBusy;

    public void BusyResourse(bool busy)
    {
        _isBusy = busy;
    }

    public void UnBusyResourse(bool busy)
    {
        _isBusy = busy;
    }

    public void Collect()
    {
        OnCollect?.Invoke(this);
    }
}