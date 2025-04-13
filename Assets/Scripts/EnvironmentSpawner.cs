using Meta.XR.MRUtilityKit;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private FindSpawnPositions[] findSpawnPositionsArray;
     
    public void StartEnvironmentalSpawning()
    {
        foreach (var findSpawnPositions in findSpawnPositionsArray)
        {
            findSpawnPositions.StartSpawn(MRUK.Instance.GetCurrentRoom());
        }
    }
}
