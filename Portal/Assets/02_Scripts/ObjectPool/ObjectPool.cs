using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 오브젝트 풀링을 수행하는 클래스
// T : 인터페이스 IObjectPoolable을 구현하는 클래스만 사용될 수 있음


public sealed partial class ObjectPool<T> where T : class, IObjectPoolable
{
    private List<T> _PoolObject = new List<T>();

    // 재활용할 오브젝트가 존재하는지 검사
    public bool canRecycle =>
        _PoolObject.Find((T poolableObject) => poolableObject.canRecyclable) != null;

    // 풀링할 새로운 오브젝트를 등록
    // return : 등록한 객체 (newRecyclableObject)를 그대로 리턴
    public T RegisterRecyclableObject(T newRecyclableObject)
    {
        _PoolObject.Add(newRecyclableObject);
        return newRecyclableObject;
    }

    // 사용하지 않는 오브젝트 제거
    public void UnRegisterRecyclableObject(params T[] recyclableObject)
    {
        foreach (var recyclableObj in recyclableObject)
            _PoolObject.Remove(recyclableObj);
    }

    // 재활용된 오브젝트를 얻음
    // checkCanRecycle : true일 경우 재사용 가능한 오브젝트 존재 여부를 검사
    // return : 재활용된 오브젝트를 리턴
    public T GetRecyclableObject(bool checkCanRecycle = false)
    {
        // 재사용 가능한 오브젝트가 존재하는지 검사
        if (checkCanRecycle) if (!canRecycle) return null;

        T recyclableObject = _PoolObject.Find((T poolableObjcect) =>
        poolableObjcect.canRecyclable);

        recyclableObject.OnRecycleStartSignature?.Invoke();

        recyclableObject.canRecyclable = true;

        recyclableObject.OnRecycleFinishSignature?.Invoke();


        return recyclableObject;
    }
}

