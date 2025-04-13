using TMPro;
using UnityEngine;

public class WaveUIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TMP_Text m_waveText;
    [SerializeField] private TMP_Text m_timerText;

    public void UpdateWaveText(string waveText) => m_waveText.text = waveText;
    public void UpdateTimerText(string timerText) => m_timerText.text = timerText;
}
