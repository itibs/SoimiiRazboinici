using UnityEngine;
using System.Collections;


public class TypeScript : MonoBehaviour {

	private bool isActive = false;

    public string orig_text;

	public int count = 0;

	public bool direction = false;

    public TextMesh textMesh;

	public bool flag_zoom = false;
	public bool flag_color = false;
	public bool flag_tran = false;

	public float color_count = 0;

    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        
    }

	// Use this for initialization
	void Start () {
        textMesh.text = orig_text;
    }

    private bool letterPressed(char c)
    {
        if (c >= 'A' && c <= 'Z') return Input.GetKeyDown(KeyCode.A + c - 'A');
        if (c >= 'a' && c <= 'z') return Input.GetKeyDown(KeyCode.A + c - 'a');
        return false;
    }

    public bool isDone()
    {
        return textMesh.text.Length <= 0;
    }

    public void showMirror()
    {
        Vector3 crtScale = textMesh.transform.localScale;
        crtScale.x = -0.1f;
        textMesh.transform.localScale = crtScale;
    }

	public void ZoomInOut()
	{
		if (direction  && count<100)
			count = count + 1;
		if (count == 100)
			direction = false;
		if (!direction && count > 0)
			count = count - 1;
		if (count == 0)
			direction = true;
		textMesh.fontSize = count;
	}

	public void Color_change ()
	{
		//Random random = new Random();

		//if (direction  && color_count<1.0f)
			color_count = color_count + 0.1f;
		
		//if (!direction && color_count > 0.0f)
		//	color_count = color_count - 0.01f;
		

		textMesh.color = new Color (
			Mathf.Cos(color_count),
			Mathf.Sin(color_count),
			Mathf.Sin(color_count));
		if(flag_tran)
			textMesh.color = new Color (
				Mathf.Cos(color_count),
				Mathf.Sin(color_count),
				Mathf.Sin(color_count),
				Mathf.Sin(color_count));


	}




	// Update is called once per frame
	void Update () {
        //if (!isActive)
        //{
        //    setActive(true);
        //}

        if (flag_zoom == true)
            ZoomInOut();
        if (flag_color == true)
			Color_change ();
		
        if (isActive == false)
        {
            return;
        }

        if (textMesh.text.Length <= 0)
        {
            // Destroy object
        } else if (letterPressed(textMesh.text[0]))
        {
            textMesh.text = textMesh.text.Substring(1);
        }
    }

    public void setActive(bool val)
    {
        isActive = val;
        if (isActive)
        {
            textMesh.color = Color.yellow;
        } else
        {
            textMesh.text = orig_text;
            textMesh.color = Color.white;
        }
    }
}
