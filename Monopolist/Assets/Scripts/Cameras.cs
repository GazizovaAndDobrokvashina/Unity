using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    public Camera[] cameras = new Camera[2];

    //0 - просмотр карты, 1- от первого лица, 2 - от третьего карты
    public static int mode { get; set; }

    public void SetCamera(Camera camera)
    {
        mode = 1;
        cameras[1] = camera;
        cameras[0].GetComponentInParent<CameraMove>().setRestrictions(15, -8.5f, 8.5f, 8.6f, -8.6f);
    }

    public Camera GetGCamera(int index)
    {
        return cameras[index];
    }

    public void SetActiveFirstCamera()
    {
        mode = 0;
        cameras[1].gameObject.GetComponent<MouseController>().enabled = false;
        cameras[1].gameObject.SetActive(false);
        cameras[0].gameObject.SetActive(true);
        cameras[0].gameObject.GetComponent<MouseController>().enabled = true;
    }
//
//    public void SetActiveSecondCamera()
//    {
//        cameras[0].gameObject.GetComponent<MouseController>().enabled = false;
//        cameras[0].gameObject.SetActive(false);
//        cameras[1].gameObject.SetActive(true);
//        cameras[1].gameObject.GetComponent<MouseController>().enabled = true;
//    }

    public void ChangeTypeOfCamera()
    {
        cameras[0].orthographic = !cameras[0].orthographic;
    }
    
    public void ChangeCamera()
    {
        if (cameras[1].gameObject.activeInHierarchy)
        {
            mode = 0;
            cameras[1].gameObject.GetComponent<MouseController>().enabled = false;
            cameras[1].gameObject.SetActive(false);
            cameras[0].gameObject.SetActive(true);
            cameras[0].gameObject.GetComponent<MouseController>().enabled = true;
        }
        else
        {
            mode = 1;
            cameras[0].gameObject.GetComponent<MouseController>().enabled = false;
            cameras[0].gameObject.SetActive(false);
            cameras[1].gameObject.SetActive(true);
            cameras[1].gameObject.GetComponent<MouseController>().enabled = true;
        }
    }

    public bool isActiveOrtoCamera()
    {
        return cameras[0].gameObject.activeInHierarchy;
    }

    public void moveOrtoCamera(Vector3 pos)
    {
        //cameras[0].transform.position = Vector3.MoveTowards(cameras[0].transform.position, new Vector3(pos.x, 0, pos.z), 1f);
        cameras[0].transform.parent.transform.position = pos;
    }

    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            SetActiveSecondCamera();
//        }
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            SetActiveFirstCamera();
//        }
//    }
}