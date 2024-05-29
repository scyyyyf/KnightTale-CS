using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D PlatformRigidbody;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 3f;
    private int currentWaypointIndex = 0;
    private Vector2 nextPosition;

    private void Awake()
    {
        PlatformRigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        if (waypoints.Length > 0)
        {
            nextPosition = waypoints[currentWaypointIndex].transform.position;
        }
    }

    private void Update()
    {
        //if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        //{
        //    currentWaypointIndex++;
        //    if(currentWaypointIndex >= waypoints.Length)
        //    {
        //        currentWaypointIndex = 0;
        //    }
        //}
        //Vector2 targetPosition = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position,Time.deltaTime * speed);
        //PlatformRigidbody.MovePosition(targetPosition);

        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, PlatformRigidbody.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
            nextPosition = waypoints[currentWaypointIndex].transform.position;
        }

        Vector2 direction = (nextPosition - PlatformRigidbody.position).normalized;
        PlatformRigidbody.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            collision.GetComponent<PlayerMovement_Kdx>().IsOnPlatfrom = true;
            collision.GetComponent<PlayerMovement_Kdx>().CurrentPlatform = PlatformRigidbody;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            collision.GetComponent<PlayerMovement_Kdx>().IsOnPlatfrom = false;
            collision.GetComponent<PlayerMovement_Kdx>().CurrentPlatform = null;
        }
    }
}
