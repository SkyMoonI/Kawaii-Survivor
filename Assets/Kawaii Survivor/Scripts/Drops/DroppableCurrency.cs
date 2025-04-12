using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    [Header("Settings")]
    protected bool m_isCollected; // flag to check if the candy is collected

    void OnEnable()
    {
        m_isCollected = false; // reset the flag when the object is enabled
    }

    public void Collect(Player player)
    {
        if (m_isCollected) return; // check if the candy is already collected

        m_isCollected = true; // set the flag to true

        StartCoroutine(MoveTowardsPlayer(player)); // start the collect routine

    }

    private IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0f;
        Vector2 startPosition = transform.position; // get the starting position of the candy

        while (timer < 1f)
        {
            Vector2 playerPosition = player.GetCenterPosition(); // get the player's position each time because player moves

            transform.position = Vector2.Lerp(startPosition, playerPosition, timer); // move the candy towards the player

            timer += Time.deltaTime; // increment the timer
            yield return null; // wait for the next frame
        }

        Collected(); // call the collected method when the candy reaches the player
    }

    protected abstract void Collected();
}
