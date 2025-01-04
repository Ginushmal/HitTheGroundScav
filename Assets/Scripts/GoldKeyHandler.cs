using UnityEngine;

public class GoldKeyHandler : MonoBehaviour
{
    private bool hasGoldKey = false;

    public GameObject closeUpGoldKey;

    public GameObject goldLock;

    public GameObject closeUpSafe;

    public PrograssTracker prograssTracker;
    public void GoldKeyCollected()
    {
        hasGoldKey = true;
        closeUpGoldKey.SetActive(true);
        closeUpSafe.SetActive(false);
        prograssTracker.Gold_Key_Found();
        Debug.Log("Gold Key Collected");
    }

    public void clickGoldLock()
    {
        if (hasGoldKey)
        {
            goldLock.SetActive(false);
            prograssTracker.Gold_Lock_Opened();
            Debug.Log("Gold Lock Opened");
        }
        else
        {
            Debug.Log("Gold Lock Locked");
        }
    }
}
