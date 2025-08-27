using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance{
        get{
            return instance;
        }
    }
    
   private void Awake(){
instance = this as T;//类型转换 把this转换为T类型的对象
}


private void OnDestory(){
instance = null;
}

}