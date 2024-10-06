using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public Texture2D defaultCursor;
    public Vector2 defaultHotspot;
    public Texture2D ItemCursor;
    public Vector2 itemHotspot;
    public Texture2D enemyCursor;
    public Vector2 enemyHotspot;

    public CursorManager(Texture2D cursorDefault, Texture2D cursorItem, Texture2D cursorEnemy)
    {
        this.defaultCursor = cursorDefault;
        this.ItemCursor = cursorItem;
        this.enemyCursor = cursorEnemy;
    }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            SetCursor(defaultCursor, defaultHotspot);
        } else {
            Destroy(gameObject);
        }
    }

    public void SetCursor(Texture2D cursorTexture, Vector2 cursorHotspot)
    {
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    public void ToggleCursor(bool showCursor)
    {
        if (showCursor) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}