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

    //отступ от краев экрана, на котором начинается движения камеры
    public float sensitivity = 0.5f;

    //может ли камера двигаться
    private bool canMove = true;
    
    //какая камера активна
    [SerializeField]
    private int currentCamera;

    //инициализация ДБворка
    void Start()
    {
        _dBwork = Camera.main.GetComponent<DBwork>();
    }

    //движение камеры в сторону, к краю которой был приведен курсор
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || Time.timeScale == 0)
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

        if (currentCamera == 1)
        {
            if (Screen.width - Input.mousePosition.x < sensitivity)
            {
                transform.RotateAround(transform.position, Vector3.up, 2.5f);
            }

            if (Input.mousePosition.x < sensitivity)
            {
                transform.RotateAround(transform.position, Vector3.up, -2.5f);
            }
        }
    }

    //при клике на какую-нибудь улицу
    void MouseOver_Street(GameObject ourHitObject)
    {
        if (Input.GetMouseButton(0) && canMove && _dBwork.GetPlayerbyId(1).GetCurrentStep() && Cameras.mode != 1)
        {
            canMove = false;
            _dBwork.GetPlayerbyId(1).move(ourHitObject.GetComponent<StreetPath>());
        }
        else if (Input.GetMouseButton(0) && canMove && _dBwork.GetPlayerbyId(1).GetCurrentStep() && Cameras.mode == 1)
        {
            if (_dBwork.GetWay(_dBwork.GetPlayerbyId(1).CurrentStreetPath.GetIdStreetPath(),
                    ourHitObject.GetComponent<StreetPath>().GetIdStreetPath()).Count == 1)
            {
                canMove = false;
                _dBwork.GetPlayerbyId(1).move(ourHitObject.GetComponent<StreetPath>());
            }
        }
        else if (Input.GetMouseButton(1) && Cameras.mode != 1)
        {
            // показать информацию о улице
            selectedStreetPath = ourHitObject.GetComponent<StreetPath>();
        }
        else if (!Input.GetMouseButton(0))
        {
            canMove = true;
        }
    }

    //при клике на здание
    void MouseOver_Build(GameObject ourHitObject)
    {
        if (Input.GetMouseButton(0))
        {
            // показать информацию о здании
            selectedBuild = ourHitObject.GetComponent<Build>();
        }
    }
    
    //при клике на игрока
    void MouseOver_Player(GameObject ourHitObject)
    {
        if (Input.GetMouseButton(0))
        {
            // показать доступную информацию о выбранном игроке
            selectedPlayer = ourHitObject.GetComponent<Player>();
        }
    }
}