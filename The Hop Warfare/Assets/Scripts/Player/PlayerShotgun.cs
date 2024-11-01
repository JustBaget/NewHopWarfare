using System.Collections;
using UnityEngine;

public class PlayerShotgun : MonoBehaviour
{
    [Header("Основные Настройки")]
    public GameObject firePoint;
    public GameObject shotgunModel;
    public Animator handRecoil;
    UltimateSoundScript soundManager;
    CameraFPV cameraFPV;
    Rigidbody playerRB;
    GameObject target;

    [Header("Обычная Атака")]
    public int defaultBulletCount;
    public float defaultReloadTime;
    public float shakeStrength;
    public int shakeDuration; 
    bool canShootDefault = true;
    public GameObject defaultBullet;
    public ParticleSystem[] effectsWeak;

    [Header("Особая Атака")]
    public int specialBulletCount;
    public float specialAttackDuration; //Время для спец атаки
    public float specialAttackTooMuch; //Предупреждение о приближении к ограничению по времени
    public float specialAttackMaxDuration; //Максимум времени для спец атаки
    public float specialAttackKnockdown;
    public float specialAttacFailKnockdown; //Отбрасывание при неудаче
    public float shakeSpecialStrength;
    public int shakeSpecialDuration;
    public float shotgunShakeForce; //Сила тряски дробовика
    public float shakeLongStrength; //Сила тряски камеры во время опасной стадии зарядки дробовика
    public GameObject extraBullet;
    public GameObject explosionDonut;
    public GameObject explosionSphere;
    float startTime; //Время начала спец атаки
    float currentTime; //Сколько прошло с начала спец атаки
    public ParticleSystem[] effectsStrong; //Подсказка: 0 - зеленый; 1 - оранжевый; 2 - красный
    bool usingSpecial = false;
    bool colorChangedToReady = false;
    bool colorChangedToTooMuch = false;
    bool isShaking = false;

    [Header("Индикатор для Особой Атаки")]
    public GameObject indicatorObject;
    public Animator indicator;
    Renderer indicatorColor;
    public ParticleSystem effectWhenReady;
    public ParticleSystem[] fire;
    public ParticleSystem[] loading;
    public Color[] colors;

    void Start()
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
        cameraFPV = GameObject.Find("FPV Camera").GetComponent<CameraFPV>();
        target = GameObject.Find("Target");
        indicator.SetTrigger("Unloading");
        indicatorColor = indicatorObject.GetComponent<Renderer>();
        indicatorColor.material.color = colors[0];
        soundManager = GameObject.Find("SoundManager").GetComponent<UltimateSoundScript>();
    }

    void Update()
    {
        Shot();
        ShotSpecial();
    }

    void Shot()
    {
        if(Input.GetKey(KeyCode.Mouse0) && canShootDefault)
        {
            for (int i = 0; i < defaultBulletCount; i++)
            {
                Instantiate(defaultBullet, firePoint.transform.position, firePoint.transform.rotation);
            }
            
            for (int i = 0; i < effectsWeak.Length; i++)
            {
                effectsWeak[i].Play();
            }

            soundManager.ShotgunShotSounds();
            handRecoil.SetTrigger("ShotgunRecoil");
            StartCoroutine(cameraFPV.InstantShake(shakeStrength, shakeDuration));
            StartCoroutine(Reload());
        }
    }

    void ShotSpecial()
    {
        if(!usingSpecial)
        {
            currentTime = 0; //Чтобы не считалось время вне атаки
        }

        else
        {
            currentTime = Time.time - startTime; //Сколько времени прошло с начала атаки
        }

        if(Input.GetMouseButtonDown(1)) //Кнопка нажата
        {
            Debug.Log("Начало зарядки спец атаки");
            startTime = Time.time; //Начался отсчет до спец атаки
            usingSpecial = true; //Спец атака используется
            canShootDefault = false; //Обычная атака отключена
            indicator.SetTrigger("Loading"); //Началась анимация зарядки
            indicatorColor.material.color = colors[0]; //Меняем цвет на зеленый
            colorChangedToReady = false; //Отмена смены цвета
            colorChangedToTooMuch = false; //Цвет не менялся
            isShaking = true; //Начинаем тряску
            StartCoroutine(ShotgunShake()); //Запускаем корутину тряски
            LoadingParticles(true); //Запускаем зарядку
        }

        else if(Input.GetMouseButtonUp(1) && currentTime >= specialAttackDuration) //Кнопку отпустили И прошло достаточно времени
        {
            Debug.Log("Атака сделана правильно");
            usingSpecial = false; //Щас не используется спец атака
            canShootDefault = true; //Можно делать обычный выстрел
            StartCoroutine(cameraFPV.InstantShake(shakeSpecialStrength, shakeSpecialDuration)); //Тряска камеры
            handRecoil.SetTrigger("ShotgunRecoil"); //Анимация отдачи
            indicator.SetTrigger("Unloading"); //Анимация разрядки индикатора
            Instantiate(explosionDonut, firePoint.transform.position, firePoint.transform.rotation); //Создание колцевого взрыва
            isShaking = false; //Отменяем тряску
            StopCoroutine(ShotgunShake()); //Отменяем корутину тряски
            Fire(false); //Тушим огонь
            LoadingParticles(false); //Отключаем зарядку
            playerRB.AddForce((playerRB.position - target.transform.position).normalized * specialAttackKnockdown, ForceMode.VelocityChange); //Отдача

                for (int i = 0; i < effectsStrong.Length; i++) //Эффекты
                {
                    effectsStrong[i].Play();
                }

                for (int i = 0; i < specialBulletCount; i++) //Выстрел
                {
                    Instantiate(extraBullet, firePoint.transform.position, firePoint.transform.rotation);
                }
        }

        //КОЛХОЗНЫЕ ТАЙМЕРЫ - их задача отслеживать время зажатия и делать что-то, отталикваясь от этого

        if(currentTime >= specialAttackDuration && !colorChangedToReady) //Игрок держит кнопку достаточно времени, а также цвет еще не менялся
        {
            colorChangedToReady = true; //Цвет поменялся внимание
            Debug.Log("Атака готова"); //Смена цвета на готовый к атаке
            indicatorColor.material.color = colors[1]; //Меняем цвет на оранжевый
            effectWhenReady.Play(); //Вспышка при готовности
            StartCoroutine(cameraFPV.InstantShake(shakeLongStrength, 10));
        }

        if(currentTime >= specialAttackTooMuch && !colorChangedToTooMuch) //Игрок держит кнопку опасно долго
        {
            colorChangedToTooMuch = true; //Цвет поменялся на опасный
            Debug.Log("Братан отпускай"); //Смена цвета на готовый к атаке
            indicatorColor.material.color = colors[2]; //Меняем цвет на красный
            Fire(true); //Запуск огня
        }

        if(currentTime >= specialAttackTooMuch && usingSpecial)
        {
            StartCoroutine(cameraFPV.InstantShake(shakeLongStrength, 1));
        }

        if(Input.GetMouseButtonUp(1) && currentTime <= specialAttackDuration && usingSpecial) //Кнопку отпустили, но слишком рано
        {
            Debug.Log("Недостаточно держал. Атака отменена");
            colorChangedToReady = false; //Цвет не менялся
            colorChangedToTooMuch = false; //Цвет не менялся
            indicator.SetTrigger("Unloading"); //Анимация разрядки индикатора
            usingSpecial = false; //Щас не используется спец атака
            canShootDefault = true; //Можно делать обычный выстрел
            isShaking = false; //Отменяем тряску
            StopCoroutine(ShotgunShake()); //Отменяем корутину тряски
            Fire(false); //Тушим огонь
            LoadingParticles(false); //Отключаем зарядку
        }

        if(currentTime >= specialAttackMaxDuration) //Игрок передежрал кнопку
        {
            Debug.Log("Слишком долго. Атака отменена");
            indicator.SetTrigger("Unloading"); //Анимация разрядки индикатора
            usingSpecial = false; //Щас не используется спец атака
            canShootDefault = true; //Можно делать обычный выстрел
            isShaking = false; //Отменяем тряску
            StopCoroutine(ShotgunShake()); //Отменяем корутину тряски
            Fire(false); //Тушим огонь
            LoadingParticles(false); //Отключаем зарядку
            Instantiate(explosionSphere, firePoint.transform.position, firePoint.transform.rotation); //Создаем взрыв
            playerRB.AddForce((playerRB.position - target.transform.position).normalized * specialAttacFailKnockdown, ForceMode.VelocityChange);
            Instantiate(explosionDonut, firePoint.transform.position, firePoint.transform.rotation); //Создание колцевого взрыва
        }
    }

    void Fire(bool startOrStop)
    {
        if(startOrStop)
        {
            for (int i = 0; i < fire.Length; i++) //Запускаем огонь
            {
                fire[i].Play();
            }
        }

        else if(!startOrStop)
        {
            for (int i = 0; i < fire.Length; i++) //Тушим огонь
            {
                fire[i].Stop();
            }
        }
    }

    void LoadingParticles(bool startOrStop)
    {
        if(startOrStop)
        {
            for (int i = 0; i < loading.Length; i++) //Запускаем зарядку
            {
                loading[i].Play();
            }
        }

        else if(!startOrStop)
        {
            for (int i = 0; i < loading.Length; i++) //Убираем зарядку
            {
                loading[i].Stop();
            }
        }
    }

    IEnumerator ShotgunShake()
    {
        Vector3 startPos = shotgunModel.transform.localPosition;
        float shotgunShakeStartForce = shotgunShakeForce;

        while(isShaking)
        {
            shotgunModel.transform.localPosition += new Vector3(Random.Range(-shotgunShakeStartForce, shotgunShakeStartForce), Random.Range(-shotgunShakeStartForce, shotgunShakeStartForce), Random.Range(-shotgunShakeStartForce, shotgunShakeStartForce));
            yield return new WaitForSeconds(0.01f);
            shotgunModel.transform.localPosition = startPos;
            shotgunShakeStartForce += shotgunShakeStartForce * 0.002f;
        }

        shotgunModel.transform.localPosition = startPos;
    }

    IEnumerator Reload()
    {
        canShootDefault = false;
        yield return new WaitForSeconds(defaultReloadTime);
        canShootDefault = true;
    }
}