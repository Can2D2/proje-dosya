using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class CameraFeedDisplay : MonoBehaviour
{
    public RawImage rawImage;
    public string pythonExePath = @"C:\Users\enest\myProject_2\Library\PythonInstall\Scripts\python.exe";
    public string pythonScriptPath = @"C:\Users\enest\myProject_2\Assets\Scripts\poseDetection.py";

    Process pythonProcess;
    Texture2D tex;

    void Start()
    {
        pythonScriptPath = Path.Combine(Application.dataPath, "Scripts", "poseDetection.py");
        StartPythonProcess();
    }

    void StartPythonProcess()
    {
        pythonProcess = new Process();
        pythonProcess.StartInfo.FileName = pythonExePath;
        pythonProcess.StartInfo.Arguments = "\"" + pythonScriptPath + "\"";
        pythonProcess.StartInfo.UseShellExecute = false;
        pythonProcess.StartInfo.RedirectStandardOutput = true;
        pythonProcess.StartInfo.RedirectStandardError = true;
        pythonProcess.StartInfo.CreateNoWindow = true;
        pythonProcess.StartInfo.WorkingDirectory = @"C:\Users\enest\myProject_2\Assets\Scripts";

        pythonProcess.OutputDataReceived += PythonOutputHandler;
        pythonProcess.ErrorDataReceived += (sender, args) => UnityEngine.Debug.LogError(args.Data);

        try
        {
            string scriptDirectory = Path.GetDirectoryName(pythonScriptPath);
            pythonProcess.Start();
            pythonProcess.BeginOutputReadLine();
            pythonProcess.BeginErrorReadLine(); 
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed to start Python process: " + e.Message);
        }
    }

    void PythonOutputHandler(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            byte[] imageBytes = System.Convert.FromBase64String(e.Data);
            tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
            tex.LoadImage(imageBytes);
            rawImage.texture = tex;
        }
    }

    private void OnDestroy()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
        }
    }
}
