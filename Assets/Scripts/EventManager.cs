using UnityEngine;
using System.Collections;

public delegate void OnStageStartedDelegate ();
public delegate void OnStageEndedDelegate (int altitude, string character);
public delegate void OnAnchorGrabbedDelegate ();
public delegate void OnMoonstoneCollectedDelegate ();

public class EventManager
{
	private static Event _instance;

	public static Event Instance {
		get {
			if (_instance == null) {
				_instance = new Event ();
			}

			return _instance;
		}
	}

	//==================================================================================
	//
	//==================================================================================

	public event OnStageStartedDelegate OnStageStartedEvent;
	public event OnStageEndedDelegate OnStageEndedEvent;
	public event OnAnchorGrabbedDelegate OnAnchorGrabbedEvent;
	public event OnMoonstoneCollectedDelegate OnMoonstoneCollectedEvent;

	//==================================================================================
	//
	//==================================================================================

	public void SendOnStageStartedEvent ()
	{
		OnStageStartedDelegate tmp = OnStageStartedEvent;

		if (tmp != null) {
			this.OnStageStartedEvent ();
		}
	}

    public void SendOnStageEndedEvent(int altitude, string character)
	{
		OnStageEndedDelegate tmp = OnStageEndedEvent;

		if (tmp != null) {
			this.OnStageEndedEvent (altitude, character);
		}
	}

	public void SendOnAnchorGrabbedEvent ()
	{
		OnAnchorGrabbedDelegate tmp = OnAnchorGrabbedEvent;

		if (tmp != null) {
			this.OnAnchorGrabbedEvent ();
		}
	}

	public void SendOnMoonstoneCollectedEvent ()
	{
		OnMoonstoneCollectedDelegate tmp = OnMoonstoneCollectedEvent;

		if (tmp != null) {
			this.OnMoonstoneCollectedEvent ();
		}
	}
}
