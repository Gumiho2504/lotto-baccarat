using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

enum Result
{
   NONE,BANKER,PLAYER,TIE
}
public class LottoBaccaratGameController : MonoBehaviour
{
    [Header("BallSprite")]
    [SerializeField] List<Sprite> ballSpriteList = new List<Sprite>();
    [SerializeField] private GameObject ballPrefab;

    [Header("Cards")]
    public List<Card> allCards = new List<Card>();
    private List<Card> bankerCards = new List<Card>();
    private List<Card> playerCards = new List<Card>();

    [SerializeField] private GameObject cardPrefab;

    [Header("Position")]

    public GameObject pos;
    public GameObject firstPos;
    public GameObject ballSpawnPos;
    public GameObject[] playerCardsPos;
    public GameObject[] bankerCardsPos;

    public GameObject playerParent;
    public GameObject bankerParent;
    public GameObject classParent;
    public GameObject bar;
    public GameObject playerButton, bankerButton, tieButton;


    [Header("CARD TYPE SPRITE")]

    [SerializeField] private Sprite heardSprite;
    [SerializeField] private Sprite spadesSprte, clubSprite, diamondSprite;
   


    [Header("Text")]

    public Text playerCardValueText;
    public Text bankerCardValueText;
    public Text timeText;

    [Header("Coin - Text")]

    [SerializeField] private Text totalCoinText;
    [SerializeField] private Text playerHolderCoinText, bankerHolderCoinText, tieHolderCoinText;

    [Header("IMAGE")]
    public Image ballShow;

    public float duration = 1f;
    int playerCardValueSum = 0;
    int bankCardValueSum = 0;

    int totalCoin = 10000000;
    int playerHolderCoin  = 0;
    int bankerHolderCoin = 0;
    int tieHolderCoin = 0;
    int betCoin = 1000;

    Result gameResult;
    Result clickPlayer, clickBanker, clickTie;


    public float spawnInterval = 0.5f;
    public float animationDuration = 0.5f;
    public Vector3 targetScale = new Vector3(1, 1, 1);
    public float rotationDuration = 1f;
    private List<GameObject> spawnedBalls = new List<GameObject>();


    private List<GameObject> playerCardPreGameObject = new List<GameObject>();
    private List<GameObject> bankerCardPreGameObject = new List<GameObject>();

    private void Awake()
    {
        totalCoinText.text = totalCoin.ToString();
        ConvertToM_K(totalCoin, totalCoinText);
        StartCoroutine(SpawnBallsWithAnimation());
       
    }

    private IEnumerator<WaitForSeconds> SpawnBallsWithAnimation()
    {
        foreach (Sprite sprite in ballSpriteList)
        {
            GameObject ball = Instantiate(ballPrefab, ballSpawnPos.transform.localPosition, Quaternion.identity);
 
            ball.transform.SetParent(classParent.transform, false);

           
            ball.GetComponent<Image>().sprite = sprite;

            
            ball.transform.localScale = Vector3.zero;
            spawnedBalls.Add(ball);
            LeanTween.scale(ball, targetScale, animationDuration).setEase(LeanTweenType.easeOutBounce);
            LeanTween.alpha(ball.GetComponent<RectTransform>(), 1f, animationDuration).setFrom(0f);
            yield return new WaitForSeconds(spawnInterval);
        }
       
    }

    public void ShakeAllBalls()
    {
        foreach (GameObject ball in spawnedBalls)
        { 
            LeanTween.moveLocalX(ball, ball.transform.localPosition.x + Random.Range(-10f, 10f), 0.1f).setEase(LeanTweenType.easeShake).setLoopPingPong(20);
            LeanTween.moveLocalY(ball, ball.transform.localPosition.y + Random.Range(-10f, 10f), 0.1f).setEase(LeanTweenType.easeShake).setLoopPingPong(20);
        }
    }
    public void RotateBar()
    {
        LeanTween.rotateAround(bar, Vector3.forward, 360, rotationDuration).setEase(LeanTweenType.easeInOutQuad).setLoopClamp();
    }
    public void AllowRotateBar()
    {

        RotateBar();
        AudioController.Instance.PlaySFX("ball");
        Invoke(nameof(StopRotateBar), 6f);
    }

    private void StopRotateBar()
    {
        
            LeanTween.cancel(bar);
    }


    private float timeNumber;
    private float time = 10;
    private bool timeAchive = true, isDeal = false, isStart = true;
    public Slider timeSlider;

    // Start
    //-----------------------------------------------------------------------------------------------
    private void Start()
    {
        timeNumber = time;


        gameResult = clickBanker = clickPlayer = clickTie = Result.NONE;

        //  print(CovertValue(19));
        //  DealCards();

    }




    // update
    //-----------------------------------------------------------------------------------------------
   
    private void Update()
    {
        if (timeAchive)
        {
            timeNumber -= Time.deltaTime;
            timeText.text = Mathf.Round(timeNumber).ToString();
            //timeSlider.value = timeNumber;
            //if (timeCount1)
            //{
            //    StartCoroutine(CountSound());
            //    timeCount1 = false;
            //}
        }
        if (timeNumber < 0 && timeAchive)
        {
            isDeal = false;
            timeAchive = false;
            isGuess = false;
            DealCards();
            //RandomCard();
            //StartCoroutine(ResetAndRestartGame());
        }
        if (timeNumber <= time && isStart)
        {
            //isStart = false;
            //StartCoroutine(StartGame());
        }
    }



    private List<int> numberList = new List<int>();

    private void DealCards()
    {
        for(int i = 0; i< allCards.Count; i++)
        {
            numberList.Add(i);
        }

        Shuffle(numberList);

        StartCoroutine(DealCardAnimation());
      

    }


    bool isPlayerDraw = false;
    bool isBankerDraw = false;
    IEnumerator DealCardAnimation()
    {
        int sumOfPlayer2Cards = 0;
        int sumOfPlayer3Cards = 0;
        int j = 0;
        int k = 0;

        for (int i = 0; i < 4; i++)
        {
            //int randNumber = Random.Range(0, numberList.Count);

            //print($"ramddom from 0 - {numberList.Count}");
            //print("random - " + randNumber);
            //print($"card add cart[numberList[{randNumber}]= {numberList[randNumber]}] = [{allCards[numberList[randNumber]].name}] - [{allCards[numberList[randNumber]].value}]");
            //print("\n#######################################");

            AllowRotateBar();

            yield return new WaitForSeconds(6f);
           
            
           

            yield return BallShowAnimation(ballSpriteList[numberList[i]]);
          
            if (i % 2 == 0)
            {

                playerCards.Add(allCards[numberList[i]]);
                playerCardValueSum += playerCards[j].value;
                playerCardValueText.text = playerCardValueSum.ToString();

                SpawnAndMovePrefab(playerCards[j].sprite, firstPos, playerCardsPos[j], playerParent);

                j++;

            }
            else
            {
                bankerCards.Add(allCards[numberList[i]]);
                bankCardValueSum += bankerCards[k].value;
                bankerCardValueText.text = bankCardValueSum.ToString();
                SpawnAndMovePrefab(bankerCards[k].sprite, firstPos, bankerCardsPos[k], bankerParent);
                k++;
            }
            yield return new WaitForSeconds(0.5f);
           // ballShow.gameObject.SetActive(false);
        }


        playerCardValueSum = CovertValue(playerCardValueSum);
        sumOfPlayer2Cards = playerCardValueSum;

        bankCardValueSum = CovertValue(bankCardValueSum);

        playerCardValueText.text = playerCardValueSum.ToString();
        bankerCardValueText.text = bankCardValueSum.ToString();

       


        // Player's rules
        //-----------------------------------------------------------------------------------------------

        if (sumOfPlayer2Cards >= 8 || bankCardValueSum >= 8) // Natural hand
        {
            isPlayerDraw = false;
            isBankerDraw = false;
        }
        else if (sumOfPlayer2Cards <= 5) // Player draws a third card
        {
            isPlayerDraw = true;
        }
        else // Player stands
        {
            isPlayerDraw = false;
        }

        if (isPlayerDraw)
        {



            AllowRotateBar();
            yield return new WaitForSeconds(6f);




            yield return BallShowAnimation(ballSpriteList[numberList[4]]);

            playerCards.Add(allCards[numberList[4]]);
            
            playerCardValueSum += playerCards[2].value;
            playerCardValueText.text = playerCardValueSum.ToString();
            sumOfPlayer3Cards = playerCardValueSum;




            SpawnAndMovePrefab(playerCards[2].sprite, firstPos, playerCardsPos[2], playerParent);

            yield return new WaitForSeconds(0.5f);
        }



        // Banker's rules
        //-----------------------------------------------------------------------------------------------

        if (!isPlayerDraw) // Player stands
        {
            if (sumOfPlayer2Cards >= 8)
            {
                isBankerDraw = false;
            }
            else if (sumOfPlayer2Cards <= 5)
            {
                isBankerDraw = true;
            }
            else
            {
                isBankerDraw = sumOfPlayer2Cards <= 2;
            }
        }
        else if(isPlayerDraw && bankCardValueSum < 7) // Player draws a third card
        {



            int playerThirdCard = sumOfPlayer3Cards;

            if (sumOfPlayer2Cards <= 2)
            {
                isBankerDraw = true;
            }
            else if (sumOfPlayer2Cards == 3 && playerThirdCard != 8)
            {
                isBankerDraw = true;
            }
            else if (sumOfPlayer2Cards == 4 && playerThirdCard >= 2 && playerThirdCard <= 7)
            {
                isBankerDraw = true;
            }
            else if (sumOfPlayer2Cards == 5 && playerThirdCard >= 4 && playerThirdCard <= 7)
            {
                isBankerDraw = true;
            }
            else if (sumOfPlayer2Cards == 6 && playerThirdCard >= 6 && playerThirdCard <= 7)
            {
                isBankerDraw = true;
            }
            else
            {
                isBankerDraw = false;
            }
        }

        Debug.Log($"Player Draw: {isPlayerDraw}, Banker Draw: {isBankerDraw}");


        if (isBankerDraw)
        {




            AllowRotateBar();
            yield return new WaitForSeconds(6f);



            yield return BallShowAnimation(ballSpriteList[numberList[5]]);

            bankerCards.Add(allCards[numberList[5]]);
            bankCardValueSum += bankerCards[2].value;
            bankerCardValueText.text = bankCardValueSum.ToString();


            SpawnAndMovePrefab(bankerCards[2].sprite, firstPos, bankerCardsPos[2], bankerParent);

            yield return new WaitForSeconds(1f);
        }

        playerCardValueSum = CovertValue(playerCardValueSum);
        playerCardValueText.text = playerCardValueSum.ToString();

        bankCardValueSum = CovertValue(bankCardValueSum);
        bankerCardValueText.text = bankCardValueSum.ToString();

        StartCoroutine(GameResultShow());

    }

    IEnumerator BallShowAnimation(Sprite sprite)
    {
        yield return new WaitForSeconds(0.5f);
        AudioController.Instance.PlaySFX("ball-show");
        LeanTween.scale(ballShow.gameObject, new Vector3(1, 1, 1), 0.5f).setEaseInCirc();
        ballShow.sprite = sprite;
        yield return new WaitForSeconds(1f);
        LeanTween.scale(ballShow.gameObject, new Vector3(0, 0, 0), 0.5f).setEaseInCirc();
       
    }


    IEnumerator GameResultShow()
    {
        if(playerCardValueSum > bankCardValueSum)
        {
            gameResult = Result.PLAYER;
            playerButton.GetComponent<Image>().enabled = true;
        }
        else if(bankCardValueSum > playerCardValueSum)
        {
            gameResult = Result.BANKER;
            bankerButton.GetComponent<Image>().enabled = true;
        }
        else
        {
            gameResult = Result.TIE;
            tieButton.GetComponent<Image>().enabled = true;
        }

        print($"GameResult - {gameResult}");
        yield return new WaitForSeconds(0.5f);

        CaculateGame();

        yield return ResetGame();
    }


    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(5f);

        numberList.Clear();
        playerCards.Clear();
        bankerCards.Clear();

        playerHolderCoin = bankerHolderCoin = tieHolderCoin = playerCardValueSum = bankCardValueSum = 0;
        playerHolderCoinText.text = bankerHolderCoinText.text = tieHolderCoinText.text =playerCardValueText.text = bankerCardValueText.text = "0";

        timeNumber = time;
        timeAchive = true;
        isGuess = true;

        for (int i = 0; i<playerParent.transform.childCount; i++)
        {
            Destroy(playerParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < bankerParent.transform.childCount; i++)
        {
            Destroy(bankerParent.transform.GetChild(i).gameObject);
        }

        playerButton.GetComponent<Image>().enabled = false;
        bankerButton.GetComponent<Image>().enabled = false;
        tieButton.GetComponent<Image>().enabled = false;
    }


    private  int CovertValue(int value)
    {

        if (value > 10)
        {
            return value % 10;
        }else if (value == 10)
        {
            return 0;
        }
        else {
            return value;
        }

        
    }



    Sprite typeOfCardSpriteCompare(string type)
    {
        switch (type)
        {
            case "C":
                return clubSprite;
            case "D":
                return diamondSprite;
            case "H":
                return heardSprite;
           
            default:
                return spadesSprte;

        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public GameObject prefab;  
    public RectTransform canvasTransform; 

    public GameObject startPosition; 
    public GameObject endPosition;  
    
    void SpawnAndMovePrefab(Sprite s,GameObject startPos, GameObject endPos,GameObject parent)
    {
        AudioController.Instance.PlaySFX("card");
        GameObject c = Instantiate(prefab, startPos.transform.position,Quaternion.Euler(0,0,endPos.transform.localRotation.z));
        c.transform.SetParent(parent.transform, false);
        c.GetComponent<Image>().sprite = s;
      

        LeanTween.move(c, endPos.transform, duration).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.rotate(c, endPos.transform.eulerAngles, duration).setEase(LeanTweenType.easeInOutQuad);
    }



    float xOffset = -1000;
    float yOffset = -450;
    private void SpawnCards(Sprite s)
    {

        GameObject c = Instantiate(cardPrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.y), Quaternion.identity);
        c.transform.SetParent(pos.transform, false);
        c.GetComponent<Image>().sprite = s;
        xOffset += 70f;
    }


    void CaculateGame()
    {
        AudioController.Instance.PlaySFX("coin-pick-up");
        if (clickPlayer == gameResult)
        {
            totalCoin += playerHolderCoin * 2;
            
            //winCoin += playerCoin;
        }
        else if (clickBanker == gameResult)
        {
            totalCoin += bankerHolderCoin * 2;
            //winCoin += bankerCoin;
        }
        else if (clickTie == gameResult)
        {
            totalCoin += tieHolderCoin * 9;
            //winCoin += tieCoin * 8;
        }
        else
        {
            Debug.LogWarning("You Lose");
        }
        ConvertToM_K(totalCoin, totalCoinText);
       // ConvertToM_K(winCoin, winCoinText);
    }


    bool isGuess = true;
    public void GuessButton()
    {
        AudioController.Instance.PlaySFX("coin");
       // string tag = "Container";
        if (isGuess && totalCoin >= betCoin)
        {
           
            string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            totalCoin -= betCoin;
            ConvertToM_K(totalCoin, totalCoinText);
            switch (buttonName)
            {
                case "PLAYER":
                    //tag = "Player";
                    clickPlayer = Result.PLAYER;
                    playerHolderCoin += betCoin;
                    ConvertToM_K(playerHolderCoin, playerHolderCoinText);
                  //  LeanTween.scale(playerHolderCoinText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.3f).setEaseInBounce().setLoopPingPong();
                    break;
                case "BANKER":
                    //tag = "banker";
                    clickBanker = Result.BANKER;
                    bankerHolderCoin += betCoin;
                    ConvertToM_K(bankerHolderCoin, bankerHolderCoinText);
                    break;
                case "TIE":
                    //tag = "tie";
                    clickTie = Result.TIE;
                    tieHolderCoin += betCoin;
                   ConvertToM_K(tieHolderCoin, tieHolderCoinText);
                    break;
            }
           // BetMove.instance.Move(coinPreb, tag, winPos);
        }
    }

    public Button curCashBTN;
    public void ChangeGuessCointImageButton(Button btn)
    {
        if (btn.GetInstanceID() != curCashBTN.GetInstanceID())
        {
            GameObject blockPanel = btn.transform.GetChild(0).gameObject;
            blockPanel.SetActive(true);

            blockPanel = curCashBTN.transform.GetChild(0).gameObject;
            blockPanel.SetActive(false);
            curCashBTN = btn;
        }
    }


    public void SwitchBetCoin()
    {
        AudioController.Instance.PlaySFX("tap");
        string btnName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name.ToUpper();
        switch (btnName)
        {
            case "1K":
                //coinPreb = coinPrefabList[0];
                betCoin = 1000;
                break;
            case "5K":
                //coinPreb = coinPrefabList[1];
                betCoin = 5000;
                break;
            case "10K":
                //coinPreb = coinPrefabList[2];
                betCoin = 10000;
                break;
            case "25K":
                //coinPreb = coinPrefabList[3];
                betCoin = 25000;
                break;
            case "50K":
                //coinPreb = coinPrefabList[4];
                betCoin = 50000;
                break;
            case "100K":
                //coinPreb = coinPrefabList[5];
                betCoin = 100000;
                break;
        }
    }





    public static void ConvertToM_K(int coint, Text cointText)
    {
        if (coint < 0)
        {
            coint = -coint;
            if (coint >= 1000000 && coint % 1000000 == 0)
                cointText.text = (coint / 1000000).ToString() + "M";
            else if (coint >= 1000000 && coint % 1000000 != 0)
                cointText.text = (coint / 1000000).ToString() + "M" + " " + ((coint % 1000000) / 1000) + "k";
            else if (coint >= 1000)
                cointText.text = (coint / 1000).ToString() + "K";
            else
                cointText.text = "OK";
        }
        else
        {
            if (coint >= 1000000 && coint % 1000000 == 0)
                cointText.text = (coint / 1000000).ToString() + "M";
            else if (coint >= 1000000 && coint % 1000000 != 0)
                cointText.text = (coint / 1000000).ToString() + "M" + " " + ((coint % 1000000) / 1000) + "k";
            else if (coint >= 1000)
                cointText.text = (coint / 1000).ToString() + "K";
            else
                cointText.text = "OK";
        }

    }

    public void ReloadScene()
    {
        AudioController.Instance.PlaySFX("tap");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);

        
    }


}//end of class
