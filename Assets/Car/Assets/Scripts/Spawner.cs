using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;

    private GameObject _previous;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (_previous != null)
            Destroy(_previous);
        _previous = Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation, transform);
    }
}
