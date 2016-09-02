using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainMaker : MonoBehaviour
{
    public RuntimeSpriteAtlas sushiAtlas;
    public float rainXRange = 20;

    private Sprite[] m_sushiSprites;
    private float m_rainStartY;
    private Coroutine m_Coroutine;
    
	void Start ()
    {
        m_sushiSprites = sushiAtlas.sprites;
        m_rainStartY = GetComponent<Transform>().position.y;
        StartRain();
    }
	
	void Update ()
    {
        if (Input.anyKey)
            StartRain();
	}

    private void StartRain()
    {
        StopCoroutine("Rain");
        StartCoroutine("Rain");
    }

    IEnumerator Rain()
    {
        foreach (var sushi in m_sushiSprites)
        {
            var go = new GameObject("raindrop");
            var transform = go.GetComponent<Transform>();
            transform.position = new Vector3(Random.Range(-rainXRange, rainXRange), m_rainStartY);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sushi;

            go.AddComponent<Rigidbody2D>();
            go.AddComponent<PolygonCollider2D>();

            yield return new WaitForEndOfFrame();
        }
    }
}
