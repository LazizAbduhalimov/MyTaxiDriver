using TMPro;

namespace Game
{
    public struct CCoinDisplayer
    {
        public TMP_Text Text;

        public void Invoke(TMP_Text text)
        {
            Text = text;
        }
    }
}