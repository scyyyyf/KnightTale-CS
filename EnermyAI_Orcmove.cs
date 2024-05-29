using System.Collections.Generic;
using UnityEngine;

public class EnermyAI_Orcmove : MonoBehaviour
{
    [Header("Patrol")]
    public EnemyBehaviour enemyBehaviour = EnemyBehaviour.IsPatrolling;
    public Transform goalPoint;
    public List<Transform> points;
    public int nextID = 0;  
    int idChangeValue = 1;
    public float Speed = 2;

    [Header("Chasing")]
    public Transform playerTarget; 
    public bool isChasingPlayer = false;
    private bool nearCliff = false;

    [Header("Jump")]
    public bool IsOnGround;
    public float jumpForce = 2f;
    public bool hasJumped = false;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask goundLayer;

    [Header("Reference")]
    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Reset()
    {
        Init();
    }

    public void Init()
    {
        GameObject root = new GameObject(name + "_Root");
        root.transform.position = transform.position;
        transform.SetParent(root.transform);
        GameObject wayPoints = new GameObject("WayPoints");
        wayPoints.transform.SetParent(root.transform);
        wayPoints.transform.position = root.transform.position;
        GameObject p1 = new GameObject("Point1"); p1.transform.SetParent(wayPoints.transform); p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("Point2"); p2.transform.SetParent(wayPoints.transform); p2.transform.position = root.transform.position;
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
        enemyBehaviour = EnemyBehaviour.IsPatrolling;
    }

    public enum EnemyBehaviour
    {
        None,
        IsPatrolling,
        IsChasing,
        IsAttack,
    }

    private void Update()
    {
        IsOnGround = IsGrounded();
        if (IsOnGround)
        {
            hasJumped = false; 
        }
        goalPoint = isChasingPlayer && !nearCliff && playerTarget != null ? playerTarget : points[nextID];
        UpdateSpriteFlip();
        UpdateNextPatrolPoint();
        switch (enemyBehaviour)
        {
            case EnemyBehaviour.None:
                break;
            case EnemyBehaviour.IsPatrolling:
                transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, Speed * Time.deltaTime);
                break;
            case EnemyBehaviour.IsChasing:
                transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, Speed * Time.deltaTime);
                break;
            case EnemyBehaviour.IsAttack:
                break;
        }
        if (!IsOnGround && !hasJumped) 
        {
            Jump(); 
        }
    }

    private void UpdateSpriteFlip()
    {
        if(goalPoint != null)
        {
            if (goalPoint.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void UpdateNextPatrolPoint()
    {
        if (!isChasingPlayer && Vector2.Distance(transform.position, goalPoint.position) < 1f)
        {
            if (nextID == points.Count - 1)
                idChangeValue = -1;
            else if (nextID == 0)
                idChangeValue = 1;
            nextID += idChangeValue;
        }
    }

    public void SetChasingPlayer(bool isChasing, Transform playerTransform = null)
    {
        isChasingPlayer = isChasing;
        playerTarget = playerTransform;
    }

    public Transform GetClosestPatrolPoint()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestPoint = null;
        foreach (Transform point in points)
        {
            float distance = Vector3.Distance(transform.position, point.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }
        return closestPoint;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, goundLayer);
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        hasJumped = true;
    }
}

