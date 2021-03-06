﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNextWave : MonoBehaviour {
    private Button button;

    private void Awake() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked() {
        button.interactable = false;
        StartCoroutine(WaitForWaveCompletion());
    }

    public IEnumerator WaitForWaveCompletion() {
        yield return new WaitUntil(() => GameController.instance.waveInProgress == false);
        button.interactable = true;
    }
}
