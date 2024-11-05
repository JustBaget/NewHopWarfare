using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float damageMultiplier;
    public float hp;
    public bool isMain;
    public Health main;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && isMain)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(float damage)
    {
        main.hp -= damage * damageMultiplier;
        Debug.Log("Damage, -"+(damage * damageMultiplier)+", multiplier x"+(damageMultiplier));
    }
}
