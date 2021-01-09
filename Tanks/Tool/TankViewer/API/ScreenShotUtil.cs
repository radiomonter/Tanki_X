namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ScreenShotUtil
    {
        public static void TakeScreenshot(Camera camera, string filePath, int scale = 1)
        {
            int width = camera.pixelWidth * scale;
            int height = camera.pixelHeight * scale;
            RenderTexture texture = new RenderTexture(width, height, 0x18, RenderTextureFormat.ARGB32);
            camera.targetTexture = texture;
            camera.Render();
            RenderTexture.active = texture;
            Texture2D textured = new Texture2D(width, height, TextureFormat.ARGB32, false);
            textured.ReadPixels(new Rect(0f, 0f, (float) width, (float) height), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(texture);
            byte[] bytes = textured.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            Debug.Log($"Took screenshot to: {filePath}");
        }

        public static void TakeScreenshotAndOpenIt(Camera camera, string filePath, int scale = 1)
        {
            TakeScreenshot(camera, filePath, scale);
            Application.OpenURL(filePath);
        }

        public static byte[] TakeScreenshotBytes(Camera camera, int scale = 1)
        {
            int width = camera.pixelWidth * scale;
            int height = camera.pixelHeight * scale;
            RenderTexture texture = new RenderTexture(width, height, 0x18, RenderTextureFormat.ARGB32);
            camera.targetTexture = texture;
            camera.Render();
            RenderTexture.active = texture;
            Texture2D textured = new Texture2D(width, height, TextureFormat.ARGB32, false);
            textured.ReadPixels(new Rect(0f, 0f, (float) width, (float) height), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(texture);
            return textured.EncodeToPNG();
        }

        public static RenderTexture TakeScreenshotTexture(Camera camera, float scale = 1f)
        {
            RenderTexture texture = new RenderTexture((int) (camera.pixelWidth * scale), (int) (camera.pixelHeight * scale), 0x18, RenderTextureFormat.ARGB32);
            camera.targetTexture = texture;
            camera.Render();
            camera.targetTexture = null;
            RenderTexture.active = null;
            return texture;
        }
    }
}

