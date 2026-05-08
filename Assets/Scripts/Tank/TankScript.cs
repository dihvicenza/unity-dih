using UnityEngine;

public class TankScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject turret;
    [SerializeField] GameObject muzzle;
    [SerializeField] float smoothRotaion;
    GameObject target;
    void Start()
    {
        //assicurarsi che il player abbia il tag settato
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var torret = turret.transform;
        var muzzl = muzzle.transform;

        //ruota il muzzle
        Vector3 localTarget = muzzl.InverseTransformPoint(target.transform.position);
        float angleX = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
        Quaternion muzzleRotation = Quaternion.Euler(angleX, 0, 0);
        muzzl.localRotation = Quaternion.Slerp(muzzl.localRotation, muzzleRotation, Time.deltaTime * smoothRotaion);

        //ruota la torretta
        Quaternion rotDestination = Quaternion.LookRotation(target.transform.position - torret.position);
        rotDestination.x = 0;
        torret.rotation = Quaternion.Slerp(torret.rotation, rotDestination, Time.deltaTime * smoothRotaion);

    }
}
