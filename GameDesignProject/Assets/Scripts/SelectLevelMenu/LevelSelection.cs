using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public bool isUnlocked = false;
    public GameObject lockGo;
    public GameObject unlockGo;

    public int levelIndex;
    public int minimumStars;
    public Text lockLevelName;
    public Text unlockLevelName;
    public Text score;
    //public Text stars;
    public Image stars;
    private int currBestScore;
    private int currStarRating;

    public int CurrBestScore { get => currBestScore; set => currBestScore = value; }
    public int CurrStarRating { get => currStarRating; set => currStarRating = value; }

    //public int startLevel;
    //public int endLevel;

    private void UpdateLevelStatus()
    {
        if (isUnlocked)
        {
            unlockGo.gameObject.SetActive(true);
            lockGo.gameObject.SetActive(false);
        }
        else
        {
            unlockGo.gameObject.SetActive(false);
            lockGo.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        UpdateLevelStatus();
        UnlockLevel();
        UpdateScore();
        UpdateStars();
    }

    private void UpdateScore()
    {
        if (isUnlocked)
        {
            if (currBestScore != 0)
            {
                score.text = "Best Score:   " + currBestScore.ToString();
            }
        }
    }

    private void UpdateStars()
    {
        if (isUnlocked)
        {
            stars.sprite = Resources.LoadAll<Sprite>($"Sprites/star_rating")[currStarRating];
        }
    }

    private void UnlockLevel()
    {

        //if (LevelManager.instance.stars >= minimumStars)
        if(FindObjectOfType<LevelManager>().stars >= minimumStars)
        {
            isUnlocked = true;
        }
        else
        {
            isUnlocked = false;
        }
    }

    
}
