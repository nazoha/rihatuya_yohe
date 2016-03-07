using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;

public class system : MonoBehaviour {
    [Header("はさみの可動域")]
    public float y=10f;
    [Header("はさみの速さ")]
    public float speed = 1f;
    private GameObject scissors;
    void Awake()
    {
        scissors = GameObject.Find("hasami");
    }

	void Start () {
        var movescissors = this.UpdateAsObservable().Select(_=>_);
        movescissors.Subscribe(_ => MoveScissors());
	}
    //はさみの上下に関するメソッド
    void MoveScissors()
    {
     var   scissorspos = scissors.GetComponent<Transform>();
        scissorspos.position = new Vector2(scissorspos.position.x, 2+Mathf.PingPong(Time.time*speed, y-2));
    }
}

