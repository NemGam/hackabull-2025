using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> Prefabs;
    [SerializeField] public float SpawnerAreaSide = 65f;
    [SerializeField] public float DelayBetweenSpawnsSecond = 10f;

    private Coroutine _coroutine;

    public void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (_coroutine != null) return;
        _coroutine = StartCoroutine(Spawning());
    }

    public void StopSpawning()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator Spawning()
    {
        while (true)
        {
            float halfSide = SpawnerAreaSide * 0.5f;
            float positionX = transform.position.x + Random.Range(-halfSide, halfSide);
            float positionZ = transform.position.z + Random.Range(-halfSide, halfSide);

            int randomObject = Random.Range(0, Prefabs.Count);

            Instantiate(Prefabs[randomObject], new Vector3(positionX, transform.position.y, positionZ),
                Quaternion.identity);

            yield return new WaitForSeconds(DelayBetweenSpawnsSecond);

        }
    }
}
