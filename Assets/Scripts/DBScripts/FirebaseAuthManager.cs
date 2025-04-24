using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Firebase.Auth;
// using Firebase.Firestore;

public class FirebaseAuthManager
{
    FirebaseAuth auth; // 로그인 / 회원가입 등에 사용
    FirebaseUser user; // 인증이 완료된 유저 정보
    // FirebaseFirestore db;

    // 싱글턴 패턴 사용

    private static FirebaseAuthManager instance = null;

    public static FirebaseAuthManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseAuthManager();
            }

            return instance;
        }
    }

    public string UserId => user.UserId;

    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != null)
        {
            LogOut();
        }

        // StateChanged = 계정 상태가 바뀔 때마다 호출이 되는 것
        auth.StateChanged += OnChanged;
    }

    // OnChanged메서드의 매개 변수들은 StateChanged의 이벤트 핸들러를 참조할 것
    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);

            if (!signed && user != null)
            {
                Debug.Log("로그아웃"); // 테스트용
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("로그인");
                LoginState?.Invoke(true);
            }
        }
    }

    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패");
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}
