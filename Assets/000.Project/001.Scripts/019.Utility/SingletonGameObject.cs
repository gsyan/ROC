using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGameObject<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static GameObject _container;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _container = new GameObject();
            _container.name = "_" + typeof(T).Name;
            _instance = _container.AddComponent(typeof(T)) as T;

            return _instance;
        }
    }
}
