using UnityEngine;
using System.Collections;

public class warfogtest : MonoBehaviour {

    [SerializeField]
    private RenderTexture mask;
    [SerializeField]
    private Material mat;
    //用来渲染图片的后期效果
    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        mat.SetTexture("_MaskTex", mask);
        Graphics.Blit(source, destination, mat);
    }
}
