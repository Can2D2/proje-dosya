using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Webview : MonoBehaviour
{
    public string url = "https://landbot.site/v3/H-2550325-OFFN8ECXEFG98Y3O/index.html";
    private WebViewObject webViewObject;
    public Button openwebview;
    public Button closewebview;
    private int topMargin = 150;

    void Start()
    {
        Debug.Log("Webview Start method called");

        // Add listener to the openwebview button
        if (openwebview != null)
        {
            openwebview.onClick.AddListener(OpenWebView);
        }
        else
        {
            Debug.LogError("Open WebView button is not assigned!");
        }
    

            // Add listener to the closewebview button
        if (closewebview != null)
        {
            closewebview.onClick.AddListener(CloseWebView);
        }
        else
        {
            Debug.LogError("Close WebView button is not assigned!");
        }
    }

    void OpenWebView()
    {
        Debug.Log("OpenWebView method called");
        StartCoroutine(InitializeWebView());
    }

    IEnumerator InitializeWebView()
    {
        Debug.Log("InitializeWebView coroutine started");

        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            Debug.LogError("WebView is only supported on Android and iOS platforms.");
            yield break;
        }

        if (webViewObject == null)
        {
            webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
            Debug.Log("WebViewObject created");
            webViewObject.Init(
                cb: (msg) => { Debug.Log($"CallFromJS: {msg}"); },
                err: (msg) => { Debug.LogError($"CallOnError: {msg}"); },
                started: (msg) => { Debug.Log($"CallOnStarted: {msg}"); },
                hooked: (msg) => { Debug.Log($"CallOnHooked: {msg}"); },
                ld: (msg) => { Debug.Log($"CallOnLoaded: {msg}"); },
                enableWKWebView: true
            );
            Debug.Log("WebViewObject initialized");
        }

        webViewObject.SetMargins(0, topMargin, 0, 0);
        webViewObject.SetVisibility(true);
        Debug.Log("WebViewObject margins set and visibility enabled");

        webViewObject.ClearCache(true);
        webViewObject.LoadURL(url);

        Debug.Log($"LoadURL called with URL: {url}");
        yield return new WaitForSeconds(5f);
        Debug.Log("InitializeWebView coroutine completed");
    }
    void CloseWebView()
    {
        Debug.Log("CloseWebView method called");
        if (webViewObject != null)
        {
            webViewObject.SetVisibility(false);
            Destroy(webViewObject.gameObject);
            webViewObject = null;
            Debug.Log("WebViewObject closed and destroyed");
        }
    }

    void OnDestroy()
    {
        if (webViewObject != null)
        {
            Destroy(webViewObject.gameObject);
            Debug.Log("WebViewObject destroyed");
        }
    }
}
