using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	private float _timeStart;
	private float _timeLastCheckpoint;

	public Text SinceStart;
	public Text SinceLastCheckpoint;

	private bool _inRace = false;

	void Start()
	{
		Checkpoint.CheckPointReached += OnCheckpointReached;
		LevelManager.RaceStarted += OnRaceStarted;
		LevelManager.RaceFinished += OnRaceFinished;
		SinceStart.text = "0.0";
		SinceLastCheckpoint.text = "0.0";
	}

	private void Update()
	{
		if (!_inRace)
		{
			return;
		}

		float timeSinceStart = (Time.time - _timeStart);
		float timeSinceCheckpoint = (Time.time - _timeLastCheckpoint);

		SinceStart.text = timeSinceStart.ToString("0.0");
		SinceLastCheckpoint.text = timeSinceCheckpoint.ToString("0.0");
	}

	private void OnRaceStarted()
	{
		_timeStart = Time.time;
		_timeLastCheckpoint = Time.time;
		_inRace = true;
	}

	void OnCheckpointReached(int checkpoint)
	{
		_timeLastCheckpoint = Time.time;
	}

	void OnRaceFinished()
	{
		_inRace = false;
	}

	void OnDestroy()
	{
		Checkpoint.CheckPointReached -= OnCheckpointReached;
		LevelManager.RaceStarted -= OnRaceStarted;
		LevelManager.RaceFinished -= OnRaceFinished;
	}
}

