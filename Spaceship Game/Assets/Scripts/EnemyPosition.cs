using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        //Pozisyonu belirlemek i�in, tasar�m k�sm�nda nereye spawnlayaca��m�z� g�rmek i�in kullanabiliriz.
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
