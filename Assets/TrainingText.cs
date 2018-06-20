using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingText : MonoBehaviour {

    public float speed;
    public Text speedText;

    Rigidbody rBody;

    // Use this for initialization
    void Start ()
    {
        //speedText = GetComponent<Text>();
        rBody = GetComponent<Rigidbody>();
        SetSpeedText();
    }
	
	// Update is called once per frame
	void Update ()
    {
        SetSpeedText();
	}

    void SetSpeedText()
    {

        speedText.text = string.Format("{0:N2}", rBody.velocity.magnitude);
    }
}
