using UnityEngine;

public class RotateAroundX : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45f; // скорость вращения в градусах в секунду

    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
