using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Competition : MonoBehaviour {

	//игрок
	GameObject player;
	//допустимое количество ошибок на уровне
	public int allowable_mistakes;
	//количество ошибок игрока
	int total_mistakes;
	//всего препятсвий на уровне
	public int total_barriers;
	//количество преодоленных препятсвий
	int barriers;
	//набор переменных для таймера
	public static bool stop;
	public static string result;
	public bool reverse;
	public enum CounterMode{hourMinSec = 0, minSec = 1};
	public CounterMode counterMode = CounterMode.hourMinSec;
	int startHour = 0, startMin = 0, startSec = 0;
	public Text textOutput;
	public bool startAwake = false;
	private int hour, min, sec;
	private string h, m, s;

	void ControlTime(){
		if (startAwake)
			stop = false;
		else
			stop = true;

		if (startHour > 0 && startHour <= 23)
			hour = startHour;
		else
			startHour = 0;

		if (startMin > 0 && startMin <= 59)
			min = startMin;
		else
			startMin = 0;

		if (startSec > 0 && startSec <= 59)
			sec = startSec;
		else
			startSec = 0;
	}
		

	void Update(){
		ControlTime ();
	}
	// Use this for initialization
	void Start () {
		StartCoroutine (RepeatingFunction ());
	}

	IEnumerator RepeatingFunction(){
		while (true) {
			if (!stop)
				TimeCount ();
			yield return new WaitForSeconds (1);
		}
	}
	void TimeCount(){
		if (reverse) {
			if (sec < 0) {
				sec = 59;
				min--;
			}
			if (min < 0) {
				min = 59;
				hour--;
			}
			if (hour < 0) {
				hour = 23;
			}
			CurrentTime ();
			sec--;
		} else {
			if (sec > 59) {
				sec = 0;
				min++;
			}
			if (min > 59) {
				min = 0;
				hour++;
			}
			if (hour > 23) {
				hour = 0;
			}
			CurrentTime ();
			sec++;
		}
	}
	void CurrentTime() {
		
		if (sec < 10)
			s = "0" + sec;
		else
			s = sec.ToString ();

		if (min < 10)
			m = "0" + min;
		else
			m = min.ToString ();

		if (hour < 10)
			h = "0" + hour;
		else
			h = hour.ToString ();
		
	}
	void OnGUI() {
		switch(counterMode) {
		case CounterMode.hourMinSec:
			result = h + ":" + m + ":" + s;
			break;
		}
		textOutput.text = result;
	}

	void OnTriggerEnter(Collider trig){
		if (trig.tag == "Start") {
			startAwake = true;
			ControlTime();
		}
		if (trig.tag == "FinishOfComp") {
			startAwake = false;
			ControlTime();
		}
		
	}
}
