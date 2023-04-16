using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    public Animator anim;
    public void AnimationEnd()
    {
        Destroy(this.gameObject);
    }
    // On Instantiate
    void Awake()
    {
        int scale = Random.Range(2, 8);
        this.transform.localScale = new Vector3(scale, scale, scale);
        anim.Play("Comet Anim");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
