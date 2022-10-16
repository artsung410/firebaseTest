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
    // ���� ���� �����ϴ� ȯ����, ���̾�̽��� �����Ҽ��ִ� ȯ������ Ȯ��
    public bool IsFirebaseReady { get; private set; }

    // ��α��� ����
    public bool IsSignInOnProgress { get; private set; }

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;


    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    // �̸��ϰ� �н����忡 �����Ǵ� ���������� �����´�.
    public static FirebaseUser User;

    public void Start()
    {
        signInButton.interactable = false;

        // ���̾�̽��� �����Ҽ� �ִ»�Ȳ���� app�� Ȯ����

        // �ݹ��̳� ü���� �ɾ��
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;

            // ���̾�̽� ������ ������ ���°� �ƴҰ��
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
                
                // �½�ũ�� �������� ���
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }

                // �½�ũ�� ���߿� ��ҵǾ��� ���
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
