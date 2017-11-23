using UnityEngine;
using UnityEngine.EventSystems;


public class MouseController : MonoBehaviour
{
	//public GameController GameController;
	private DBwork _dBwork;
	
    private Build selectedBuild;
    private Player selectedPlayer;
    private StreetPath selectedStreetPath;

	void Start()
	{
		_dBwork = Camera.main.GetComponent<DBwork>();
		
	}
    void Update () {

		if(EventSystem.current.IsPointerOverGameObject()) {

			return;
		}

		Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay( Input.mousePosition );

		RaycastHit hitInfo;

		if( Physics.Raycast(ray, out hitInfo) ) {
			GameObject ourHitObject = hitInfo.collider.transform.gameObject;

			if (ourHitObject.GetComponent<StreetPath> () != null ) {
				
				MouseOver_Street (ourHitObject);
			} else if (ourHitObject.GetComponentInParent<Player> () != null) {
				
				MouseOver_Player (ourHitObject);
			} else if (ourHitObject.GetComponentInParent<Build> () != null) {
				
				MouseOver_Build (ourHitObject);
			}


		}


	}


	void MouseOver_Street(GameObject ourHitObject) {
		

		if (Input.GetMouseButton (0) && CameraMove.mode == 1) {
			_dBwork.GetPlayerbyId(1).move(ourHitObject.GetComponent<StreetPath>());
		} else if (Input.GetMouseButton(0))
		{
			// показать информацию о улице
			selectedStreetPath = ourHitObject.GetComponent<StreetPath>();
		}
			
	}

	void MouseOver_Build(GameObject ourHitObject) {
		
		if (Input.GetMouseButton(0))
		{
			// показать информацию о здании
			selectedBuild = ourHitObject.GetComponent<Build>();
		}

	}

	void MouseOver_Player(GameObject ourHitObject) {

		
		if (Input.GetMouseButton(0))
		{
			// показать доступную информацию о выбранном игроке
			selectedPlayer = ourHitObject.GetComponent<Player>();
		}

	}
    
}
