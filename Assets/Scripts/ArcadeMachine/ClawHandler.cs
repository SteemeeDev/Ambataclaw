using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawHandler : MonoBehaviour
{
    [SerializeField] LineRenderer ropeRenderer;
    [SerializeField] Transform ropeStartPoint;
    [SerializeField] Transform ropeEndPoint;
    [SerializeField] SkinnedMeshRenderer clawRenderer;
    [SerializeField] Transform grabPoint;

    [Header("Settings")]
    [SerializeField] float grabForce = 1f;
    [SerializeField] float closeTime = 0.3f;
    [SerializeField] float letGoDist = 0.3f;

    [SerializeField] LayerMask PlushiesLayer;

    public Collider HeldItem;

    public bool clawOpen;
    public bool clawIsAnimating;

    public Coroutine openingRoutine;

    float elapsed;

    private void Start()
    {
        ropeRenderer.positionCount = 2;
    }

    float distToPlush;
    private void FixedUpdate()
    {
        if (HeldItem != null)
        {
            distToPlush = Vector3.Distance(grabPoint.position, HeldItem.transform.position);

            if (distToPlush <= letGoDist)
            {
                HeldItem.transform.position = Vector3.MoveTowards(HeldItem.transform.position, grabPoint.position, grabForce * Time.fixedDeltaTime);
                HeldItem.GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                HeldItem.GetComponent<Rigidbody>().isKinematic = false;
                HeldItem = null;
            }
        }
    }

    private void Update()
    {
        UpdateRopeRenderer();
    }

    void UpdateRopeRenderer()
    {
        ropeRenderer.SetPosition(0, ropeStartPoint.position);
        ropeRenderer.SetPosition(1, ropeEndPoint.position);
    }
    public void GrabObject()
    {
        if (grabPoint == null) return;

        Collider[] hitObjects = Physics.OverlapSphere(grabPoint.position, letGoDist * 0.8f, PlushiesLayer);
        Collider nearestObject = new Collider();

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Collider hitObject = hitObjects[i];
            if (i == 0) nearestObject = hitObject;
            else if (Vector3.Distance(grabPoint.position, hitObject.transform.position) < Vector3.Distance(grabPoint.position, nearestObject.transform.position))
                nearestObject = hitObject;  
        }
        HeldItem = nearestObject;
        if (HeldItem != null) HeldItem.transform.position = grabPoint.position;
    }

    public IEnumerator IEcloseClaw()
    {
        clawIsAnimating = true;
        clawOpen = false;

        elapsed = 0f;

        while (elapsed < closeTime)
        {
            elapsed += Time.deltaTime;

            float blend = Mathf.Lerp(0f, 200f, elapsed / closeTime);

            clawRenderer.SetBlendShapeWeight(0, blend);

            yield return null;
        }

        clawIsAnimating = false;

        GrabObject();
    }
    public IEnumerator IEopenClaw()
    {
        clawIsAnimating = true;
        clawOpen = true;

        elapsed = closeTime;

        while (elapsed >= 0)
        {
            elapsed -= Time.deltaTime;

            float blend = Mathf.Lerp(0f, 200f, elapsed / closeTime);

            clawRenderer.SetBlendShapeWeight(0, blend);

            yield return null;
        }

        clawIsAnimating = false;

        if (HeldItem != null)
        {
            HeldItem.GetComponent<Rigidbody>().isKinematic = false; 
            HeldItem = null;
        }
    }
}
