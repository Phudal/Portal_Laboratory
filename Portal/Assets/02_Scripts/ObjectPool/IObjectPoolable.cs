using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 오브젝트 풀링에 사용되는 컴포넌트에서 구현해야하는 인터페이스

public interface IObjectPoolable
{
    // 오브젝트가 재사용될 수 있음을 나타내는 프로퍼티
    bool canRecyclable { get; set; }

    // 오브젝트가 재사용되기 전에 호출되는 대리자
    Action OnRecycleStartSignature { get; }

    // 오브젝트가 재사용된 후에 호출되는 대리자
    Action OnRecycleFinishSignature { get; }
}

