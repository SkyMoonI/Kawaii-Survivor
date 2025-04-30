using System;
using Tabsil.Sijil;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour, IWantToBeSaved
{
    [Header("SFX")]
    [SerializeField] private Button m_sfxButton;
    [SerializeField] private Color m_sfxOnColor;
    [SerializeField] private Color m_sfxOffColor;
    [SerializeField] private TMP_Text m_sfxText;
    private bool m_sfxState = true;
    private const string SFX_STATE_KEY = "SFX_STATE_KEY";

    [Header("Music")]
    [SerializeField] private Button m_musicButton;
    [SerializeField] private Color m_musicOnColor;
    [SerializeField] private Color m_musicOffColor;
    [SerializeField] private TMP_Text m_musicText;
    private bool m_musicState = true;
    private const string MUSIC_STATE_KEY = "MUSIC_STATE_KEY";

    [Header("Privacy Policy")]
    [SerializeField] private Button m_privacyPolicyButton;

    [Header("Help")]
    [SerializeField] private Button m_helpButton;

    [Header("Credits")]
    [SerializeField] private Button m_openCreditsButton;
    [SerializeField] private Button m_closeCreditsButton;
    [SerializeField] private GameObject m_creditsPanel;

    [Header("Actions")]
    public static Action<bool> OnSFXStateChanged;
    public static Action<bool> OnMusicStateChanged;


    void Start()
    {
        UpdateSFXVisuals();
        UpdateMusicVisuals();
    }

    void OnEnable()
    {
        m_sfxButton.onClick.RemoveAllListeners();
        m_sfxButton.onClick.AddListener(SfxButtonCallBack);

        m_musicButton.onClick.RemoveAllListeners();
        m_musicButton.onClick.AddListener(MusicButtonCallBack);

        m_privacyPolicyButton.onClick.RemoveAllListeners();
        m_privacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonCallBack);

        m_helpButton.onClick.RemoveAllListeners();
        m_helpButton.onClick.AddListener(HelpButtonCallBack);

        m_openCreditsButton.onClick.RemoveAllListeners();
        m_openCreditsButton.onClick.AddListener(ShowCreditsPanel);

        m_closeCreditsButton.onClick.RemoveAllListeners();
        m_closeCreditsButton.onClick.AddListener(HideCreditsPanel);
    }

    void OnDisable()
    {
        m_sfxButton.onClick.RemoveAllListeners();
        m_musicButton.onClick.RemoveAllListeners();
        m_privacyPolicyButton.onClick.RemoveAllListeners();
        m_helpButton.onClick.RemoveAllListeners();
        m_openCreditsButton.onClick.RemoveAllListeners();
        m_closeCreditsButton.onClick.RemoveAllListeners();
    }

    void OnDestroy()
    {
        m_sfxButton.onClick.RemoveAllListeners();
        m_musicButton.onClick.RemoveAllListeners();
        m_privacyPolicyButton.onClick.RemoveAllListeners();
        m_helpButton.onClick.RemoveAllListeners();
        m_openCreditsButton.onClick.RemoveAllListeners();
        m_closeCreditsButton.onClick.RemoveAllListeners();
    }

    private void SfxButtonCallBack()
    {
        m_sfxState = !m_sfxState;
        UpdateSFXVisuals();
        Save();

        OnSFXStateChanged?.Invoke(m_sfxState);
    }

    private void UpdateSFXVisuals()
    {
        if (m_sfxState)
        {
            m_sfxText.text = "ON";
            m_sfxButton.GetComponent<Image>().color = m_sfxOnColor;

        }
        else
        {
            m_sfxText.text = "OFF";
            m_sfxButton.GetComponent<Image>().color = m_sfxOffColor;
        }
    }

    private void MusicButtonCallBack()
    {
        m_musicState = !m_musicState;
        UpdateMusicVisuals();
        Save();

        OnMusicStateChanged?.Invoke(m_musicState);
    }

    private void UpdateMusicVisuals()
    {
        if (m_musicState)
        {
            m_musicText.text = "ON";
            m_musicButton.GetComponent<Image>().color = m_musicOnColor;

        }
        else
        {
            m_musicText.text = "OFF";
            m_musicButton.GetComponent<Image>().color = m_musicOffColor;
        }
    }

    private void PrivacyPolicyButtonCallBack()
    {
        Application.OpenURL("https://skymooni.github.io");
    }

    private void HelpButtonCallBack()
    {
        string email = "gokay.iseri@outlook.com";
        string subject = MyEscapeURL("Help");
        string body = MyEscapeURL("Hey! I need help with this...");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private string MyEscapeURL(string s)
    {
        return UnityWebRequest.EscapeURL(s).Replace("+", "%20");
    }

    private void ShowCreditsPanel()
    {
        m_creditsPanel.SetActive(true);
    }

    private void HideCreditsPanel()
    {
        m_creditsPanel.SetActive(false);
    }

    public void Load()
    {
        Sijil.TryLoad(this, SFX_STATE_KEY, out object sfxState);
        Sijil.TryLoad(this, MUSIC_STATE_KEY, out object musicState);

        if (sfxState != null)
        {
            m_sfxState = (bool)sfxState;
        }

        if (musicState != null)
        {
            m_musicState = (bool)musicState;
        }
    }

    public void Save()
    {
        Sijil.Save(this, SFX_STATE_KEY, m_sfxState);
        Sijil.Save(this, MUSIC_STATE_KEY, m_musicState);
    }
}
