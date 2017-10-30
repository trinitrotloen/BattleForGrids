using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanges : MonoBehaviour
{

    public Transform target;
    private Camera myCam;
    float maxSize;
    Resolution OrgResolution;
    public float mouseSensitivity = 0.01f;
    private Vector3 lastPosition ;

    // Use this for initialization
    void Start()
    {
        myCam = GetComponent<Camera>();
        myCam.transform.position = new Vector3(myCam.transform.position.x, myCam.transform.position.y, -(GameObject.Find("Map").GetComponent<GridBoardWithQuads>().mapSize.x * GameObject.Find("Map").GetComponent<GridBoardWithQuads>().tileSize));
        myCam.orthographicSize = (GameObject.Find("Map").GetComponent<GridBoardWithQuads>().mapSize.x* GameObject.Find("Map").GetComponent<GridBoardWithQuads>().tileSize) /2;
        maxSize = myCam.orthographicSize;
        OrgResolution = Screen.currentResolution;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Screen.currentResolution.Equals(OrgResolution))
        {
            myCam.orthographicSize = (Screen.height / 30f / 4f);
            maxSize = myCam.orthographicSize;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            if (myCam.orthographicSize > 2)
                myCam.orthographicSize -= 0.2f;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            if (myCam.orthographicSize < maxSize)
                myCam.orthographicSize += 0.2f;

        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            RaycastHit2D hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast(ray.origin, ray.direction, 100.0f);
                if (hit.transform)
                {
                    print(hit.transform.gameObject.name);
                }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * -mouseSensitivity, delta.y * -mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }
    }
}
