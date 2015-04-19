using UnityEngine;

public class SetFPSLimit : MonoBehaviour 
{
	void Awake()
	{
		Application.targetFrameRate = 60;
	}
}
