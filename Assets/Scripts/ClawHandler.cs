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
    [SerializeField] float closeTime = 0.5f;

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

    private void FixedUpdate()
    {
        if (HeldItem != null)
        {
            Rigidbody rigidbody = HeldItem.gameObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddForce((grabPoint.position - HeldItem.transform.position) * 20);
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

        Collider[] hitObjects = Physics.OverlapSphere(grabPoint.position, 1f, PlushiesLayer);

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

            float blend = Mathf.Lerp(0f, 100f, elapsed / closeTime);

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

        elapsed = 1f;

        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;

            float blend = Mathf.Lerp(0f, 100f, elapsed / closeTime);

            clawRenderer.SetBlendShapeWeight(0, blend);

            yield return null;
        }

        clawIsAnimating = false;

        HeldItem = null;
    }
}
