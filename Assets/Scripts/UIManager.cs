
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject panel;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionText;
    public void ShowInfo(string name, string description)
    {
        panel.SetActive(true);
        nameText.text = name;
        descriptionText.text = description;
    }

    public void HideInfo()
    {
        panel.SetActive(false);
    }
}
