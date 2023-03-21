using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseEvent : MonoBehaviour
{
    private float startTime = 0;
    private Vector3 startPos = new Vector3();
    private Quaternion startRot = Quaternion.identity;
    private GameObject item;
    private Vector3 offset;
    [SerializeField]
    public float speed = 0.5f;
    public void SnapRelease(GameObject obj)
    {
        // Deparent just in case.
        obj.transform.parent = null;

        // Align with area.
        obj.transform.rotation = this.transform.rotation;
        obj.transform.position = this.transform.position;
        Bounds bounds = obj.GetComponent<Collider>().bounds;
        obj.transform.position = new Vector3(transform.position.x, transform.position.y + bounds.size.y / 2,
        transform.position.z);
    }

    // Register and unregister within range.
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Hand" || other.name.Contains("Controller"))
        {
            CollectEvent script = other.gameObject.GetComponentInParent<CollectEvent>();
            script.setReleaseFunction(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Hand" || other.name.Contains("Controller"))
        {
            CollectEvent script = other.gameObject.GetComponentInParent<CollectEvent>();
            script.setReleaseFunction(null);
        }
    }

    // Start is called before the first frame update.
    void Start()
    {
        
    }

    // Update is called once per frame.
    public void Update()
    {
        if (item != null)
        {
            float percent = (Time.realtimeSinceStartup - startTime) / speed;
            Vector3 position = Vector3.Slerp(startPos, transform.position + offset, percent);
            Quaternion rot = Quaternion.Slerp(startRot, transform.rotation, percent);
            item.transform.position = position;
            item.transform.rotation = rot;

            // Turn off animation for efficiency.
            if (percent >= 1)
                item = null;
        }
    }
}
