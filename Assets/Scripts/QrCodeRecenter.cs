using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public class QrCodeRecenter : MonoBehaviour
{
    [SerializeField] private ARSession _session;
    [SerializeField] private ARSessionOrigin sessionOrigin;
    [SerializeField] private ARCameraManager _cameraManager;
    [SerializeField] private List<Target> _navigationTargets = new List<Target>();

    private Texture2D _cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetQrCodeRecenterTarget("Bedroom");
        }
    }

    private void OnEnable()
    {
        _cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        _cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!_cameraManager.TryAcquireLatestCpuImage(out var image))
        {
            return;
        }

        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.MirrorY
        };

        var size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);
        
        image.Convert(conversionParams, buffer);
        image.Dispose();

        _cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);
        
        _cameraImageTexture.LoadRawTextureData(buffer);
        _cameraImageTexture.Apply();
        
        buffer.Dispose();

        var result = reader.Decode(_cameraImageTexture.GetPixels32(), _cameraImageTexture.width,
            _cameraImageTexture.height);
        if (result != null)
        {
            SetQrCodeRecenterTarget(result.Text);
        }
    }

    private void SetQrCodeRecenterTarget(string targetName)
    {
        var currentTarget = _navigationTargets.Find(t =>
            t.Name.Equals(targetName, StringComparison.InvariantCultureIgnoreCase));
        if (currentTarget != null)
        {
            _session.Reset();
            sessionOrigin.transform.SetPositionAndRotation(currentTarget.transform.position,
                currentTarget.transform.rotation);
        }
    }
}
