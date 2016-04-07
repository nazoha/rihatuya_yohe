using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

//切断に関するクラス
public class Cutting : MonoBehaviour
{
    public GameObject cuttinghair;
    private GameObject hairpos;
    //{
    //    get { return _hairpos; }
    //    set { _hairpos = GameObject.Find("customers/hair"); }
    //}
    private GameObject _hairpos;
    void Start()
    {
        hairpos = GameObject.Find("customers/hair");

    }
    public void CutMain(float pos)
    {
        hairpos = GameObject.Find("customers/hair");
        var scissorsPos = pos/0.65f;
        GameObject customers = GameObject.Find("customers");
        var newhair = Instantiate(cuttinghair, new Vector2(-0.05f, 4.5f), Quaternion.identity) as GameObject;
        newhair.transform.parent = customers.transform;
        newhair.AddComponent<Rigidbody2D>();

        newhair.transform.localPosition = GameObject.Find("customers/hair").transform.localPosition;

        GameObject oldhair = GameObject.Find("customers/hair");
        var oldhairpos = oldhair.transform.localPosition.y;
        newhair.transform.localPosition = new Vector2(oldhair.transform.localPosition.x, CutPos(scissorsPos));
        newhair.transform.localScale = new Vector2(oldhair.transform.localScale.x, CutScale(scissorsPos));
        //はさみが髪より低い場所だったら
        if (scissorsPos < oldhairpos)
        {
            oldhair.transform.localScale = new Vector2(oldhair.transform.localScale.x, scissorsPos);
        }

    }

    float ResizedefPos(float ypos = 0)
    {
        float newScale = 0;

        newScale = ypos / 0.65f;
        print(newScale);
        return newScale;

    }

    //切られたときの髪の長さ　吹っ飛ぶ方
    float CutScale(float ypos = 0)
    {

        var scale = hairpos.transform.localScale.y - ypos;
        if (scale < 0)
        {
            scale = 0;
        }
        return scale;
    }

    //カットされた髪の座標
    float CutPos(float ypos = 0)
    {
        var scale = ypos + hairpos.transform.localPosition.y;
        if (scale < 0)
        {
            scale = 0;
        }
        return scale;
    }

}
