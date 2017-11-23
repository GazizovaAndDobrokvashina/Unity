using System.Diagnostics;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private int speed;
    private float minHeigth;
    private float maxHeigth;
    //0 - просмотр карты, 1- от первого лица, 2 - от третьего карты
    public static int mode { get; set;}

    void Update()
    {
        mode = 1;
    }
    
    
}

