using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Crosstales.NAudio.Wave;
//using Crosstales.Radio;
using NAudio.Wave;

/// <summary>
/// Manages radio functionality, including volume control and channel selection.
/// </summary>
public class Radio : MonoBehaviour
{

    /// <summary>
    /// The UI Slider element for controlling volume.
    /// </summary>
    public Slider volumeSlider;

    /// <summary>
    /// The UI Dropdown element for radio channel selection.
    /// </summary>
    public TMP_Dropdown radioDropdown;

    /// <summary>
    /// Array of Icecast URLs for SomaFM radio channels.
    /// </summary>
    public string[] icecastUrls = {
        "https://online.lyubimyehity.ru:8005//stream",
        "https://stream.overdrivelive.net:8000//alternative_128.mp3",
        "https://nashe1.hostingradio.ru//jazz-128.mp3",
        "https://radio4.vip-radios.fm:18060//stream-128kmp3-SmoothJazzLounge",
        "https://stream.deep1.ru//deep1aac",
        "https://icecast-vgtrk.cdnvideo.ru//mayakfm_mp3_128kbps",
        "https://hfm.amgradio.ru//HypeFM",
        "https://maximum.hostingradio.ru//maximum128.mp3",
        "https://ic1.radiosignal.one//vanya-mp3",
        "https://chanson.hostingradio.ru:8041//chanson128.mp3",
        "https://chanson.hostingradio.ru:8041//chanson-romantic256.mp3",
        "https://rodniki.hostingradio.ru//rodniki128.mp3",
        "https://rusradio.hostingradio.ru//rusradio128.mp3",
        // Add more URLs here
    };

    public string[] stationNames = {
        "Любимые Хиты",
        "Овердрайв",
        "Джаз",
        "Мягкий Джаз",
        "Дип Хаус",
        "Маяк",
        "Хайп ФМ",
        "Радио Максимум",
        "Радио Ваня",
        "Шансон",
        "Романтический Шансон",
        "Радио Родники",
        "Русское Радио"
    };

    public TextMeshProUGUI stationNamesText;

    /// <summary>
    /// The MediaFoundationReader for audio processing.
    /// </summary>
    private MediaFoundationReader mediaFoundationReader;

    /// <summary>
    /// The WaveOutEvent for audio output.
    /// </summary>
    private WaveOutEvent waveOut;

    private int currentRadioIndex = 0;

    void OnEnable()
    {

        if (volumeSlider != null)
        {
            volumeSlider.value = 1f;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }


        if (radioDropdown != null)
        {
            radioDropdown.onValueChanged.AddListener(ChangeRadioStation);
            radioDropdown.value = 0;
        }


        StartCoroutine(PlayRadio(icecastUrls[currentRadioIndex]));
        stationNamesText.text = "Радио: " + stationNames[currentRadioIndex];
    }

    private IEnumerator PlayRadio(string url)
    {
        yield return null;
        try
        {
            mediaFoundationReader = new MediaFoundationReader(url);
            waveOut = new WaveOutEvent();
            waveOut.Init(mediaFoundationReader);
            waveOut.Play();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error playing radio: {ex.Message}");
        }
    }

    void OnDisable()
    {
        if (waveOut != null)
        {
            waveOut.Stop();
            waveOut.Dispose();
        }

        if (mediaFoundationReader != null)
        {
            mediaFoundationReader.Dispose();
        }
    }

    public void StopRadio()
    {
        if (waveOut != null)
        {
            waveOut.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        if (waveOut != null)
        {
            waveOut.Volume = Mathf.Clamp01(volume);
        }
    }

    public float GetVolume()
    {
        return waveOut != null ? waveOut.Volume : 0;
    }

    public void PlayNextStation()
    {
        StopRadio();
        currentRadioIndex++;
        if (currentRadioIndex > icecastUrls.Length - 1) currentRadioIndex = 0;
        StartCoroutine(PlayRadio(icecastUrls[currentRadioIndex]));
        stationNamesText.text = "Радио: " + stationNames[currentRadioIndex];
    }

    public void PlayPreviousStation()
    {
        StopRadio();
        currentRadioIndex--;
        if (currentRadioIndex < 0) currentRadioIndex = icecastUrls.Length - 1;
        StartCoroutine(PlayRadio(icecastUrls[currentRadioIndex]));
        stationNamesText.text = "Радио: " + stationNames[currentRadioIndex];
    }


    // Do this shit later
    public void ChangeRadioStation(int dropdownIndex)
    {
        if (dropdownIndex < 0 || dropdownIndex >= icecastUrls.Length)
        {
            Debug.LogError("Invalid dropdown index");
            return;
        }

        // Stop the current radio station and then play the new one after a delay
        StartCoroutine(ChangeRadioStationWithDelay(dropdownIndex));
    }

    private IEnumerator ChangeRadioStationWithDelay(int dropdownIndex)
    {
        // Stop the current radio station
        StopRadio();

        yield return new WaitForSeconds(0.1f); // Wait for 100 milliseconds

        // Start playing the new radio station
        StartCoroutine(PlayRadio(icecastUrls[dropdownIndex]));
    }
}