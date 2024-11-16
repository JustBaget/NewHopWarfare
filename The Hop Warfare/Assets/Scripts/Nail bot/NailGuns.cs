using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGuns : MonoBehaviour
{
    public Transform spawn; // Позиция, откуда будут вылетать гвозди
    public GameObject nail; // Префаб гвоздя
    public GameObject aim; // Объект прицела (можно использовать для отображения)
    public Transform player; // Ссылка на игрока
    public float rotationSpeed; // Скорость вращения (если нужно)
    public float shootingDistance = 10f; // Максимальное расстояние для стрельбы
    public float shootingInterval = 1f; // Интервал между выстрелами
    public bool canShoot = true; // Могу ли я стрелять
    public bool isRunningAway; // Убегает ли дрон

    void Start()
    {

    }

    void Update()
    {
        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если игрок близко
        if (distanceToPlayer < shootingDistance && !isRunningAway)
        {
            // Поворачиваем дрон к игроку (можно использовать, если хотите следить за игроком)
            Vector3 direction = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Стрельба
            if (canShoot)
            {
                canShoot = false; // Убедимся, что не стреляем постоянно
                Instantiate(nail, spawn.position, spawn.rotation);
                StartCoroutine(CD(shootingInterval)); // Ждем перед следующим выстрелом
            }
        }
        else
        {
            // Если игрок далеко, можно выполнить логику убегания
            if (isRunningAway)
            {
                // Логика убегания (например, движение в сторону противоположную к игроку)
                Vector3 runDirection = (transform.position - player.position).normalized;
                transform.position += runDirection * Time.deltaTime * 5; // 5 - скорость убегания
            }
        }
    }

    IEnumerator CD(float interval)
    {
        yield return new WaitForSeconds(interval);
        canShoot = true; // Разрешаем стрелять снова через указанный интервал
    }
}