using UnityEngine;
using System.Collections;
 
public class Spectator : MonoBehaviour {
 
    public int speed =20;
    
    void Start () {
    
    }
    
    void Update () {
    
        // Speed
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = 40; 
        }
        else
        {
            speed = 20; 
        }

        // Move
        if(Input.GetKey(KeyCode.Z))
        {
            transform.position = transform.position + Camera.main.transform.forward * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + Camera.main.transform.forward * -1 * speed * Time.deltaTime;
        
        }
        if(Input.GetKey(KeyCode.Q))
        {
            transform.position = transform.position + Camera.main.transform.right * -1 * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + Camera.main.transform.right * speed * Time.deltaTime;
        }

        // Elevation
        if(Input.GetKey(KeyCode.E))
        {
            transform.position = transform.position + Camera.main.transform.up * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + Camera.main.transform.up * -1 * speed * Time.deltaTime;
        }
    }
}
