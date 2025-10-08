using UnityEngine;

public class RawPlayerView : MonoBehaviour
{
    [SerializeField] private Transform viewer;
    [SerializeField] private float speed;

    private bool isRotateActive;
    private Vector3 defaultRotate;

    private void Awake()
    {
        defaultRotate = viewer.localEulerAngles;
    }

    public void Restart()
    {
        viewer.transform.localEulerAngles = defaultRotate;
    }

    #region event trigger
    public void ChangeRotateActiveState(bool isActive)
    {
        isRotateActive = isActive;
    }
    #endregion

    private void Update()
    {
        if (isRotateActive == false)
            return;

        float axisRotationX = Input.GetAxis("Mouse X");
        viewer.transform.Rotate(Vector3.up * -axisRotationX * speed * Time.deltaTime);
    }
}