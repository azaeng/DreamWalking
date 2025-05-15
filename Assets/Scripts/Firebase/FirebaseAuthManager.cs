using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        db = FirebaseFirestore.DefaultInstance;

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
    public void Create(string Id, string Pwd, string Nick, Action onSuccess, Action<string> onFailure)
    {
        auth.CreateUserWithEmailAndPasswordAsync(Id, Pwd).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                onFailure?.Invoke("회원가입 실패: " + task.Exception?.Flatten().Message);
                return;
            }

            AuthResult result = task.Result;
            user = result.User;

            // Firestore에 유저 정보 저장
            Dictionary<string, object> userData = new Dictionary<string, object>
            {
                { "nickname", Nick },
                { "level", 1 },
                { "items", new List<string>() }
            };

            db.Collection("players").Document(user.UserId).SetAsync(userData).ContinueWith(setTask =>
            {
                if (setTask.IsCompleted)
                {
                    onSuccess?.Invoke();
                }
                else
                {
                    onFailure?.Invoke("DB 저장 실패: " + setTask.Exception?.Flatten().Message);
                }
            });
        });
    }

    // 닉네임 중복 검사
    public async Task<bool> IsNicknameDuplicate(string Nick)
    {
        Query query = db.Collection("players").WhereEqualTo("nickname", Nick);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        return snapshot.Count > 0;
    }

    // 로그인 요청
    public void Login(string email, string password, Action onSuccess, Action<string> onFailure)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                string errorMsg = "로그인 실패: " + task.Exception?.Flatten().Message;
                Debug.LogError(errorMsg);
                onFailure?.Invoke(errorMsg);
                return;
            }

            AuthResult authResult = task.Result;
            user = authResult.User;

            Debug.LogFormat("로그인 성공: {0}", user.UserId);
            onSuccess?.Invoke();
        });
    }


    // 로그아웃 요청
    public void LogOut()
    {
        auth.SignOut();
        user = null;
        Debug.Log("로그아웃 되었습니다.");
    }

    // 닉네임 요청
    public async Task<string> GetNickname()
    {
        if (user == null) return null;

        DocumentReference docRef = db.Collection("players").Document(user.UserId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists && snapshot.ContainsField("nickname"))
        {
            return snapshot.GetValue<string>("nickname");
        }

        return null;
    }
}
