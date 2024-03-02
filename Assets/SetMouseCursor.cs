using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorSprite;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
