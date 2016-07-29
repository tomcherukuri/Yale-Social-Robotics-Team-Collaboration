using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {
	private bool isActive;
	private string flowInDirection;
	private string flowOutDirection;

	public delegate void PieceAdded(GameObject pieceAdded);
	public static event PieceAdded OnPieceAdded;

	private PieceInfo pieceInfo;
	private GameObject pieceBeingDragged;

	public GameObject item {
		get {
			if (transform.childCount > 0) {
				return transform.GetChild (0).gameObject;
			}
			return null;
		}
	}

	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData) {
		pieceBeingDragged = DragHandler.itemBeingDragged;
		pieceInfo = pieceBeingDragged.GetComponent<PieceInfo> ();
		if (!item && getActive() && validPiece()) {
			DragHandler.itemBeingDragged.transform.SetParent (transform);
			determineOutflow ();
		}
		// If a piece has been dragged successfully, we'll let the game manager know
		OnPieceAdded (DragHandler.itemBeingDragged);
	}
	#endregion
	 
	private void determineOutflow() {
		if (flowInDirection == "top") {
			if (pieceInfo.right)
				flowOutDirection = "right";
			if (pieceInfo.bottom)
				flowOutDirection = "bottom";
			if (pieceInfo.left)
				flowOutDirection = "left";
		} else if (flowInDirection == "right") {
			if (pieceInfo.bottom)
				flowOutDirection = "bottom";
			if (pieceInfo.left)
				flowOutDirection = "left";
			if (pieceInfo.top)
				flowOutDirection = "top";
		} else if (flowInDirection == "bottom") {
			if (pieceInfo.left)
				flowOutDirection = "left";
			if (pieceInfo.top)
				flowOutDirection = "top";
			if (pieceInfo.right)
				flowOutDirection = "right";
		} else if (flowInDirection == "left") {
			if (pieceInfo.top)
				flowOutDirection = "top";
			if (pieceInfo.right)
				flowOutDirection = "right";
			if (pieceInfo.bottom)
				flowOutDirection = "bottom";
		}
	}

	private bool validPiece() { 
		if (flowInDirection == "top")
			return pieceInfo.top;
		else if (flowInDirection == "right")
			return pieceInfo.right;
		else if (flowInDirection == "bottom")
			return pieceInfo.bottom;
		else if (flowInDirection == "left")
			return pieceInfo.left;
		return false;
	}

	public void setActive(bool state) {
		isActive = state;
	}
	public bool getActive() {
		return isActive;
	}

	public void setOutlow(string direction) {
		flowInDirection = direction;
	}
	public string getOutlow() {
		return flowInDirection;
	}

	public void setOutflow(string direction) {
		flowOutDirection = direction;
	}
	public string getOutflow() {
		return flowOutDirection;
	}


}
