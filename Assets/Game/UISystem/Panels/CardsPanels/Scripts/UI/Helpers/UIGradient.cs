using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class UIGradient : BaseMeshEffect
{
    [System.Serializable]
    public class GradientPreset
    {
        public Color topColor = Color.white;
        public Color bottomColor = Color.black;
    }

    public Color color1 = Color.white;
    public Color color2 = Color.black;
    public bool isHorizontal = false;
    
    [SerializeField]
    private List<GradientPreset> gradientPresets = new List<GradientPreset>();
    
    public List<GradientPreset> GradientPresets => gradientPresets;
    
    [SerializeField]
    private float transitionDuration = 0.2f;
    
    private Coroutine currentTransition;

    [SerializeField]
    private int currentPresetId = 0;
    
    public int CurrentPreset
    {
        get => currentPresetId;
        set => TransitionToPreset(value);
    }

    // Sets the color
    public void SetColor(Color inputColor)
    {
        // Set
        color1 = inputColor;
        color2 = new Color(0.85f * color1.r, 0.85f * color1.g, 0.85f * color1.b, inputColor.a >= 1f ? 1 : inputColor.a);

        // Update
        if (graphic)
            graphic.SetVerticesDirty();
    }

    // Modifies the mesh
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        var vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);

        float min = float.MaxValue;
        float max = float.MinValue;

        for (int i = 0; i < vertexList.Count; i++)
        {
            float value = isHorizontal ? vertexList[i].position.x : vertexList[i].position.y;
            if (value > max) max = value;
            if (value < min) min = value;
        }

        float range = max - min;

        for (int i = 0; i < vertexList.Count; i++)
        {
            var uiVertex = vertexList[i];
            float value = isHorizontal ? uiVertex.position.x : uiVertex.position.y;
            float normalizedValue = (value - min) / range;
            uiVertex.color = Color.Lerp(color2, color1, normalizedValue);
            vertexList[i] = uiVertex;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);
    }

    public void TransitionToPreset(int presetId)
    {
        if (presetId < 0 || presetId >= gradientPresets.Count)
        {
            Debug.LogWarning($"Preset ID {presetId} is out of range!");
            return;
        }

        currentPresetId = presetId;

        // Если объект неактивен, просто установим целевые цвета без анимации
        if (!gameObject.activeSelf)
        {
            color1 = gradientPresets[presetId].topColor;
            color2 = gradientPresets[presetId].bottomColor;
            if (graphic)
                graphic.SetVerticesDirty();
            return;
        }

        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(TransitionGradientCoroutine(
            gradientPresets[presetId].topColor,
            gradientPresets[presetId].bottomColor
        ));
    }

    private IEnumerator TransitionGradientCoroutine(Color targetColor1, Color targetColor2)
    {
        Color startColor1 = color1;
        Color startColor2 = color2;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            color1 = Color.Lerp(startColor1, targetColor1, t);
            color2 = Color.Lerp(startColor2, targetColor2, t);

            if (graphic)
                graphic.SetVerticesDirty();

            yield return null;
        }

        color1 = targetColor1;
        color2 = targetColor2;
        currentTransition = null;
    }

    public void SetGradientColors(Color newColor1, Color newColor2)
    {
        color1 = newColor1;
        color2 = newColor2;
        
        if (graphic)
            graphic.SetVerticesDirty();
    }
}