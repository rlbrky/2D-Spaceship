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
    

    /*Aþaðýdaki uç deðerlerinde objenin yarýsý ekranýn dýþýna çýkabiliyor,
     * bunu düzeltmek için aradaki farka bakýp objenin sýðmasý için gereken deðeri bulduk ve aþaðýdaki deðiþkene atadýk.*/
    float fixCorners = 0.72f;
    //Padding deniyor.

    // Start is called before the first frame update
    void Start()
    {
        //Objenin kameraya olan z uzaklýðýný aldýk.
        float distance = transform.position.z - Camera.main.transform.position.z;

        //Uçlarýn arasýndaki deðerlerin 1 ve 0 arasýnda olmasýný saðladýk, böylece saçma deðerlerle uðraþmayacaðýz.
        //Ör: 16:9 ekranda -8.9 ile 8.9 arasý bir deðerde geziyorduk, aþaðýdaki kodla 0 ile 1 arasýnda gezeceðiz.
        /*Yukarýdaki deðerleri öðrenmek için:
            Debug.Log(leftNib);
         */
        Vector3 leftNib = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)); //Sol UÇ
        Vector3 rightNib = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)); //Sað UÇ

        //Uçlarý atadýk.
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
        //Sadece basýldýðýnda devreye gireceðinden basýlý tutulsa bile 1 kez çalýþacak.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Adý verilen fonksiyonu belirtilen süre geçtikten sonra en sondaki aralýklarla otomatik olarak ardarda çaðýrýr.
            InvokeRepeating("Fire", 0.00001f, cooldown);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }



        //Sýnýrlayýcý, objenin sahnenin dýþýna çýkmasýný engellemek adýna sýnýrladýk.
        float Xdelimeter = Mathf.Clamp(transform.position.x, xmin, xmax);
        /*Sýnýrlayýcý deðiþkeni kullanarak dinamik bir sýnýr oluþturduk, 
         * böylece ekran boyutu deðiþse de sýnýrlar bozulmayacak ve gemi her zaman ekranýn içerisinde olacak.*/
        transform.position = new Vector3(Xdelimeter, transform.position.y, transform.position.z);
        //Move Right
        if (Input.GetKey(KeyCode.D))
        {
            //transform.position += new Vector3(speed*Time.deltaTime,0,0);
            //Vector3.right -> Vector3(1,0,0)'a eþit.
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
