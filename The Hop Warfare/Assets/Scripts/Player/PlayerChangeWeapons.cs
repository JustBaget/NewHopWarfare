using UnityEngine;

public class PlayerChangeWeapons : MonoBehaviour
{
    public int currentGun;
    public GameObject[] weapons;
    void Start()
    {
        
    }

    void Update()
    {
        ChangeWeapons();
    }

    void ChangeWeapons()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Убрана пушка - " + weapons[currentGun] + ", Вытащена пушка - " + weapons[(currentGun + 1) % weapons.Length]);
            weapons[currentGun].SetActive(false);
            weapons[(currentGun + 1) % weapons.Length].SetActive(true);

            currentGun++;
            if(currentGun >= weapons.Length)
            {
                currentGun = 0;
            }
        }
    }
}
