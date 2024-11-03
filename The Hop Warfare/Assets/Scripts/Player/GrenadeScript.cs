using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float expForce = 10f;
    public float force;
    public float radius = 5f;
    public float damage;
    public float playerDamage;
    public float shakeStrength;
    public int shakeDuration;
    public GameObject explosion;
    Rigidbody rb;
    GameObject target;
    CameraFPV cameraFPV;
    UltimateSoundScript soundManager;
    void Start()
    {
        target = GameObject.Find("Target");
        rb = GetComponent<Rigidbody>();
        cameraFPV = GameObject.Find("FPV Camera").GetComponent<CameraFPV>();
        soundManager = GameObject.Find("SoundManager").GetComponent<UltimateSoundScript>();
        Vector3 targetVector = (target.transform.position - transform.position) * 1000;

        rb.AddForce(targetVector.normalized * force, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {
        soundManager.GrenadeExplodeSounds();
        //StartCoroutine(cameraFPV.InstantShake(shakeStrength, shakeDuration));
        Instantiate(explosion, transform.position, transform.rotation);
        Debug.Log("Exploded");

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.GetComponent<Rigidbody>() != null)
            {
                nearbyObject.GetComponent<Rigidbody>().AddExplosionForce(expForce, transform.position, radius);
            }
            if (nearbyObject.GetComponent<Health>() != null)
            {
                nearbyObject.GetComponent<Health>().hp -= damage;
            }
            if (nearbyObject.GetComponent<PlayerHealth>() != null)
            {
                nearbyObject.GetComponent<PlayerHealth>().Damage(playerDamage);
            }
        }
        Destroy(gameObject);
    }
}
