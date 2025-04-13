using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Random = UnityEngine.Random;

public class AnimalWalkAround : MonoBehaviour
{
    [SerializeField] private float walkRadius = 1f;
    [SerializeField] private float acceptableYDiff = 0.001f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float rotationSpeed = 5f;

    private BoxCollider boxCollider;
    private Vector3 walkPoint;
    private bool walkPointSet;
    
    // Start is called once before the first execution of Update after
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
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
        float randomZ = transform.position.z;
        float randomX = transform.position.x;
        float randPosY = -500f;         // Just initalize it as a very bad thing 
        RaycastHit hit;

        while (!(randPosY > transform.position.y - acceptableYDiff && randPosY < transform.position.y + acceptableYDiff))
        {
            randomZ = Random.Range(-walkRadius, walkRadius);
            randomX = Random.Range(-walkRadius, walkRadius);
            
            Physics.Raycast(
                new Vector3(transform.position.x + randomX, transform.position.y + .15f,
                    transform.position.z + randomZ), Vector3.down, out hit, .2f);
            
            randPosY = hit.point.y;
        }
        
        walkPoint = new Vector3(randomX, randPosY, randomZ);
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

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collided with something tagged: " + other.gameObject.tag);
        if (!other.gameObject.CompareTag("Unwalkable")) return;
        
        walkPoint = -transform.forward * 0.08f;
        Debug.Log("Collided with something tagged: YourTagName");
        
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
