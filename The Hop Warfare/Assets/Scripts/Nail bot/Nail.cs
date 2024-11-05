using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public Rigidbody rb;
    public Transform spawn;
    public float speed;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * speed, ForceMode.Impulse);
        StartCoroutine(Death());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
