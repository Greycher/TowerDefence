using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _crossObject;

    public void UpdateIndication(bool blocked)
    {
        _crossObject.SetActive(blocked);
    }
}
