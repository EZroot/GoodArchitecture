using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSettings", menuName = "EZROOT/GameSettings/DataSetting")]
public class DataSettingsScriptableObject : ScriptableObject
{
    [SerializeField] private string _addressableEntityDataSettingLabel;
    [SerializeField] private DataSO[] _entityDataSettingsCollection;
    [SerializeField] private string[] dataPath;
    [SerializeField] private string[] assetPath;

    public string AddressableEntityDataSettingLabel => _addressableEntityDataSettingLabel;
    public DataSO[] EntityDataSettingsCollection => _entityDataSettingsCollection;
    public string[] DataPath => dataPath;
    public string[] AssetPath => assetPath;
}
