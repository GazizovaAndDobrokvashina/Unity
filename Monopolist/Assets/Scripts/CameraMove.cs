﻿using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //скорость камеры
    private float speed = 10f;

    //минимальная высота
    private float minHeigth = 7.5f;

    //максимальная высота
    private float maxHeigth;

    float k1, k2, k3, k4, b;


    float leftRestriction;
    float rightRestriction;
    float upRestriction;
    float downRestriction;

    public void setRestrictions(float maxHeigth, float left, float right, float up, float down)
    {
        leftRestriction = left;
        rightRestriction = right;
        upRestriction = up;
        downRestriction = down;
        this.maxHeigth = maxHeigth;
//		if (MapInfo.current.gridWidth > MapInfo.current.gridHeigth) {
//			maxHeigth = MapInfo.current.gridHeigth * 133/ (upRestriction);
//		} else {
//			maxHeigth = MapInfo.current.gridWidth * 133/ (leftRestriction);
//		}

        k1 = maxHeigth / rightRestriction;
        k2 = maxHeigth / upRestriction;
        k3 = maxHeigth / leftRestriction;
        k4 = maxHeigth / downRestriction;

        b = maxHeigth + 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cameras.mode == 1) 
            return;
        
        //Debug.Log(transform.position.x + " " + transform.position.z + " " + upRestriction + " " + downRestriction);
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