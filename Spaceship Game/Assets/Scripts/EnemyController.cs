using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float HP = 100;
    [SerializeField] GameObject missile;
    [SerializeField] float missileSpeed;
    [SerializeField] float cooldown;
    [SerializeField] int scoreValue = 100;

    private ScoreController scoreController;

    private void Start()
    {
        scoreController = GameObject.Find("Score").GetComponent<ScoreController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MissileController missileHit = collision.gameObject.GetComponent<MissileController>();
        if (missileHit)
        {
            missileHit.DestroyMissile();
            HP -= missileHit.DamageEnemy();
            if(HP <= 0)
            {
                Destroy(gameObject);
                scoreController.AddScore(scoreValue);
            }
        }
    }

    private void Update()
    {
        float fireRate = Time.deltaTime * cooldown;
        if(Random.value < fireRate)
        {
            Fire();
        }
    }

    void Fire()
    {
        Vector3 startingPosition = transform.position + new Vector3(0, -1.2f, 0);
        GameObject ammo = Instantiate(missile, startingPosition, Quaternion.identity) as GameObject;
        //Aþaðý yönde olmasý için - verdik.
        ammo.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -missileSpeed);
    }
}
