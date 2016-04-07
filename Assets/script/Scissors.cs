using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Scissors : MonoBehaviour
{

    [Header("はさみの可動域")]
    public float y = 10f;
    [Header("はさみの速さ")]
    public float speed = 1f;
    private GameObject scissors;
    private float limitpos = 3f;
    IObservable<Unit> moverleft;
    IObservable<Unit> rightmove;
    //はさみを右に移動させるメソッド
    float timer = 0;
    bool movedirection = false;
    Transform scissorspos;

    void Awake()
    {
        scissors = GameObject.Find("hasami");

    }
    void Start()
    {
        scissorspos = scissors.GetComponent<Transform>();
        //マウスの右移動
        rightmove = this.UpdateAsObservable()
             .FirstOrDefault(_ => Input.GetMouseButtonDown(0));
        rightmove
            .Subscribe(_ =>
            {
                Right();
                GameObject.Find("hasami").GetComponent<touchhair>().SentCutPos();
            }
            );
        //マウスの上下移動 1クリックしたらupanddonwscissorsを終了させる
        var move = this.UpdateAsObservable();
        move
            .TakeUntil(rightmove)
            .Subscribe(_ => UpandDown());
    }

    //はさみの上下に関するメソッド
    void UpandDown()
    {

        timer += Time.deltaTime;
        var scissorspos = scissors.GetComponent<Transform>();
        scissorspos.localPosition = new Vector2(scissorspos.localPosition.x, Mathf.PingPong((timer * speed), y));
    }


    void Right()
    {
        timer = 0;
        //var scissorspos = scissors.GetComponent<Transform>();
        var moveright = this.UpdateAsObservable()

        .TakeWhile(_ => !movedirection);
        //.Where(x => (int)scissorspos.position.x < limitpos);

        moveright
            .Subscribe(move =>
            {
                //時間ごとに更新
                timer += Time.deltaTime;
                //scissorspos.position = new Vector2(Mathf.PingPong(timer * speed,y-2), scissorspos.position.y);
                scissorspos.position = Vector2.Lerp(new Vector2(0, scissorspos.position.y), new Vector2(timer * speed, scissorspos.position.y), 1f);
                if ((int)scissorspos.position.x == limitpos)
                {
                    movedirection = true;
                    Invoke("Left", 0.3f);
                    //Left();
                }

            }
            );
    }

    void Left()
    {
        moverleft = this.UpdateAsObservable()
            .TakeWhile(_ => movedirection);
        moverleft
            .Subscribe(_ =>
            {
                timer -= Time.deltaTime;
                scissorspos.position = Vector2.Lerp(new Vector2(0, scissorspos.position.y), new Vector2(timer * speed, scissorspos.position.y), 1f);

                if (scissorspos.position.x <= 0f)
                {
                    movedirection = false;
                    Invoke("Start", 0.3f);


                    Invoke("callMove", 0.3f);
                }

            });
    }
    void callMove()
    {
        var move = this.GetComponent<Move>();
        move.Customer();
    }

}
