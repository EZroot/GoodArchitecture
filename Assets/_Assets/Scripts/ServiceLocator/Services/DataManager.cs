using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectScare.ServiceLocator
{
    public class DataManager : IServiceDataManager
    {

        // Dictionary to store loaded DataSOs
        private List<DataSO> _entityDataSOCollection = null;
        private Dictionary<Type, List<EntityData>> _entityDataDictionary = null;

        public DataManager()
        {
            _entityDataSOCollection = new List<DataSO>();
            _entityDataDictionary = new Dictionary<Type, List<EntityData>>();

            var dataSettings = ServiceLocator.Get<IServiceGameManager>().GameSettings.DataSettings;
            FileUtils.InitDirectories(dataSettings);
            //LoadAndProcessData().Forget();
        }

        public List<T> GetEntityData<T>(Type type) where T : EntityData
        {
            List<T> _shallowList = new List<T>();
            foreach (var data in _entityDataDictionary[type])
            {
                _shallowList.Add(data as T);
            }
            return _shallowList;
        }

        public async UniTask LoadAndProcessData()
        {
            // Create a dictionary to map each type to an action
            Dictionary<Type, Func<DataSO, UniTask>> typeActions = new Dictionary<Type, Func<DataSO, UniTask>>
        {
            { typeof(PlayerDataSO), async(data) => await PlayerData_Processor(data) },
            { typeof(ObjectEntityDataSO), async(data) => await ObjectData_Processor(data) },
            { typeof(CreatureDataSO), async(data) => await CreatureData_Processor(data) }

        };
            var label = ServiceLocator.Get<IServiceGameManager>().GameSettings.DataSettings.AddressableEntityDataSettingLabel;
            IList<DataSO> dataSOList = await Addressables.LoadAssetsAsync<DataSO>(label, DataSO_OnLoad).Task;

            // Add more types and their corresponding actions as needed
            foreach (var baseSO in dataSOList)
            {
                // Get the type of the current ScriptableObject
                var baseSOType = baseSO.GetType();
                Debug.Log($"Loading {baseSOType}");

                // Check if the type is present in the dictionary, and execute the corresponding action
                if (typeActions.TryGetValue(baseSOType, out Func<DataSO, UniTask> action))
                {
                    await action(baseSO);
                }
                else
                {
                    // Handle the base class or any unknown types here
                    Debug.Log("ProcessData: Found DataSO or unknown type");
                }
            }
        }

        private async UniTask PlayerData_Processor(DataSO baseSO)
        {
            var playerSO = (PlayerDataSO)baseSO;
            var entityData = playerSO.EntityData;

            //Initial Copy of data
            var dataObj = DataUtils.DeepCopy(entityData);
            //Check if saved data exists
            var fileName = dataObj.ID;
            var loadedData = await LoadDataFromJson<PlayerEntityData>(fileName);

            //No saved data exists
            if (loadedData == null)
                await SaveDataAsJson(dataObj);

            AddEntityDataToDictionary(dataObj.GetType(), dataObj);
            // Do something with the EntityDataA
            Debug.Log("PlayerData_Processor: <color=green>Loaded</color>");
        }

        private async UniTask ObjectData_Processor(DataSO baseSO)
        {
            var dataSO = (ObjectEntityDataSO)baseSO;
            var entityData = dataSO.EntityData;

            var dataObj = DataUtils.DeepCopy(entityData);
            //Check if saved data exists
            var fileName = dataObj.ID;
            var loadedData = await LoadDataFromJson<PlayerEntityData>(fileName);
            //No saved data exists
            if (loadedData == null)
                await SaveDataAsJson(dataObj);

            AddEntityDataToDictionary(dataObj.GetType(), dataObj);
            // Do something with the EntityDataB
            Debug.Log("ObjectData_Processor: <color=green>Loaded</color>");
        }

        private async UniTask CreatureData_Processor(DataSO baseSO)
        {
            var dataSO = (CreatureDataSO)baseSO;
            var entityData = dataSO.CreatureEntityData;

            var dataObj = DataUtils.DeepCopy(entityData);
            //Check if saved data exists
            var fileName = dataObj.ID;
            var loadedData = await LoadDataFromJson<CreatureEntityData>(fileName);
            //No saved data exists
            if (loadedData == null)
                await SaveDataAsJson(dataObj);

            AddEntityDataToDictionary(dataObj.GetType(), dataObj);
            // Do something with the EntityDataB
            Debug.Log("CreatureData_Processor: <color=green>Loaded</color>");
        }

        public async UniTask SaveDataAsJson<T>(T data) where T : EntityData
        {
            var json = JsonUtility.ToJson(data);
            await FileUtils.SaveFile(json, data.ID);
        }

        public async UniTask<T> LoadDataFromJson<T>(string fileName) where T : EntityData
        {
            var data = await FileUtils.LoadFile<T>(fileName);
            return data;
        }


        private void AddEntityDataToDictionary(Type type, EntityData data)
        {
            if (_entityDataDictionary.TryGetValue(type, out List<EntityData> values))
            {
                Debug.Log($"AddEntityDataToDictionary: Added {type} to existing Dictionary Data");
                values.Add(data);
            }
            else
            {
                Debug.Log($"AddEntityDataToDictionary: Created {type} in Dictionary Data");
                _entityDataDictionary.Add(type, new List<EntityData>() { data }); // Add a new empty list for the ID
            }
        }

        private void DataSO_OnLoad(DataSO data)
        {
            // Process the loaded DataSO object here
            // For example:
            Debug.Log($"DataSO_OnLoad: addressable SO asset loaded {data.name}");
        }

        public void TestFunc()
        {
            Debug.Log("DataManager Tested and working");
        }

    }
}