using System.Collections;
using UnityEngine;

public class swordEnergy : MonoBehaviour
{
    [SerializeField] private float swordEnergySpeed = 20f;
    [SerializeField] private Rigidbody2D swordEnergyRb;
    [SerializeField] private GameObject explodeEffect;
    [SerializeField] private GameObject WallToActivate;
    public float damage = 10f;
    public Transform shootPoint;
    private void Awake()
    {
        shootPoint = GameObject.Find("ShootPoint").transform;
    }

    void Start()
    {
        swordEnergyRb.velocity = transform.right * swordEnergySpeed;
    }

    public void Initialize(float damage, float lifetime)
    {
        this.damage = damage;
        StartCoroutine(Delay(lifetime));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CameraBoundary")
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            var enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                GameObject Temp = Instantiate(explodeEffect, transform.position, Quaternion.identity);
                Destroy(Temp, 5.0f);
                Destroy(gameObject); 
            }
        }

        if (collision.CompareTag("DestructibleWall"))
            if (collision.CompareTag("DestructibleWall"))
            {
                GameObject Temp = Instantiate(explodeEffect, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySFX("HitRock");

                wallProperties wallProps = collision.GetComponent<wallProperties>();
                if (wallProps != null)
                {
                    wallProps.ActivateObject();  // 调用激活方法，而不是生成新物体
                }

                Destroy(collision.gameObject);
                Destroy(Temp, 5.0f);
            }
    }

    IEnumerator Delay(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
