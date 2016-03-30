using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScript : MonoBehaviour
{
    public GameObject  explosivePrefeb,scene;
    public bool initialDataLoaded = false;

	private GameObject currentExplosive;
    private bool touchDown = false, blowed = false;
    private Vector3 mousePosition;
	Vector3 pos;


    void Start()
    {
		loadInitialData();
    }

    //初期データ
    public void loadInitialData()
    {
        blowed = false;
        initialDataLoaded = true;
    }

    void Update()
    {
        
        checkForTouchActions();
    }
   
    private void checkForTouchActions()
    {
        //クリックした場所に爆弾を設置
        if (Input.GetMouseButton(0))
        {
            touchDown = true;
            //設置する座標を取得
            RaycastHit hit;
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.name.Equals("Blow"))
                    {
                        blastExplosive();
                        return;
                    }
                }
            }
            if (!currentExplosive)
            {
                createCurrentExplosive(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchDown = false;
            if (currentExplosive && currentExplosive.GetComponent<ExplosiveScript>().actionUpPerformed())
                currentExplosive = null;

        }
        if (touchDown)
        {
            if (currentExplosive)
            {
				currentExplosive.GetComponent<ExplosiveScript>().setProperPosition();
            }
        }

    }

   
    private void createCurrentExplosive(Vector3 position)
    {
        position.z = 0;
        currentExplosive = (GameObject)Instantiate(explosivePrefeb, position, Quaternion.identity);
        currentExplosive.tag = "Explosive";
    }
    private void blastExplosive()
    {

        if (!blowed)
        {
            blowed = true;
            scene.BroadcastMessage("blastClicked", SendMessageOptions.DontRequireReceiver);
        }
    }
}