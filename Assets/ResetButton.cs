using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResetButton : MonoBehaviour
{
    public Transform[] savedTransforms;

    private Button _button;
    private Vector3[] _savedPositions;

#if DEBUG
    void Start()
    {
        _savedPositions = new Vector3[savedTransforms.Length];

        for(int i = 0; i < _savedPositions.Length;i++)
        {
            _savedPositions[i] = savedTransforms[i].position;
        }

        _button = GetComponent<Button>();
        if(!_button)
        {
            Debug.LogError("There's no button component in it!");
            return;
        }

        _button.onClick.AddListener(() => { OnButtonClicked(); });
    }
    public void OnButtonClicked()
    {
        for (int i = 0; i < _savedPositions.Length; i++)
        {
            savedTransforms[i].position = _savedPositions[i];
        }
    }
#else
    void Start()
    {
        gameObject.SetActive(false);
    }
#endif
}
