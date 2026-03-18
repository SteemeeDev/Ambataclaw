using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawHandler : MonoBehaviour
{
    [SerializeField] LineRenderer ropeRenderer;
    [SerializeField] Transform ropeStartPoint;
    [SerializeField] Transform ropeEndPoint;
    [SerializeField] float grabForce = 1f;
    [SerializeField] SkinnedMeshRenderer clawRenderer;
    [SerializeField] Transform grabPoint;
    [SerializeField] float closeTime = 0.5f;
    [SerializeField] float letGoDist = 0.5f;

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

                HeldItem.GetComponent<Rigidbody>().isKinematic = true;
                HeldItem.transform.position = Vector3.MoveTowards(HeldItem.transform.position, grabPoint.position, grabForce * Time.fixedDeltaTime);
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

        Collider[] hitObjects = Physics.OverlapSphere(grabPoint.position, 0.4f, PlushiesLayer);

        foreach (Collider hitObject in hitObjects)
        {
            HeldItem = hitObject;
        }
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
