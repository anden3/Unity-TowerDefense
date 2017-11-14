using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSell : MonoBehaviour {
    private Button button;

    private void Awake() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked() {
        Debug.Assert(GameController.instance.selectedTower != null);

        int refundedMoney = GameController.instance.selectedTower.Sell();
        GameController.instance.Money += refundedMoney;
    }
}
