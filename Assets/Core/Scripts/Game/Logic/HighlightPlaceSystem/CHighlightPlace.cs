namespace Client.Game
{
    public struct CHighlightPlace
    {
        public HighlightPlaceMb HighlightPlaceMb;

        public void Invoke(HighlightPlaceMb highlightPlaceMb)
        {
            HighlightPlaceMb = highlightPlaceMb;
        }
    }
}