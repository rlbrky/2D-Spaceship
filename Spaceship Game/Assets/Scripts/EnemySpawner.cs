using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    //Objelerin toplu hareketini sa�lamak i�in �izmemiz gereken alan� ve objelerin h�z�n� belirleyecek de�i�keni olu�turduk.
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float speed;

    [Header("Enemy Spawn")]
    [SerializeField] float spawnDelay;

    //Objelerin hareketinin y�n�n� kontrol etmek i�in olu�turulan de�i�ken.
    private bool moveRight = true;

    //
    private float xmax;
    private float xmin;


    // Start is called before the first frame update
    void Start()
    {
        //Kamera ile obje aras�ndaki Z fark�, bunu hesaplamadan kameran�n g�r�p g�rmedi�inden emin olamay�z.
        float differenceBetweenCameraAndObject = transform.position.z - Camera.main.transform.position.z;
        Vector3 camLeftNib = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, differenceBetweenCameraAndObject));
        Vector3 camRightNib = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, differenceBetweenCameraAndObject));

        //S�n�rlar�n atanmas�
        xmax = camRightNib.x;
        xmin = camLeftNib.x;



        /* 1 Adet Spawn i�in. 
         * //Elimizdeki prefab'� kullanarak belirtlien pozisyonda bir d��man olu�turduk.
         GameObject oEnemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
         //Objemizin child-parent ili�kisini transform kullanarak olu�turuyoruz.
         oEnemy.transform.parent = transform;*/

        SpawnOneByOne();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight)
        {
            //transform.position += new Vector3(speed * Time.deltaTime,0);
            transform.position += Vector3.right * speed *Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEnd = transform.position.x + width / 2;
        float leftEnd = transform.position.x - width / 2;

        //S�n�rlar� ge�ti�i anda
        if(rightEnd > xmax)
        {
            moveRight = false;
        }
        else if(leftEnd < xmin)
        {
            moveRight = true;
        }

        if (AreEnemiesDead())
        {
            SpawnOneByOne();
        }
    }

    void SpawnOneByOne()
    {
        Transform availablePosition = NextAvailablePosition();
        if (availablePosition)
        {
            GameObject oEnemy = Instantiate(enemyPrefab, availablePosition.transform.position, Quaternion.identity);
            oEnemy.transform.parent = availablePosition;
        }

        if (NextAvailablePosition())
        {
            Invoke("SpawnOneByOne", spawnDelay);
        }
    }

    Transform NextAvailablePosition()
    {
        foreach (Transform childPosition in transform)
        {
            if (childPosition.childCount == 0) //�len d��man�n pozisyonunu tutuyor.
            {
                return childPosition;
            }
        }
        return null; //�len d��man yoksa bo� d�nd�rd�k.
    }

    void SpawnEnemy()
    {
        //�oklu spawn
        foreach (Transform child in transform)
        {
            GameObject oEnemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity);
            oEnemy.transform.parent = child.transform;
        }
    }
    //Ya�ayan d��man kald� m� kontrol�.
        bool AreEnemiesDead()
    {
        foreach(Transform childPosition in transform)
        {
            if(childPosition.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }
}
