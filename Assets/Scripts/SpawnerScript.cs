    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class SpawnerScript : MonoBehaviour
    {
        public static SpawnerScript Instance { get; private set; }
        
        [SerializeField] private List<GameObject> Prefabs;
        [SerializeField] public float SpawnerAreaSide = 65f;
        [SerializeField] public float DelayBetweenSpawnsSecond = 10f;
        [SerializeField] private AudioSourceHandler ash;

        

        private Coroutine _coroutine;
        
        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void StartSpawning(float delay, int targetObjects, bool lastPhase)
        {
            DelayBetweenSpawnsSecond = delay;
            
            if (_coroutine != null) return;

            if (lastPhase)
            {
                _coroutine = StartCoroutine(FinalSpawning());
            }
            else
            {
                _coroutine = StartCoroutine(Spawning(targetObjects));    
            }
            
        }

        public void ExpressCleanUp()
        {
            for (int i = transform.childCount - 1; i > -1; i--)
            {
                Destroy(transform.GetChild(i));
            }
        }

        private IEnumerator Spawning(int targetObjects)
        {
            int counter = 0;
            while (counter < targetObjects)
            {
                if (transform.childCount >= 30)
                {
                    yield return new WaitForSeconds(DelayBetweenSpawnsSecond);
                    continue;
                }
                float halfSide = SpawnerAreaSide * 0.5f;
                float positionX = transform.position.x + Random.Range(-halfSide, halfSide);
                float positionZ = transform.position.z + Random.Range(-halfSide, halfSide);

                int randomObject = Random.Range(0, Prefabs.Count);

                Instantiate(Prefabs[randomObject], new Vector3(positionX, transform.position.y, positionZ),
                    Quaternion.identity, transform);
                
                counter += 1;

                yield return new WaitForSeconds(DelayBetweenSpawnsSecond);
            }
            _coroutine = null;
            DialogueScript.Instance.ProgressPhase();
        }
        
        private IEnumerator FinalSpawning()
        {
            while (PassthroughtWorldDesctruction.Instance.WorldState < 0.8f)
            {
                if (transform.childCount >= 30)
                {
                    yield return new WaitForSeconds(DelayBetweenSpawnsSecond);
                    continue;
                }
                float halfSide = SpawnerAreaSide * 0.5f;
                float positionX = transform.position.x + Random.Range(-halfSide, halfSide);
                float positionZ = transform.position.z + Random.Range(-halfSide, halfSide);

                int randomObject = Random.Range(0, Prefabs.Count);

                Instantiate(Prefabs[randomObject], new Vector3(positionX, transform.position.y, positionZ),
                    Quaternion.identity);

                DelayBetweenSpawnsSecond = Mathf.Max(0.6f, DelayBetweenSpawnsSecond - 0.2f);

                yield return new WaitForSeconds(DelayBetweenSpawnsSecond);
            }
            ash.SwitchPeacefulToChaotic();
            _coroutine = null;
            DialogueScript.Instance.ProgressPhase();
        }
    }
