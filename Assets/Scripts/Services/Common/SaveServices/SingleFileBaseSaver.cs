using UnityEngine;

public class OneFileBaseSaver<T> : BaseSaver<T> where T : struct
{
    [SerializeField] protected string _defaultSaveName = "Save";

    public void Save(T obj) => base.Save(obj, _defaultSaveName, true);
    public T Load() => base.Load(_defaultSaveName);
    public bool DeleteSave() => base.DeleteSave(_defaultSaveName);

    public override bool Save(T obj, string saveName, bool forceOverride = false) => base.Save(obj, _defaultSaveName, true);
    public override T Load(string saveName = null) => base.Load(_defaultSaveName);
    public override bool DeleteSave(string saveName) => base.DeleteSave(_defaultSaveName);
}
