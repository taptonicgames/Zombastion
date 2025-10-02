using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractUIManager : MonoBehaviour
{
    [SerializeField] private List<AbstractPanel> Panels;

    public virtual void Init()
    {
        foreach (var panel in Panels)
            panel.Init();
    }

    protected AbstractPanel GetPanel(PanelType type)
    {
        return Panels.First(p => p.Type == type);
    }

    protected void HideAllPanels()
    {
        foreach (var panel in Panels)
            panel.Hide();
    }
}