namespace Scripts.CognitiveServices.Speech
{
    public enum VoiceStyle_en_US_Neural
    {
        Amber,
        Aria,
        Ashley,
        Cora,
        Elizabeth,
        Jenny,
        Michelle,
        Monica,
        Sara,
        Ana,
        Brandon,
        Christopher,
        Eric,
        Guy,
        Jacob
    }

    public class VoiceStyle
    {
        public static string GetValue(VoiceStyle_en_US_Neural value)
        {
            return values[(int)value];
        }

        private static readonly string[] values = new string[] {
            "en-US-AmberNeural",
            "en-US-AriaNeural",
            "en-US-AshleyNeural",
            "en-US-CoraNeural",
            "en-US-ElizabethNeural",
            "en-US-JennyNeural",
            "en-US-MichelleNeural",
            "en-US-MonicaNeural",
            "en-US-SaraNeural",
            "en-US-AnaNeural",
            "en-US-BrandonNeural",
            "en-US-ChristopherNeural",
            "en-US-EricNeural",
            "en-US-GuyNeural",
            "en-US-JacobNeural"
        };
    }
}
