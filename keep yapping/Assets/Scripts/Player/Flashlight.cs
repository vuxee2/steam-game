using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
    public GameObject lightSrc;
    public Animator anim;

    private float changeDelay = 1f;
    private float lastChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && lastChange + changeDelay < Time.time)
        {
            StartCoroutine(OnOffFlash());

            if(!lightSrc.activeSelf)
                anim.SetTrigger("onFlashTrigger");
            else
                anim.SetTrigger("offFlashTrigger");

            lastChange = Time.time;
        }
    }

    private IEnumerator OnOffFlash()
    {
        yield return new WaitForSeconds(.7f);
        lightSrc.SetActive(!lightSrc.activeSelf);
    }
}
