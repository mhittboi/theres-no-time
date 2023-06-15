using UnityEngine;
using UnityEngine.UI;

public class CrownCollection : MonoBehaviour
{
    public Sprite crownCollectedSprite;
    public Image crownUIImage;

    // check if colliding with player then disappear if yes & set crownCollected to true
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().crownCollected = true;
            crownUIImage.sprite = crownCollectedSprite;
            Destroy(gameObject);
        }
    }
}