using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Speedometer : MonoBehaviour
{
	private Rigidbody _car;
	private Text _text;

	private void Awake()
	{
		_text = GetComponent<Text>();
		_car = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
	}

	void Update()
	{
		_text.text = (Vector3.Dot(_car.velocity, _car.transform.forward)*3.6f).ToString("0.0");
	}
}
