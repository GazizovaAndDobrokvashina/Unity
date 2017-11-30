using UnityEngine;
using UnityEngine.EventSystems;


public class MouseController : MonoBehaviour
{
    //ссылка на текущий ДБворк
    private DBwork _dBwork;

    //выбранное здание
    private Build selectedBuild;

    //выбранный игрок
    private Player selectedPlayer;

    //выбранная часть улицы
    private StreetPath selectedStreetPath;

    //
    public float sensitivity = 0.5f;

    //
    private bool canMove = true;

    //инициализация ДБворка
    void Start()
    {
        _dBwork = Camera.main.GetComponent<DBwork>();
    }
    
    //
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.transform.gameObject;

            if (ourHitObject.GetComponent<StreetPath>() != null)
            {
                MouseOver_Street(ourHitObject);
            }
            else if (ourHitObject.GetComponentInParent<Player>() != null)
            {
                MouseOver_Player(ourHitObject);
            }
            else if (ourHitObject.GetComponentInParent<Build>() != null)
            {
                MouseOver_Build(ourHitObject);
            }
        }


        if (Screen.width - Input.mousePosition.x < sensitivity)
        {
            transform.RotateAround(transform.position, Vector3.up, 2.5f);
        }

        if (Input.mousePosition.x < sensitivity)
        {
            transform.RotateAround(transform.position, Vector3.up, -2.5f);
        }
    }


    void MouseOver_Street(GameObject ourHitObject)
    {
        if (Input.GetMouseButton(0) && canMove)// && CameraMove.mode == 1 )
        {
            canMove = false;
            _dBwork.GetPlayerbyId(1).move(ourHitObject.GetComponent<StreetPath>());
        }
        else if (Input.GetMouseButton(0) && CameraMove.mode != 1)
        {
            // показать информацию о улице
            selectedStreetPath = ourHitObject.GetComponent<StreetPath>();
        }
        else if (!Input.GetMouseButton(0))
        {
            canMove = true;
        }
    }

    void MouseOver_Build(GameObject ourHitObject)
    {
        if (Input.GetMouseButton(0))
        {
            // показать информацию о здании
            selectedBuild = ourHitObject.GetComponent<Build>();
        }
    }

    void MouseOver_Player(GameObject ourHitObject)
    {
        if (Input.GetMouseButton(0))
        {
            // показать доступную информацию о выбранном игроке
            selectedPlayer = ourHitObject.GetComponent<Player>();
        }
    }
}