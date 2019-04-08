using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pix2Pix;
using Klak.Spout;

public class Pix2Pix2Spout : MonoBehaviour
{
    [SerializeField]
    Pix2PixController pix2pixController;
    [SerializeField]
    SpoutSender spoutSender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spoutSender.sourceTexture = pix2pixController._resultTexture;
    }
}
