using UnityEngine;

public class RadioManager : MonoBehaviour
{
    public Radio radio;
    public bool isRadioOn;


    private void OnEnable()
    {
        BCG_EnterExitPlayer.OnBCGPlayerEnteredAVehicle += RadioOn;
        BCG_EnterExitPlayer.OnBCGPlayerExitedFromAVehicle += RadioOff;
    }

    private void OnDisable()
    {
        BCG_EnterExitPlayer.OnBCGPlayerEnteredAVehicle -= RadioOn;
        BCG_EnterExitPlayer.OnBCGPlayerExitedFromAVehicle -= RadioOff;
    }

    public void RadioOn(BCG_EnterExitPlayer player, BCG_EnterExitVehicle vehicle)
    {
        radio.gameObject.SetActive(true);
        isRadioOn = true;
    }

    public void RadioOff(BCG_EnterExitPlayer player, BCG_EnterExitVehicle vehicle)
    {
        radio.gameObject.SetActive(false);
        isRadioOn = false;
    }

    private void Update()
    {
        if (isRadioOn)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                radio.PlayPreviousStation();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                radio.PlayNextStation();
            }
        }
    }
}
