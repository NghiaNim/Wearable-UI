using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WatchVisualController : MonoBehaviour
{
    public Transform looker;
    public Transform target;

    [Range(-1,0)]
    public float angularThresthold = -.75f;

    [Min(0)]
    public float distanceThreshold = .75f;

    public float timein = .75f;
    public float timeout = 2f;

    public UnityEvent onLookEntered = new UnityEvent();
    public UnityEvent onLookExited = new UnityEvent();

    bool isLooking = false;
    bool wasLooking = false;
    Coroutine delay = null;

    // Update is called once per frame
    void Update()
    {
        isLooking = Vector3.Dot(looker.forward, target.forward) < angularThresthold && Vector3.Distance(looker.position, target.position) < distanceThreshold;

        if (isLooking && !wasLooking && delay == null)
            delay = StartCoroutine(Timein());
        else if (!isLooking && wasLooking && delay == null)
            delay = StartCoroutine(Timeout());
        else if (!isLooking && !wasLooking && delay != null)
            ClearDelay();
        else if (isLooking && wasLooking && delay != null)
            ClearDelay();


    }

    void ClearDelay()
    {
        StopCoroutine(delay);
        delay = null;
    }

    IEnumerator Timein()
    {
        yield return new WaitForSeconds(timein);
        wasLooking = true;
        onLookEntered.Invoke();
        delay = null;
    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeout);
        wasLooking = false;
        onLookExited.Invoke();
        delay = null;
    }
}
