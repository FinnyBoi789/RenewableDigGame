
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject scanPanel;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] private TMPro.TextMeshProUGUI progressLabel;
    [SerializeField] private Slider scanProgressSlider;

    public void ShowInfo(string name, string description)
    {
        infoPanel.SetActive(true);
        scanPanel.SetActive(false);
        nameText.text = name;
        descriptionText.text = description;
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }

    public void ShowScanProgress(float percent)
    {
        scanPanel.SetActive(true);
        infoPanel.SetActive(false);

        scanProgressSlider.value = percent;
        progressLabel.text = "Scanning... " + Mathf.RoundToInt(percent * 100f) + "%";
    }

    public void ShowMineProgress(float percent)
    {
        scanPanel.SetActive(true);
        infoPanel.SetActive(false);

        scanProgressSlider.value = percent;
        progressLabel.text = "Mining... " + Mathf.RoundToInt(percent * 100f) + "%";
    }

    public void HideProgress()
    {
        scanPanel.SetActive(false);
    }
}
