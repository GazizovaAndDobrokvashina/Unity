using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    //массив камер на сцене
    public Camera[] cameras = new Camera[2];

    //0 - просмотр карты, 1- от первого лица, 2 - от третьего карты
    public static int mode { get; set; }

    //устанавливаем камеру от первого лица как стартовую
    private void Start()
    {
        mode = 1;
    }

    //назначаем ограничители, в пределах которых может двигаться верхняя камера, назначаем камеру от первого лица в массив
    public void SetCamera(Camera camera)
    {
        mode = 1;
        cameras[1] = camera;

        Vector3 size = gameObject.GetComponent<BoxCollider>().size * 100;

        //cameras[0].GetComponentInParent<CameraMove>().setRestrictions(15, -8.5f, 8.5f, 8.6f, -8.6f);
        cameras[0].GetComponentInParent<CameraMove>().setRestrictions(cameras[0].transform.parent.position.y,
            -size.x / 2f, size.x / 2f, size.y / 2f, -size.y / 2f);
    }

    //Включить верхнюю камеру
    public void SetActiveFirstCamera()
    {
        mode = 0;
        cameras[1].gameObject.GetComponent<MouseController>().enabled = false;
        cameras[1].gameObject.SetActive(false);
        cameras[0].gameObject.SetActive(true);
        cameras[0].gameObject.GetComponent<MouseController>().enabled = true;
    }

    //переключить режим верхней камеры между орто и перспекивой
    public void ChangeTypeOfCamera()
    {
        cameras[0].orthographic = !cameras[0].orthographic;
    }

    //переключение между верхней камерой и камерой от первого лица
    public void ChangeCamera()
    {
        if (cameras[1].gameObject.activeInHierarchy)
        {
            SetActiveFirstCamera();
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

    //включена ли верхняя камера
    public bool isActiveOrtoCamera()
    {
        return cameras[0].gameObject.activeInHierarchy;
    }

    //перемещение верхней камеры
    public void moveOrtoCamera(Vector3 pos)
    {
        cameras[0].transform.parent.transform.position = pos;
    }

    public IEnumerator moveTopCamera(Vector3 pos)
    {
        Vector3 currentPos = cameras[0].transform.parent.transform.position;
        
        while (!cameras[0].transform.parent.transform.position.Equals(pos))
        {
            Debug.Log(cameras[0].transform.parent.transform.position);
            cameras[0].transform.parent.transform.position = Vector3.MoveTowards(currentPos, pos, 10 * Time.deltaTime);
        }
       yield break;
    }
}