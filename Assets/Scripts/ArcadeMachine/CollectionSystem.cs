using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CollectionSystem : MonoBehaviour
{
    [SerializeField] GameObject[] ShelfPlushies;
    [SerializeField] AudioSource audioSource;
    List<PlushieType> collectedPlushies = new List<PlushieType>();

    // This needs to be in the order they show up on the shelf
    public enum PlushieType
    {
        Kanin,
        Bjørn,
        Skildpadde,
        Key
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Plushie"))
        {
            Plushie plushie = collision.gameObject.GetComponent<Plushie>();
            bool alreadyCollected = collectedPlushies.Contains(plushie.plushieType);
            if (!alreadyCollected)
            {
                collectedPlushies.Add(plushie.plushieType);
                UpdateShelf();
            }
            audioSource.Play();
            Destroy(collision.gameObject);
        }
    }

    void UpdateShelf()
    {
        foreach (PlushieType type in collectedPlushies)
        {
            ShelfPlushies[(int)type].SetActive(true);
        }
    }
}
