using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Player Settings")]
    [SerializeField] private Transform instanceTransform;
    public float moveSpeed = 5;
    public GameObject bulletPrefab;

    [SerializeField] private float startingHealth;
    public float currentHealth {get; private set;}
    [SerializeField] private Animator animator;

    [Header ("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header ("UI")]
    public GameObject pauseMenu;
    public GameObject gameoverMenu;
    public ScorePoints finalscorePoints;

    [Header ("DOTween UI")]
    [SerializeField] RectTransform pausePanelRect;
    [SerializeField] float topPosY, midPosY;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup pauseCanvasGroup; //Dark Panel canvas

    [SerializeField] float fadeTime = 1f;
    [SerializeField] RectTransform gameoverPanelRect;
    [SerializeField] CanvasGroup gameoverCanvasGroup; //Dark Panel canvas

    [Header ("Tutorial UI")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private CanvasGroup tutorialCanvasGroup;
    [SerializeField] private RectTransform tutorialPanelRect;

    private bool isTutorialActive;
    public static bool skipTutorial = false;

    void Start()
    {
        if (skipTutorial)
        {
        Time.timeScale = 1f;
        skipTutorial = false; // reset for next fresh launch
        }
        else
        { ShowTutorial(); } // Start with tutorial instead of gameplay
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;
        // Block gameplay input while paused or tutorial is active
        if (Time.timeScale == 0f) return;

        Movement();
        Shoot();

        if (Input.GetKeyDown(KeyCode.Escape)){ PauseMenu(); }
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Movement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float verticalMovement = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Translate(new Vector2(horizontalMovement, verticalMovement));
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab, instanceTransform.position, instanceTransform.rotation);
            animator.SetBool("isShooting", true);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy"){
            TakeDamage(1);
            StartCoroutine(Invulnerability());
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int takeDamage)
    {
        if (GameManager.Instance.isGameOver) return;

        currentHealth -= takeDamage;

        if (currentHealth <= 0) // Player Lose Life Frames
        {
            GameManager.Instance.GameOver();

            StopAllCoroutines();
            GetComponent<Collider2D>().enabled = false;

            Time.timeScale = 0f;
            GameOverMenu();

            enabled = false;
        }
    }

    //iFrames
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(3,6, true);

        //Invul duration
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(3,6, false);
    }

    // Tutorial Panel
    void ShowTutorial()
    {
        isTutorialActive = true;

        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        tutorialPanelRect.anchoredPosition = new Vector2(0f, 1500f);
        tutorialCanvasGroup.alpha = 0;

        tutorialCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true);

        tutorialPanelRect.DOAnchorPosY(midPosY, tweenDuration).SetEase(Ease.InOutBack).SetUpdate(true);

    }

    public async void CloseTutorial()
    {
        tutorialCanvasGroup.DOFade(0, tweenDuration).SetUpdate(true);

        await tutorialPanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();

        tutorialPanel.SetActive(false);
        isTutorialActive = false;

        Time.timeScale = 1f;
    }

    //Pause Panel
    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        PausePanelIntro();
    }

    public async void ResumeGame()
    {
        await PausePanelOutro();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    //PausePanel Animations
    void PausePanelIntro()
    {
        pausePanelRect.anchoredPosition = new Vector2(0f, 1500f);
        pauseCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        pausePanelRect.DOAnchorPosY(midPosY, tweenDuration).SetUpdate(true).SetEase(Ease.InOutBack);
    }

    async Task PausePanelOutro()
    {
        pauseCanvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await pausePanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    //Game Over Panel FadeIn Animation
    public void GameOverMenu()
    {
        gameoverMenu.SetActive(true);
        Canvas.ForceUpdateCanvases();

        finalscorePoints.UpdateFinalScore();
        
        gameoverPanelRect.DOKill();
        gameoverCanvasGroup.DOKill();

        gameoverCanvasGroup.alpha = 0f;
        gameoverPanelRect.anchoredPosition = new Vector2(0f, -1568f);

        gameoverCanvasGroup.DOFade(1, fadeTime).SetUpdate(true);
        gameoverPanelRect.DOAnchorPos(Vector2.zero, fadeTime).SetUpdate(true).SetEase(Ease.InOutQuint);
    }

    public void RetryScene()
    {
        Time.timeScale = 1f;
        skipTutorial = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    
    public void BackGame(){ SceneManager.LoadScene(0); }
}