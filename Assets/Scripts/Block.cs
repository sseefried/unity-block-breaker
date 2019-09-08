using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // configuration parameters
    [SerializeField] GameObject blockSparklesVFX;
	[SerializeField] Sprite[] hitSprites;

    // cached references
    Level level;
    GameSession gameSession;
    AudioSource breakSoundSource;

    // state
    [SerializeField] int timesHit; // TODO only serialized for debug purposes

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        IncrementBreakableBlocks();
    }

    private void IncrementBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.IncrementBlocks();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (tag == "Breakable")
        {
			HandleHit();
		}
    }


    private void HandleHit()
	{
		timesHit++;
        int maxHits = hitSprites.Length + 1;
		if (timesHit >= maxHits)
		{
			DestroyBlock();
		}
		else
		{
			ShowNextHitSprite();
		}

	}

    private void ShowNextHitSprite()
	{
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[timesHit - 1];
        } else
        {
         
            Debug.LogError("Block sprite at index not found: " + gameObject.name);
        }
    }

	private void DestroyBlock()
    {
        gameSession.PlayBlockBreakSound();
        gameSession.AddToScore();
        level.BlockWasBroken();
        TriggerSparklesVFX();
        Destroy(gameObject);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 2f);
    }
}
