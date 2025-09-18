using System;
using System.Collections.Generic;
using Zenject;

[Serializable]
public class UnitActionPermissionHandler : IInitializable, ITickable
{
    public List<UnitActionPermission> permissions = new();
    public int unitsPerFrame = 10;
    private Dictionary<UnitActionType, UnitActionPermission> permissionsPairs = new();
    private int counter;
    private Dictionary<AbstractUnit, bool> unitsWhoAskedPair = new();

    [Serializable]
    public class UnitActionPermission
    {
        public UnitActionType type;
        public List<UnitActionType> permissionsList = new();
        private Dictionary<UnitActionType, bool> permissionDictionary = new();

        public void FillDictionary()
        {
            for (int i = 0; i < permissionsList.Count; i++)
            {
                permissionDictionary.Add(permissionsList[i], true);
            }
        }

        public bool CheckActionType(UnitActionType type)
        {
            return permissionDictionary.ContainsKey(type);
        }
    }

    public void Initialize()
    {
        foreach (var item in permissions)
        {
            item.FillDictionary();
            permissionsPairs.Add(item.type, item);
        }
    }

    public void Tick()
    {
        if (counter == 0)
            unitsWhoAskedPair.Clear();
        counter = 0;
    }

    public bool CheckPermission(UnitActionType askType, UnitActionType activeType)
    {
        return permissionsPairs[askType].CheckActionType(activeType);
    }

    public bool CanIAskPermission(AbstractUnit unit)
    {
        if (counter >= unitsPerFrame)
            return false;
        if (!unitsWhoAskedPair.TryGetValue(unit, out var b))
            unitsWhoAskedPair.Add(unit, true);
        else
            return false;
        counter++;
        return true;
    }
}
