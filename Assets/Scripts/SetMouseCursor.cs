using UnityEngine;

public class SetMouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorSprite;
    [SerializeField] private Vector2 cursorHotSpot;
        
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.SetCursor(cursorSprite, cursorHotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
