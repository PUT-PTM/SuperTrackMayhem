using UnityEngine;

public class CarController : MonoBehaviour
{
	private Engine _engine;
	private float _gas;
	private Steering _steering;
	public Transform SteeringWheel;
	private bool _controlsEnabled = false;

	private void Awake()
	{
		_engine = GetComponent<Engine>();
		_steering = GetComponent<Steering>();
		LevelManager.RaceStarted += EnableControls;
		LevelManager.RaceFinished += OnRaceFinished;
	}

	private void OnDestroy()
	{
		LevelManager.RaceStarted -= EnableControls;
		LevelManager.RaceFinished -= OnRaceFinished;
	}

    private void OnRaceFinished(bool success)
    {
        DisableControls();
    }

	private void EnableControls()
	{
		EnableControls(true);
	}

	private void DisableControls()
	{
		EnableControls(false);
	}

	public void EnableControls(bool controlsEnabled)
	{
		_controlsEnabled = controlsEnabled;
		if (!_controlsEnabled)
		{
			_engine.SetGas(0);
		}
	}

	public void SetMoveDirection(bool forward)
	{
		if (!_controlsEnabled)
		{
            _engine.SetGas(0);
			return;
		}
		// We move forward as default
		_engine.SetGas(forward ? 1 : -1);
	}

	public void SetSteer(float steer)
	{
		if (!_controlsEnabled)
		{
			return;
		}
		SteeringWheel.localEulerAngles = new Vector3(0, steer*90, 0);
		_steering.SetSteering(steer);
	}
}