using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] int scoreValue = 10;
    [SerializeField] int hitPoints = 50;
    [SerializeField] float waitForHitView = 0.02f;

    ScoreBoard scoreBoard;
    PlayerControls player;
    Renderer enemyRenderer;
    GameObject parentGameObject;

    private void Start()
    {
        parentGameObject = GameObject.FindWithTag("SpawnAtRunTime");
        scoreBoard = FindObjectOfType<ScoreBoard>();
        player = FindObjectOfType<PlayerControls>();
        enemyRenderer = GetComponent<Renderer>();
        AddRigidbody();
    }

    private void AddRigidbody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if(hitPoints < 1)
        {
            KillEnemy();
        }
    }

    private void ProcessHit()
    {
        hitPoints -= player.getLaserDamage();
        StartCoroutine(hitVisualizer());
    }

    IEnumerator hitVisualizer()
    {
        Color originalColor = enemyRenderer.material.color;
        enemyRenderer.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(waitForHitView);
        enemyRenderer.material.SetColor("_Color", originalColor);
    }

    private void KillEnemy()
    {
        scoreBoard.IncreaseScore(scoreValue);
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parentGameObject.transform;
        Destroy(this.gameObject);
    }
}
