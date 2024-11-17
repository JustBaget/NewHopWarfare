using UnityEngine;

public class DoorSingle : MonoBehaviour
{
    public Transform doorObj;
    public Vector3 targetPosition;
    public float animDuration = 1f; // Длительность анимации в секундах

    private bool isAnimating = false;
    private float animationProgress = 0f;

    void Update()
    {
        if (isAnimating)
        {
            animationProgress += Time.deltaTime / animDuration;
            doorObj.position = Vector3.Lerp(doorObj.position, targetPosition, animationProgress);

            if (animationProgress >= 1f)
            {
                isAnimating = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAnimating)
        {
            isAnimating = true;
            Debug.Log("Дверь открылась");
        }
    }
}
