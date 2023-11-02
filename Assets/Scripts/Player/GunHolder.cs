using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gunSpriteRenderer;

    void Update()
    {
        Vector2 mouseDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        transform.up = mouseDir;

        if (mouseDir.x < 0)
            gunSpriteRenderer.flipY = true;
        else
            gunSpriteRenderer.flipY = false;

    }
}
