using System;
using System.Collections;
using UnityEngine;

public class PollutionMonitor : MonoBehaviour
{
    private int matrixWidth = 40;
    private int matrixHeight = 40;

    [SerializeField] private Transform minPoint, maxPoint;
    
    private PollutionPoint[,] _matrix; 
    
    private void Start()
    {
        _matrix = new PollutionPoint[matrixHeight, matrixWidth];
        for (int i = 0; i < matrixHeight; i++)
        {
            for (int j = 0; j < matrixWidth; j++)
            {
                _matrix[i, j] = new PollutionPoint(new Vector3(
                    (maxPoint.position.x - minPoint.position.x) / matrixHeight * i + minPoint.position.x, 
                    0.85f, 
                    (maxPoint.position.z - minPoint.position.z) / matrixWidth * j + minPoint.position.z)
                );
            }
        }

        StartCoroutine(CalculateNewPollutionLevel());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_matrix == null) return;
        for (int i = 0; i < matrixHeight; i++)
        {
            for (int j = 0; j < matrixWidth; j++)
            {
                Gizmos.color = _matrix[i, j].Polluted ?  Color.red : Color.white;
                Gizmos.DrawSphere(_matrix[i, j].Position, 0.01f);
            }
        }
    }
#endif
    private IEnumerator CalculateNewPollutionLevel()
    {
        while (true)
        {
            var temp = VolumeClip.Instance.SphereTransforms;
            int polluted = 0;
            for (int i = 0; i < matrixHeight; i++)
            {
                for (int j = 0; j < matrixWidth; j++)
                {
                    foreach (var bubble in temp)
                    {
                        float dx = Mathf.Abs(bubble.position.x - _matrix[i, j].Position.x);
                        float dz = Mathf.Abs(bubble.position.z - _matrix[i, j].Position.z);

                        if (dx * dx + dz * dz <= ((bubble.localScale.x * bubble.localScale.x / 4)))
                        {
                            _matrix[i, j].ChangePollutionState(true);
                            polluted++;
                            break;
                        }

                        _matrix[i, j].ChangePollutionState(false);

                    }
                }
            }
            // Debug.LogError((float)polluted / (matrixHeight * matrixWidth));
            PassthroughtWorldDesctruction.Instance.UpdateWorldState((float)polluted / (matrixHeight * matrixWidth));
            yield return new WaitForSeconds(2f);
        }
    }

    struct PollutionPoint
    {
        public PollutionPoint(Vector3 position)
        {
            this.Position = position;
            Polluted = false;
        }

        public readonly Vector3 Position;
        public bool Polluted { get; private set; }

        public void ChangePollutionState(bool newState)
        {
            Polluted = newState;
        }
    }
}
