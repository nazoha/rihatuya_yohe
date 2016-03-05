using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

//  startui/background/startbuttonにアタッチ
public class StartGame : MonoBehaviour {

	void Start () {
        var tapStart = this.UpdateAsObservable()
            .Where(startButton => Input.GetMouseButtonDown(0));
        tapStart
        .Subscribe(statButton =>
        {

        });
	}
   public void StartButton()
    {
        //UIの非表示
        GameObject.Find("startui").GetComponent<Canvas>().enabled = false;
        //ボタンが押されないようにする
        this.GetComponent<Button>().interactable = false;
        print("スタートゲーム");
    }
	

}
