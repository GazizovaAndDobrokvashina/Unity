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
        cameras[1] = camera;
        cameras[0].GetComponentInParent<CameraMove>().setRestrictions(15, -8.5f, 8.5f, 8.6f, -8.6f);
    }

    public Camera GetGCamera(int index)
    {
        return cameras[index];
    }

    public void SetActiveFirstCamera()
    {
        cameras[1].gameObject.GetComponent<MouseController>().enabled = false;
        cameras[1].gameObject.SetActive(false);
        cameras[0].gameObject.SetActive(true);
        cameras[0].gameObject.GetComponent<MouseController>().enabled = true;
    }

    public void SetActiveSecondCamera()
    {
        cameras[0].gameObject.GetComponent<MouseController>().enabled = false;
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        cameras[1].gameObject.GetComponent<MouseController>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetActiveSecondCamera();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetActiveFirstCamera();
        }
    }
}