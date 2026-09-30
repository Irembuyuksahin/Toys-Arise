using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private VoidEvent onWaveEnd;
    [SerializeField] private VoidEvent onWaveStart;
    [SerializeField] private Text timerText;
    [SerializeField] private Text roundText;
    [SerializeField] private GameObject timerImage;
    [SerializeField] private GameObject roomCollider;
    [SerializeField] private GameObject arrowObject;

    public float remainingTime;
    public int turSayisi = 0;

    private float initialTime = 0;
    public bool isPressed;
    private bool isWaveEnd;

    private void Start()
    {
        roundText.text = "Round 1";

        isPressed = false;
        isWaveEnd = false;

        initialTime = remainingTime;
    }

    private void Update()
    {
        if (turSayisi == 0)
        {
            remainingTime = 0;
        }

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (turSayisi != 0)
            {
                roomCollider.SetActive(true);
            }

            arrowObject.SetActive(false);
            isWaveEnd = false;
        }
        else if (remainingTime < 0 && !isWaveEnd)
        {
            isPressed = false;
            remainingTime = 0;
            timerText.color = Color.red;
            roomCollider.SetActive(false);
            arrowObject.SetActive(true);

            onWaveEnd.Raise();
            isWaveEnd = true;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void NewTimer()
    {
        isPressed = true;

        if (turSayisi == 0)
        {
            timerImage.SetActive(true);
        }

        if (initialTime <= 195)
        {
            turSayisi++;
            roundText.text = "Round " + turSayisi;

            initialTime += 15;
            remainingTime = initialTime;

            if (turSayisi == 1)
            {
                remainingTime = 60;
            }

            timerText.color = Color.black;
            onWaveStart.Raise();
        }
    }
}