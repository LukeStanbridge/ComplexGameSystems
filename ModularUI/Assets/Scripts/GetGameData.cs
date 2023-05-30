using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGameData : MonoBehaviour
{
    public List<float> sliderValues;
    public List<bool> toggleValues;
    public List<int> toggleConvertedValues;
    public List<int> dropdownOptions;

    private int savedSliderCount;
    private int savedToggleCount;
    private int savedDropdownCount;
    public void SaveSettings()
    {
        //save slider values
        for (int i = 0; i < sliderValues.Count; i++)
        {
            PlayerPrefs.SetFloat("SliderValues" + i, sliderValues[i]);

        }
        PlayerPrefs.SetInt("SliderCount", sliderValues.Count);

        //save toggle values
        for (int i = 0; i < toggleValues.Count; i++)
        {
            toggleConvertedValues.Add(boolToInt(toggleValues[i]));
            PlayerPrefs.SetInt("ToggleValues" + i, toggleConvertedValues[i]);
        }
        PlayerPrefs.SetInt("ToggleCount", toggleValues.Count);

        //save slider values
        for (int i = 0; i < dropdownOptions.Count; i++)
        {
            PlayerPrefs.SetInt("DropdownValues" + i, dropdownOptions[i]);
        }
        PlayerPrefs.SetInt("DropdownCount", dropdownOptions.Count);
    }

    public void LoadSettings()
    {
        //load slider values
        sliderValues.Clear();
        savedSliderCount = PlayerPrefs.GetInt("SliderCount");

        for (int i = 0; i < savedSliderCount; i++)
        {
            float sliderValue = PlayerPrefs.GetFloat("SliderValues" + i);
            sliderValues.Add(sliderValue);
        }

        //load toggle values
        toggleValues.Clear();
        toggleConvertedValues.Clear();
        savedToggleCount = PlayerPrefs.GetInt("ToggleCount");

        for (int i = 0; i < savedToggleCount; i++)
        {
            int toggleValue = PlayerPrefs.GetInt("ToggleValues" + i);
            toggleValues.Add(intToBool(toggleValue));
        }

        //load dropdown values
        dropdownOptions.Clear();
        savedDropdownCount = PlayerPrefs.GetInt("DropdownCount");

        for (int i = 0; i < savedDropdownCount; i++)
        {
            int dropdownValue = PlayerPrefs.GetInt("DropdownValues" + i);
            dropdownOptions.Add(dropdownValue);
        }
    }

    private int boolToInt(bool value)
    {
        if (value) return 1;
        else return 0;
    }

    private bool intToBool(int val)
    {
        if (val != 0) return true;
        else return false;
    }
}
