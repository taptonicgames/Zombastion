using System.Collections.Generic;
using UnityEngine;

public class UpgradesSavingData : AbstractSavingData
{
    [SerializeField]
    private Dictionary<CharacterType, List<GetUpgradeConfigDTO>> pairs = new();

    public override void ResetData(int flag = 0)
    {
        pairs.Clear();
    }

    public IEnumerable<GetUpgradeConfigDTO> GetUpgradeConfigDTOs(CharacterType characterType)
    {
        pairs.TryGetValue(characterType, out var configDTOs);
        return configDTOs != null ? configDTOs : new();
    }

    public void AddUpgradeConfigDTO(GetUpgradeConfigDTO configDTO)
    {
        if (pairs.TryGetValue(configDTO.characterType, out var configDTOs))
        {
            if (pairs[configDTO.characterType] == null)
                pairs[configDTO.characterType] = new();

            pairs[configDTO.characterType].Add(configDTO);
        }
        else
        {
            pairs.Add(configDTO.characterType, new() { configDTO });
        }
    }

    protected override void SaveDataObject() { }
}
