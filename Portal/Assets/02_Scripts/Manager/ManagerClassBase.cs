using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityProjectStartUp;

public abstract class ManagerClassBase<T> : MonoBehaviour, IManagerClass
	where T : class, IManagerClass
{
	private static T _Instance = null;

	public static T Instance => (_Instance = _Instance ?? GameManager.GetManagerClass<T>());

	public abstract void InitializeManagerClass();
}
