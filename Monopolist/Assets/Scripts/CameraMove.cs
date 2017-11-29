using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera[] cameras;
    //скорость камеры
    private int speed;

    //минимальная высота
    private float minHeigth;

    //максимальная высота
    private float maxHeigth;

    //0 - просмотр карты, 1- от первого лица, 2 - от третьего карты
    public static int mode { get; set; }
    
    //временно для теста
    public int type = 0;

    void Start()
    {
        cameras = FindObjectsOfType<Camera>();
    }

    void Update()
    {
        mode = 1;
        if (cameras.Length > 2)
        {
            if (type == 0)
            {                          
                cameras[0].gameObject.SetActive(true);
                cameras[1].gameObject.SetActive(false);
            }

            else
            {
                cameras[0].gameObject.SetActive(false);
                cameras[1].gameObject.SetActive(true);
            }
        }
    }
}