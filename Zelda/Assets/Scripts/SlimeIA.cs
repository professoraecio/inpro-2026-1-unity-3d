using UnityEngine;
using System.Collections;
public class SlimeIA : MonoBehaviour
{
    private Animator anim;
    public int HP = 3;
    private bool isDie = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    #region 
    void GetHit(int amount)
    {
        if(isDie){return;}
        HP -= amount;
        if(HP > 0)
            anim.SetTrigger("GetHit");
        else
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }
    #endregion
}
