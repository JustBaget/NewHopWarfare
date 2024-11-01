using UnityEngine;

public class PistolCanShootIndicators : MonoBehaviour
{
    public Color inactiveColor;
    public Color activeColor;
    public GameObject pistol;
    Renderer render;
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    
    void Update()
    {
        if(pistol.GetComponent<PlayerPistol>().canShotGrenade)
        {
            render.material.color = activeColor;
        }

        else
        {
            render.material.color = inactiveColor;
        }
    }
}
