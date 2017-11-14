﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class TowerController : MonoBehaviour {
    static public CanvasGroup towerOptionsCanvas = null;

    public float AttackRange {
        get { return attackRange; }
        set {
            attackRange = value;
            radiusVisualizer.transform.localScale = new Vector3(attackRange * 2, attackRange * 2, 1.0f);
        }
    }

    [Header("Tower Settings")]
    public int cost;
    public float fireDelay;
    [SerializeField] private float attackRange;
    public bool shouldRotate;

    [Header("Tower Modules")]
    public Projectile projectile;
    public MonoScript fireScript;
    public List<Upgrade> upgrades;

    [Header("Placement Settings")]
    public Color suitableLocationColor = new Color(0.020f, 0.527f, 0.008f);
    public float colorTolerance = 0.1f;

    [Header("UI References")]
    public Button upgradeButton;

    [HideInInspector] public int projectileDurability = 1;

    private bool placedDown = false;
    private bool suitableLocation = true;
    private bool highlighted = false;

    private FireFunction fireFunction;

    private BoxCollider2D towerCollider;

    private GameObject radiusVisualizer;
    private SpriteRenderer radiusSprite;
    private float radiusTransparency;

    private Texture2D backgroundTexture;

    private Vector3 lastMousePosition;

    private Enemy target = null;
    private List<Enemy> enemies = new List<Enemy>();
    private List<Button> upgradeButtons = new List<Button>();

    private void Awake() {
        towerCollider = gameObject.GetComponent<BoxCollider2D>();

        fireFunction = gameObject.AddComponent(Type.GetType(fireScript.name)) as FireFunction;
        fireFunction.Initialize(gameObject);
    }

    private void Start() {
        radiusVisualizer = transform.GetChild(0).gameObject;
        radiusSprite = radiusVisualizer.GetComponent<SpriteRenderer>();
        radiusTransparency = radiusSprite.color.a;

        radiusVisualizer.transform.localScale = new Vector3(attackRange * 2, attackRange * 2, 1.0f);

        backgroundTexture = (Texture2D)GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().sprite.texture;
        GameObject upgradeWindow = GameObject.FindGameObjectWithTag("UpgradeWindow");

        foreach (Upgrade u in upgrades) {
            Button button = Instantiate(upgradeButton, upgradeWindow.transform);

            Text upgradeName = button.transform.GetChild(0).GetComponent<Text>();
            Text upgradeCost = button.transform.GetChild(1).GetComponent<Text>();

            upgradeName.text = u.name;
            upgradeCost.text = u.cost.ToString();

            button.onClick.AddListener(delegate {
                AddUpgrade(button, u);
            });

            button.gameObject.SetActive(false);
            upgradeButtons.Add(button);
        }
    }
	
	private void Update () {
        if (placedDown) {
            if (target != null && fireFunction.canFire) {
                if (shouldRotate) {
                    Vector3 vectorToTarget = target.transform.position - transform.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }

                fireFunction.canFire = false;
                StartCoroutine(fireFunction.Fire(target));
            }

            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)),
                    Vector2.zero
                );

                bool wasHighlighted = highlighted;
                highlighted = false;

                foreach (RaycastHit2D hit in hits) {
                    if (hit.collider.gameObject == gameObject && !wasHighlighted) {
                        highlighted = true;
                        break;
                    }
                }

                // Check if highlighted status was removed.
                if (wasHighlighted && !highlighted) {
                    GameObject selectedUIElement = EventSystem.current.currentSelectedGameObject;

                    // Keep highlight status if element clicked was an upgrade.
                    if (selectedUIElement != null && selectedUIElement.tag == "UpgradeButton") {
                        highlighted = true;
                    }
                }

                radiusSprite.color = highlighted ?
                            new Color(1, 1, 1, 0.5f) :
                            new Color(0, 0, 0, 0);

                towerOptionsCanvas.alpha = highlighted ? 1.0f : 0.0f;

                foreach (Button b in upgradeButtons) {
                    b.gameObject.SetActive(highlighted);
                }
            }
        }
        else {
            // Cancel placement.
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) {
                Destroy(gameObject);
                return;
            }

            if (suitableLocation && Input.GetMouseButtonDown(0)) {
                placedDown = true;
                radiusSprite.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                GameController.instance.Money -= cost;
            }
            else {
                // Make tower follow the mouse.
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;

                // Don't bother recalculating if the mouse hasn't moved.
                if (mousePosition == lastMousePosition) {
                    return;
                }

                lastMousePosition = mousePosition;
                transform.position = mousePosition;

                bool locationWasSuitable = suitableLocation;
                suitableLocation = CheckIfOnSuitableTerrarin();

                // Check if need to update radius color.
                if (suitableLocation != locationWasSuitable) {
                    radiusSprite.color = suitableLocation ?
                        new Color(1, 1, 1, radiusTransparency) :
                        new Color(1, 0, 0, radiusTransparency);
                }
            }
        }
	}

    public void EnemySpotted(Enemy enemy) {
        enemies.Add(enemy);

        if (target == null) {
            target = enemies[0];
        }
    }

    public void EnemyOutOfRange(Enemy enemy) {
        enemies.Remove(enemy);

        // Make sure target doesn't turn invalid.
        target = (enemies.Count > 0) ? enemies[0] : null;
    }

    private bool CheckIfOnSuitableTerrarin() {
        Vector2[] points = new Vector2[5] {
            towerCollider.bounds.center,
            towerCollider.bounds.min,
            (Vector2)towerCollider.bounds.min + new Vector2(towerCollider.bounds.size.x, 0),
            (Vector2)towerCollider.bounds.min + new Vector2(0, towerCollider.bounds.size.y),
            towerCollider.bounds.max
        };

        foreach (Vector2 point in points) {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(point);
            Color c = backgroundTexture.GetPixel((int)screenPoint.x, (int)screenPoint.y);
            Color x = suitableLocationColor;
                
            float diffSqr = (
                new Vector3(x.r, x.g, x.b) - new Vector3(c.r, c.g, c.b)
            ).sqrMagnitude;

            if (diffSqr > colorTolerance) {
                return false;
            }
        }

        return true;
    }

    private void AddUpgrade(Button button, Upgrade upgrade) {
        if (upgrade.cost > GameController.instance.Money) {
            return;
        }

        GameController.instance.Money -= upgrade.cost;
        upgrade.AddUpgrade(gameObject);

        upgradeButtons.Remove(button);
        Destroy(button.gameObject);
    }
}
