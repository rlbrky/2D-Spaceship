using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] float Damage;

    public void DestroyMissile()
    {
        Destroy(gameObject);
    }

    public float DamageEnemy()
    {
        return Damage;
    }
}
