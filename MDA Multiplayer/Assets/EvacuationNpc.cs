using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacuationNpc : MonoBehaviour
{





    public void OnInteracted()
    {
        GameManager.Instance.OnEvacuateNPCClicked();
    }
}
