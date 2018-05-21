using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //скорость камеры
    private float speed = 50f;

    //минимальная высота
    private float minHeigth = 10f;

    //максимальная высота
    private float maxHeigth;

    //коэффициенты для перемещения камеры
    float k1, k2, k3, k4, b;

    //ограничение слева
    float leftRestriction;

    //ограничение справа
    float rightRestriction;

    //ограничение сверху
    float upRestriction;

    //ограничение снизу
    float downRestriction;

    private Cameras _cameras;

    //установить ограничения движения камеры
    public void setRestrictions(float maxHeigth, float left, float right, float up, float down)
    {
        leftRestriction = left;
        rightRestriction = right;
        upRestriction = up;
        downRestriction = down;
        this.maxHeigth = maxHeigth;

        b = maxHeigth + 10;

        k3 = (3 * minHeigth - maxHeigth) / rightRestriction;
        k4 = (3 * minHeigth - maxHeigth) / upRestriction;
        k1 = (3 * minHeigth - maxHeigth) / leftRestriction;
        k2 = (3 * minHeigth - maxHeigth) / downRestriction;
    }

    private void Start()
    {
        _cameras = GameObject.Find("/Town").GetComponent<Cameras>();
        
    }

    public float GetMinHeight()
    {
        return minHeigth;
    }
    void Update()
    {
        if ((int) Input.mousePosition.x < 2 || (int) Input.mousePosition.x > Screen.width - 2 ||
            Input.mousePosition.y > Screen.height - 2 || Input.mousePosition.y < 2)
        {
            if (_cameras == null)
                _cameras = GameObject.Find("/Town").GetComponent<Cameras>();

            _cameras.StopMoveTopCamera();
        }

        if (Cameras.mode == 1)
            return;

        if ((transform.position.x >= leftRestriction) && ((int) Input.mousePosition.x < 2))
            transform.position -= transform.right * Time.deltaTime * speed;

        if ((transform.position.x <= rightRestriction) && (int) Input.mousePosition.x > Screen.width - 2)
            transform.position += transform.right * Time.deltaTime * speed;

        if ((transform.position.z <= upRestriction) && Input.mousePosition.y > Screen.height - 2)
            transform.position += transform.forward * Time.deltaTime * speed;

        if ((transform.position.z >= downRestriction) && Input.mousePosition.y < 2)
            transform.position -= transform.forward * Time.deltaTime * speed;

        checkHeigth();

        if (transform.position.z < downRestriction)
            transform.position = new Vector3(transform.position.x, transform.position.y, downRestriction);

        if (transform.position.z > upRestriction)
            transform.position = new Vector3(transform.position.x, transform.position.y, upRestriction);

        if (transform.position.x > rightRestriction)
            transform.position = new Vector3(rightRestriction, transform.position.y, transform.position.z);

        if (transform.position.x < leftRestriction)
            transform.position = new Vector3(leftRestriction, transform.position.y, transform.position.z);

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > minHeigth)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < maxHeigth)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        }

        if (transform.position.y < minHeigth)
            transform.position = new Vector3(transform.position.x, minHeigth, transform.position.z);

        if (transform.position.y > maxHeigth)
            transform.position = new Vector3(transform.position.x, maxHeigth, transform.position.z);
    }

    void checkHeigth()
    {
        if (transform.position.y > k1 * transform.position.x + b)
        {
            transform.position =
                new Vector3((transform.position.y - b) / k1, transform.position.y, transform.position.z);
        }

        if (transform.position.y > k3 * transform.position.x + b)
        {
            transform.position =
                new Vector3((transform.position.y - b) / k3, transform.position.y, transform.position.z);
        }

        if (transform.position.y > k2 * transform.position.z + b)
        {
            transform.position =
                new Vector3(transform.position.x, transform.position.y, (transform.position.y - b) / k2);
        }

        if (transform.position.y > k4 * transform.position.z + b)
        {
            transform.position =
                new Vector3(transform.position.x, transform.position.y, (transform.position.y - b) / k4);
        }
    }
}