using UnityEngine;
using FMODUnity;

public class FMODAnimationEventsSimple : MonoBehaviour
{
    [Header("Assign FMOD Events")]
    public EventReference event1;
    public EventReference event2;
    public EventReference event3;
    public EventReference event4;
    public EventReference event5;
    public EventReference event6;
    public EventReference event7;
    public EventReference event8;
    public EventReference event9;
    public EventReference event10;

    public void PlayEvent1() => RuntimeManager.PlayOneShot(event1, transform.position);
    public void PlayEvent2() => RuntimeManager.PlayOneShot(event2, transform.position);
    public void PlayEvent3() => RuntimeManager.PlayOneShot(event3, transform.position);
    public void PlayEvent4() => RuntimeManager.PlayOneShot(event4, transform.position);
    public void PlayEvent5() => RuntimeManager.PlayOneShot(event5, transform.position);
    public void PlayEvent6() => RuntimeManager.PlayOneShot(event6, transform.position);
    public void PlayEvent7() => RuntimeManager.PlayOneShot(event7, transform.position);
    public void PlayEvent8() => RuntimeManager.PlayOneShot(event8, transform.position);
    public void PlayEvent9() => RuntimeManager.PlayOneShot(event9, transform.position);
    public void PlayEvent10() => RuntimeManager.PlayOneShot(event10, transform.position);
}