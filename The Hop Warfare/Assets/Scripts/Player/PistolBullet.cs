using System.Collections;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifeTime;
    public GameObject explosion;
    GameObject targetPoint;
    Rigidbody rb;
    Vector3 target;
    void Start()
    {
        StartCoroutine(DestroyAfterSomeTime());
        rb = GetComponent<Rigidbody>();
        targetPoint = GameObject.Find("Target");
        target = (targetPoint.transform.position - transform.position) * 1000;
    }

    void Update()
    {
        rb.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    IEnumerator DestroyAfterSomeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
