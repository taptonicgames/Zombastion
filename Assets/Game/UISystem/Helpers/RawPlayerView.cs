using System;
using UnityEngine;

public class RawPlayerView : MonoBehaviour
{
    [SerializeField] private Transform viewer;
    [SerializeField] private float speed;
    [SerializeField] private Transform[] models; // test

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

    public void ChangePlayerModel(int indexPlayerModel)
    {
        foreach (var model in models)
            model.gameObject.SetActive(false);

        models[indexPlayerModel].gameObject.SetActive(true);
    }

    public void ChangeRotateActiveState(bool isActive)
    {
        isRotateActive = isActive;
    }

    private void Update()
    {
        if (isRotateActive == false)
            return;

        float axisRotationX = Input.GetAxis("Mouse X");
        viewer.transform.Rotate(Vector3.up * -axisRotationX * speed * Time.deltaTime);
    }

}