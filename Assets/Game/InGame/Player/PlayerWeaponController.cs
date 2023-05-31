// 日本語対応
using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] weapons = default;

    public void ActivateWeapon(int index)
    {
        weapons[index].SetActive(true);
    }
    public void DeactivateWeapon(int index)
    {
        weapons[index].SetActive(false);
    }
}