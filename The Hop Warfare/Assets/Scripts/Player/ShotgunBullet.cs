using System.Collections;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifeTime;
    public float spread;
    private Rigidbody rb;
    private Camera cameraObj;
    private Vector3 bulletVector;
    void Start()
    {
        float randomSpread = Random.Range(-spread, spread);
        float randomSpreadButOther = Random.Range(-spread, spread);
        cameraObj = GameObject.Find("FPV Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        bulletVector = cameraObj.ScreenToWorldPoint(new Vector3(Screen.width/2 + randomSpread, Screen.height/2 + randomSpreadButOther, 1000)) - transform.position;
        rb.AddForce(bulletVector * speed * Time.deltaTime, ForceMode.Impulse);
        StartCoroutine(DestroyAfterSomeTime());
    }

    void FixedUpdate()
    {
        
    }

    IEnumerator DestroyAfterSomeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.GetComponent<Health>() != null && collision.gameObject.GetComponent<Health>())
        {
            collision.gameObject.GetComponent<Health>().Damage(damage);
        }
    }
}
