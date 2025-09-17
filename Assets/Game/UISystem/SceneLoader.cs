using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SlicedFilledImage filler;
    public Transform barIconTr;
    public Vector2 rangeXPosBarIcon;
    public TMP_Text progressText;
    private float progress,
        barProgress;
    private AsyncOperation operation;

    private int sceneIndex;
    private bool canLoad;

    private void Start()
    {
        if (barIconTr)
            barIconTr.localPosition = new Vector3(rangeXPosBarIcon.x, barIconTr.localPosition.y, 0);
    }

    public void LoadScene(int sceneIndexToLoad)
    {
        sceneIndex = sceneIndexToLoad;
        gameObject.SetActive(true);
        StartCoroutine(AsyncLoading());
    }

    private void Update()
    {
        canLoad = true;
        if (operation != null)
            progress = operation.progress / 0.9f;

        if (barProgress < progress)
        {
            barProgress += Time.deltaTime;
            barProgress = Mathf.Clamp(barProgress, 0, 1);
            filler.fillAmount = barProgress;
            progressText.text = $"{Math.Round(barProgress * 100, 0)}%";

            if (barIconTr)
            {
                var pixelsPerUnit = (rangeXPosBarIcon.y - rangeXPosBarIcon.x) / 100;
                barIconTr.localPosition = new Vector3(
                    rangeXPosBarIcon.x + pixelsPerUnit * barProgress * 100,
                    barIconTr.localPosition.y,
                    0
                );
            }

            if (barProgress == 1)
                operation.allowSceneActivation = true;
        }
    }

    private IEnumerator AsyncLoading()
    {
        yield return new WaitUntil(() => canLoad);
        operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
    }
}
