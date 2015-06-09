using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private static LevelManager _instance ;
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
			return;
		}

		_instance = this;
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
			RaceStarted = null;
			RaceFinished = null;
		}
	}

	public static event Action RaceStarted;
	public static void StartRace()
	{
		if (RaceStarted != null)
		{
			RaceStarted();
		}
	}

    // Argument is wheter the race is finished with a success 
    // (i.e. player crossed the finish line)
    // or failure (roll over etc).
	public static event Action<bool> RaceFinished;
	public static void FinishRace(bool success)
	{
		if (RaceFinished != null)
		{
			RaceFinished(success);
		}
	}
}
