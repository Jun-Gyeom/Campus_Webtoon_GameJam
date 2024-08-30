using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    // path에 있는 파일을 로드하는 함수, 로드되는 조건은 Object 일 때
    public T Load<T>(string path) where T : Object
    {
        var loadedObject = Resources.Load<T>(path);
        
        if (!loadedObject)
        {
            Debug.LogError($"다음 이름의 리소스를 찾을 수 없습니다. : {path}");
            return null;
        }
        
        return loadedObject;
    }
    
    // path에 있는 폴더 안 리소스들을 로드하는 함수, 로드되는 조건은 Object 일 때
    public Dictionary<string, T> LoadAll<T>(string path) where T : Object
    {
        Dictionary<string, T> loadedObjectDictionary = new Dictionary<string, T>();
        T[] loadedObjects = Resources.LoadAll<T>(path);
        
        if (loadedObjects.Length == 0)
        {
            Debug.LogError($"다음 폴더 안의 리소스들을 찾을 수 없습니다. : {path}");
        }
        else
        {
            foreach (var loadedObject in loadedObjects)
            {
                loadedObjectDictionary[loadedObject.name] = loadedObject;
            }
        }

        return loadedObjectDictionary;
    }
}