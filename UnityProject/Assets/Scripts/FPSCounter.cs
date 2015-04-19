using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	private Text _text;

	private void Awake()
	{
		_text = GetComponent<Text>();
	}

	private void Update()
	{
		_text.text = (1/Time.deltaTime).ToString(CultureInfo.InvariantCulture);
	}
}
