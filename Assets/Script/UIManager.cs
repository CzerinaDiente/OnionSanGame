using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Icon Hover")]
    public RectTransform titleSpaceship;
    public float hoverHeight = 20f;
    public float hoverDuration = 2f;

    private Tween hoverTween;

    void Start()
    {
        Screen.SetResolution(540, 960, false);
        StartHover();
    }

    public void ExitGame(){ Application.Quit(); }

    public void LoadScene(string sceneName){ SceneManager.LoadScene(sceneName); }

    void StartHover()
    {
        if (titleSpaceship == null) return;

        titleSpaceship.DOKill();

        hoverTween = titleSpaceship.DOAnchorPosY(hoverHeight, hoverDuration).SetRelative(true).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    void OnDisable()
    {
        titleSpaceship?.DOKill();
    }
}
