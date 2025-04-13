using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueScript : MonoBehaviour
{
    public static DialogueScript Instance { get; private set; }
    
    [SerializeField] private GameObject Dialogue;
    [SerializeField] private TMP_Text DialoguesText;


    [SerializeField] private List<String> Phase1;
    [SerializeField] private List<String> Phase2;
    [SerializeField] private List<String> Phase3;
    [SerializeField] private List<String> Phase4;

    [SerializeField] private UnityEvent OnEndOfPhase1;
    [SerializeField] private UnityEvent OnEndOfPhase2;
    [SerializeField] private UnityEvent OnEndOfPhase3;
    [SerializeField] private UnityEvent OnEndOfPhase4;

    private List<List<string>> _phases;
    private List<UnityEvent> _events;
    private int _currentPhase = 0;
    private Coroutine _activeCoroutine;
    
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    void Start()
    {
        _phases = new List<List<string>>() { Phase1, Phase2, Phase3, Phase4 };
        _events = new List<UnityEvent>() { OnEndOfPhase1, OnEndOfPhase2, OnEndOfPhase3, OnEndOfPhase4 };

        OnEndOfPhase1.AddListener((() => {SpawnerScript.Instance.StartSpawning(10f, 6, false);})); 
        OnEndOfPhase2.AddListener((() => {SpawnerScript.Instance.StartSpawning(5f, 12, false);})); 
        OnEndOfPhase3.AddListener((() => {SpawnerScript.Instance.StartSpawning(4f, 6, true);}));
        
        ProgressPhase();
    }

    public void ProgressPhase()
    {
        if (_currentPhase < 0 || _currentPhase >= _phases.Count) return;
        if (_phases[_currentPhase] == null || _events[_currentPhase] == null) return;
        if (_activeCoroutine != null) return; 

        Dialogue.SetActive(true);
        _activeCoroutine = StartCoroutine(ShowReplica(_phases[_currentPhase]));
    }

    public IEnumerator ShowReplica(List<String> phase)
    {
        foreach (var line in phase)
        {
            DialoguesText.text = line;
            yield return new WaitForSeconds(5f);
        }
        
        Dialogue.SetActive(false);
        _events[_currentPhase].Invoke();
        _currentPhase += 1;
        _activeCoroutine = null;
    }

    

}
