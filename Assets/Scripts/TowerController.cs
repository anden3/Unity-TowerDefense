using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour {
    static private Text sellValue;
    static private CanvasGroup towerOptionsCanvas = null;

    public float AttackRange {
        get { return attackRange; }
        set {
            attackRange = value;
            radiusVisualizer.transform.localScale = new Vector3(attackRange * 2, attackRange * 2, 1.0f);
        }
    }

    [Header("Tower Settings")]
    public new string name;
    public int cost;
    public float fireDelay;
    [SerializeField] private float attackRange;
    public bool shouldRotate;

    [Header("Tower Modules")]
    public Projectile projectile;
    public GameObject fireObject;
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

    private int towerValue {
        get { return _towerValue; }
        set {
            _towerValue = value;
            sellValue.text = Mathf.RoundToInt(_towerValue * 0.8f).ToString();
        }
    }

    private int _towerValue;

    public static void Initialize() {
        sellValue = GameObject.FindGameObjectWithTag("TowerSellValue").GetComponent<Text>();
        towerOptionsCanvas = GameObject.FindGameObjectWithTag("TowerOptions").GetComponent<CanvasGroup>();
        towerOptionsCanvas.alpha = 0;
    }

    private static bool HasParentWithTag(GameObject obj, string tag) {
        Transform currentTransform = obj.transform;

        while (currentTransform.parent != null) {
            if (currentTransform.parent.tag == tag) {
                return true;
            }

            currentTransform = currentTransform.parent;
        }

        return false;
    }

    private void Awake() {
        towerValue = cost;
        towerCollider = gameObject.GetComponent<BoxCollider2D>();

        fireFunction = fireObject.GetComponent<FireFunction>();
        fireFunction.Initialize(gameObject);
    }

    private void Start() {
        radiusVisualizer = transform.GetChild(0).gameObject;
        radiusSprite = radiusVisualizer.GetComponent<SpriteRenderer>();
        radiusTransparency = radiusSprite.color.a;

        radiusVisualizer.transform.localScale = new Vector3(attackRange * 2, attackRange * 2, 1.0f);

        backgroundTexture = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().sprite.texture;
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
                    if (selectedUIElement != null && HasParentWithTag(selectedUIElement, "TowerOptions")) {
                        highlighted = true;
                    }
                }

                radiusSprite.color = highlighted ?
                            new Color(1, 1, 1, 0.5f) :
                            new Color(0, 0, 0, 0);

                foreach (Button b in upgradeButtons) {
                    b.gameObject.SetActive(highlighted);
                }

                // Keep track of which tower is currently selected.
                if (highlighted) {
                    GameController.instance.selectedTower = this;
                    towerOptionsCanvas.alpha = 1.0f;
                }
                else if (GameController.instance.selectedTower == this) {
                    GameController.instance.selectedTower = null;
                    towerOptionsCanvas.alpha = 0.0f;
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
                suitableLocation = CheckIfOnSuitableTerrain();

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

    public int Sell() {
        towerOptionsCanvas.alpha = 0.0f;

        foreach (Button b in upgradeButtons) {
            Destroy(b.gameObject);
        }

        GameController.instance.selectedTower = null;
        Destroy(gameObject);

        return Mathf.RoundToInt(towerValue * 0.8f);
    }

    private bool CheckIfOnSuitableTerrain() {
        Vector2[] points = {
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
        towerValue += upgrade.cost;

        upgradeButtons.Remove(button);
        Destroy(button.gameObject);
    }
}
