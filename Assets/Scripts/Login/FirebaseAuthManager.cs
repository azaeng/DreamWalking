using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Firebase.Auth;
using Firebase.Firestore;

public class FirebaseAuthManager
{
    private FirebaseAuth auth;          // 로그인 / 회원가입 등 인증에 사용
    private FirebaseUser user;          // 인증이 완료된 사용자 정보
    private FirebaseFirestore db;       // Firestore (사용 가능 시 활용)

    // 싱글톤 패턴을 위한 인스턴스
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

    // 현재 로그인된 사용자의 UID
    public string UserId => user != null ? user.UserId : "";

    // 로그인 상태 변화시 알림을 주는 델리게이트
    public Action<bool> LoginState;

    // Firebase 초기화
    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        // 현재 유저가 로그인된 상태라면 로그아웃 처리
        if (auth.CurrentUser != null)
        {
            LogOut();
        }

        // 로그인 상태 변경 이벤트 핸들러 등록
        auth.StateChanged += OnChanged;
    }

    // 로그인 상태가 변경되었을 때 호출되는 메서드
    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != null);

            if (!signed && user != null)
            {
                Debug.Log("로그아웃됨");
                LoginState?.Invoke(false); // 로그인 상태 false 알림
            }

            user = auth.CurrentUser;

            if (signed)
            {
                Debug.Log("로그인됨");
                LoginState?.Invoke(true); // 로그인 상태 true 알림
            }
        }
    }

    // 회원가입 요청
    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입이 취소되었습니다.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패: " + task.Exception?.Flatten().Message);
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.LogFormat("회원가입 성공: {0} ({1})", user.DisplayName, user.UserId);
        });
    }

    // 로그인 요청
    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인이 취소되었습니다.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패: " + task.Exception?.Flatten().Message);
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.LogFormat("로그인 성공: {0} ({1})", user.DisplayName, user.UserId);
        });
    }

    // 로그아웃 요청
    public void LogOut()
    {
        auth.SignOut();
        user = null;
        Debug.Log("로그아웃 되었습니다.");
    }
}
