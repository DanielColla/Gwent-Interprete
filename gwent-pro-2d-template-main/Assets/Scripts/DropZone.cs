using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public Draggable.Mycardenum tipozona = Draggable.Mycardenum.Handcard;
	public void OnPointerEnter(PointerEventData eventData) {
		Debug.Log("OnPointerEnter");
	}
	
	public void OnPointerExit(PointerEventData eventData) {
		Debug.Log("OnPointerExit");
	}
	
	public void OnDrop(PointerEventData eventData) {
		Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);
       
		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null) {
			if(tipozona == d.tipozona || tipozona == Draggable.Mycardenum.Handcard){
				d.parentToReturnTo = this.transform;
			}
		}
	}
}
