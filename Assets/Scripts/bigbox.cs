using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PA_DronePack;

public class bigbox : MonoBehaviour
{
    public GameObject UAV1, UAV2;
    private bool isHold1, isHold2;
    public Vector3 destPos;

    void Start()
    {
        isHold1 = isHold2 = false;
    }


    void Update()
    {
        if (isHold1 && isHold2)
        {
            Vector3 agent1pos = UAV1.transform.position;
            Vector3 agent2pos = UAV2.transform.position;

            if (Vector3.Distance(agent1pos, agent2pos) > 3.5f) {
                isHold1 = false;
                isHold2 = false;
                UAV1.GetComponent<UAVAgent>().isHold = false;
                UAV2.GetComponent<UAVAgent>().isHold = false;
                UAV1.GetComponent<UAVAgent>().boxType = 0;
                UAV2.GetComponent<UAVAgent>().boxType = 0;
                UAV1.GetComponent<UAVAgent>().GiveReward(-0.8f);
                UAV2.GetComponent<UAVAgent>().GiveReward(-0.8f);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            else {
                Vector3 p = (agent1pos + agent2pos) / 2;
                p.y = Mathf.Max(0.3f, p.y - 1.2f);
                gameObject.transform.position = p;
                UAV1.GetComponent<UAVAgent>().boxPos = p;
                UAV2.GetComponent<UAVAgent>().boxPos = p;
                //
            }
        }

        if (isHold1 && !isHold2)
        {
            if (Vector3.Distance(UAV1.transform.position, gameObject.transform.position) > 2.5f) {
                isHold1 = false;
                UAV1.GetComponent<UAVAgent>().isHold = false;
                UAV1.GetComponent<UAVAgent>().boxType = 0;

                UAV1.GetComponent<UAVAgent>().GiveReward(-0.5f);
            } 
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isHold1)
        {
            if (!isHold1)
            {
                if (other.gameObject.CompareTag("uav"))
                {
                    UAV1 = GameObject.Find(other.gameObject.name);
                    if (!UAV1.GetComponent<UAVAgent>().isHold)
                    {
                        isHold1 = true;
                        UAV1.GetComponent<UAVAgent>().boxPos = gameObject.transform.position;
                        UAV1.GetComponent<UAVAgent>().isHold = true;
                        UAV1.GetComponent<UAVAgent>().boxType = 2;
                        UAV1.GetComponent<UAVAgent>().destinationPos = destPos;
                        
                        UAV1.GetComponent<UAVAgent>().GiveReward(1.0f);
                    }
                }
            }
            if (isHold1 == true && !isHold2)
            {
                if (other.gameObject.CompareTag("uav"))
                {
                    UAV2 = GameObject.Find(other.gameObject.name);
                    if (!UAV2.GetComponent<UAVAgent>().isHold)
                    {
                        isHold2 = true;
                        UAV2.GetComponent<UAVAgent>().boxPos = gameObject.transform.position;
                        UAV2.GetComponent<UAVAgent>().isHold = true;
                        UAV2.GetComponent<UAVAgent>().boxType = 2;
                        UAV2.GetComponent<UAVAgent>().destinationPos = destPos;
                        
                        UAV1.GetComponent<UAVAgent>().GiveReward(1.0f);
                        UAV2.GetComponent<UAVAgent>().GiveReward(2.0f);
                    }
                }
            }

            if (isHold1 && isHold2)
            {
                if (other.gameObject.CompareTag("destination"))
                {
                    if (destPos == other.transform.position) {
                        UAV1.GetComponent<UAVAgent>().isHold = false;
                        UAV2.GetComponent<UAVAgent>().isHold = false;
                        UAV1.GetComponent<UAVAgent>().boxType = 0;
                        UAV2.GetComponent<UAVAgent>().boxType = 0;
                        isHold1 = false;
                        isHold2 = false;

                        UAV1.GetComponent<UAVAgent>().GiveReward(8.0f);
                        UAV2.GetComponent<UAVAgent>().GiveReward(8.0f);

                        Destroy(this);
                        Destroy(GameObject.Find(other.gameObject.name));
                    }
                }
            }
        }
    }
}
