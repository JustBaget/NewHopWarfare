using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float force;
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
        StartCoroutine(cameraFPV.InstantShake(shakeStrength, shakeDuration));
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
