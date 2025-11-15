using UnityEngine;
using System.IO;

public class TextureToPNGExporter : MonoBehaviour
{
    [Header("Assign a RenderTexture to export")]
    public RenderTexture sourceTexture;

    [Header("Output file name (no extension)")]
    public string fileName = "CapturedIcon";

    [Header("Folder relative to project root")]
    public string folderPath = "Assets/Exports/";

    [ContextMenu("Export PNG")]
    public void ExportPNG()
    {
        if (sourceTexture == null)
        {
            Debug.LogError("No RenderTexture assigned!");
            return;
        }

        // Set active
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = sourceTexture;

        // Read into Texture2D
        Texture2D tex = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, sourceTexture.width, sourceTexture.height), 0, 0);
        tex.Apply();

        // Restore RT
        RenderTexture.active = currentRT;

        // Encode
        byte[] pngData = tex.EncodeToPNG();

        // Ensure folder exists
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Final path
        string fullPath = Path.Combine(folderPath, fileName + ".png");

        // Write file
        File.WriteAllBytes(fullPath, pngData);

        Debug.Log("PNG exported: " + fullPath);

        // Cleanup
        DestroyImmediate(tex);
    }

    void Start()
    {
        ExportPNG();
    }
}
