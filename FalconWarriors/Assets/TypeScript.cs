using UnityEngine;
using System.Collections;

public class TypeScript : MonoBehaviour {

    private bool isActive = false;

    public string orig_text;

    private TextMesh textMesh;

	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = orig_text;
    }

    private bool letterPressed(char c)
    {
        if (c >= 'A' && c <= 'Z') return Input.GetKeyDown(KeyCode.A + c - 'A');
        if (c >= 'a' && c <= 'z') return Input.GetKeyDown(KeyCode.A + c - 'a');
        return false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isActive)
        {
            setActive(true);
        }

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
            textMesh.color = Color.white;
        }
        textMesh.text = orig_text;
    }
}
