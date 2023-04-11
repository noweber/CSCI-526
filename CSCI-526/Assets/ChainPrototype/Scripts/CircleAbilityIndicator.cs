using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// +1Move GameObject instantiated whenever ANY circle uses its ability
public class CircleAbilityIndicator : MonoBehaviour
{
    [SerializeField] SpriteRenderer plus1, plus2;      // +
    [SerializeField] SpriteRenderer one;        // 1 icon
    [SerializeField] SpriteRenderer shoe;
    [SerializeField] Color startColor;
    float moveSpeed = 0.25f;       // How fast the indicator will float upwards
    float fadeSpeed = 0.75f;        // How fast the indicator will fade

    private IEnumerator FadeAway()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime * fadeSpeed)
        {
            plus1.color = new Color(startColor.r, startColor.g, startColor.b, i);
            plus2.color = new Color(startColor.r, startColor.g, startColor.b, i);
            one.color = new Color(startColor.r, startColor.g, startColor.b, i);
            shoe.color = new Color(startColor.r, startColor.g, startColor.b, i);
            this.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    // Start the coroutine when instantiated
    void Awake()
    {
        plus1.color = startColor;
        plus2.color = startColor;
        one.color = startColor;
        shoe.color = startColor;
        StartCoroutine(FadeAway());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
