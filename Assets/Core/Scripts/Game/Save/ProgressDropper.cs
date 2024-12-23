using Esper.ESave;
using LGrid;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Scripts.Game.Save
{
    public class ProgressDropper : MonoBehaviour
    {
        public SaveFileSetup File;
        
        public void Drop()
        {
            Map.Instance.Clear();
            var file = File.GetSaveFile();
            Bank.SetCoins(this, 0);
            file.DeleteData("PurchaseNumber");
            file.DeleteData("Coins");
            file.Save();
            SceneManager.LoadScene(0);
        }
    }
}