using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
using DG.Tweening;

public enum ShakeMode
{
    none,
    weak,
    moderate,
    strong
}

public static class CameraManager
{
    // public static CinemachineVirtualCamera virtualCam = GameObject.FindGameObjectWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();
    // private static CinemachineBasicMultiChannelPerlin cameraNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    public static void SetNoise(ShakeMode mode)
    {
        switch (mode)
        {
            case ShakeMode.weak:
                Camera.main.DOShakePosition(1f, 0.5f, 1, 5f).SetEase(Ease.OutCubic);
                return;
            case ShakeMode.moderate:
                Camera.main.DOShakePosition(1.5f, 1f, 3, 20f).SetEase(Ease.OutCubic);
                return;
            case ShakeMode.strong:
                Camera.main.DOShakePosition(2f, 2f, 6, 40).SetEase(Ease.OutCubic);
                return;
            default :
                Camera.main.DOShakePosition(1f, 0.5f, 3, 20).SetEase(Ease.OutCubic);
                return;
        }
    }
}
