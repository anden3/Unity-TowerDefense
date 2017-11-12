using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour {
    public GameObject tower;

    private Button button;

	private void Awake() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked() {
        Instantiate(tower, Input.mousePosition, Quaternion.identity);
    }
}
