using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace LAddressables
{
    public static class AddressableUtility
    {
        private static readonly Dictionary<string, AsyncOperationHandle> TemporaryLoadedAssets = new ();
        private static readonly Dictionary<string, AsyncOperationHandle> PermanentLoadedAssets = new();

        public static async Task<T> LoadAssetAsync<T>(AssetReference assetReference) where T: Object
        {
            var assetGuid = assetReference.AssetGUID;
            if (TemporaryLoadedAssets.TryGetValue(assetGuid, out var existingHandle))
                return existingHandle.Result as T;

            var handle = assetReference.LoadAssetAsync<T>();
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load Addressable asset: {assetGuid}");
            
            TemporaryLoadedAssets[assetGuid] = handle;
            return handle.Result;
        }
        
        public static async Task<T> LoadAssetAsync<T>(string address) where T : Object
        {
            if (TemporaryLoadedAssets.TryGetValue(address, out var existingHandle))
                return existingHandle.Result as T;

            var handle = Addressables.LoadAssetAsync<T>(address);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load Addressable asset: {address}");
            
            TemporaryLoadedAssets[address] = handle;
            return handle.Result;
        }
        
        public static async Task<GameObject> InstantiateAsync(string address, Vector3 position, Quaternion rotation)
        {
            GameObject result = null;

            if (!TemporaryLoadedAssets.ContainsKey(address))
            {
                await LoadAssetAsync<GameObject>(address);
            }

            var handle = Addressables.InstantiateAsync(address, position, rotation);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to instantiate Addressable prefab: {address}");
            
            result = handle.Result;
            return result;
        }

        public static void Release(AssetReference assetReference)
        {
            Release(assetReference.AssetGUID);
        }
        
        public static void Release(string address)
        {
            if (TemporaryLoadedAssets.ContainsKey(address))
            {
                Addressables.Release(TemporaryLoadedAssets[address]);
                TemporaryLoadedAssets.Remove(address);
                return;
            }
            
            Debug.LogWarning($"Addressable asset not loaded: {address}");
        }
        
        public static void ReleaseAll()
        {
            foreach (var handle in TemporaryLoadedAssets.Values)
            {
                Addressables.Release(handle);
            }

            TemporaryLoadedAssets.Clear();
        }
    }
}
