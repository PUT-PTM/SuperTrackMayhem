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
	public static void OnStartRace()
	{
		if (RaceStarted != null)
		{
			RaceStarted();
		}
	}

	public static event Action RaceFinished;
	public static void OnFinishRace()
	{
		if (RaceFinished != null)
		{
			RaceFinished();
		}
	}
}
