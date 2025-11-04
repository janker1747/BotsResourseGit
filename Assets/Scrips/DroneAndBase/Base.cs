using UnityEngine;

public class Base : MonoBehaviour
{
    [Header("Настройки базы")]
    [SerializeField] private string _factionName;
   

    private int _collectedResources = 0;

    public string FactionName => _factionName;
    public int CollectedResources => _collectedResources;

    private void OnTriggerEnter(Collider other)
    {
        Resource resource = other.GetComponent<Resource>();
        Debug.Log("Ресурс");
    }

    public void AddResource()
    {
        _collectedResources++;
    }
}
