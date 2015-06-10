using UnityEngine;
using UnityStandardAssets.Cameras;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AutoCam))]
public class CameraTourController : MonoBehaviour 
{
    public void EndTour()
    {
        GetComponent<Animator>().SetTrigger("Stop");
    }

    public void EndTourAnimatorEvent()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<AutoCam>().enabled = true;
    }

    public void StartTour()
    {
        var animator = GetComponent<Animator>();
        animator.enabled = true;
        GetComponent<AutoCam>().enabled = false;
        animator.SetTrigger("Start");
    }
}
