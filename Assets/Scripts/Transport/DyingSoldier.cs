using UnityEngine;

public class DyingSoldier : MonoBehaviour, IPickable, ILoadable
{
    public void OnLoad() => OnPick();
    public void OnUnload() => OnPut();

    public void OnPick()
    {
        //GetComponent<Soldier>().SetRagdoll(false);
        GetComponent<Soldier>().FreezeRagdollCenter(true);
        GetComponent<Collider>().enabled = false;
    }

    public void OnPut()
    {
        //GetComponent<Soldier>().SetRagdoll(true);
        GetComponent<Soldier>().FreezeRagdollCenter(false);
        GetComponent<Collider>().enabled = true;
    }
}
