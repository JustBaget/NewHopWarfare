using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifeTime;
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
