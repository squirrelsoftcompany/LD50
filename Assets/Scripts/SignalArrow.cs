using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SignalArrow : MonoBehaviour
{
    public GameObject mArrowSignal;
    public Renderer mFarmEggRd;
    // Start is called before the first frame update
    void Start()
    {
        Transform lArrowTransform = mArrowSignal.GetComponent<Transform>();
        Transform lCivilTransform = this.GetComponent<Transform>();
        lArrowTransform.SetPositionAndRotation(new Vector3(0,0, lCivilTransform.position.z), lArrowTransform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        Transform lArrowTransform = mArrowSignal.GetComponent<Transform>();

        if (mFarmEggRd && mFarmEggRd.isVisible)
        {
            mArrowSignal.SetActive(false);
        }
        else
        {
            mArrowSignal.SetActive(true);
        }

        Transform lCivilTransform = this.GetComponent<Transform>();
        Camera cam = Camera.main;
        Vector3 arrowScreenPos = cam.WorldToScreenPoint(mArrowSignal.GetComponent<Transform>().position);   //x1,y1
        Vector3 civilScreenPos = cam.WorldToScreenPoint(this.GetComponent<Transform>().position);           //x2,y2

        Vector2 topLeftScreenPoint = new Vector2(50.0f, cam.pixelHeight - 50.0f);                           //x3,y3
        Vector2 bottomLeftScreenPoint = new Vector2(50.0f,50.0f);                                           //x4,y4
        Vector2 bottomRightScreenPoint = new Vector2(cam.pixelWidth - 50.0f, 50.0f);                        //x5,y6
        Vector2 topRightScreenPoint = new Vector2(cam.pixelWidth - 50.0f, cam.pixelHeight - 50.0f);         //x7,y8


        Vector2 intersectionLeftPoint = lineIntersection(arrowScreenPos.x, arrowScreenPos.y, civilScreenPos.x, civilScreenPos.y, topLeftScreenPoint.x, topLeftScreenPoint.y, bottomLeftScreenPoint.x, bottomLeftScreenPoint.y);
        Vector2 intersectionBottomPoint = lineIntersection(arrowScreenPos.x, arrowScreenPos.y, civilScreenPos.x, civilScreenPos.y, bottomRightScreenPoint.x, bottomRightScreenPoint.y, bottomLeftScreenPoint.x, bottomLeftScreenPoint.y);
        Vector2 intersectionTopPoint = lineIntersection(arrowScreenPos.x, arrowScreenPos.y, civilScreenPos.x, civilScreenPos.y, topLeftScreenPoint.x, topLeftScreenPoint.y, topRightScreenPoint.x, topRightScreenPoint.y);
        Vector2 intersectionRightPoint = lineIntersection(arrowScreenPos.x, arrowScreenPos.y, civilScreenPos.x, civilScreenPos.y, bottomRightScreenPoint.x, bottomRightScreenPoint.y, topRightScreenPoint.x, topRightScreenPoint.y);

        List<Vector2> DistanceList = new List<Vector2>();

        //Check valid intersections
        if (intersectionBottomPoint.x >= 50.0f  && intersectionBottomPoint.x <= cam.pixelWidth - 50.0f)
        {
            DistanceList.Add(intersectionBottomPoint);
        }
        if (intersectionTopPoint.x >= 50.0f && intersectionTopPoint.x <= cam.pixelWidth - 50.0f)
        {
            DistanceList.Add(intersectionTopPoint);
        }
        if (intersectionLeftPoint.y >= 50.0f && intersectionLeftPoint.y <= cam.pixelHeight - 50.0f)
        {
            DistanceList.Add(intersectionLeftPoint);
        }
        if (intersectionRightPoint.y >= 50.0f && intersectionRightPoint.y <= cam.pixelHeight - 50.0f)
        {
            DistanceList.Add(intersectionRightPoint);
        }

        float minVal = Mathf.Infinity;
        Vector2 finalIntersectionPoint = Vector2.zero;

        for (int i = 0; i < DistanceList.Count; i++)
        {
            if (Vector2.Distance(DistanceList[i], civilScreenPos) < minVal)
            {
                minVal = Vector2.Distance(DistanceList[i], civilScreenPos);
                finalIntersectionPoint = DistanceList[i];
            }
        }

        float z = 10;//Camera.main.nearClipPlane ;
        Vector3 screenPositionWithZ = new Vector3(finalIntersectionPoint.x, finalIntersectionPoint.y, z);

        Ray ray = cam.ScreenPointToRay(screenPositionWithZ);

        // create a plane at 0,0,0 whose normal points to +Y:
        Plane hPlane = new Plane(Vector3.up, Vector3.zero);
        // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
        float distance = 0;
        Vector3 worldPosition = Vector3.zero;
        if (hPlane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        Debug.DrawRay(cam.transform.position, ray.direction*100, Color.green);
        Vector3 FinalWorldPosition = new Vector3(worldPosition.x, worldPosition.y, lCivilTransform.position.z);

        lArrowTransform.SetPositionAndRotation(FinalWorldPosition, lArrowTransform.rotation);

    }

    Vector2 lineIntersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        //Ready for some math ? So compute line intersection (see https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection)
        float T = ((x1 - x3) * (y3-y4) - (y1-y3) * (x3-x4)) / ((x1-x2) * (y3-y4) - (y1-y2) * (x3-x4));
        Vector2 P = new Vector2( x1 + T * (x2-x1), y1 + T * (y2 - y1));
        return P;
    }
}
