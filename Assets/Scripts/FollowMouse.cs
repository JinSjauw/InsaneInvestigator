using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private bool followY;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    //Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (followY)
            {
                transform.position = hit.point;
            }
            else
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }
    }
    
}
