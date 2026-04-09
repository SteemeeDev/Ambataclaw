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
    [SerializeField] LightFlicker dropOffLight;

    [Header("Settings")]
    [SerializeField] float grabForce = 1f;
    [SerializeField] float closeTime = 0.3f;
    [SerializeField] float letGoDist = 0.3f;

    [SerializeField] LayerMask plushiesLayer;

    public Collider heldItem;
    private Rigidbody heldItemRB;

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
        if (heldItem != null)
        {
            distToPlush = Vector3.Distance(grabPoint.position, heldItem.transform.position);

            if (distToPlush <= letGoDist)
            {
                heldItemRB.isKinematic = true;
                heldItemRB.MovePosition(Vector3.MoveTowards(heldItem.transform.position, grabPoint.position, grabForce * Time.fixedDeltaTime));
            }
            else
            {
                heldItemRB.isKinematic = false;
                heldItem = null;
                dropOffLight.StartCoroutine(dropOffLight.IEFlickerLight(1, true));
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

        Collider[] hitObjects = Physics.OverlapSphere(grabPoint.position, letGoDist*0.75f, plushiesLayer);
        Collider nearestObject = new Collider();

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Collider hitObject = hitObjects[i];
            if (i == 0) nearestObject = hitObject;
            else if (Vector3.Distance(grabPoint.position, hitObject.transform.position) < Vector3.Distance(grabPoint.position, nearestObject.transform.position))
                nearestObject = hitObject;  
        }
        heldItem = nearestObject;
        if (heldItem != null)
        {
            heldItemRB = heldItem.GetComponent<Rigidbody>();
            heldItem.transform.position = grabPoint.position;
            dropOffLight.StartCoroutine(dropOffLight.IEFlickerLight(1, false));
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

        if (heldItem != null)
        {
            heldItemRB.isKinematic = false;
            heldItem = null;
            dropOffLight.StartCoroutine(dropOffLight.IEFlickerLight(1, true));
        }
    }
}
