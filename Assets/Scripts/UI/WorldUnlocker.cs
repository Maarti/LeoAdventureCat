using UnityEngine;
using UnityEngine.UI;



public class WorldUnlocker : MonoBehaviour {

    public int price = 20;
    public WorldButtonInitializer btnInit;
    public Button buyBtn;

    public void DisplayPopup(WorldButtonInitializer init) {
        this.btnInit = init;
        buyBtn.interactable = !(ApplicationController.ac.playerData.kittyz < price);
        this.gameObject.SetActive(true);
    }

    public void BuyWorld() {
        if(ApplicationController.ac.playerData.kittyz >= price && btnInit != null) {
            ApplicationController.ac.UnlockWorld(btnInit.worldEnum);
            ApplicationController.ac.playerData.updateKittys(-price, null, true);
            btnInit.Init();
        }
        this.gameObject.SetActive(false);
    }
}
