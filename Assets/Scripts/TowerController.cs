using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour {
    public float radius;
    public float rotationSpeed;

    private bool placedDown = false;

    private BoxCollider2D towerCollider;

    private GameObject radiusVisualizer;
    private SpriteRenderer radiusSprite;
    private float radiusTransparency;

    private Texture2D backgroundTexture;

    private bool suitableLocation = true;
    private Vector3 suitableLocationColor = new Vector3(0.020f, 0.527f, 0.008f);
    private const float maxColorDifferenceSqr = 0.1f;

    private Vector3 lastMousePosition = new Vector3();

    private List<Enemy> enemies = new List<Enemy>();

    private void Awake() {
        towerCollider = gameObject.GetComponent<BoxCollider2D>();

        radiusVisualizer = transform.GetChild(0).gameObject;
        radiusSprite = radiusVisualizer.GetComponent<SpriteRenderer>();
        radiusTransparency = radiusSprite.color.a;

        radiusVisualizer.transform.localScale = new Vector3(radius * 2, radius * 2, 1.0f);

        backgroundTexture = (Texture2D)GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().sprite.texture;
    }
	
	private void Update () {
        if (!placedDown) {
            if (suitableLocation && Input.GetMouseButtonDown(0)) {
                placedDown = true;
                radiusSprite.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
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

                CheckIfOnGrass();

                bool locationWasSuitable = suitableLocation;
                suitableLocation = CheckIfOnGrass();

                // Check if need to update radius color.
                if (suitableLocation != locationWasSuitable) {
                    radiusSprite.color = suitableLocation ?
                        new Color(1, 1, 1, radiusTransparency) :
                        new Color(1, 0, 0, radiusTransparency);
                }
            }
        }
	}

    private void FixedUpdate() {
        if (!placedDown || enemies.Count == 0) {
            return;
        }

        Enemy target = enemies[0];

        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void EnemySpotted(Enemy enemy) {
        enemies.Add(enemy);
    }

    public void EnemyOutOfRange(Enemy enemy) {
        enemies.Remove(enemy);
    }

    private bool CheckIfOnGrass() {
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

            float diffSqr = (
                suitableLocationColor - new Vector3(c.r, c.g, c.b)
            ).sqrMagnitude;

            if (diffSqr > maxColorDifferenceSqr) {
                return false;
            }
        }

        return true;
    }
}
