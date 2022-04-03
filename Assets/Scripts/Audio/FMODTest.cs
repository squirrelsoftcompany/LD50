using FMODUnity;
using UnityEngine;

public class FMODTest : MonoBehaviour
{
    public int value = 0;
    private FMODUnity.StudioEventEmitter emitter;


    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        emitter.SetParameter("Fire", value);
    }
}
