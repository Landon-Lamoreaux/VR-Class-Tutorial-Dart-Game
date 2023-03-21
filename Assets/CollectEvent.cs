using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectEvent : MonoBehaviour
{
    private PlayerInput filter;
    private InputAction grab;
    private InputAction release;
    private List<GameObject> inRange = new List<GameObject>();
    private GameObject inHand = null;
    private List<GameObject> darts = new List<GameObject>();

    // Start is called before the first frame update.
    void Start()
    {
        filter = GetComponent<PlayerInput>();
        grab = filter.actions["Grab"];
        release = filter.actions["Release"];
        filter.actions["Throw"].performed += OnThrow;
    }

    private ReleaseEvent releaseScript;
    public void setReleaseFunction(ReleaseEvent obj)
    {
        releaseScript = obj;
    }

    [SerializeField]
    private float force = 75f;
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (darts.Count > 0)
        {
            Transform hand = transform.Find("Camera/Hand");

            // Get dart, and reenable.
            GameObject dart = darts[0];
            darts.RemoveAt(0);
            dart.SetActive(true);

            // Place in hand, and do a final rotation since the axis are not the same.
            dart.transform.position = hand.position;
            dart.transform.localRotation = hand.rotation;
            Quaternion rotate = Quaternion.Euler(90, 0, 0);
            dart.transform.localRotation *= rotate;

            // Apply force.
            Rigidbody body = dart.GetComponent<Rigidbody>();
            body.AddForce(hand.forward * force, ForceMode.Force);
        }
    }

    // Grab action.
    public void OnGrab(InputAction.CallbackContext context)
    {
        Debug.Log("Grabbed");
        Transform hand = transform.Find("Camera/Hand");

        // Sanity check, do not grab is hands are full.
        if (inHand == null)
        {
            GameObject closest = getClosest();

            // Found something.
            if (closest != null)
            {
                if (closest.tag == "Pickup")
                {
                    closest.SetActive(false);
                    inRange.Remove(closest);
                    darts.Add(closest);
                }
                else
                {
                    inHand = closest;
                    PlaceInHand(inHand);
                }
            }
        }
    }

    private GameObject getClosest()
    {
        Transform hand = transform.Find("Camera/Hand");
        GameObject closest = null;
        for (int i = 0; i < inRange.Count; i++)
        {
            GameObject temp = inRange[i];

            // If there is nothing blocking us continue.
            Vector3 offset = new Vector3(0, 0.2f, 0);
            Vector3 lineTo = (temp.transform.position + offset) - hand.position;

            // Ray r = new Ray(hand.position, (hand.position - temp.transform.position).normalized);
            Ray r = new Ray(hand.position, lineTo);
            RaycastHit hit;
            Physics.Raycast(r, out hit, lineTo.magnitude, 1);

            // If no hit, nothing there, but also ignore self collision.
            if (hit.collider == null || hit.collider.gameObject == temp)
            {
                if (closest == null || lineTo.magnitude < (hand.position -
                closest.transform.position).magnitude)
                {
                    closest = temp;
                }
            }
        }
        return closest;
    }

    private void PlaceInHand(GameObject inHand)
    {
        Transform hand = transform.Find("Camera/Hand");

        // Break old parenting and positioning.
        inHand.transform.parent = null;

        // Parent to hand.
        inHand.transform.parent = hand;
        inHand.transform.localPosition = new Vector3(0, 0, 0);
        inHand.transform.localRotation = hand.localRotation;
    }

    // Release action.
    public void OnRelease(InputAction.CallbackContext context)
    {
        if (inHand != null)
        {
            if (releaseScript != null)
            {
                releaseScript.SnapRelease(inHand);
            }
            else
            {
                inHand.transform.parent = null;
            }
            inHand = null;
        }
    }

    // Register and unregiser within range.
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.tag == "Grabbable" || other.tag == "Pickup")
        {
            // Remove before add to ensure one OnGrab\release call.
            grab.started -= OnGrab;
            release.performed -= OnRelease;
            grab.started += OnGrab;
            release.performed += OnRelease;
            inRange.Add(other.gameObject);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Grabbable" || other.tag == "Pickup")
        {
            inRange.Remove(other.gameObject);
            // Last one, deregister.
            if (inRange.Count == 0)
            {
                grab.started -= OnGrab;
                release.performed -= OnRelease;
            }
        }
    }

    // Update is called once per frame.
    void Update()
    {
        
    }
}
