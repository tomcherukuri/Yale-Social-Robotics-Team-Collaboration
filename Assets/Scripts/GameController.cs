using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

	public Text countdownTimer;
	public int roundTimeGiven;
	public int roundsGiven;

	private float timeElapsed = 0.0f;
	private float remainingRoundTime = 0.0f;
	private int roundsLeft = 0;
	private int remainingTimeSec = 0;

	public GameObject[] possiblePieces;
	public static int numberOfPiecesInPlay = 3;
	private GameObject[] piecesInPlay = new GameObject[numberOfPiecesInPlay];
	public GameObject pieceMenu;

	public Slot[] slots;
	public int columns; 
	public int rows;
	public GameObject startPiecePrefab;
	public int startPosition;
	private int activeSlotPos;

	void Awake() {
		roundsLeft = roundsGiven;
		remainingRoundTime = roundTimeGiven;
		// subscribe to the event that alerts the game manager of pieces added to the rocket
		Slot.OnPieceAdded += PieceAdded;
//		putPiecesIntoPlay ();
		for (int i = 0; i < numberOfPiecesInPlay; i++) {
			GameObject piece = (GameObject)GameObject.Instantiate (possiblePieces[(int)UnityEngine.Random.Range(0.0f,5.9f)]);
			piece.transform.SetParent(pieceMenu.transform, false);				
			piecesInPlay [i] = piece;
		}
		for (int i = 0; i < slots.Length; i++) {
			slots[i].setActive(false);
		}
		GameObject startPiece = (GameObject)GameObject.Instantiate (startPiecePrefab);
		startPiece.transform.SetParent (slots [startPosition].transform, false);
		slots [startPosition].setOutflow ("right");
		activeSlotPos = startPosition+1;
		slots [activeSlotPos].setActive (true);
		slots [activeSlotPos].setInflow ("left");
	}

	// Use this for initialization
	void Start () {
		SetCountdownTimerText(roundTimeGiven);
	}

	// Update is called once per frame
	void Update () {
		remainingRoundTime = roundTimeGiven - timeElapsed;
		if (roundsLeft != 0) {
			if (remainingRoundTime > 0.0) { //There still is time in the round
				if (GetSeconds (remainingRoundTime) != remainingTimeSec) {
					// get remaining seconds
					remainingTimeSec = GetSeconds (remainingRoundTime);
					// set the timer text
					SetCountdownTimerText (remainingTimeSec);
				}
			} else { // The end of the round
				roundsLeft--;
				remainingTimeSec = roundTimeGiven;
				SetCountdownTimerText (remainingTimeSec);
				timeElapsed = 0.0f;
//				clearPieceMenu ();
//				putPiecesIntoPlay ();

			}
		} else { // End the game
			EndGame();
		}
		// increment the time elapsed
		timeElapsed += Time.deltaTime;
	}
//	void clearPieceMenu() {
//		for (int i = 0; i < pieceMenu.transform.childCount; i++) {
//			Destroy (pieceMenu.transform.GetChild(i));
//		}
//	}
//	void putPiecesIntoPlay() {
//		for (int i = 0; i < numberOfPiecesInPlay; i++) {
//			GameObject piece = (GameObject)GameObject.Instantiate (possiblePieces[(int)UnityEngine.Random.Range(0.0f,5.9f)]);
//			piece.transform.SetParent(pieceMenu.transform, false);
//			piecesInPlay [i] = piece;
//		}
//	}
	void PieceAdded(GameObject pieceAdded) {
		slots [activeSlotPos].setActive(false);
		string inFlow = "";
		if (slots [activeSlotPos].getOutflow () == "top") {
			activeSlotPos = activeSlotPos - columns;
			slots [activeSlotPos].setActive (true);
			inFlow = "bottom";
		} else if (slots [activeSlotPos].getOutflow () == "right") {
			activeSlotPos = activeSlotPos + 1;
			slots [activeSlotPos].setActive (true);
			inFlow = "left";
		} else if (slots [activeSlotPos].getOutflow () == "bottom") {
			activeSlotPos = activeSlotPos + columns;
			slots [activeSlotPos].setActive (true);
			inFlow = "top";
		} else if (slots [activeSlotPos].getOutflow () == "left") {
			activeSlotPos = activeSlotPos - 1;
			slots [activeSlotPos].setActive(true);
			inFlow = "right";
		}
		slots [activeSlotPos].setInflow (inFlow);
		Debug.Log (inFlow);
	}

	void EndGame() {
		
	}

	void SetCountdownTimerText(int timerSec)
	{
		string timerText = FormatTime2 (timerSec);
		countdownTimer.text = timerText;
	}

	int GetSeconds(float value) {
		TimeSpan t = TimeSpan.FromSeconds (value);
		return (t.Minutes * 60 + t.Seconds);
	}

	string FormatTime(float value) {
		TimeSpan t = TimeSpan.FromSeconds (value);
		return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
	}

	string FormatTime2(int value) 
	{
		int min, sec;
		min = value / 60;
		sec = value % 60;
		return string.Format("{0:D2}:{1:D2}", min, sec);
	}
}


