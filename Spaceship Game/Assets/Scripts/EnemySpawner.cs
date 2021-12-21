using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    //Objelerin toplu hareketini saðlamak için çizmemiz gereken alaný ve objelerin hýzýný belirleyecek deðiþkeni oluþturduk.
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float speed;

    [Header("Enemy Spawn")]
    [SerializeField] float spawnDelay;

    //Objelerin hareketinin yönünü kontrol etmek için oluþturulan deðiþken.
    private bool moveRight = true;

    //
    private float xmax;
    private float xmin;


    // Start is called before the first frame update
    void Start()
    {
        //Kamera ile obje arasýndaki Z farký, bunu hesaplamadan kameranýn görüp görmediðinden emin olamayýz.
        float differenceBetweenCameraAndObject = transform.position.z - Camera.main.transform.position.z;
        Vector3 camLeftNib = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, differenceBetweenCameraAndObject));
        Vector3 camRightNib = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, differenceBetweenCameraAndObject));

        //Sýnýrlarýn atanmasý
        xmax = camRightNib.x;
        xmin = camLeftNib.x;



        /* 1 Adet Spawn için. 
         * //Elimizdeki prefab'ý kullanarak belirtlien pozisyonda bir düþman oluþturduk.
         GameObject oEnemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
         //Objemizin child-parent iliþkisini transform kullanarak oluþturuyoruz.
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

        //Sýnýrlarý geçtiði anda
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
            if (childPosition.childCount == 0) //Ölen düþmanýn pozisyonunu tutuyor.
            {
                return childPosition;
            }
        }
        return null; //Ölen düþman yoksa boþ döndürdük.
    }

    void SpawnEnemy()
    {
        //Çoklu spawn
        foreach (Transform child in transform)
        {
            GameObject oEnemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity);
            oEnemy.transform.parent = child.transform;
        }
    }
    //Yaþayan düþman kaldý mý kontrolü.
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
