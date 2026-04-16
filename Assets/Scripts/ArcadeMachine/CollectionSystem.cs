using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CollectionSystem : MonoBehaviour
{
    [SerializeField] GameObject[] ShelfPlushies;
    [SerializeField] AudioSource audioSource;
    [SerializeField] TMP_Text plushiesCollectedCounter;
    List<PlushieType> collectedPlushies = new List<PlushieType>();
    public int plushiesCollected = 0;
    public int winCondition;

    [SerializeField] GameObject[] infoPanels;
    Coroutine InfoPanelRoutine;
    GameObject currentInfoPanel;

    // This needs to be in the order they show up on the shelf
    public enum PlushieType
    {
        Rabbit,
        Bear,
        Turtle,
        Racoon,
        Key,
        Fox,
        Cat
    }

    private void Start()
    {
        plushiesCollectedCounter.text = plushiesCollected.ToString() + "/" + winCondition.ToString();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Plushie"))
        {
            plushiesCollected++;
            plushiesCollectedCounter.text = plushiesCollected.ToString() + "/" + winCondition.ToString();

            Plushie plushie = collision.gameObject.GetComponent<Plushie>();
            bool alreadyCollected = collectedPlushies.Contains(plushie.plushieType);
            if (!alreadyCollected)
            {
                collectedPlushies.Add(plushie.plushieType);
                UpdateShelf();
            }
            if (currentInfoPanel != null)
            {
                StopCoroutine(InfoPanelRoutine);
                currentInfoPanel.SetActive(false);
            }
            InfoPanelRoutine = StartCoroutine(ShowPlushieInfo(plushie.plushieType));

            audioSource.Play();
            Destroy(collision.gameObject);


        }
    }

    IEnumerator ShowPlushieInfo(PlushieType type)
    {
        currentInfoPanel = infoPanels[(int)type];

        infoPanels[(int)type].SetActive(true);

        yield return new WaitForSeconds(5f);

        infoPanels[(int)type].SetActive(false);

        currentInfoPanel = null;
    }

    void UpdateShelf()
    {
        foreach (PlushieType type in collectedPlushies)
        {
            ShelfPlushies[(int)type].SetActive(true);
        }
    }
}
