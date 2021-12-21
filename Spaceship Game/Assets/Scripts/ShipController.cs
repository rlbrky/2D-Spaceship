using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float speed;
    float xmin;
    float xmax;
    [Header("Combat Stuff")]
    [SerializeField] GameObject ammo;
    [SerializeField] float missileSpeed;
    [SerializeField] float cooldown;
    [SerializeField] float HP;
    

    /*A�a��daki u� de�erlerinde objenin yar�s� ekran�n d���na ��kabiliyor,
     * bunu d�zeltmek i�in aradaki farka bak�p objenin s��mas� i�in gereken de�eri bulduk ve a�a��daki de�i�kene atad�k.*/
    float fixCorners = 0.72f;
    //Padding deniyor.

    // Start is called before the first frame update
    void Start()
    {
        //Objenin kameraya olan z uzakl���n� ald�k.
        float distance = transform.position.z - Camera.main.transform.position.z;

        //U�lar�n aras�ndaki de�erlerin 1 ve 0 aras�nda olmas�n� sa�lad�k, b�ylece sa�ma de�erlerle u�ra�mayaca��z.
        //�r: 16:9 ekranda -8.9 ile 8.9 aras� bir de�erde geziyorduk, a�a��daki kodla 0 ile 1 aras�nda gezece�iz.
        /*Yukar�daki de�erleri ��renmek i�in:
            Debug.Log(leftNib);
         */
        Vector3 leftNib = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)); //Sol U�
        Vector3 rightNib = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)); //Sa� U�

        //U�lar� atad�k.
        xmin = leftNib.x + fixCorners;
        xmax = rightNib.x - fixCorners;
    }

    void Fire()
    {
        GameObject shipAmmo = Instantiate(ammo, transform.position + new Vector3(0,1.1f,0), Quaternion.identity) as GameObject;
        shipAmmo.GetComponent<Rigidbody2D>().velocity = new Vector3(0, missileSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Sadece bas�ld���nda devreye girece�inden bas�l� tutulsa bile 1 kez �al��acak.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Ad� verilen fonksiyonu belirtilen s�re ge�tikten sonra en sondaki aral�klarla otomatik olarak ardarda �a��r�r.
            InvokeRepeating("Fire", 0.00001f, cooldown);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }



        //S�n�rlay�c�, objenin sahnenin d���na ��kmas�n� engellemek ad�na s�n�rlad�k.
        float Xdelimeter = Mathf.Clamp(transform.position.x, xmin, xmax);
        /*S�n�rlay�c� de�i�keni kullanarak dinamik bir s�n�r olu�turduk, 
         * b�ylece ekran boyutu de�i�se de s�n�rlar bozulmayacak ve gemi her zaman ekran�n i�erisinde olacak.*/
        transform.position = new Vector3(Xdelimeter, transform.position.y, transform.position.z);
        //Move Right
        if (Input.GetKey(KeyCode.D))
        {
            //transform.position += new Vector3(speed*Time.deltaTime,0,0);
            //Vector3.right -> Vector3(1,0,0)'a e�it.
            transform.position += Vector3.right * speed * Time.deltaTime;
        }    
        //Move Left
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            //transform.position += new Vector3(-speed*Time.deltaTime,0,0);
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MissileController missileHit = collision.gameObject.GetComponent<MissileController>();
        if (missileHit)
        {
            missileHit.DestroyMissile();
            HP -= missileHit.DamageEnemy();
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
