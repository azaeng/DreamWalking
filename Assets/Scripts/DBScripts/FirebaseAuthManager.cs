using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Firebase.Auth;
// using Firebase.Firestore;

public class FirebaseAuthManager
{
    FirebaseAuth auth; // �α��� / ȸ������ � ���
    FirebaseUser user; // ������ �Ϸ�� ���� ����
    // FirebaseFirestore db;

    // �̱��� ���� ���

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

        // StateChanged = ���� ���°� �ٲ� ������ ȣ���� �Ǵ� ��
        auth.StateChanged += OnChanged;
    }

    // OnChanged�޼����� �Ű� �������� StateChanged�� �̺�Ʈ �ڵ鷯�� ������ ��
    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);

            if (!signed && user != null)
            {
                Debug.Log("�α׾ƿ�"); // �׽�Ʈ��
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("�α���");
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
                Debug.LogError("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ȸ������ ����");
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
                Debug.LogError("�α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("�α��� ����");
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }
}
