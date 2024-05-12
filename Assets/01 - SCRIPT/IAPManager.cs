using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager :Singleton<IAPManager>
{
    [SerializeField] Button change;
    [SerializeField] Button hint;
    [SerializeField] LoadSavePP _pp;
    // Start is called before the first frame update
    //void Start()
    //{
      
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
   
    
    public void BuyChangeSus()
    {
        //GameController.instance.Change();
        Debug.Log("Buy Success!");
        setBtnChange();
        _pp.GetIAP(true);
    }    
    public void BuyChangeFail()
    {
        Debug.Log("Buy Failed!");
        setBtnChange();
    }
    
    public void BuyHintSus()
    {
        //GameController.instance.Hint();
        setBtnHint();
        _pp.GetIAP(false);
    }
    public void BuyHintFail()
    {
        setBtnHint();
    }
    private void setBtnHint()
    {
        hint.interactable = true;
        //GameManager.instance.isIAP = false;
    }
    private void setBtnChange()
    {
        change.interactable = true;
        //GameManager.instance.isIAP = false;
    }

}
