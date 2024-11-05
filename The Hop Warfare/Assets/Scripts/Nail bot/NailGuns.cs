using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGuns : MonoBehaviour
{
    public Transform spawn;
    public GameObject nail;
    public GameObject aim;
    public float rotationSpeed;
    public bool canShoot = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            canShoot = false;
            Instantiate(nail, spawn.position, spawn.rotation);
            StartCoroutine(CD());
        }
    }

    IEnumerator CD()
    {
        yield return new WaitForSeconds(0.1f);
        canShoot = true;
    }
}
