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
    float ypos;

    void Awake()
    {
        scissors = GameObject.Find("hasami");
        ypos = 2;
    }
    void Start()
    {
        //マウスの右移動
        rightmove = this.UpdateAsObservable()
             .FirstOrDefault(_ => Input.GetMouseButtonDown(0));
        rightmove
            .Subscribe(_ =>
            {
                Right();
                print("終わり");
                ypos= scissors.GetComponent<Transform>().position.y;
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
        scissorspos.position = new Vector2(scissorspos.position.x, 2 + Mathf.PingPong((timer* speed), y - 2));
    }

    //はさみを右に移動させるメソッド
    float timer = 0;
    bool movedirection = false;
    void Right()
    {
        timer = 0;
        var scissorspos = scissors.GetComponent<Transform>();
        var moveright = this.UpdateAsObservable()

        .TakeWhile(_=>!movedirection);
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
                        Left();
                    }

            }
            );
    }
    int i = 0;

    void Left()
    {
        var scissorspos = scissors.GetComponent<Transform>();
        moverleft = this.UpdateAsObservable()
            .TakeWhile(_ => movedirection); 
        moverleft
            .Subscribe(_ =>
            {
                    timer -= Time.deltaTime;
                    //scissorspos.position = new Vector2(speed * timer, scissorspos.position.y);
                   scissorspos.position = Vector2.Lerp(new Vector2(0, scissorspos.position.y), new Vector2(timer * speed, scissorspos.position.y), 1f);

                    print("主家えじゃ");
                    if (scissorspos.position.x<=0f)
                    {
                        movedirection = false;
                        Invoke("Start", 0.3f);
                        print("ｙ座標="+ypos);

                    }
                
            });
    }

}
