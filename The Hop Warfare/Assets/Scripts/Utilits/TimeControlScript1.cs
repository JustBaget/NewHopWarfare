
using UnityEngine;
public class TimeControlScript : MonoBehaviour
{
    public float time;
    public float slowTime;
    void Update()
    {
        Time.timeScale = time;

        if(Input.GetKey(KeyCode.Alpha1))
        {
            time = slowTime;
        }
    }
}
