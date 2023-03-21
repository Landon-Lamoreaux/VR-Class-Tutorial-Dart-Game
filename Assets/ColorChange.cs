using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.white };
    private int i = 0;
    public void OnColorChange()
    {
        Renderer r = this.GetComponent<Renderer>();
        Material m = r.material;
        m.color = colors[i];
        i = (i + 1) % colors.Length;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
