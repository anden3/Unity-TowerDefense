using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {
    private bool placedDown = false;
    private bool suitableLocation = true;

    private SpriteRenderer towerSprite;
    private CircleCollider2D col;

    private GameObject radius;
    private SpriteRenderer radiusSprite;
    private float radiusTransparency;

    private Texture2D backgroundTexture;

    private float maxColorDifferenceSqr = 1.0f;

    private void Awake() {
        towerSprite = gameObject.GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<CircleCollider2D>();

        radius = transform.GetChild(0).gameObject;
        radiusSprite = radius.GetComponent<SpriteRenderer>();
        radiusTransparency = radiusSprite.color.a;

        backgroundTexture = (Texture2D)GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().sprite.texture;
    }
	
	private void Update () {
        if (!placedDown) {
            if (suitableLocation && Input.GetMouseButtonDown(0)) {
                placedDown = true;
                Destroy(radius);
            }
            else {
                // Make tower follow the mouse.
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                transform.position = mousePosition;

                CheckIfOnGrass();

                /*
                bool locationWasSuitable = suitableLocation;
                suitableLocation = CheckIfOnGrass();

                // Check if need to update radius color.
                if (suitableLocation != locationWasSuitable) {
                    radiusSprite.color = suitableLocation ?
                        new Color(1, 1, 1, radiusTransparency) :
                        new Color(1, 0, 0, radiusTransparency);
                }
                */
            }
        }
	}

    private bool CheckIfOnGrass() {
        Vector2 pixelCoord = Input.mousePosition;
        radiusSprite.color = new Color(0, 0, 0, 0);
        // radiusSprite.color = backgroundTexture.GetPixel((int)pixelCoord.x, (int)pixelCoord.y);
        backgroundTexture.SetPixel((int)pixelCoord.x, (int)pixelCoord.y, new Color(1.0f, 0.0f, 0.0f));
        backgroundTexture.Apply();

        /*
        Vector3 grassSample = new Vector3(0.020f, 0.527f, 0.008f);

        Vector2[] points = new Vector2[4] {
            towerSprite.bounds.min,
            (Vector2)towerSprite.bounds.min + new Vector2(towerSprite.bounds.size.x, 0),
            (Vector2)towerSprite.bounds.min + new Vector2(0, towerSprite.bounds.size.y),
            towerSprite.bounds.max
        };

        int i = 0;

        foreach (Vector2 point in points) {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(point);
            Color c = backgroundTexture.GetPixel((int)screenPoint.x, (int)screenPoint.y);

            // Debug.Log(i + " " + screenPoint);
            i++;

            float diffSqr = (
                grassSample - new Vector3(c.r, c.g, c.b)
            ).sqrMagnitude;

            // Debug.Log(c);
            // Debug.Log(diffSqr);

            if (diffSqr > maxColorDifferenceSqr) {
                return false;
            }
        }
        */

        return true;
    }
}
