using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TurnOnLight : MonoBehaviour
{
	public Color OnColor;

	public void TurnOn()
	{
		GetComponent<Image>().color = OnColor;
	}
}
