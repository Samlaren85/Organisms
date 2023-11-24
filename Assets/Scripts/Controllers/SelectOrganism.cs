using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOrganism : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI OrganismInfo;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left) 
            {
                OrganismInfo.text = name;
            }
            if (eventData.button != PointerEventData.InputButton.Right)
            {
                Debug.Log("Clicked right mouse!");
            }
        }
    }
}
