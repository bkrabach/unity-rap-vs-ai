namespace Scripts.CognitiveServices.Speech

{
    public struct Viseme
    {
        public Viseme(uint id, ulong offset)
        {
            Id = id;
            Offset = offset;
        }

        public uint Id;
        public ulong Offset;
    }
}
