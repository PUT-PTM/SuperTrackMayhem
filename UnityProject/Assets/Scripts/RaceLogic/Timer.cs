using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	private const string BestTimeKey = "BestTime";
	private const string CheckpointTimePrefix = "Time";
	private readonly float[] _bestTimes = new float[Constants.CheckpointCount];
	private float _bestLapTime;
	private bool _inRace;
	private float _timeLastCheckpoint;
	private float _timeStart;
	public Text BestCheckpoint;
	public Text BestTotal;
	public Text SinceLastCheckpoint;
	public Text SinceStart;

	private void Start()
	{
		Checkpoint.CheckPointReached += OnCheckpointReached;
		LevelManager.RaceStarted += OnRaceStarted;
		LevelManager.RaceFinished += OnRaceFinished;
		SinceStart.text = "0.0";
		SinceLastCheckpoint.text = "0.0";
		if (PlayerPrefs.HasKey(BestTimeKey))
		{
			_bestLapTime = PlayerPrefs.GetFloat(BestTimeKey);
			for (var i = 0; i < Constants.CheckpointCount; i++)
			{
				_bestTimes[i] = PlayerPrefs.GetFloat(CheckpointTimePrefix + i);
			}
		}
		else
		{
			_bestLapTime = Mathf.Infinity;
			for (var i = 0; i < Constants.CheckpointCount; i++)
			{
				_bestTimes[i] = Mathf.Infinity;
			}
		}
		BestTotal.text = _bestLapTime.ToString("0.0");
		BestCheckpoint.text = _bestTimes[0].ToString("0.0");
	}

	private void Update()
	{
		if (!_inRace)
		{
			return;
		}

		var timeSinceStart = Time.time - _timeStart;
		var timeSinceCheckpoint = Time.time - _timeLastCheckpoint;

		SinceStart.text = timeSinceStart.ToString("0.0");
		SinceLastCheckpoint.text = timeSinceCheckpoint.ToString("0.0");
	}

	private void OnRaceStarted()
	{
		_timeStart = Time.time;
		_timeLastCheckpoint = Time.time;
		_inRace = true;
	}

	private void OnCheckpointReached(int checkpoint)
	{
		var timeSinceLastCheckpoint = Time.time - _timeLastCheckpoint;
		if (timeSinceLastCheckpoint < _bestTimes[checkpoint])
		{
			_bestTimes[checkpoint] = timeSinceLastCheckpoint;
		}
		if (checkpoint + 1 < Constants.CheckpointCount)
		{
			BestCheckpoint.text = _bestTimes[checkpoint + 1].ToString("0.0");
		}

		_timeLastCheckpoint = Time.time;
	}

	private void OnRaceFinished(bool success)
	{
		_inRace = false;

	    if (!success)
	    {
	        return;
	    }

		var lapTime = Time.time - _timeStart;

		if (lapTime < _bestLapTime)
		{
			PlayerPrefs.SetFloat(BestTimeKey, lapTime);
		}

		for (var i = 0; i < Constants.CheckpointCount; i++)
		{
			PlayerPrefs.SetFloat(CheckpointTimePrefix + i, _bestTimes[i]);
		}
	}

	private void OnDestroy()
	{
		Checkpoint.CheckPointReached -= OnCheckpointReached;
		LevelManager.RaceStarted -= OnRaceStarted;
		LevelManager.RaceFinished -= OnRaceFinished;
	}
}