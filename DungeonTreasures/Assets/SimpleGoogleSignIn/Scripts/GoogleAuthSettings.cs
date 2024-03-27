using System.Collections.Generic;
using UnityEngine;

namespace Assets.SimpleGoogleSignIn.Scripts
{
    [CreateAssetMenu(fileName = "GoogleAuthSettings", menuName = "Simple Google Sign-In/GoogleAuthSettings")]
    public class GoogleAuthSettings : ScriptableObject
    {
        public string Id = "Default";

        [Header("Android / iOS / macOS / Universal Windows Platform")]
        [SerializeField] private string ClientIdGeneric;
        [SerializeField] private string CustomUriSchemeGeneric;

        [Header("Windows")]
        [SerializeField] private string ClientIdWindows;
        [SerializeField] private string ClientSecretWindows;
        [SerializeField] private string CustomUriSchemeWindows;

        [Header("WebGL")]
        [SerializeField] private string ClientIdWebGL;
        [SerializeField] private string ClientSecretWebGL;

        [Header("Editor")]
        [SerializeField] private string ClientIdDesktop;
        [SerializeField] private string ClientSecretDesktop;

        [Header("Options")]
        public List<string> AccessScopes = new() { "openid", "email", "profile" };
        [Tooltip("`GoogleAuth.Cancel()` method should be called manually. `User cancelled` callback will not called automatically when the user returns to the app without performing auth.")]
        public bool ManualCancellation;
        [Tooltip("Use Safari API on iOS instead of a default web browser. This option is required for passing App Store review.")]
        public bool UseSafariViewController = true;

        #if UNITY_EDITOR

        public string ClientId => ClientIdDesktop;
        public string ClientSecret => ClientSecretDesktop;

        #elif UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_WSA

        public string ClientId => ClientIdGeneric;
        public string ClientSecret => null;
        public string CustomUriScheme => CustomUriSchemeGeneric;

        #elif UNITY_STANDALONE_WIN

        public string ClientId => ClientIdWindows;
        public string ClientSecret => ClientSecretWindows;
        public string CustomUriScheme => CustomUriSchemeWindows;
        
        #elif UNITY_WEBGL

        public string ClientId => ClientIdWebGL;
        public string ClientSecret => ClientSecretWebGL;
        
        #endif

        public bool Redefined()
        {
            return ClientIdGeneric != "275731233438-nv59vdj6ornprhtppnm8qle2jt3ebjol.apps.googleusercontent.com"
                   && CustomUriSchemeGeneric != "simple.oauth"
                   && ClientIdDesktop != "275731233438-v1anl61611mmer6ohqes9a310mkc2di8.apps.googleusercontent.com"
                   && ClientSecretDesktop != "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"
                   && ClientIdWebGL != "275731233438-3hvi982vbhpcpkjihioa4d3gum074lds.apps.googleusercontent.com"
                   && ClientSecretWebGL != "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";
        }
    }
}