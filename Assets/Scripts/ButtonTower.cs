using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour {
    public TowerController tower;

    private Button button;
    private Text towerCostField;

	private void Awake() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);

        towerCostField = transform.GetChild(0).GetComponent<Text>();
        towerCostField.text = tower.cost.ToString();
    }

    private void ButtonClicked() {
        int towerCost = tower.cost;

        if (towerCost > GameController.instance.Money) {
            return;
        }

        Instantiate(tower.gameObject, Input.mousePosition, Quaternion.identity);
    }
}
