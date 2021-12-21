using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        //Pozisyonu belirlemek için, tasarým kýsmýnda nereye spawnlayacaðýmýzý görmek için kullanabiliriz.
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
