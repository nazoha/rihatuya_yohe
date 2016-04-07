using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Move : MonoBehaviour {

    public GameObject customer;
    GameObject hoge1, hoge2;
    public Setting setting;
    [System.Serializable]
    public struct Setting
    {
        public float CustomerDefPos;
        public float Interval;
        public float DestroyPos;

    }
	void Start () {
        Application.targetFrameRate = 60;

	}
    void Customer()
    {
        float timer = 0;
        var customer = GameObject.Find("customers");
        var ExitShop = this.UpdateAsObservable()
            .Where(x => setting.DestroyPos < customer.transform.position.x);
        ExitShop
            .Subscribe(x =>
            {
                timer += Time.deltaTime;
                customer.transform.localPosition = new Vector2(timer * setting.Interval, customer.transform.localPosition.y);
                // Vector2.Lerp(new Vector2(setting.CustomerDefPos, customer.transform.localPosition.y), new Vector2(timer * setting.Interval, customer.transform.localPosition.y), 1f);
                MoveAnotherCustomer(timer);
                if (customer.transform.position.x < setting.DestroyPos)
                {
                    Destroy(customer);
                    CreateCustomer();
                }

            })
            .AddTo(customer);

    }
    void CreateCustomer()
    {
        var customer = GameObject.Find("customer1");
        customer.name = "customers";

        var customer2 = GameObject.Find("customer2");
        customer2.name = "customer1";
        var newCustomer=Instantiate(customer,new Vector2((setting.CustomerDefPos + (setting.DestroyPos * -2)), -2.61f),Quaternion.identity);
        newCustomer.name = "customer2";
    }
    void MoveAnotherCustomer(float time)
    {
        hoge1 = GameObject.Find("customer1");
        hoge2 = GameObject.Find("customer2");

        hoge1.transform.localPosition = new Vector2(-(-setting.CustomerDefPos+setting.DestroyPos)+ time*setting.Interval, hoge1.transform.localPosition.y);
        print(hoge1.transform.localPosition);

        hoge2.transform.localPosition = new Vector2((setting.CustomerDefPos+(setting.DestroyPos * -2))+ time*setting.Interval, hoge2.transform.localPosition.y);
    }
}
