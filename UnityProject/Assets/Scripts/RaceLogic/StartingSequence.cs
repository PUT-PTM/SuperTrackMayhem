using System.Collections;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class StartingSequence : MonoBehaviour
{
    private ControlScheme _selectedControl;
    public Transform Camera;
    public Transform Car;
    public Transform CarSpawnPoint;
    public float FadeOutAnimationTime;
    public Animator FadeOutTitleAnimator;
    public GameObject[] StartSequenceElements;
    public GameObject[] ToEnableAfterSequence;

    private void OnEnable()
    {
        Car.position = CarSpawnPoint.position;
        foreach (var startSequenceElement in StartSequenceElements)
        {
            startSequenceElement.SetActive(true);
        }
        StartCoroutine(SelectControlsCoroutine());
    }

    private IEnumerator SelectControlsCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _selectedControl = ControlScheme.Keyboard;
                StartCoroutine(FadeOutTitleCoroutine());
                yield break;
            }
            if (STMReceiver.Instance.Buttons.BreakButtonDown)
            {
                _selectedControl = ControlScheme.Stm;
                StartCoroutine(FadeOutTitleCoroutine());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOutTitleCoroutine()
    {
        FadeOutTitleAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(FadeOutAnimationTime);
        foreach (var element in StartSequenceElements)
        {
            element.SetActive(false);
        }
        foreach (var element in ToEnableAfterSequence)
        {
            element.SetActive(true);
        }
        Car.gameObject.GetComponent<CarKeyboardControl>().enabled = _selectedControl == ControlScheme.Keyboard;
        Car.gameObject.GetComponent<CarSTMControl>().enabled = _selectedControl == ControlScheme.Stm;
        gameObject.SetActive(false);
    }

    private enum ControlScheme
    {
        Keyboard,
        Stm
    }
}