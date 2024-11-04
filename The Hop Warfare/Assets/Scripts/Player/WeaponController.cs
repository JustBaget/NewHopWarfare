using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Пистолет")]
    public bool canPistolShoot;
    public bool canPistolShootGrenade;
    [Header("Дробовик")]
    public bool canShotgunShootDefault;
    public bool isUsingSpecial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PistolReloadStart(float reloadTime)
    {
        StartCoroutine(PistolReload(reloadTime));
    }
    public void PistolReloadGrenadeStart(float reloadTime)
    {
        StartCoroutine(PistolReloadGrenade(reloadTime));
    }

    public IEnumerator PistolReload(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        canPistolShoot = true;
    }
    public IEnumerator PistolReloadGrenade(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        canPistolShootGrenade = true;
    }

    public void ShotgunReloadStart(float reloadTime)
    {
        StartCoroutine(ShotgunReload(reloadTime));
    }
    IEnumerator ShotgunReload(float reloadTime)
    {
        canShotgunShootDefault = false;
        yield return new WaitForSeconds(reloadTime);
        if (!isUsingSpecial)
        {
            canShotgunShootDefault = true;
        }
    }
}
