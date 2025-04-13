using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Random = UnityEngine.Random;

public class AnimalWalkAround : MonoBehaviour
{
    [SerializeField] private float walkRadius = 0.15f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] Transform walkRegion;
    
    private Vector3 walkPoint;
    private bool walkPointSet;
    
    // Start is called once before the first execution of Update after
    void Start()
    {
        StartCoroutine(SearchPosRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Patroling();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        else
        {
            SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        if (distanceToWalkPoint.magnitude < 0.005f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = 0f;
        float randomX = 0f;
        // float randPosY = -500f;         // Just initalize it to a spot that would trigger the while loop
        RaycastHit hit;

        
        randomZ = Random.Range(-walkRadius, walkRadius);
        randomX = Random.Range(-walkRadius, walkRadius);
        
        // Physics.Raycast(
        //     new Vector3(transform.position.x + randomX, transform.position.y + .15f,
        //         transform.position.z + randomZ), Vector3.down, out hit, .2f);
        //
        // randPosY = hit.point.y;
       
        
        walkPoint = new Vector3(walkRegion.position.x + randomX, walkRegion.position.y, walkRegion.position.z +randomZ);
        walkPointSet = true;
    }

    private void SetDestination(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        // _rb.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by something tagged: " + other.gameObject.tag);

        if (!other.gameObject.CompareTag("Unwalkable")) return;

        walkPoint = transform.position - transform.forward * 0.04f;
    }

    private IEnumerator SearchPosRoutine()
    {
        while (true)
        {
            SearchWalkPoint();
            yield return new WaitForSeconds(5f); // wait 5 seconds
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(walkPoint, 0.01f);
    }
}
