using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RevealPropOnPatient : Action
{
    [Header("Prefab References")]
    [SerializeField] private Props _prop;

    [Header("Item Name")]
    [SerializeField] private string _itemName;

    public void InstantiateOnPatient()
    {
        GetActionData();

        if (CurrentPatient.IsPlayerJoined(LocalPlayerData))
        {
            int propIndex = (int)_prop;
            CurrentPatient.PropList[propIndex].SetActive(true);

            TextToLog = $"Used {_itemName}";

            if (_shouldUpdateLog)
            {
                LogText(TextToLog);
            }
        }
    }
}
