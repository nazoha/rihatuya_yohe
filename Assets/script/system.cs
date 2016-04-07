using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;

public class system : MonoBehaviour
{
    [Header("はさみの可動域")]
    public float y = 10f;
    [Header("はさみの速さ")]
    public float speed = 1f;
    private GameObject scissors;

    private float limitpos = 3f;

    IObservable<Unit> rightmovescissors;
    void Awake()
    {
        scissors = GameObject.Find("hasami");
    }

    void Start()
    {
        //マウスの右移動
        rightmovescissors = this.UpdateAsObservable()
           .First(_ => Input.GetMouseButtonDown(0));
        rightmovescissors
            .Subscribe(_ =>
            {
                RightScissors();
                print("終わり");
            }

        );
        //マウスの上下移動 1クリックしたらupanddonwscissorsを終了させる
        var movescissors = this.UpdateAsObservable();
        movescissors
            .TakeUntil(rightmovescissors)
            .Subscribe(_ => UpandDownScissors());


    }

    //はさみの上下に関するメソッド
    void UpandDownScissors()
    {
        var scissorspos = scissors.GetComponent<Transform>();
        scissorspos.position = new Vector2(scissorspos.position.x, 2 + Mathf.PingPong(Time.time * speed, y - 2));
    }
    private IObservable<Unit> moveright;
    //はさみを右に移動させるメソッド
    float timer = 0;
    bool movedirection = false;
    void RightScissors()
    {
        var scissorspos = scissors.GetComponent<Transform>();
        moveright = this.UpdateAsObservable();
            //.TakeUntil(rightmovescissors)
            //.Where(x => (int)scissorspos.position.x < limitpos);

        moveright
            .Subscribe(move =>
            {
                if (!movedirection)
                {
                    //時間ごとに更新
                    timer += Time.deltaTime;
                    //scissorspos.position = new Vector2(Mathf.PingPong(timer * speed,y-2), scissorspos.position.y);
                    scissorspos.position = Vector2.Lerp(new Vector2(0, scissorspos.position.y), new Vector2(timer * speed, scissorspos.position.y), 1f);
                    if ((int)scissorspos.position.x == limitpos)
                    {
                        movedirection = true;
                    }
                }
                else
                {
                    LeftScissors();
                    print("ほげ");
                }
            }
            );
    }
    private IObservable<Unit> moverleft;
    void LeftScissors()
    {
        var scissorspos = scissors.GetComponent<Transform>();
        moverleft = this.UpdateAsObservable();
            //.TakeUntil(moveright)
            //.Where(x => 0f < scissorspos.position.x);
        moverleft
            .Subscribe(_ =>
            {
                timer -= Time.deltaTime;
                scissorspos.position = new Vector2(timer * speed, scissorspos.position.y);
                print("主家えじゃ");
                if(0f < scissorspos.position.x)
                {
                    movedirection = false;
                }
            });
    }
}

