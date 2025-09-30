using UnityEngine;

public class ContainerButtonSizeChanger : MonoBehaviour
{
    [SerializeField] private ButtonClickInvoker[] buttons;
    [SerializeField] private ButtonClickInvoker startButton;
    [SerializeField] private Vector2 defaultSize;
    [SerializeField] private Vector2 clickedSize;

    private const float SIZE_CONVERT_MODIFIER = 720;

    private void Awake()
    {
        foreach (var btn in buttons)
            btn.ButtonClicked += ChangeSize;

        ChangeSize(startButton);
    }

    public void ChangeSize(ButtonClickInvoker targetButton)
    {
        var rect = gameObject.GetComponent<RectTransform>();
        var arr = new Vector3[4];
        rect.GetLocalCorners(arr);
        var containerSizeX = arr[3].x - arr[0].x;
        var sizeX = defaultSize.x * (containerSizeX / SIZE_CONVERT_MODIFIER);
        var clickedSizeX = clickedSize.x * (containerSizeX / SIZE_CONVERT_MODIFIER);

        foreach (var button in buttons)
        {
            if (button == targetButton)
                targetButton.GetComponent<RectTransform>().sizeDelta = new Vector2(clickedSizeX, 0);
            else
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, 0);
        }
    }

    private void OnDestroy()
    {
        foreach (var btn in buttons)
            btn.ButtonClicked -= ChangeSize;
    }
}