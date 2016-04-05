using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;

public class touchhair : MonoBehaviour
{

    void Start()
    {

    }
    public void SentCutPos()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "hair")
            .FirstOrDefault()
            .Subscribe(x => {
                var system = GameObject.Find("System").GetComponent<Cutting>();
                system.CutMain(this.transform.localPosition.y);
               
                
            });

    }

}
