using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyhealthbat : MonoBehaviour
{
    public Transform PrefabBoom;
    private SpriteRenderer[] renders;
    private Animator animator;
    public float Hp = 50;
    public float RedTime = 0.1f;
    private float ChangeColorTime;

    void Start()
    {
        renders = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.time > ChangeColorTime)
        {
            SetColor(Color.white);
        }
    }

    public void SetColor(Color color)
    {
        foreach (var r in renders)
        {
            r.color = color;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        SetColor(Color.red);
        ChangeColorTime = Time.time + RedTime;

        Hp -= damageAmount;
        if (Hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (PrefabBoom != null)
        {
            Instantiate(PrefabBoom, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
