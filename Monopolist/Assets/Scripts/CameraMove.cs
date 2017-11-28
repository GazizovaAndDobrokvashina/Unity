using System.Diagnostics;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //скорость камеры
    private int speed;

    //минимальная высота
    private float minHeigth;

    //максимальная высота
    private float maxHeigth;

    //0 - просмотр карты, 1- от первого лица, 2 - от третьего карты
    public static int mode { get; set; }

    void Update()
    {
        mode = 1;
    }
}