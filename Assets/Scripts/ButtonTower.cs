using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private static Text towerInfoName;
    private static Text towerInfoCost;

    public TowerController tower;

    private Button button;

    public static void Initialize() {
        towerInfoName = GameObject.FindGameObjectWithTag("TowerInfoName").GetComponent<Text>();
        towerInfoCost = GameObject.FindGameObjectWithTag("TowerInfoCost").GetComponent<Text>();
    }

	private void Awake() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        towerInfoName.text = tower.name;
        towerInfoCost.text = tower.cost.ToString();
    }

    public void OnPointerExit(PointerEventData eventData) {
        towerInfoName.text = "";
        towerInfoCost.text = "";
    }

    private void ButtonClicked() {
        if (tower.cost > GameController.instance.Money) {
            return;
        }

        Instantiate(tower.gameObject, Input.mousePosition, Quaternion.identity);
    }
}
