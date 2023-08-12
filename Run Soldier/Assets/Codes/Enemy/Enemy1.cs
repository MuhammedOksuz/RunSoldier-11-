using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    Animator anim;
    BoxCollider boxC;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject bullet_UI;
    private void Awake()
    {
        boxC = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
    }
    public void Die()
    {
        StartCoroutine(Die_());
    }
    IEnumerator Die_()
    {
        int chance = Random.Range(0, 101);
        anim.SetTrigger("Die");
        Destroy(pistol.gameObject);
        yield return new WaitForSeconds(0.5f);
        boxC.enabled = false;
        if (chance < 75)
        {
            GameObject go = Instantiate(bullet_UI, transform.position + new Vector3(0, 1.5f, -1f), bullet_UI.transform.rotation);
            go.transform.parent = null;
            Destroy(go.gameObject, 15);
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
