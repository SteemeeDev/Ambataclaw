using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlushiePipeManager : MonoBehaviour
{
    [SerializeField] GameObject[] containedPlushiesPrefabs;
    int plushieIndex;
    [SerializeField] LayerMask interactionLayer;

    [SerializeField] float minTimeToSpawnPlushie;
    [SerializeField] float maxTimeToSpawnPlushie;
    [SerializeField] float cooldownTime = 0.5f;
    float cooldown = 0f;

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform plushieParent;
    [SerializeField] CameraManager camManager;
    [SerializeField] Animator leverAnimator;

    float timeToNextSpawn = 0f;
    float timeSinceLastSpawn = 0f;

    [SerializeField] int storedPlushies = 0;

    private void Update()
    {
        Ray ray = camManager.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        timeSinceLastSpawn += Time.deltaTime;
        cooldown += Time.deltaTime;

        if (timeSinceLastSpawn > timeToNextSpawn)
        {
            timeSinceLastSpawn = 0;
            timeToNextSpawn = Random.Range(minTimeToSpawnPlushie, maxTimeToSpawnPlushie);

            storedPlushies++;
        }

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, interactionLayer) && cooldown > cooldownTime)
        {
            if (hit.collider.gameObject == gameObject)
            {
                cooldown = 0f;
                leverAnimator.SetTrigger("Flip");
                if (storedPlushies > 0 && plushieIndex < containedPlushiesPrefabs.Length )
                {
                    storedPlushies--;
                    StartCoroutine(IESpawnPlushie());
                }
            }
        }

    }

    IEnumerator IESpawnPlushie()
    {
        GameObject spawnedPlushie = Instantiate(containedPlushiesPrefabs[plushieIndex], plushieParent);
        Rigidbody plushieRb = spawnedPlushie.GetComponent<Rigidbody>();
        plushieRb.isKinematic = true;
        spawnedPlushie.transform.localScale = Vector3.zero;
        spawnedPlushie.transform.position = spawnPoint.position;
        plushieIndex++;

        float elapsed = 0f;
        while (elapsed < 0.35f)
        {
            spawnedPlushie.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsed / 0.35f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        plushieRb.isKinematic = false;
    }
}
