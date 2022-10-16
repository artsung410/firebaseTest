using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Threading.Tasks;


public class AuthManager : MonoBehaviour
{
    // 현재 앱을 실행하는 환경이, 파이어베이스를 구동할수있는 환경인지 확인
    public bool IsFirebaseReady { get; private set; }

    // 재로그인 방지
    public bool IsSignInOnProgress { get; private set; }

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;


    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    // 이메일과 패스워드에 대응되는 유저정보를 가져온다.
    public static FirebaseUser User;

    public void Start()
    {
        signInButton.interactable = false;

        // 파이어베이스를 구동할수 있는상황인지 app이 확이함

        // 콜백이나 체인을 걸어둠
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;

            // 파이어베이스 구동이 가능한 상태가 아닐경우
            if (result != DependencyStatus.Available)
            {
                Debug.LogError(result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;

                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
            }

            signInButton.interactable = IsFirebaseReady;
        });
    }

    public void SignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null) return;

        IsSignInOnProgress = true;
        signInButton.interactable = false;

        // 
        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(
            task =>
            {
                Debug.Log($"Sign in status : ");

                IsSignInOnProgress = false;
                signInButton.interactable = true;
                
                // 태스크가 실패했을 경우
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }

                // 태스크가 도중에 취소되었을 경우
                else if (task.IsCanceled)
                {
                    Debug.LogError("It's canceled");
                }
                else
                {
                    User = task.Result;
                    Debug.Log(User.Email);
                    SceneManager.LoadScene("Lobby");
                }
            });
    }
}
