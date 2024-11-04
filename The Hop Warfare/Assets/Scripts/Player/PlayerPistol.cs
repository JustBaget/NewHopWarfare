using System;
using System.Collections;
using UnityEngine;

public class PlayerPistol : MonoBehaviour
{
    [Header("Настройки Пистолета")]
    public float reloadTime;
    public float reloadTimeForGrenade;
    public GameObject bullet;
    public GameObject grenade;
    public GameObject firePoint;
    bool canShot = true;
    public bool canShotGrenade = true;
    CameraFPV cameraFPV;

    [Header("Тряска Камеры от Пистолета")]
    public float shakeStrength;
    public int shakeDuration;

    [Header("Эффекты Пистолета")]
    public Animator pistolAnimation;
    public Animator pistolAnimation2;
    public ParticleSystem[] effect;
    UltimateSoundScript soundManager;

    [Header("Контроллер оружия")]
    public WeaponController weaponController;
    public GameObject indicator;
    void Start()
    {
        cameraFPV = GameObject.Find("FPV Camera").GetComponent<CameraFPV>();
        soundManager = GameObject.Find("SoundManager").GetComponent<UltimateSoundScript>();
    }

    void Update()
    {
        Shot();
        ShotGrenade();

        if(weaponController.canPistolShootGrenade)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    void Shot()
    {
        if(Input.GetKey(KeyCode.Mouse0) && weaponController.canPistolShoot)
        {
            weaponController.PistolReloadStart(reloadTime);
            Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
            pistolAnimation.SetTrigger("ShotTrigger");
            pistolAnimation2.SetTrigger("ShotUsuialBullet");
            weaponController.canPistolShoot = false;
            soundManager.PistolShotSounds();
            //StartCoroutine(cameraFPV.InstantShake(shakeStrength, shakeDuration));

            for (int i = 0; i < effect.Length; i++)
            {
                effect[i].Play();
            }
        }
    }

    void ShotGrenade()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && weaponController.canPistolShootGrenade)
        {
            weaponController.PistolReloadGrenadeStart(reloadTimeForGrenade);
            pistolAnimation.SetTrigger("ShotTrigger");
            Instantiate(grenade, firePoint.transform.position, firePoint.transform.rotation);
            //StartCoroutine(cameraFPV.InstantShake(shakeStrength, shakeDuration));
            soundManager.PistolGrenadeSound();
            weaponController.canPistolShootGrenade = false;
        }
    }
}