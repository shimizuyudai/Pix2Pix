using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;
using Pix2Pix;

public class TextureHolder2Pix2Pix : MonoBehaviour
{
    [SerializeField]
    TextureHolderBase textureHolder;
    [SerializeField]
    Pix2PixController pix2pixController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Graphics.Blit(textureHolder.GetTexture(), pix2pixController._sourceTexture);
    }
}
