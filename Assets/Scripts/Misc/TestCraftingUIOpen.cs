using FP.Input;
using FP.Player.Combat;
using FP.Player.Core;
using FP.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PLACEHOLDER FOR TEST
public class TestCraftingUIOpen : MonoBehaviour
{
    [SerializeField] private GameObject _openText;

    private bool _playerInside;
    private WeaponController _wc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _openText.SetActive(true);
            _playerInside = true;
            _wc = other.GetComponent<PlayerController>().WeaponController;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _openText.SetActive(false);
            _playerInside = false;
            UIManager.Instance.CraftingUI.CloseUI();
        }
    }

    private void Update()
    {
        if (_playerInside && InputManager.Instance.IsActionTriggered("OpenUI"))
        {
            UIManager.Instance.CraftingUI.OpenUI(_wc);
        }
    }

}
