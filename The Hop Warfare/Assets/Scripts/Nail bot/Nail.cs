using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public Difficulty difficulty;
    public Rigidbody rb;
    public float speed;
    public float lifetime;
    public float[] speedModifier;
    // Start is called before the first frame update
    void Start()
    {
        difficulty = GameObject.FindWithTag("Difficulty").GetComponent<Difficulty>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * (speed * speedModifier[difficulty.difficultyIndex]), ForceMode.Impulse);
        StartCoroutine(Death());
        if(difficulty.difficultyIndex == 2)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
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
