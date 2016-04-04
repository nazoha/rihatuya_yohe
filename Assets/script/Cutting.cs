using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

//切断に関するクラス
public class Cutting : MonoBehaviour
{
    public GameObject cuttinghair;

    private GameObject hairpos
    {
        get { return _hairpos; }
        set { _hairpos = GameObject.Find("customers/hair"); }
    }
    private GameObject _hairpos;
    void Start()
    {


        GameObject customers = GameObject.Find("customers");

        var instance = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .FirstOrDefault();
        instance
            .Subscribe(_ => {

                var newhair = Instantiate(cuttinghair, new Vector2(-0.05f, 4.5f), Quaternion.identity) as GameObject;
                newhair.transform.parent = customers.transform;
                newhair.AddComponent<Rigidbody2D>();
                
                newhair.transform.localPosition = GameObject.Find("customers/hair").transform.localPosition;

                GameObject oldhair = GameObject.Find("customers/hair");
                
                newhair.transform.localPosition = new Vector2(oldhair.transform.localPosition.x, CutPos(1f));
                newhair.transform.localScale = new Vector2(oldhair.transform.localScale.x, CutScale(1f));
                oldhair.transform.localScale = new Vector2(oldhair.transform.localScale.x, 1f);
            });
    }

    //切られたときの髪の長さ　吹っ飛ぶ方
    float CutScale(float ypos = 0)
    {
        GameObject hairpos = GameObject.Find("customers/hair");

        print("hairpos.transform.lossyScale.y=" + hairpos.transform.localScale.y);
        var scale = hairpos.transform.localScale.y - ypos;
        print("scale="+scale+ "hairpos.transform.localScale.y="+ hairpos.transform.localScale.y+ "ypos="+ ypos);
        if (scale < 0)
        {
            scale = 0;
        }
        return scale;
    }

    //吹っ飛ばされない髪の法
    float CutPos(float ypos = 0)
    {
        GameObject hairpos = GameObject.Find("customers/hair");
        var scale = ypos + hairpos.transform.localPosition.y;
        if (scale < 0)
        {
            scale = 0;
        }
        return scale;
    }
}
