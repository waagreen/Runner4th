using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;

public enum ShakeMode
{
    none,
    weak,
    moderate,
    strong
}

public static class CameraManager
{
    public static CinemachineVirtualCamera virtualCam = GameObject.FindGameObjectWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();
    private static CinemachineBasicMultiChannelPerlin cameraNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    public static void SetNoise(ShakeMode mode)
    {
        switch (mode)
        {
            case ShakeMode.weak:
                CameraNoise(150, 1.5f, 1.5f);
                return;
            case ShakeMode.moderate:
                CameraNoise(150, 3f, 3f);
                return;
            case ShakeMode.strong:
                CameraNoise(175, 6f, 6f);
                return;
            default :
                CameraNoise(100, 0.7f, 1);
                return;
        }
    }

    private async static void CameraNoise(int shakeDuration, float shakeAmplitude, float ShakeFrequency)
    {
        cameraNoise.m_AmplitudeGain = shakeAmplitude;
        cameraNoise.m_FrequencyGain = ShakeFrequency;
        
        await Task.Delay(shakeDuration);

        cameraNoise.m_AmplitudeGain = 0f;
        cameraNoise.m_FrequencyGain = 0f;
    }
}
